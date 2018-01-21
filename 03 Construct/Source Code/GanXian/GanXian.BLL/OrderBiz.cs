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
    }
}
