using CommonLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GanXian.Model;
using GanXian.BLL;

namespace Domain.Controllers
{
    public class ProductsController : Controller
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Products
        public ActionResult Index(int id, string code)
        {
            #region 用户信息部分
            string userOpenId = string.Empty;

            userOpenId = CookieHelper.GetCookieValue("userOpenId");
            if (string.IsNullOrEmpty(userOpenId))
            {
                if (string.IsNullOrEmpty(code))
                {
                    //请求微信接口获取code
                }
                else
                {
                    //请求微信接口获取openid
                }
                if (string.IsNullOrEmpty(userOpenId))//理论上不应该存在这种情况
                {
                    _Apilog.WriteLog("userOpenId 为空,缓存为空，code不会空，code: " + code);
                }
                else
                {
                    CookieHelper.SetCookie("userOpenId", userOpenId);
                }
            }
            else//稳定后可去除日志
            {
                _Apilog.WriteLog("cookie 不为空,用户OpenId: " + userOpenId);
            }
            ViewBag.userOpenId = userOpenId;
            #endregion


            #region 产品信息部分
            products products = new products();
            var resCache = CacheHelper.GetCache("product_" + id.ToString());
            if (resCache != null) products = (products)resCache;
            else {
                products = ProductsBiz.CreateNew().getProductById(id);
                if (products != null)
                {
                    var start = DateTime.Now;
                    var expiredDate = start.AddDays(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("product_" + id.ToString(), products, ts);
                }
            }
            return View(products);
            #endregion
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

        public ActionResult Category(string sortOrder, string searchString)
        {
            List<ProductsAndSalesNum> productsAndSalesNumList;
            var resCache = CacheHelper.GetCache("productsAndSalesNumList");
            if (resCache != null) productsAndSalesNumList = (List<ProductsAndSalesNum>)resCache;
            else {
                productsAndSalesNumList = ProductsBiz.CreateNew().getAllProductsAndSalesNum();
                if (productsAndSalesNumList != null)
                {
                    var start = DateTime.Now;
                    var expiredDate = start.AddHours(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("productsAndSalesNumList", productsAndSalesNumList, ts);
                }
            }
            ViewBag.SortMethod = "defaultSort";
            ViewBag.ReleaseDateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.SalesSortParm = sortOrder == "sales" ? "sales_desc" : "sales";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "defaultSort" : sortOrder;
            ViewBag.SortClass = sortOrder.IndexOf("desc") > 0 ? "icon_sort_down" : "icon_sort_up";

            if (!string.IsNullOrEmpty(searchString))
            {
                productsAndSalesNumList = productsAndSalesNumList.Where(w => w.remark.Contains(searchString)
                                                                        || w.productName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "date":
                    productsAndSalesNumList = productsAndSalesNumList.OrderBy(s => s.createDate).ToList();
                    ViewBag.SortMethod = "date";
                    break;
                case "date_desc":
                    productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.createDate).ToList();
                    ViewBag.SortMethod = "date";
                    break;
                case "sales":
                    productsAndSalesNumList = productsAndSalesNumList.OrderBy(s => s.soldNum).ToList();
                    ViewBag.SortMethod = "sales";
                    break;
                case "sales_desc":
                    productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.soldNum).ToList();
                    ViewBag.SortMethod = "sales";
                    break;
                case "price":
                    productsAndSalesNumList = productsAndSalesNumList.OrderBy(s => s.discountedPrice).ToList();
                    ViewBag.SortMethod = "price";
                    break;
                case "price_desc":
                    productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.discountedPrice).ToList();
                    ViewBag.SortMethod = "price";
                    break;
                case "defaultSort":
                    productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.productId).ToList();
                    break;
                default:
                    productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.productId).ToList();
                    break;
            }

            ViewBag.PageType = "ProductsPage";
            ViewBag.PageName = "产品列表";
            return View(productsAndSalesNumList);
        }

        public ActionResult Shopcart()
        {
            ViewBag.PageName = "购物车";
            return View();
        }
    }
}