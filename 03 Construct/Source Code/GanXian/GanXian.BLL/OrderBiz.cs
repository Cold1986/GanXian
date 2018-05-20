using CommonLib;
using Dapper;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WechatService.Biz;

namespace GanXian.BLL
{
    public class OrderBiz
    {
        public static OrderBiz CreateNew()
        {
            return new OrderBiz();
        }

        /// <summary>
        /// 创建单产品订单
        /// </summary>
        /// <param name="productId">产品id</param>
        /// <param name="num">数量</param>
        /// <param name="userOpenId">用户微信openid</param>
        /// <returns></returns>
        public bool createOrder(string productId, string num, string userOpenId, string salesNo)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string insertSalesSlipSQL = "insert into SalesSlip(salesNo,userOpenId,createDate,status,column2) values(@salesNo,@userOpenId,now(),0,now());select @@IDENTITY;";
                    string insertSales2ProductsSQL = @"insert into Sales2Products(salesId,productId,num,createDate,status,createdTimePrice) 
                                                        select @salesId, @productId, @num, now(), 0, discountedPrice from products a
                                                        where a.status = 1 and a.productId = @productId
                                                        ";
                    string salesId = conn.ExecuteScalar(insertSalesSlipSQL, new { salesNo = salesNo, userOpenId = userOpenId }, transaction, null, null).ToString();
                    conn.Execute(insertSales2ProductsSQL, new { salesId = salesId, productId = productId, num = num }, transaction, null, null);
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 从购物车中创建订单
        /// </summary>
        /// <param name="prodIds"></param>
        /// <param name="userOpenId"></param>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        public bool createOrderFromShopcart(string prodIds, string userOpenId, string salesNo)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string[] prods = prodIds.Split(',');
                    string insertSalesSlipSQL = "insert into SalesSlip(salesNo,userOpenId,createDate,status,column2) values(@salesNo,@userOpenId,now(),0,now());select @@IDENTITY;";
                    string insertSales2ProductsSQL = "";
                    string updateShoppingcartSQL = "";
                    for (int i = 0; i < prods.Length; i++)
                    {
                        insertSales2ProductsSQL += @"insert into Sales2Products(salesId,productId,num,createDate,status,createdTimePrice) 
                                                    select @salesId, a.productId,num,now(),0,b.discountedPrice from shoppingcart a
                                                    inner join products b on a.productId = b.productId
                                                    where a.status = 1 and b.status = 1 and userOpenId = @userOpenId and a.productId = " + prods[i] + ";";
                        updateShoppingcartSQL += "update shoppingcart set status=2,column1=now() where status=1 and userOpenId=@userOpenId and productId =" + prods[i] + ";";
                    }
                    string salesId = conn.ExecuteScalar(insertSalesSlipSQL, new { salesNo = salesNo, userOpenId = userOpenId }, transaction, null, null).ToString();
                    conn.Execute(insertSales2ProductsSQL, new { salesId = salesId, productId = prodIds, userOpenId = userOpenId }, transaction, null, null);
                    conn.Execute(updateShoppingcartSQL, new { salesId = salesId, productId = prodIds, userOpenId = userOpenId }, transaction, null, null);
                    //提交事务
                    transaction.Commit();
                    res = true;

                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 获取订单结算信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="userOpenId">用户微信OpenId</param>
        public salesslip getCheckOutInfo(string orderId, string userOpenId)
        {
            salesslip userSalesSlip = new salesslip();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM ganxian.salesslip where salesNo=@orderId and userOpenId=@userOpenId";
                userSalesSlip = conn.Query<salesslip>(sqlCommandText, new { orderId = orderId, userOpenId = userOpenId }).FirstOrDefault();
            }
            return userSalesSlip;
        }

        public void updateWechatOrderId(string orderId, string userOpenId, string wechatOrderNo)
        {
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"update ganxian.salesslip set wechatOrderNo=@wechatOrderNo where salesNo=@orderId and userOpenId=@userOpenId";
                conn.Execute(sqlCommandText, new { orderId = orderId, userOpenId = userOpenId, wechatOrderNo = wechatOrderNo });
            }
        }



        /// <summary>
        /// 获取用户未付款订单
        /// </summary>
        /// <param name="salesId"></param>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<UserShopcartsInfo> getUnpaidOrderInfo(int salesId)
        {
            List<UserShopcartsInfo> res;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                //c.status0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款
                string sqlCommandText = @"SELECT a.num,a.createdTimePrice ,a.id,b.* FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where  b.status=1 and a.salesId=@salesId
                                            order by a.createDate desc";
                res = conn.Query<UserShopcartsInfo>(sqlCommandText, new { salesId = salesId }).ToList();
                if (res.Any())
                {
                    res.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                    foreach (var item in res)
                    {
                        System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                        foreach (System.Reflection.PropertyInfo item2 in pro)
                        {
                            try
                            {
                                if (item2.Name == item.showPic)
                                {
                                    item.showPic = item2.GetValue(item).ToString();
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return res;
        }

        public bool userPaidOrder(salesslip paidOrder)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string updateSalesSlipSQL = "update salesslip set receiver=@receiver,province=@province,city=@city,county=@county,detailAddress=@detailAddress,Phone=@Phone,amount=@amount,postage=@postage,payDate=@payDate,status=@status,column1=@column1,column2=@payDate where salesId=@salesId and salesNo=@salesNo and userOpenId = @userOpenId";
                    string updateSales2ProductsSQL = "update sales2products a inner join products b on a.productid=b.productid  set a.originalPrice=b.originalPrice,a.discountedPrice=b.discountedPrice,a.nw=b.nw,a.status=@status where a.salesId = @salesId";
                    conn.Execute(updateSalesSlipSQL, paidOrder, transaction).ToString();
                    conn.Execute(updateSales2ProductsSQL, paidOrder, transaction).ToString();
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 管理员修改价格
        /// </summary>
        /// <param name="salesNo"></param>
        /// <param name="adminChangeAmount"></param>
        /// <returns></returns>
        public bool adminChangeAmount(string salesNo, decimal adminChangeAmount)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = "update salesslip set adminChangeAmount=@adminChangeAmount where salesNo=@salesNo";
                    conn.Execute(updateSalesSlipSQL, new { salesNo = salesNo, adminChangeAmount = adminChangeAmount }).ToString();
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 管理员执行退货操作
        /// </summary>
        /// <param name="salesNo"></param>
        /// <param name="adminChangeAmount"></param>
        /// <returns></returns>
        public bool adminReturnProd(string salesNo)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = "update salesslip set status=7,column2=now() where salesNo=@salesNo";
                    conn.Execute(updateSalesSlipSQL, new { salesNo = salesNo }).ToString();
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 管理员发送物品操作
        /// </summary>
        /// <param name="salesNo"></param>
        /// <param name="expressNo"></param>
        /// <returns></returns>
        public bool deliverProds(string salesNo, string expressNo)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = "update salesslip set status=2,deliveryDate=now(),column2=now(),expressNo=@expressNo where salesNo=@salesNo";
                    conn.Execute(updateSalesSlipSQL, new { salesNo = salesNo, expressNo = expressNo }).ToString();
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 管理员修改邮费
        /// </summary>
        /// <param name="salesNo"></param>
        /// <param name="adminChangeAmount"></param>
        /// <returns></returns>
        public bool adminChangePostage(string salesNo, decimal adminChangePostage)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = "update salesslip set adminChangePostage=@adminChangePostage where salesNo=@salesNo";
                    conn.Execute(updateSalesSlipSQL, new { salesNo = salesNo, adminChangePostage = adminChangePostage }).ToString();
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }


        public bool userUpdateOrderAddress(salesslip paidOrder)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string updateSalesSlipSQL = "update salesslip set receiver=@receiver,province=@province,city=@city,county=@county,detailAddress=@detailAddress,Phone=@Phone where salesId=@salesId and salesNo=@salesNo and userOpenId = @userOpenId";
                    conn.Execute(updateSalesSlipSQL, paidOrder, transaction).ToString();
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        public int dealExpectionOrder(string salesNo, string wechatOrderNo)
        {
            int status = 0;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string queryRes = OrderQuery.Run("", wechatOrderNo); //调用订单查询业务逻辑
                string[] qRes = queryRes.Split(new[] { "<br>" }, StringSplitOptions.None);
                foreach (var q in qRes)
                {
                    if (q.IndexOf("trade_state") >= 0)
                    {
                        string trade_state = q.Split('=')[1];
                        if (trade_state.ToLower() == "success")
                        {
                            string sqlCommandTextUpdateOrder = "update salesslip set status=1 where salesNo=@salesNo";
                            conn.Execute(sqlCommandTextUpdateOrder, new { salesNo = salesNo });
                            status = 1;
                        }
                        else
                        {
                            string sqlCommandTextUpdateOrder = "update salesslip set status=0,paydate=NULL where salesNo=@salesNo";
                            conn.Execute(sqlCommandTextUpdateOrder, new { salesNo = salesNo });
                            status = 0;
                        }
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// 获取用户订单信息
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<UserOrderListInfo> getUserOrderListInfo(string userOpenId, string salesNo = "")
        {
            List<UserOrderListInfo> userOrderList = new List<UserOrderListInfo>();
            List<UserShopcartsInfo> orderProductList = new List<UserShopcartsInfo>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                if (string.IsNullOrEmpty(salesNo))
                {
                    string sqlCommandText = @"SELECT * FROM ganxian.salesslip where status<>4 and userOpenId=@userOpenId and display =1  order by column2 desc, salesid desc";
                    userOrderList = conn.Query<UserOrderListInfo>(sqlCommandText, new { userOpenId = userOpenId }).ToList();
                }
                else
                {
                    string sqlCommandText = @"SELECT * FROM ganxian.salesslip where status<>4 and userOpenId=@userOpenId and display =1 and salesNo=@salesNo  order by column2 desc, salesid desc";
                    userOrderList = conn.Query<UserOrderListInfo>(sqlCommandText, new { userOpenId = userOpenId, salesNo = salesNo }).ToList();
                }
                foreach (var userOrder in userOrderList)
                {
                    //0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款 6 已失效 7 已退货
                    #region 异常数据情况
                    if (userOrder.status == 5)
                    {
                        userOrder.status = dealExpectionOrder(userOrder.salesNo, userOrder.wechatOrderNo);
                    }
                    #endregion
                    #region status==0 未付款，已失效情况
                    if (userOrder.status == 0 || userOrder.status == 6)
                    {
                        //未付款订单30分钟后失效
                        if (userOrder.status == 0)
                        {
                            double mins = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["orderExpiredMins"]);
                            if (DateTime.Now.AddMinutes(-mins) > userOrder.createDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=6 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 6;
                            }
                        }

                        decimal totalPrice = 0;//总价，未付款时需要关联产品表获取当前价格
                        string sqlCommandTextStatus0 = @"SELECT a.num ,b.* FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where  b.status=1 and a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus0 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus0, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus0.Any())
                        {
                            resStatus0.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus0)
                            {
                                totalPrice += item.productTotalPrice ?? 0;
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    try
                                    {
                                        if (item2.Name == item.showPic)
                                        {
                                            item.showPic = item2.GetValue(item).ToString();
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus0;
                        userOrder.amount = totalPrice;
                        //userOrder.postage
                    }
                    #endregion
                    #region 已付款处理
                    else if (userOrder.status == 1 || userOrder.status == 2 || userOrder.status == 3 || userOrder.status == 7)
                    {
                        //已发货一周后变为已完成状态
                        if (userOrder.status == 2 && userOrder.deliveryDate != null)
                        {
                            if (DateTime.Now.AddDays(-7) > userOrder.deliveryDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=3 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 3;
                            }
                        }


                        string sqlCommandTextStatus123 = @"SELECT a.num ,b.`productId`,
                                                        b.`productName`,
                                                        b.`specs`,
                                                        a.`originalPrice`,
                                                        a.`discountedPrice`,
                                                        b.`discountedExpiredDate`,
                                                        b.`pic1`,
                                                        b.`pic2`,
                                                        b.`pic3`,
                                                        b.`pic4`,
                                                        b.`showPic`,
                                                        b.`origin`,
                                                        a.`nw`,
                                                        b.`storageCondition`,
                                                        b.`remark`,
                                                        b.`createDate`,
                                                        b.`status`,
                                                        b.`column1`,
                                                        b.`column2` FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus123 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus123, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus123.Any())
                        {
                            resStatus123.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus123)
                            {
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    try
                                    {
                                        if (item2.Name == item.showPic)
                                        {
                                            item.showPic = item2.GetValue(item).ToString();
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus123;
                    }
                    #endregion
                }
            }
            return userOrderList;
        }

        /// <summary>
        /// 获取所有订单信息
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<UserOrderListInfo> getAllOrderListInfo(string salesNo = "")
        {
            List<UserOrderListInfo> userOrderList = new List<UserOrderListInfo>();
            List<UserShopcartsInfo> orderProductList = new List<UserShopcartsInfo>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                if (string.IsNullOrEmpty(salesNo))
                {
                    string sqlCommandText = @"SELECT salesslip.*,users.nickname as orderName,users.phone as orderPhone
                                                FROM ganxian.salesslip 
                                                join ganxian.users on salesslip.userOpenId=users.openid
                                                where salesslip.status<>4 and salesslip.status<>6 order by salesslip.column2 desc, salesslip.salesid desc limit 1000";
                    userOrderList = conn.Query<UserOrderListInfo>(sqlCommandText).ToList();
                }
                else
                {
                    string sqlCommandText = @"SELECT salesslip.*,users.nickname as orderName,users.phone as orderPhone
                                                FROM ganxian.salesslip 
                                                join ganxian.users on salesslip.userOpenId=users.openid
                                                where salesslip.status<>4 and salesslip.status<>6 and salesNo=@salesNo  order by salesslip.column2 desc, salesslip.salesid desc";
                    userOrderList = conn.Query<UserOrderListInfo>(sqlCommandText, new { salesNo = salesNo }).ToList();
                }
                foreach (var userOrder in userOrderList)
                {
                    //0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款 6 已失效
                    #region 异常数据情况
                    if (userOrder.status == 5)
                    {
                        userOrder.status = dealExpectionOrder(userOrder.salesNo, userOrder.wechatOrderNo);
                    }
                    #endregion
                    #region status==0 未付款，已失效情况
                    if (userOrder.status == 0 || userOrder.status == 6)
                    {
                        //未付款订单30分钟后失效
                        if (userOrder.status == 0)
                        {
                            double mins = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["orderExpiredMins"]);
                            if (DateTime.Now.AddMinutes(-mins) > userOrder.createDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=6 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 6;
                            }
                        }

                        decimal totalPrice = 0;//总价，未付款时需要关联产品表获取当前价格
                        string sqlCommandTextStatus0 = @"SELECT a.num ,b.* FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where  b.status=1 and a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus0 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus0, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus0.Any())
                        {
                            resStatus0.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus0)
                            {
                                totalPrice += item.productTotalPrice ?? 0;
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    if (item2.Name == item.showPic)
                                    {
                                        item.showPic = item2.GetValue(item).ToString();
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus0;
                        userOrder.amount = totalPrice;
                        //userOrder.postage
                    }
                    #endregion
                    #region 显示订单处理
                    else if (userOrder.status == 0 || userOrder.status == 1 || userOrder.status == 2 || userOrder.status == 3 || userOrder.status == 7)
                    {
                        //已发货一周后变为已完成状态
                        if (userOrder.status == 2 && userOrder.deliveryDate != null)
                        {
                            if (DateTime.Now.AddDays(-7) > userOrder.deliveryDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=3 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 3;
                            }
                        }


                        string sqlCommandTextStatus123 = @"SELECT a.num ,b.`productId`,
                                                        b.`productName`,
                                                        b.`specs`,
                                                        a.`originalPrice`,
                                                        a.`discountedPrice`,
                                                        b.`discountedExpiredDate`,
                                                        b.`pic1`,
                                                        b.`pic2`,
                                                        b.`pic3`,
                                                        b.`pic4`,
                                                        b.`showPic`,
                                                        b.`origin`,
                                                        a.`nw`,
                                                        b.`storageCondition`,
                                                        b.`remark`,
                                                        b.`createDate`,
                                                        b.`status`,
                                                        b.`column1`,
                                                        b.`column2` FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus123 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus123, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus123.Any())
                        {
                            resStatus123.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus123)
                            {
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    if (item2.Name == item.showPic)
                                    {
                                        try
                                        {
                                            item.showPic = item2.GetValue(item).ToString();
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus123;
                    }
                    #endregion
                }
            }
            return userOrderList;
        }

        /// <summary>
        /// 根据条件获取订单信息
        /// </summary>
        /// <returns></returns>
        public List<UserOrderListInfo> getOrderListInfoByCondition(salesslip slipCondition)
        {
            List<UserOrderListInfo> userOrderList = new List<UserOrderListInfo>();
            List<UserShopcartsInfo> orderProductList = new List<UserShopcartsInfo>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCondition = string.Empty;
                if (slipCondition.status != null)
                {
                    sqlCondition += " and  salesslip.status='" + slipCondition.status + "'";
                }
                if (!string.IsNullOrEmpty(slipCondition.salesNo))
                {
                    sqlCondition += " and  salesslip.salesNo like '%" + slipCondition.salesNo + "%'";
                }
                if (!string.IsNullOrEmpty(slipCondition.receiver))
                {
                    sqlCondition += " and  salesslip.receiver like '%" + slipCondition.receiver + "%'";
                }
                if (!string.IsNullOrEmpty(slipCondition.expressNo))
                {
                    sqlCondition += " and  salesslip.expressNo like '%" + slipCondition.expressNo + "%'";
                }
                if (slipCondition.createDate != null && slipCondition.createDate != default(DateTime))
                {
                    sqlCondition += " and  datediff(salesslip.createDate,'" + slipCondition.createDate + "')= 0";
                }

                string sqlCommandText = @"SELECT salesslip.*,users.nickname as orderName,users.phone as orderPhone
                                                FROM ganxian.salesslip 
                                                join ganxian.users on salesslip.userOpenId=users.openid
                                                where salesslip.status<>4 and salesslip.status<>6 " + sqlCondition + " order by salesslip.column2 desc, salesslip.salesid desc limit 1000";
                userOrderList = conn.Query<UserOrderListInfo>(sqlCommandText).ToList();

                foreach (var userOrder in userOrderList)
                {
                    //0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款 6 已失效
                    #region 异常数据情况
                    if (userOrder.status == 5)
                    {
                        userOrder.status = dealExpectionOrder(userOrder.salesNo, userOrder.wechatOrderNo);
                    }
                    #endregion
                    #region status==0 未付款，已失效情况
                    if (userOrder.status == 0 || userOrder.status == 6)
                    {
                        //未付款订单30分钟后失效
                        if (userOrder.status == 0)
                        {
                            double mins = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["orderExpiredMins"]);
                            if (DateTime.Now.AddMinutes(-mins) > userOrder.createDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=6 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 6;
                            }
                        }

                        decimal totalPrice = 0;//总价，未付款时需要关联产品表获取当前价格
                        string sqlCommandTextStatus0 = @"SELECT a.num ,b.* FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where  b.status=1 and a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus0 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus0, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus0.Any())
                        {
                            resStatus0.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus0)
                            {
                                totalPrice += item.productTotalPrice ?? 0;
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    if (item2.Name == item.showPic)
                                    {
                                        item.showPic = item2.GetValue(item).ToString();
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus0;
                        userOrder.amount = totalPrice;
                        //userOrder.postage
                    }
                    #endregion
                    #region 显示订单处理
                    else if (userOrder.status == 0 || userOrder.status == 1 || userOrder.status == 2 || userOrder.status == 3 || userOrder.status == 7)
                    {
                        //已发货一周后变为已完成状态
                        if (userOrder.status == 2 && userOrder.deliveryDate != null)
                        {
                            if (DateTime.Now.AddDays(-7) > userOrder.deliveryDate)
                            {
                                string sqlCommandTextUpdateOrder = "update salesslip set status=3 ,column2=now() where salesId=@salesId";
                                conn.Execute(sqlCommandTextUpdateOrder, new { salesId = userOrder.salesId });
                                userOrder.status = 3;
                            }
                        }


                        string sqlCommandTextStatus123 = @"SELECT a.num ,b.`productId`,
                                                        b.`productName`,
                                                        b.`specs`,
                                                        a.`originalPrice`,
                                                        a.`discountedPrice`,
                                                        b.`discountedExpiredDate`,
                                                        b.`pic1`,
                                                        b.`pic2`,
                                                        b.`pic3`,
                                                        b.`pic4`,
                                                        b.`showPic`,
                                                        b.`origin`,
                                                        a.`nw`,
                                                        b.`storageCondition`,
                                                        b.`remark`,
                                                        b.`createDate`,
                                                        b.`status`,
                                                        b.`column1`,
                                                        b.`column2` FROM ganxian.sales2products a 
                                            inner join products b on a.productid=b.productid
                                            where a.salesId=@salesId
                                            order by a.createDate desc";
                        var resStatus123 = conn.Query<UserShopcartsInfo>(sqlCommandTextStatus123, new { salesId = userOrder.salesId }).ToList();
                        if (resStatus123.Any())
                        {
                            resStatus123.ForEach(x => x.productTotalPrice = x.num * x.discountedPrice);
                            foreach (var item in resStatus123)
                            {
                                System.Reflection.PropertyInfo[] pro = item.GetType().GetProperties();
                                foreach (System.Reflection.PropertyInfo item2 in pro)
                                {
                                    if (item2.Name == item.showPic)
                                    {
                                        try
                                        {
                                            item.showPic = item2.GetValue(item).ToString();
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        userOrder.Order2ProductsList = resStatus123;
                    }
                    #endregion
                }
            }
            return userOrderList;
        }

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool delOrder(string userOpenId, string orderId)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = "update salesslip set status=4,display=0,column2=now() where salesNo=@salesNo and userOpenId = @userOpenId";
                    conn.Execute(updateSalesSlipSQL, new { salesNo = orderId, userOpenId = userOpenId }).ToString();
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 更新产品下单时的产品日志记录id，用户算出付款时所对应成本
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool updateOrder2ProductLogField(int productId, int id)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string updateSalesSlipSQL = @"update sales2products  a
                                                inner join(select max(logID) as logId, productId from productchangelog  where productId = @productId) as b on a.productId = b.productId
                                                set a.productLogId = b.logId
                                                where a.productId = @productId and a.id=@id;";
                    conn.Query(updateSalesSlipSQL, new { productId = productId, id = id });
                    res = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return res;
        }
    }
}
