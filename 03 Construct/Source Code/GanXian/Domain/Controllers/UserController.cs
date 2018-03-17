﻿using CommonLib;
using GanXian.BLL;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class UserController : BaseController
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户收货地址页
        /// </summary>
        /// <returns></returns>
        public ActionResult Addresslist(string code, string fromURL)
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

            ViewBag.fromURL = fromURL;
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "管理收货地址";
            List<useraddress> userAddressList = new List<useraddress>();
            if (!string.IsNullOrEmpty(userOpenId)) userAddressList = UserBiz.CreateNew().getUserAddressList(userOpenId);
            return View(userAddressList);
        }

        public ActionResult AddressSelect(string code,string fromURL)
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

            ViewBag.fromURL = fromURL;
            List<useraddress> userAddressList = new List<useraddress>();
            if (!string.IsNullOrEmpty(userOpenId)) userAddressList = UserBiz.CreateNew().getUserAddressList(userOpenId);

            return View(userAddressList);
        }

        /// <summary>
        /// 用户添加新地址
        /// </summary>
        /// <returns></returns>
        public ActionResult Addressnew(string code, string fromURL)
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

            ViewBag.fromBaseURL = fromURL;
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "添加新地址";
            return View();
        }

        [HttpPost]
        public JsonResult SelAddress(string receiver, string rPhone,string province,string city,string county, string detailAddress, string orderId)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            salesslip userSalesSlip = new salesslip();
            string res = "fail";
            try
            {
                userSalesSlip = OrderBiz.CreateNew().getCheckOutInfo(orderId, userOpenId);

                userSalesSlip.receiver = receiver;
                userSalesSlip.province = province;
                userSalesSlip.city = city;
                userSalesSlip.county = county;
                userSalesSlip.detailAddress = detailAddress;
                userSalesSlip.Phone = rPhone;

                OrderBiz.CreateNew().userUpdateOrderAddress(userSalesSlip);
            }
            catch(Exception ex)
            {
                _Apilog.WriteLog("ProductsController/SelAddress 异常: " + userOpenId + " orderId: " + orderId + ex.Message);
            }
            return Json(res);
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="receiver">收货人</param>
        /// <param name="rPhone">联系电话</param>
        /// <param name="district">所在地区</param>
        /// <param name="detailAddress">详细地址</param>
        /// <param name="setAsDefault">设为默认</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddressAdd(string receiver, string rPhone, string district, string detailAddress, string setAsDefault)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    useraddress user = new useraddress();
                    user.userOpenId = userOpenId;
                    user.receiver = receiver;
                    user.Phone = rPhone;
                    user.detailAddress = detailAddress;
                    user.SetAsDefault = setAsDefault.ToLower() == "true" ? "1" : "0";
                    user.status = 1;
                    user.createDate = DateTime.Now;

                    string[] tempDistrict = district.Split(',');//10,183,1116
                    user.province = tempDistrict[0];
                    if (tempDistrict.Length >= 2) user.city = tempDistrict[1];
                    if (tempDistrict.Length >= 3) user.county = tempDistrict[2];

                    UserBiz.CreateNew().insertUserAddress(user);
                    res = "success";
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("UserController/AddressAdd 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("UserController/AddressAdd 用户userOpenId 为空： ");
            }
            return Json(res);
        }

        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="rPhone"></param>
        /// <param name="district"></param>
        /// <param name="detailAddress"></param>
        /// <param name="setAsDefault"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddressUpdate(string id, string receiver, string rPhone, string district, string detailAddress, string setAsDefault)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    useraddress user = new useraddress();
                    user.Id = Convert.ToInt32(id);
                    user.userOpenId = userOpenId;
                    user.receiver = receiver;
                    user.Phone = rPhone;
                    user.detailAddress = detailAddress;
                    user.SetAsDefault = setAsDefault.ToLower() == "true" ? "1" : "0";
                    user.status = 1;
                    user.updateDate = DateTime.Now;

                    string[] tempDistrict = district.Split(',');//10,183,1116
                    user.province = tempDistrict[0];
                    user.city = "";
                    user.county = "";
                    if (tempDistrict.Length >= 2) user.city = tempDistrict[1];
                    if (tempDistrict.Length >= 3) user.county = tempDistrict[2];

                    UserBiz.CreateNew().updateUserAddress(user);
                    res = "success";
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("UserController/AddressAdd 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("UserController/AddressAdd 用户userOpenId 为空： ");
            }
            return Json(res);
        }


        /// <summary>
        /// 用户管理收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult Addressedit(string code, string fromURL, string addressId)
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

            #region 收货地址部分
            if (string.IsNullOrEmpty(addressId))
            {
                return RedirectToAction("Addresslist", "User", new { fromURL = fromURL });
            }
            else
            {
                useraddress_extension userAddress = UserBiz.CreateNew().getUserAddressById(userOpenId, addressId);
                ViewBag.fromBaseURL = fromURL;
                ViewBag.FooterType = "custom";
                ViewBag.PageName = "管理收货地址";
                return View(userAddress);
            }
            #endregion
        }

        /// <summary>
        /// 用户删除收货地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public ActionResult DeleteAddress(string addressId, string fromURL)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            try
            {
                if (!string.IsNullOrEmpty(userOpenId))
                {
                    UserBiz.CreateNew().deleteUserAddress(userOpenId, addressId);
                }
                else
                {
                    _Apilog.WriteLog("UserController 下的DeleteAddress 异常，userOpenId为空，addressId:" + addressId);
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("UserController 下的DeleteAddress 异常，addressId:" + addressId + "异常: " + e.Message);
            }
            return RedirectToAction("Addresslist", "User", new { fromURL = fromURL });
        }

        /// <summary>
        /// 将地址设为默认地址
        /// </summary>
        /// <param name="addressId">地址id</param>
        /// <param name="fromURL"></param>
        /// <returns></returns>
        public ActionResult SetDefaultAddress(string addressId, string fromURL)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            try
            {
                if (!string.IsNullOrEmpty(userOpenId))
                {
                    UserBiz.CreateNew().setDefaultAddress(userOpenId, addressId);
                }
                else
                {
                    _Apilog.WriteLog("UserController SetDefaultAddress 异常，userOpenId为空，addressId:" + addressId);
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("UserController 下的SetDefaultAddress 异常，addressId:" + addressId + "异常: " + e.Message);
            }
            return RedirectToAction("Addresslist", "User", new { fromURL = fromURL });
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Register(string returnUrl)
        {
            //加个开关来控制是否需要 用户注册

            ViewBag.FooterType = "会员注册";
            ViewBag.PageName = "管理收货地址";
            return View();
        }

        /// <summary>
        /// 我的 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserHome(string code)
        {
            #region 用户关注部分
            try
            {
                _Apilog.WriteLog("begin code: " + code);
                Tuple<string, users> result = base.getUserInfoByAuthorize(code);
                ViewBag.headImg = "../images/noavatar.png";//缺省图片
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    _Apilog.WriteLog("redirectURL: " + result.Item1);
                    return Redirect(result.Item1);
                }
                else if (result.Item2 != null)
                {
                    ViewBag.userName = result.Item2.nickname;
                    ViewBag.headImg = result.Item2.headimgurl;
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("exception:" + e.Message);
            }
            var res = base.getUserInfoByAuthorize(code);
            #endregion


            #region 热销推荐部分
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
            productsAndSalesNumList = productsAndSalesNumList.OrderByDescending(s => s.soldNum).Take(4).ToList();
            ViewBag.ProjectUrl = base.projectURL;
            return View(productsAndSalesNumList);
            #endregion
        }

        public JsonResult GetAllDistrict()
        {
            string JsonRes = string.Empty;
            return Json(JsonRes);
        }

    }
}