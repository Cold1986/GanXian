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
                }
                return productsAndSalesNum;
            }
        }
    }
}
