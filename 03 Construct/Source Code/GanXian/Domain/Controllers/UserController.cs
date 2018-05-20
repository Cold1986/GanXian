using Aliyun.Acs.Dysmsapi.Model.V20170525;
using AliyunSMSService;
using CommonLib;
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
        public LogHelper _SMSlog = new LogHelper("SMSlog");
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

        public ActionResult AddressSelect(string code, string fromURL)
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
        public JsonResult SelAddress(string receiver, string rPhone, string province, string city, string county, string detailAddress, string orderId)
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
            catch (Exception ex)
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
        public ActionResult Register(string returnUrl, string needRegister, string code)
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
            //加个开关来控制是否需要 用户注册
            ViewBag.PageName = "会员注册";
            ViewBag.NeedRegister = needRegister == "1" ? true : false;
            return View();
        }

        /// <summary>
        /// 我的 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserHome(string code, string ignoreRegister)
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

            #region 绑定手机部分
            //用户未点击跳过注册 并且电话为空
            if (string.IsNullOrEmpty(ignoreRegister) && string.IsNullOrEmpty(res.Item2.phone))
            {
                return RedirectToAction("Register", "User", new { needRegister = "0", fromUrl = Request.RawUrl });//跳转到注册页面，但不是必须注册 
            }
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

        [HttpPost]
        public JsonResult SendSMS(string Phone, string OpenId)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            if (string.IsNullOrEmpty(userOpenId))
            {
                userOpenId = OpenId;
            }
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    String accessKeyId = System.Configuration.ConfigurationSettings.AppSettings["aliSMS:accessKeyId"];
                    String accessKeySecret = System.Configuration.ConfigurationSettings.AppSettings["aliSMS:accessKeySecret"];

                    SendSmsRequest request = new SendSmsRequest();
                    //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                    request.PhoneNumbers = Phone;
                    //必填:短信签名-可在短信控制台中找到
                    request.SignName = System.Configuration.ConfigurationSettings.AppSettings["aliSMS:SignName"];
                    //必填:短信模板-可在短信控制台中找到
                    request.TemplateCode = System.Configuration.ConfigurationSettings.AppSettings["aliSMS:TemplateCode"];
                    //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                    string content = Str(6, true);
                    request.TemplateParam = "{\"code\":\"" + content + "\"}";
                    //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                    request.OutId = System.DateTime.Now.ToLongTimeString();
                    SendSmsResponse response = null;
                    //_SMSlog.WriteLog("发送短信给" + Phone + ": " + Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    _Apilog.WriteLog("发送短信给" + Phone + ": " + Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    response = SendSMSBiz.sendSms(accessKeyId, accessKeySecret, request);
                    if (response.Code.ToLower() != "ok")
                    {
                        res = "fail";
                    }
                    else
                    {
                        TimeSpan ts = DateTime.Now.AddMinutes(5) - DateTime.Now;
                        CacheHelper.SetCache("smsInfo" + userOpenId + Phone, content, ts);
                    }
                    //_SMSlog.WriteLog("发送短信给" + Phone + "结果: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    _Apilog.WriteLog("发送短信给" + Phone + "结果: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("UserController/SendSMS 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("UserController/SendSMS 用户userOpenId 为空： ");
            }
            return Json(res);
        }

        public JsonResult CheckSMS(string Phone, string SMS, string OpenId)
        {
            string userOpenId = base.getUserOpenIdFromCookie();
            if (string.IsNullOrEmpty(userOpenId))
            {
                userOpenId = OpenId;
            }
            string res = "fail";
            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {

                    string strCache = CacheHelper.GetCache("smsInfo" + userOpenId + Phone).ToString();
                    if (string.IsNullOrEmpty(strCache))
                    {
                        res = "验证码已失效";
                    }
                    else
                    {
                        if (SMS != strCache)
                        {
                            res = "验证码不正确";
                        }
                        else
                        {
                            //注册成功
                            //更新数据库
                            //删除缓存
                            users user = new users();
                            user.phone = Phone;
                            user.openid = OpenId;
                            UserBiz uBiz = UserBiz.CreateNew();
                            var resUser=uBiz.getUserInfoByOpenId(user.openid);
                            if (resUser != null)
                            {
                                uBiz.updateUserPhone(user);
                            }
                            else
                            {
                                uBiz.insertUserPhone(user);
                            }
                            CacheHelper.RemoveCacheByKey("userInfo" + userOpenId);
                            res = "success";
                        }
                    }

                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("UserController/SendSMS 异常： " + e.Message);
                }
            }
            else
            {
                _Apilog.WriteLog("UserController/SendSMS 用户userOpenId 为空： ");
            }
            return Json(res);
        }

        /// <summary>
        /// 生成随机字母与数字或字符
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        private string Str(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];

            }
            return result;
        }
    }
}