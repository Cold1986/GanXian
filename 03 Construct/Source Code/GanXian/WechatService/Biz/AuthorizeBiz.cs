using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;


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
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string link = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + wechatAppid + "&secret=" + wechatAppSecret + "&code=" + code + "&grant_type=authorization_code";
                var res = RequestHelper.SendRequest(link, "");
                Authorize aut = serializer.Deserialize<Authorize>(res);
                return aut;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
   
    }
    public class Authorize
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
    }
}
