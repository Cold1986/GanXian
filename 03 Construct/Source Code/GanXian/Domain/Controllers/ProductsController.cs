using CommonLib;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GanXian.Model;

namespace Domain.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(int id)
        {
            products products = new products();
            var resCache = CacheHelper.GetCache("product_" + id.ToString());
            if (resCache != null) products = (products)resCache;
            else {

                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"SELECT * FROM Products WHERE productId=@ID";
                    products = conn.Query<products>(sqlCommandText, new { ID = id }).FirstOrDefault();
                }
                if (products != null)
                {
                    products.remark = products.remark.Replace("\n", "<br /><br />").Replace("\r", "<br /><br />");
                    var start = DateTime.Now;
                    var expiredDate = start.AddDays(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("product_" + id.ToString(), products, ts);
                }
            }
            return View(products);
        }

        /// <summary>
        /// 删除指定产品缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveProductCacheById(int id)
        {
            CacheHelper.RemoveCacheByKey("product_" + id.ToString());
            return Json("success");
        }

        public ActionResult Category()
        {
            ViewBag.PageType = "ProductsPage";
            ViewBag.PageName = "产品列表";
            return View();
        }

        public ActionResult Shopcart()
        {
            ViewBag.PageName = "购物车";
            return View();
        }
    }
}