using CommonLib;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GanXian.BLL
{
    public class ProductsBiz
    {
        public static ProductsBiz CreateNew()
        {
            return new ProductsBiz();
        }

        /// <summary>
        /// 根据ID获取产品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public products getProductById(int productId)
        {
            products products = new products();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM Products WHERE productId=@ID";
                products = conn.Query<products>(sqlCommandText, new { ID = productId }).FirstOrDefault();
            }
            if (products != null)
            {
                products.remark = products.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />");
            }
            return products;
        }

        /// <summary>
        /// 获取所有产品信息及销售量
        /// </summary>
        /// <returns></returns>
        public List<ProductsAndSalesNum> getAllProductsAndSalesNum()
        {
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"select IFNULL(b.num,0) soldNum,a.* from products a 
                                            left join (select sum(num) as num ,productid from sales2products group by productid) as b on a.productid=b.productid
                                            where a.status=1";
                List<ProductsAndSalesNum> productsAndSalesNum = conn.Query<ProductsAndSalesNum>(sqlCommandText, new { }).ToList();
                if (productsAndSalesNum.Any())
                {
                    productsAndSalesNum.ForEach(p => p.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />"));
                    foreach (var item in productsAndSalesNum)
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
                return productsAndSalesNum;
            }
        }

        /// <summary>
        /// 获得首页显示产品内容
        /// 该方法不是最优，to do Dapper 可以一次性一对多返回结果
        /// </summary>
        /// <returns></returns>
        public List<products2Tab> getHomePageShowProducts2Tab()
        {
            List<products2Tab> products2TabList = new List<products2Tab>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT a.tabId,c.productId FROM ganxian.tablist a
                            inner join products2tabs b on a.tabId=b.tabId
                            inner join products c on b.productId=c.productId 
                            where a.isShow=1 and a.status=1 and b.status=1 and c.status=1 order by a.sort,b.sort,b.createDate";
                //products2TabList = conn.Query<products2Tab, products, products2Tab>(sqlCommandText, (products2Tab, products) => products2Tab.Products = products;)

                //return products2TabList;

                List<tablist> tabList = conn.Query<tablist>(@"SELECT a.* FROM ganxian.tablist a
                                                                where a.isShow=1 and a.status=1 and exists(select 1 from products2tabs b where status=1 and b.tabId=a.tabId) order by a.sort,a.createDate", new { }).ToList();
                tabList.ForEach(x =>
                {
                    products2Tab prod2Tab = new products2Tab();
                    prod2Tab.Tablist = x;
                    prod2Tab.Products = conn.Query<products>(@"SELECT c.* FROM ganxian.tablist a
                            inner join products2tabs b on a.tabId=b.tabId
                            inner join products c on b.productId=c.productId 
                            where a.tabId=@tabId and a.isShow=1 and a.status=1 and b.status=1 and c.status=1 order by a.sort,b.sort,b.createDate", new { tabId = x.tabId }).ToList();

                    foreach (var item in prod2Tab.Products)
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
                    products2TabList.Add(prod2Tab);
                });

                return products2TabList;
            }
        }
    }
}
