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
        /// 获取所有产品列表
        /// </summary>
        /// <returns></returns>
        public List<products> getProductList()
        {
            List<products> products = new List<products>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM Products";
                products = conn.Query<products>(sqlCommandText).ToList();
            }
            if (products.Any())
            {
                products.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.remark)) p.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />");
                });
                foreach (var item in products)
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
            return products;
        }

        public string insertProduct(products prod, string tabs)
        {
            string prodId = string.Empty;
            //bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string insertProdSQL = "insert into products(productName,specs,originalPrice,discountedPrice,pic1,pic2,pic3,pic4,showPic,origin,nw,storageCondition,remark,createDate,status,cost) values(@productName,@specs,@originalPrice,@discountedPrice,@pic1,@pic2,@pic3,@pic4,@showPic,@origin,@nw,@storageCondition,@remark,now(),1,@cost);select @@IDENTITY";

                    prodId = conn.ExecuteScalar(insertProdSQL, prod, transaction).ToString();
                    if (!string.IsNullOrEmpty(tabs))
                    {
                        string[] t = tabs.Split(',');
                        for (int i = 0; i < t.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(t[i]))
                            {
                                string sqlCommandText = @"insert into products2tabs(productId,tabId,isShow,createDate,status) values(@prodId,@tabId,0,now(),1) ";
                                conn.Execute(sqlCommandText, new { prodId = prodId, tabId = t[i] }, transaction);
                            }
                        }
                    }
                    //提交事务
                    transaction.Commit();
                    //res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return prodId;
        }

        public bool insertProductChangeLog(productchangelog changelog)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {

                try
                {
                    string insertProdSQL = @"INSERT INTO `ganxian`.`productchangelog`
                                            (`Operator`,
                                            `LogTime`,
                                            `productId`,
                                            `productName`,
                                            `specs`,
                                            `originalPrice`,
                                            `discountedPrice`,
                                            `discountedExpiredDate`,
                                            `cost`,
                                            `pic1`,
                                            `pic2`,
                                            `pic3`,
                                            `pic4`,
                                            `showPic`,
                                            `origin`,
                                            `nw`,
                                            `storageCondition`,
                                            `remark`,
                                            `createDate`,
                                            `status`,
                                            `column1`,
                                            `column2`)
                                            VALUES
                                            (@Operator,
                                            @LogTime,
                                            @productId,
                                            @productName,
                                            @specs,
                                            @originalPrice,
                                            @discountedPrice,
                                            @discountedExpiredDate,
                                            @cost,
                                            @pic1,
                                            @pic2,
                                            @pic3,
                                            @pic4,
                                            @showPic,
                                            @origin,
                                            @nw,
                                            @storageCondition,
                                            @remark,
                                            @createDate,
                                            @status,
                                            @column1,
                                            @column2);";

                    conn.Execute(insertProdSQL, changelog);

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
        /// 根据ID获取产品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public products getProductById(int productId, bool needReplace = true)
        {
            products products = new products();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM Products WHERE productId=@ID";
                products = conn.Query<products>(sqlCommandText, new { ID = productId }).FirstOrDefault();
            }
            if (needReplace && products != null && !string.IsNullOrEmpty(products.remark))
            {
                products.remark = products.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />");
            }
            return products;
        }

        public List<products2tabs> getProduct2Tabs(int productId)
        {
            List<products2tabs> prod2Tabs = new List<products2tabs>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM ganxian.products2tabs WHERE productId=@productId";
                prod2Tabs = conn.Query<products2tabs>(sqlCommandText, new { productId = productId }).ToList();
            }

            return prod2Tabs;
        }

        public bool updateProductById(products product, string tabs = null)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    string sqlCommandText = @"update Products set cost=@cost, productName=@productName,specs=@specs,originalPrice=@originalPrice,discountedPrice=@discountedPrice,discountedExpiredDate=@discountedExpiredDate,pic1=@pic1,pic2=@pic2,pic3=@pic3,pic4=@pic4,showPic=@showPic,origin=@origin,nw=@nw,storageCondition=@storageCondition,remark=@remark,status=@status,column1=@column1,column2=@column2 WHERE productId=@productId";
                    conn.Execute(sqlCommandText, product, transaction);

                    string sql = @"update products2tabs set status = 0 where productId=@prodId; ";
                    conn.Execute(sql, new { prodId = product.productId }, transaction);

                    if (!string.IsNullOrEmpty(tabs))
                    {
                        string[] t = tabs.Split(',');
                        for (int i = 0; i < t.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(t[i]))
                            {
                                string sql3 = @"select 1 from products2tabs where productId=@prodId and tabId=@tabId";
                                var r = conn.Query(sql3, new { prodId = product.productId, tabId = t[i] }).FirstOrDefault();

                                if (r == null)
                                {
                                    string sql4 = @"insert into products2tabs(productId,tabId,isShow,createDate,status) values(@prodId,@tabId,0,now(),1) ";
                                    conn.Execute(sql4, new { prodId = product.productId, tabId = t[i] }, transaction);
                                }
                                else
                                {
                                    string sql5 = @"update products2tabs set status = 1 where productId=@prodId and tabId=@tabId; ";
                                    conn.Execute(sql5, new { prodId = product.productId, tabId = t[i] }, transaction);
                                }
                            }
                        }
                    }
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return res;
        }

        /// <summary>
        /// 获取所有产品信息及销售量
        /// </summary>
        /// <returns></returns>
        public List<ProductsAndSalesNum> getAllProductsAndSalesNum()
        {
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"select IFNULL(b.num,0) soldNum,a.* ,d.typeName from products a 
                                            left join products2tabs c on c.productId=a.productId and c.status=1
                                            left join tablist d on c.tabId=d.tabId
                                            left join (select sum(num) as num ,productid from sales2products group by productid) as b on a.productid=b.productid
                                            where a.status=1 ";
                List<ProductsAndSalesNum> productsAndSalesNum = conn.Query<ProductsAndSalesNum>(sqlCommandText, new { }).ToList();
                if (productsAndSalesNum.Any())
                {
                    productsAndSalesNum.ForEach(p =>
                    {
                        if (!string.IsNullOrEmpty(p.remark)) p.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />");
                    });

                    foreach (var item in productsAndSalesNum)
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

                return productsAndSalesNum;
            }
        }

        /// <summary>
        /// 产品列表页面，只过滤出有产品的产品类别
        /// </summary>
        /// <returns></returns>
        public List<tablist> getAllTabList()
        {
            List<tablist> tabList = new List<tablist>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                tabList = conn.Query<tablist>(@"SELECT a.* FROM ganxian.tablist a
                                                                where a.isShow=1 and a.status=1 and exists(select 1 from products2tabs b where status=1 and b.tabId=a.tabId) order by a.sort,a.createDate", new { }).ToList();
            }
            return tabList;
        }

        public List<tablist> getAllTab()
        {
            List<tablist> tabList = new List<tablist>();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                tabList = conn.Query<tablist>(@"SELECT a.* FROM ganxian.tablist a where a.status=1  order by a.tabId", new { }).ToList();
            }
            return tabList;
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
                            where a.isShow=1 and a.status=1 and b.status=1 and b.isShow=1 and c.status=1 order by a.sort,b.sort,b.createDate";
                //products2TabList = conn.Query<products2Tab, products, products2Tab>(sqlCommandText, (products2Tab, products) => products2Tab.Products = products;)

                //return products2TabList;

                List<tablist> tabList = conn.Query<tablist>(@"SELECT a.* FROM ganxian.tablist a
                                                                where a.isShow=1 and a.status=1 and exists(select 1 from products2tabs b where b.status=1 and b.isShow=1 and b.tabId=a.tabId) order by a.sort,a.createDate", new { }).ToList();
                tabList.ForEach(x =>
                {
                    products2Tab prod2Tab = new products2Tab();
                    prod2Tab.Tablist = x;
                    prod2Tab.Products = conn.Query<products>(@"SELECT c.* FROM ganxian.tablist a
                            inner join products2tabs b on a.tabId=b.tabId
                            inner join products c on b.productId=c.productId 
                            where a.tabId=@tabId and a.isShow=1 and a.status=1 and b.status=1 and b.isShow=1 and c.status=1 order by a.sort,b.sort,b.createDate desc", new { tabId = x.tabId }).ToList();

                    foreach (var item in prod2Tab.Products)
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
