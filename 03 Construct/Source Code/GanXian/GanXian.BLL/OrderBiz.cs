using CommonLib;
using Dapper;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    string insertSalesSlipSQL = "insert into SalesSlip(salesNo,userOpenId,createDate,status) values(@salesNo,@userOpenId,now(),0);select @@IDENTITY;";
                    string insertSales2ProductsSQL = "insert into Sales2Products(salesId,productId,num,createDate,status) values(@salesId,@productId,@num,now(),0)";
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
                    string insertSalesSlipSQL = "insert into SalesSlip(salesNo,userOpenId,createDate,status) values(@salesNo,@userOpenId,now(),0);select @@IDENTITY;";
                    string insertSales2ProductsSQL = "";
                    string updateShoppingcartSQL = "";
                    for (int i = 0; i < prods.Length; i++)
                    {
                        insertSales2ProductsSQL += "insert into Sales2Products(salesId,productId,num,createDate,status) select @salesId,productId,num,now(),0 from shoppingcart where status=1 and userOpenId=@userOpenId and productId =" + prods[i] + ";";
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
                //c.status 0未付款 1已付款 2待发货 3 待收货 4 已完成
                string sqlCommandText = @"SELECT a.num ,b.* FROM ganxian.sales2products a 
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
                            if (item2.Name == item.showPic)
                            {
                                item.showPic = item2.GetValue(item).ToString();
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
                    string updateSalesSlipSQL = "update salesslip set receiver=@receiver,province=@province,city=@city,county=@county,detailAddress=@detailAddress,Phone=@Phone,amount=@amount,postage=@postage,payDate=@payDate,status=@status,column1=@column1 where salesId=@salesId and salesNo=@salesNo and userOpenId = @userOpenId";
                    string updateSales2ProductsSQL = "update sales2products a inner join products b on a.productid=b.productid  set a.originalPrice=b.originalPrice,a.discountedPrice=b.discountedPrice,a.nw=b.nw,a.status=1 where a.salesId = @salesId";
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
    }
}
