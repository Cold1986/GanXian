using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using WechatService.Entity;

namespace WechatService.Biz
{
    /// <summary>
    /// 微信静默授权业务
    /// </summary>
    public class AuthorizeBiz
    {
        private static string wechatAppid = System.Configuration.ConfigurationSettings.AppSettings["wechat:appid"];
        private static string wechatAppSecret = System.Configuration.ConfigurationSettings.AppSettings["wechat:AppSecret"];

        /// <summary>
        /// 获取微信静默授权链接
        /// </summary>
        /// <param name="redirect_uri">授权后重定向的回调链接地址</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节</param>
        /// <returns></returns>
        public static string getSnsapi_Base_Link(string redirect_uri, string state = null)
        {
            string redirectURL = HttpUtility.UrlEncode(redirect_uri).ToLower();
            string snsapi_Base_Link = "https://open.weixin.qq.com/connect/oauth2/authorize?response_type=code&scope=snsapi_base&appid=" + wechatAppid + "&redirect_uri=" + redirectURL;
            if (!string.IsNullOrEmpty(state))
            {
                snsapi_Base_Link = snsapi_Base_Link + "&state=" + state;
            }
            snsapi_Base_Link += "#wechat_redirect";
            return snsapi_Base_Link;
        }

        public static Authorize getUserInfo(string code)
        {
            try
            {
                Authorize aut = new Authorize();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string link = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + wechatAppid + "&secret=" + wechatAppSecret + "&code=" + code + "&grant_type=authorization_code";
                var res = RequestHelper.SendRequest(link, "");
                aut = serializer.Deserialize<Authorize>(res);//todo 
                return aut;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 用户授权操作
        /// </summary>
        /// <param name="redirect_uri"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string getSnsapi_userinfo_Link(string redirect_uri, string state = null)
        {
            string redirectURL = HttpUtility.UrlEncode(redirect_uri).ToLower();
            string snsapi_Base_Link = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + wechatAppid + "&redirect_uri=" + redirectURL;
            snsapi_Base_Link = snsapi_Base_Link + "&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect";
            return snsapi_Base_Link;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static Userinfo getUserinfo(string access_token, string openid)
        {
            Userinfo aut = new Userinfo();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string link = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            var res = RequestHelper.SendRequest(link, "");
            aut = serializer.Deserialize<Userinfo>(res);
            return aut;
        }


    }

}
