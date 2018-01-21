using CommonLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GanXian.Model;
using GanXian.BLL;
using WechatService.Biz;
using Domain.Models;

namespace Domain.Controllers
{
    public class ProductsController : BaseController
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Products
        public ActionResult Index(int id, string code)
        {
            #region 用户信息部分
            string userOpenId = string.Empty;
            Tuple<string, string> result = base.getUserOpenId(code);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                userOpenId = result.Item1;
            }
            else if (!string.IsNullOrEmpty(result.Item2))
            {
                return Redirect(result.Item2);
            }
            ViewBag.userOpenId = userOpenId;
            ViewBag.userShopcartNum = ShopCartBiz.CreateNew().getUserCartsNum(userOpenId).ToString();//获取用户购物车数量
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
            ViewBag.PageName = "商品详情";
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
                productsAndSalesNumList = productsAndSalesNumList.Where(w => (!string.IsNullOrEmpty(w.remark) && w.remark.Contains(searchString))
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

        [HttpPost]
        public JsonResult AddShopcart(string prodId, int num)
        {
            string res = string.Empty;
            string userOpenId = base.getUserOpenIdFromCookie();
            if (string.IsNullOrEmpty(userOpenId))
            {
                res = "false";//这里可以扩展，可以换成枚举，目前功能不需要
            }
            else
            {
                try
                {
                    ShopCartBiz shopcartBiz = ShopCartBiz.CreateNew();
                    shoppingcart userShopcart = shopcartBiz.checkProdExistInCarts(userOpenId, prodId);
                    if (userShopcart != null)
                    {
                        int addNum = userShopcart.num + num;
                        shopcartBiz.UpdateProdInCarts(userOpenId, prodId, addNum);
                    }
                    else
                    {
                        shopcartBiz.AddProdInCarts(userOpenId, prodId, num);
                    }
                    res = shopcartBiz.getUserCartsNum(userOpenId).ToString();
                }
                catch (Exception e)
                {
                    _Apilog.WriteLog("添加购物车异常: " + e.Message);
                    res = "false";
                }
            }
            return Json(res);
        }

        public ActionResult Shopcart(string code)
        {
            #region 用户信息部分
            string userOpenId = string.Empty;
            Tuple<string, string> result = base.getUserOpenId(code);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                userOpenId = result.Item1;
            }
            else if (!string.IsNullOrEmpty(result.Item2))
            {
                return Redirect(result.Item2);
            }
            ViewBag.userOpenId = userOpenId;
            #endregion

            List<UserShopcartsInfo> userShopcartsInfoList = null;
            if (!string.IsNullOrEmpty(userOpenId)) userShopcartsInfoList = ShopCartBiz.CreateNew().getUserShopcartsInfo(userOpenId);

            ViewBag.PageName = "购物车";
            return View(userShopcartsInfoList);
        }

        [HttpPost]
        public JsonResult DelShopcartById(string productId, string userOpenId)
        {
            string res = "fail";
            if (string.IsNullOrEmpty(userOpenId))
            {
                userOpenId = base.getUserOpenIdFromCookie();
            }
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    res = "success";
                    ShopCartBiz.CreateNew().delUserShopcartsByProductId(userOpenId, productId);
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("ProductsController/DelShopcartById 异常： " + e.Message);
                }
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult UpdateShopcartById(string num, string productId, string userOpenId)
        {
            string res = "fail";
            if (string.IsNullOrEmpty(userOpenId))
            {
                userOpenId = base.getUserOpenIdFromCookie();
            }
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    res = "success";
                    ShopCartBiz.CreateNew().updateUserShopcartsByProductId(userOpenId, productId, num);
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("ProductsController/updateUserShopcartsByProductId 异常： " + e.Message);
                }
            }
            return Json(res);
        }

        /// <summary>
        /// 结算页面
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Checkout(string orderId, string code)
        {
            #region 用户信息部分
            string userOpenId = string.Empty;
            Tuple<string, string> result = base.getUserOpenId(code);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                userOpenId = result.Item1;
            }
            else if (!string.IsNullOrEmpty(result.Item2))
            {
                return Redirect(result.Item2);
            }
            ViewBag.userOpenId = userOpenId;
            #endregion

            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(userOpenId))
            {
                return RedirectToAction("OrderList", "Order");
            }

            CheckOutModels checkOutModels = new CheckOutModels();
            useraddress userRes = new useraddress();
            salesslip userSalesSlip = new salesslip();

            userSalesSlip = OrderBiz.CreateNew().getCheckOutInfo(orderId, userOpenId);
            if (userSalesSlip == null)//查不到销售单
            {
                return RedirectToAction("OrderList", "Order");//查不到销售单,跳转至订单列表页面
            }
            else if (userSalesSlip.status == 0)//0未付款 1已付款 2待发货 3 待收货 4 已完成
            {
                #region 用户收货地址部分
                if (!string.IsNullOrEmpty(userSalesSlip.province) && !string.IsNullOrEmpty(userSalesSlip.receiver)) //先看该订单用户是否已经设置收货地址，没有设置过则读取默认地址，还没有则为空
                {
                    userRes.receiver = userSalesSlip.receiver;
                    userRes.Phone = userSalesSlip.Phone;
                    userRes.province = userSalesSlip.province;
                    userRes.city = userSalesSlip.city;
                    userRes.county = userSalesSlip.county;
                    userRes.detailAddress = userSalesSlip.detailAddress;
                }
                else
                {
                    var userAddressList = UserBiz.CreateNew().getUserAddressList(userOpenId);
                    if (userAddressList.Any())
                    {
                        userRes = userAddressList.Find(x => x.SetAsDefault == "1");
                    }
                    if (userRes == null)
                    {
                        userRes = new useraddress();
                        userRes.receiver = userRes.Phone = userRes.province = userRes.city = userRes.county = userRes.detailAddress = "";
                    }
                }
                #endregion
            }
            else//订单状态不为 未付款，需要跳转到对应页面
            {
                return RedirectToAction("OrderList", "Order");//to do..
            }
            _Apilog.WriteLog(orderId);
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "结算";
            checkOutModels.UserAddress = userRes;
            return View(checkOutModels);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateOrder(string prodId, string num)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    string salesNo = Guid.NewGuid().ToString();
                    OrderBiz.CreateNew().createOrder(prodId, num, userOpenId, salesNo);
                    res = salesNo;
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("ProductsController/CreateOrder 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("ProductsController/CreateOrder 用户userOpenId 为空： " + prodId + " " + num);
            }
            return Json(res);
        }

        /// <summary>
        /// 从购物车中创建订单
        /// </summary>
        /// <param name="prodIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateOrderFromShopcart(string prodIds)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    string salesNo = Guid.NewGuid().ToString();
                    OrderBiz.CreateNew().createOrderFromShopcart(prodIds, userOpenId, salesNo);
                    res = salesNo;
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("ProductsController/CreateOrderFromShopcart 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("ProductsController/CreateOrderFromShopcart 用户userOpenId 为空： " + prodIds);
            }
            return Json(res);
        }

    }
}