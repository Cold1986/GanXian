using CommonLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GanXian.Model;

namespace GanXian.BLL
{
    public class ShopCartBiz
    {
        public static ShopCartBiz CreateNew()
        {
            return new ShopCartBiz();
        }

        /// <summary>
        /// 检查购物车中是否有这个商品
        /// </summary>
        /// <param name="userOpenId">用户微信openId</param>
        /// <param name="prodId">商品Id</param>
        public shoppingcart checkProdExistInCarts(string userOpenId, string prodId)
        {
            shoppingcart queryshoppingcart = new shoppingcart();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM ShoppingCart WHERE userOpenId=@userOpenId and productId=@productId and status=1 ";
                queryshoppingcart = conn.Query<shoppingcart>(sqlCommandText, new { userOpenId = userOpenId, productId = prodId }).FirstOrDefault();

            }
            return queryshoppingcart;
        }

        /// <summary>
        /// 添加商品进入购物车
        /// </summary>
        /// <param name="userOpenId">用户微信openId</param>
        /// <param name="prodId">商品Id</param>
        /// <param name="num">添加数量</param> 
        /// <returns>true 成功 false 失败</returns>
        public bool AddProdInCarts(string userOpenId, string prodId, int num)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"insert into ShoppingCart(userOpenId,productId,num,status) values(@userOpenId,@productId,@num,1) ";
                    conn.Query(sqlCommandText, new { userOpenId = userOpenId, productId = prodId, num = num });
                    res = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// 更新产品数量
        /// </summary>
        /// <param name="userOpenId">用户微信OpenId</param>
        /// <param name="prodId">产品Id</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public bool UpdateProdInCarts(string userOpenId, string prodId, int num)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"Update ShoppingCart set num=@num where userOpenId=@userOpenId and productId=@productId and status=1 ";
                    conn.Query(sqlCommandText, new { userOpenId = userOpenId, productId = prodId, num = num });
                    res = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        public int getUserCartsNum(string userOpenId)
        {
            int res = 0;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"SELECT count(cartId) FROM ganxian.shoppingcart where status=1 and userOpenId=@userOpenId ";
                    res = Convert.ToInt32(conn.ExecuteScalar(sqlCommandText, new { userOpenId = userOpenId }));

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// 获取用户购物车信息
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<UserShopcartsInfo> getUserShopcartsInfo(string userOpenId)
        {
            List<UserShopcartsInfo> res;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT a.num,a.userOpenId,b.* FROM ganxian.shoppingcart a 
                                            inner join products b on a.productid=b.productid
                                            where a.status=1 and b.status=1 and a.userOpenId=@userOpenId";
                res = conn.Query<UserShopcartsInfo>(sqlCommandText, new { userOpenId = userOpenId }).ToList();
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
    }
}
