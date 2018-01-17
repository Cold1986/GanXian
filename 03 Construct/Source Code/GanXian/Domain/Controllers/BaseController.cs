using CommonLib;
using GanXian.BLL;
using GanXian.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WechatService.Biz;
using WechatService.Entity;


namespace Domain.Controllers
{
    public class BaseController : Controller
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Base
        private static string isWechatTest = System.Configuration.ConfigurationSettings.AppSettings["isWechatTest"];

        /// <summary>
        /// 微信静默授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Tuple<string, string> getUserOpenId(string code)
        {
            string userOpenId = string.Empty;
            if (isWechatTest == "false")
            {
                userOpenId = CookieHelper.GetCookieValue("userOpenId");
                //缓存为空
                if (string.IsNullOrEmpty(userOpenId))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(code))
                        {
                            string url = Request.Url.ToString();
                            if (url.IndexOf("www.") < 0)
                            {
                                url = url.Replace("http://", "http://www.");
                            }

                            //请求微信接口获取code
                            string snsapi_Base_Link = AuthorizeBiz.getSnsapi_Base_Link(url);
                            return new Tuple<string, string>(null, snsapi_Base_Link);
                        }
                        else
                        {
                            Authorize userInfo = AuthorizeBiz.getUserInfo(code);
                            if (userInfo != null) userOpenId = userInfo.openid;
                            //请求微信接口获取openid
                        }
                    }
                    catch (Exception e)
                    {
                        _Apilog.WriteLog("获取微信公众号静默授权 异常： " + e.Message);
                    }

                    if (!string.IsNullOrEmpty(userOpenId))
                    {
                        CookieHelper.SetCookie("userOpenId", userOpenId);
                    }
                }
                return new Tuple<string, string>(userOpenId, null);
            }
            else
            {
                return new Tuple<string, string>("test", null);
            }
        }

        /// <summary>
        /// 返回关注授权地址
        /// </summary>
        /// <returns></returns>
        private string getSnsapi_userinfo_Link()
        {
            string url = Request.Url.ToString();
            if (url.IndexOf("www.") < 0)
            {
                url = url.Replace("http://", "http://www.");
            }

            //请求微信接口获取code
            string Snsapi_userinfo_Link = AuthorizeBiz.getSnsapi_userinfo_Link(url);
            return Snsapi_userinfo_Link;
        }

        private Authorize getAuthorizeInfo(string code)
        {
            try
            {
                return AuthorizeBiz.getUserInfo(code);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Userinfo getWechatUserInfo(Authorize authorizeInfo)
        {
            var res = AuthorizeBiz.getUserinfo(authorizeInfo.access_token, authorizeInfo.openid);
            //_Apilog.WriteLog(JsonConvert.SerializeObject(res));
            return res;
        }

        /// <summary>
        /// 用户提供授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Tuple<string, users> getUserInfoByAuthorize(string code)
        {
            try
            {
                users user = new users();
                if (isWechatTest == "false")
                {
                    Userinfo wechatInfo = new Userinfo();
                    Authorize authorizeInfo = new Authorize();
                    string userOpenId = CookieHelper.GetCookieValue("userOpenId");
                    //缓存不为空，说明可能先点过购物车或者产品
                    if (!string.IsNullOrEmpty(userOpenId))
                    {
                        _Apilog.WriteLog("1.userOpenId 不为空:" + userOpenId);
                        var resCache = CacheHelper.GetCache(userOpenId);
                        if (resCache != null)
                        {
                            user = (users)resCache;
                            _Apilog.WriteLog("2.userOpenId 不为空,缓存不为空:" + userOpenId);
                            return new Tuple<string, users>(string.Empty, user);
                        }


                        UserBiz userBiz = UserBiz.CreateNew();
                        //判断openid是否已存在或者需要更新
                        user = userBiz.getUserInfoByOpenId(userOpenId);
                        if (user != null)
                        {
                            _Apilog.WriteLog("3.userOpenId 不为空,表不为空:" + userOpenId);
                            DateTime dtCreate = user.updateDate ?? user.createDate;
                            DateTime dtNow = DateTime.Now;
                            if (dtCreate.AddDays(7) < dtNow || string.IsNullOrEmpty(user.nickname))
                            {
                                //第一步
                                if (string.IsNullOrEmpty(code))
                                {
                                    _Apilog.WriteLog("4.userOpenId 不为空,表不为空，第一步:" + userOpenId);
                                    return new Tuple<string, users>(getSnsapi_userinfo_Link(), user);
                                }
                                else
                                {
                                    _Apilog.WriteLog("5.userOpenId 不为空,表不为空，第二步:" + userOpenId);
                                    //授权第二步，获取openid
                                    authorizeInfo = getAuthorizeInfo(code);
                                    //授权3
                                    wechatInfo = getWechatUserInfo(authorizeInfo);
                                    //update 
                                    user = setUserinfoFromWechat(wechatInfo);
                                    user.updateDate = DateTime.Now;
                                    userBiz.updateUserWeChatInfo(user);
                                }
                            }
                        }
                        else//需要插入
                        {
                            //第一步
                            if (string.IsNullOrEmpty(code))
                            {
                                _Apilog.WriteLog("6.userOpenId 不为空,表为空，第一步:" + userOpenId);
                                return new Tuple<string, users>(getSnsapi_userinfo_Link(), user);
                            }
                            else
                            {
                                _Apilog.WriteLog("7.userOpenId 不为空,表为空，第二步:" + userOpenId);
                                _Apilog.WriteLog("7.1.userOpenId 不为空,表为空，第二步:code" + code);
                                //授权第二步，获取openid
                                authorizeInfo = getAuthorizeInfo(code);
                                _Apilog.WriteLog("7.2.userOpenId 不为空,表为空，第二步:access_token" + authorizeInfo.access_token + " openid: " + authorizeInfo.openid);

                                //授权3
                                wechatInfo = getWechatUserInfo(authorizeInfo);
                                _Apilog.WriteLog("7.3.userOpenId 不为空,表为空，第二步:wechatInfo" + wechatInfo.nickname);
                                //insert 
                                user = setUserinfoFromWechat(wechatInfo);
                                _Apilog.WriteLog("7.4.userOpenId 不为空,表为空，第二步:user" + user.nickname);
                                user.createDate = DateTime.Now;
                                user.updateDate = user.createDate;
                                userBiz.insertUserWeChatInfo(user);
                                _Apilog.WriteLog("7.5.userOpenId 不为空,表为空，第二步:code" + code);
                            }
                        }
                    }
                    //缓存为空
                    else
                    {
                        //授权第一步，返回地址
                        if (string.IsNullOrEmpty(code))
                        {
                            _Apilog.WriteLog("8.userOpenId 为空，第一步:");
                            return new Tuple<string, users>(getSnsapi_userinfo_Link(), user);
                        }
                        else
                        {
                            //授权第二步，获取openid
                            _Apilog.WriteLog("9.userOpenId 为空，第二步:" + code);
                            authorizeInfo = getAuthorizeInfo(code);
                            _Apilog.WriteLog("10.userOpenId 为空，第二.2步:");
                            var resCache = CacheHelper.GetCache(authorizeInfo.openid);
                            if (resCache != null)
                            {
                                _Apilog.WriteLog("11.userOpenId 为空,缓存不为空，第二步:" + code);
                                user = (users)resCache;
                                return new Tuple<string, users>(string.Empty, user);
                            }
                            UserBiz userBiz = UserBiz.CreateNew();
                            //判断openid是否已存在或者需要更新
                            user = userBiz.getUserInfoByOpenId(authorizeInfo.openid);
                            _Apilog.WriteLog("12.userOpenId 为空，第二.3步:");
                            if (user != null)
                            {
                                DateTime dtCreate = user.updateDate ?? user.createDate;
                                DateTime dtNow = DateTime.Now;
                                if (dtCreate.AddDays(7) < dtNow || string.IsNullOrEmpty(user.nickname))
                                {
                                    //授权3
                                    _Apilog.WriteLog("13.userOpenId 为空,缓存为空，表不为空，第三步:");
                                    wechatInfo = getWechatUserInfo(authorizeInfo);
                                    //update 
                                    user = setUserinfoFromWechat(wechatInfo);
                                    user.updateDate = DateTime.Now;
                                    userBiz.updateUserWeChatInfo(user);
                                }
                            }
                            else
                            {
                                _Apilog.WriteLog("14.userOpenId 为空,缓存为空，表为空，第三步:");
                                wechatInfo = getWechatUserInfo(authorizeInfo);
                                //insert 
                                user = setUserinfoFromWechat(wechatInfo);
                                user.createDate = DateTime.Now;
                                user.updateDate = user.createDate;
                                userBiz.insertUserWeChatInfo(user);
                            }
                        }
                    }
                    if (user != null)
                    {
                        var start = DateTime.Now;
                        var expiredDate = start.AddDays(1);
                        TimeSpan ts = expiredDate - start;
                        CacheHelper.SetCache("userInfo" + user.openid.ToString(), user, ts);
                        if (!string.IsNullOrEmpty(user.openid))
                        {
                            CookieHelper.SetCookie("userOpenId", user.openid);
                        }
                    }
                }
                else
                {
                    user.nickname = "test";
                    ViewBag.headImg = "../images/noavatar.png";//缺省图片
                }
                return new Tuple<string, users>(string.Empty, user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string getUserOpenIdFromCookie()
        {
            if (isWechatTest == "false")
            {
                return CookieHelper.GetCookieValue("userOpenId");
            }
            else
            {
                return "test";
            }
        }

        private users setUserinfoFromWechat(Userinfo userWechatInfo)
        {
            users myUser = new users();
            myUser.openid = userWechatInfo.openid;
            myUser.nickname = userWechatInfo.nickname;
            myUser.sex = userWechatInfo.sex;
            myUser.province = userWechatInfo.province;
            myUser.city = userWechatInfo.city;
            myUser.country = userWechatInfo.country;
            myUser.headimgurl = userWechatInfo.headimgurl;
            //myUser.privilege = userWechatInfo.privilege;
            myUser.unionid = userWechatInfo.unionid;
            return myUser;
        }

        private Userinfo getUserWechatInfo(Authorize authorInfo)
        {
            return AuthorizeBiz.getUserinfo(authorInfo.access_token, authorInfo.openid);
        }
    }
}