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
        public LogHelper _Orderlog = new LogHelper("OrderLog");// 考虑到以后日志查询的可查询性，记录内容   salesNo|message|msgType
        private double orderExpiredMins = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["orderExpiredMins"]);
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
                else
                {
                    return RedirectToAction("Category");
                }
            }
            ViewBag.PageName = "商品详情";
            ViewBag.ProjectUrl = base.projectURL;
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

        public ActionResult Category(string sortOrder, string searchString, string typeName)
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
                    //CacheHelper.SetCache("productsAndSalesNumList", productsAndSalesNumList, ts);
                }
            }


            ViewBag.TypeName = string.IsNullOrEmpty(typeName) ? "all" : typeName;
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

            //过滤类型
            if (!string.IsNullOrEmpty(typeName) && typeName.ToLower() != "all")
            {
                productsAndSalesNumList = productsAndSalesNumList.Where(w => (!string.IsNullOrEmpty(w.typeName) && w.typeName.Equals(typeName))).ToList();
            }
            else
            {
                productsAndSalesNumList.ForEach(w => w.typeName = "");
                productsAndSalesNumList.Distinct().ToList();
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
            CategoryViewModel viewModel = new CategoryViewModel();
            viewModel.productsAndSalesNum = productsAndSalesNumList;
            viewModel.tabList = ProductsBiz.CreateNew().getAllTabList();
            ViewBag.ProjectUrl = base.projectURL;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddShopcart(string prodId, int num, string prodPrice)
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
                        shopcartBiz.UpdateProdInCarts(userOpenId, prodId, addNum, prodPrice);
                    }
                    else
                    {
                        shopcartBiz.AddProdInCarts(userOpenId, prodId, num, prodPrice);
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
            ViewBag.ProjectUrl = base.projectURL;
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

            #region 绑定手机部分
            //用户未点击跳过注册 并且电话为空
            users res = new users();
            var resCache = CacheHelper.GetCache("userInfo" + userOpenId);
            if (resCache != null)
            {
                res = (users)resCache;
            }
            else
            {
                res = UserBiz.CreateNew().getUserInfoByOpenId(userOpenId);
                if (res == null)
                {
                    return RedirectToAction("Register", "User", new { needRegister = "1", fromUrl = Request.RawUrl });//跳转到注册页面，且必须注册 
                }
                else {
                    var start = DateTime.Now;
                    var expiredDate = start.AddDays(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("userInfo" + res.openid.ToString(), res, ts);
                    if (!string.IsNullOrEmpty(res.openid))
                    {
                        CookieHelper.SetCookie("userOpenId", res.openid);
                    }
                }
            }

            if (string.IsNullOrEmpty(res.phone))
            {
                return RedirectToAction("Register", "User", new { needRegister = "1", fromUrl = Request.RawUrl });//跳转到注册页面，且必须注册 
            }
            #endregion

            CheckOutModels checkOutModels = new CheckOutModels();
            useraddress userRes = new useraddress();
            List<UserShopcartsInfo> userUnpaidOrderInfo = new List<UserShopcartsInfo>();
            salesslip userSalesSlip = new salesslip();

            decimal productsPrice = 0;
            decimal postage = 0;
            decimal SFJZF = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["sf:JZH"]);//顺丰江浙沪快递费
            decimal SFNonJZF = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["sf:NonJZH"]);//顺丰非江浙沪快递费

            string wechatBody = string.Empty;
            userSalesSlip = OrderBiz.CreateNew().getCheckOutInfo(orderId, userOpenId);
            if (userSalesSlip == null)//查不到销售单
            {
                return RedirectToAction("OrderList", "Order");//查不到销售单,跳转至订单列表页面
            }
            else if (userSalesSlip.status == 5)
            {
                userSalesSlip.status = OrderBiz.CreateNew().dealExpectionOrder(userSalesSlip.salesNo, userSalesSlip.wechatOrderNo);
            }
            if (userSalesSlip.status == 0)//0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款 6 已过期
            {
                if (DateTime.Now.AddMinutes(-orderExpiredMins) > userSalesSlip.createDate)//订单已失效，为了减少数据库操作，这边做跳转；
                {
                    return RedirectToAction("OrderList", "Order", new { status = "all" });
                }

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
                if (!string.IsNullOrEmpty(userRes.province))
                {
                    if (userRes.province.IndexOf("上海") >= 0
                        || userRes.province.IndexOf("江苏") >= 0
                        || userRes.province.IndexOf("浙江") >= 0
                        || userRes.province.IndexOf("安徽") >= 0)
                    {
                        postage = SFJZF;
                    }
                    else
                    {
                        postage = SFNonJZF;
                    }
                }

                #endregion

                #region 订单产品部分
                OrderBiz orderBiz = OrderBiz.CreateNew();
                userUnpaidOrderInfo = orderBiz.getUnpaidOrderInfo(userSalesSlip.salesId);
                checkOutModels.UserOrderInfo = userUnpaidOrderInfo;

                foreach (var i in userUnpaidOrderInfo)
                {
                    try
                    {
                        orderBiz.updateOrder2ProductLogField(i.productId, i.id);
                    }
                    catch (Exception ex)
                    {
                        _Apilog.WriteLog("ProductsController/Checkout更新订单logid异常" + ex.Message);
                    }
                    wechatBody += i.productName + "*" + i.num.ToString() + ";";
                    productsPrice += i.productTotalPrice ?? 0;
                }
                #endregion
            }
            else//订单状态不为 未付款，需要跳转到对应页面
            {
                return RedirectToAction("OrderList", "Order", new { status = userSalesSlip.status });
            }

            decimal factPrice = 0;
            decimal factPostage = 0;
            if (Convert.IsDBNull(userSalesSlip.adminChangeAmount) || userSalesSlip.adminChangeAmount == null)
            {
                factPrice = productsPrice;
            }
            else
            {
                factPrice = userSalesSlip.adminChangeAmount ?? 1;
            }

            if (Convert.IsDBNull(userSalesSlip.adminChangePostage) || userSalesSlip.adminChangePostage == null)
            {
                factPostage = postage;
            }
            else
            {
                factPostage = userSalesSlip.adminChangePostage ?? 1;
            }

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = userOpenId;
            jsApiPay.total_fee = base.isPayTest == "false" ? decimal.ToInt32(factPrice * 100 + factPostage * 100) : 1;//测试环境默认支付1分

            //JSAPI支付预处理
            try
            {
                string wechatOrderId = Guid.NewGuid().ToString("N").ToLower();
                OrderBiz.CreateNew().updateWechatOrderId(orderId, userOpenId, wechatOrderId);

                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(wechatBody, wechatOrderId);//orderId
                ViewBag.wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数    
                _Apilog.WriteLog("ProductsController/Checkout 用户userOpenId: " + userOpenId + " wxJsApiParam : " + ViewBag.wxJsApiParam);
                //Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                //在页面上显示订单信息
            }
            catch (Exception ex)
            {
                _Apilog.WriteLog("ProductsController/Checkout下单失败" + ex.Message);
            }



            //_Apilog.WriteLog(orderId);
            ViewBag.productsPrice = productsPrice;
            ViewBag.postage = postage;
            ViewBag.totalCost = factPrice + factPostage;//productsPrice + postage;
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "结算";
            ViewBag.ProjectUrl = base.projectURL;
            ViewBag.adminChangeAmount = userSalesSlip.adminChangeAmount;
            ViewBag.adminChangePostage = userSalesSlip.adminChangePostage;
            checkOutModels.UserAddress = userRes;
            return View(checkOutModels);
        }

        [HttpPost]
        public JsonResult PayOrder(string receiver, string rPhone, string province, string city, string county, string detailAddress, string orderId, string toStatus)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            string res = "fail";
            try
            {
                if (!string.IsNullOrEmpty(userOpenId) && !string.IsNullOrEmpty(orderId))
                {
                    _Orderlog.WriteLog(orderId + " | " + "用户: " + userOpenId + " 开始付款 | " + (int)EnumOrderLogType.normal);
                    salesslip userSalesSlip = new salesslip();
                    useraddress userRes = new useraddress();
                    List<UserShopcartsInfo> userUnpaidOrderInfo = new List<UserShopcartsInfo>();
                    decimal productsPrice = 0;
                    decimal postage = 0;
                    decimal SFJZF = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["sf:JZH"]);//顺丰江浙沪快递费
                    decimal SFNonJZF = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["sf:NonJZH"]);//顺丰非江浙沪快递费
                    userSalesSlip = OrderBiz.CreateNew().getCheckOutInfo(orderId, userOpenId);
                    if (userSalesSlip == null)//查不到销售单
                    {
                        res = "订单不存在";//查不到销售单,跳转至订单列表页面
                    }
                    else if (userSalesSlip.status == 0)//0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除 5 预付款 6 已过期
                    {
                        if (DateTime.Now.AddMinutes(-orderExpiredMins) > userSalesSlip.createDate)//订单已失效，为了减少数据库操作，这边做跳转；
                        {
                            res = "订单已过期";//前端页面会做跳转,数据更新部分统一放到了订单列表
                        }
                        else {
                            #region 邮费计算
                            if (!string.IsNullOrEmpty(province))
                            {
                                if (province.IndexOf("上海") >= 0
                                    || province.IndexOf("江苏") >= 0
                                    || province.IndexOf("浙江") >= 0)
                                {
                                    postage = SFJZF;
                                }
                                else
                                {
                                    postage = SFNonJZF;
                                }
                            }
                            #endregion
                            #region 订单产品部分
                            userUnpaidOrderInfo = OrderBiz.CreateNew().getUnpaidOrderInfo(userSalesSlip.salesId);

                            foreach (var i in userUnpaidOrderInfo)
                            {
                                productsPrice += i.productTotalPrice ?? 0;
                            }
                            #endregion

                            #region 更新记录
                            salesslip newOrder = new salesslip();
                            newOrder.salesId = userSalesSlip.salesId;
                            newOrder.salesNo = orderId;
                            newOrder.userOpenId = userOpenId;
                            newOrder.receiver = receiver;
                            newOrder.province = province;
                            newOrder.city = city;
                            newOrder.county = county;
                            newOrder.detailAddress = detailAddress;
                            newOrder.Phone = rPhone;
                            newOrder.amount = productsPrice;
                            newOrder.postage = postage;
                            newOrder.payDate = System.DateTime.Now;
                            newOrder.status = 5;//预付款  实际付款后会再更新成1

                            string remark = Newtonsoft.Json.JsonConvert.SerializeObject(userUnpaidOrderInfo);
                            newOrder.column1 = remark;

                            if (OrderBiz.CreateNew().userPaidOrder(newOrder))
                            {
                                _Orderlog.WriteLog(orderId + " | " + "用户预支付成功,订单: " + Newtonsoft.Json.JsonConvert.SerializeObject(newOrder) + "详情： " + remark + "| " + (int)EnumOrderLogType.normal);
                                res = "success";
                            }
                            else
                            {
                                _Orderlog.WriteLog(orderId + " | " + "用户预支付失败！,订单: " + Newtonsoft.Json.JsonConvert.SerializeObject(newOrder) + "详情： " + remark + "| " + (int)EnumOrderLogType.fail);
                                res = "付款失败";
                            }
                            #endregion
                        }
                    }
                    else if (userSalesSlip.status == 5)
                    {
                        userSalesSlip.status = Convert.ToInt32(toStatus);
                        if (Convert.ToInt32(toStatus) == 0)
                        {
                            userSalesSlip.payDate = null;
                        }

                        string remark = Newtonsoft.Json.JsonConvert.SerializeObject(userSalesSlip);
                        if (OrderBiz.CreateNew().userPaidOrder(userSalesSlip))
                        {
                            _Orderlog.WriteLog(orderId + " | " + "用户订单状态更新成功,订单: " + Newtonsoft.Json.JsonConvert.SerializeObject(userSalesSlip) + "详情： " + remark + "| " + (int)EnumOrderLogType.normal);
                            res = "success";// "用户订单状态更新成功";
                        }
                        else
                        {
                            _Orderlog.WriteLog(orderId + " | " + "用户订单状态更新失败！,订单: " + Newtonsoft.Json.JsonConvert.SerializeObject(userSalesSlip) + "详情： " + remark + "| " + (int)EnumOrderLogType.fail);
                            res = "用户订单状态更新失败";
                        }
                    }
                    else//订单状态不为 未付款，需要跳转到对应页面
                    {
                        res = "订单已支付";
                    }

                }
                else
                {
                    _Apilog.WriteLog("ProductsController/PayOrder 用户userOpenId 或 orderId 为空, 用户userOpenId: " + userOpenId + " orderId: " + orderId);
                    _Orderlog.WriteLog(orderId + " | " + "用户userOpenId 或 orderId 为空, 用户userOpenId: " + userOpenId + " orderId: " + orderId + "| " + (int)EnumOrderLogType.fail);
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("ProductsController/PayOrder 异常: " + userOpenId + " orderId: " + orderId + e.Message);
                _Orderlog.WriteLog(orderId + " | " + "支付异常: " + e.Message + e.Source + "| " + (int)EnumOrderLogType.error);

            }
            return Json(res);
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
                    string salesNo = WxPayApi.GenerateOutTradeNo();// Guid.NewGuid().ToString();
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
                    //订单付款页面用户回退购物车页面，然后再创单时实际没有商品，规避此类垃圾数据
                    string[] prods = prodIds.Split(',');
                    bool prodExist = false;
                    for (int i = 0; i < prods.Length; i++)
                    {
                        shoppingcart checkResult = ShopCartBiz.CreateNew().checkProdExistInCarts(userOpenId, prods[i]);
                        if (checkResult != null)
                        {
                            prodExist = true;
                            break;
                        }
                    }

                    if (prodExist)
                    {
                        string salesNo = WxPayApi.GenerateOutTradeNo();// Guid.NewGuid().ToString();
                        OrderBiz.CreateNew().createOrderFromShopcart(prodIds, userOpenId, salesNo);
                        res = salesNo;
                    }
                    else
                    {
                        res = "noProds";
                    }
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