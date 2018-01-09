using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatService.Biz;

namespace Domain.Controllers
{
    public class BaseController : Controller
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Base


        public Tuple<string, string> getUserOpenId(string code)
        {
            string userOpenId = string.Empty;
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
                    _Apilog.WriteLog("ProductsController/Index 异常： " + e.Message);
                }

                if (!string.IsNullOrEmpty(userOpenId))//理论上不应该存在这种情况
                {
                    CookieHelper.SetCookie("userOpenId", userOpenId);
                }
            }
            return new Tuple<string, string>(userOpenId, null);
        }
    }
}