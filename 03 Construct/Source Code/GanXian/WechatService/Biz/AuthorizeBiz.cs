using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            string redirectURL = HttpUtility.UrlEncode(redirect_uri);
            string snsapi_Base_Link = "https://open.weixin.qq.com/connect/oauth2/authorize?response_type=code&scope=snsapi_base&appid=" + wechatAppid + redirectURL;
            if (!string.IsNullOrEmpty(state))
            {
                snsapi_Base_Link = snsapi_Base_Link + "&state=" + state;
            }
            snsapi_Base_Link += "#wechat_redirect";
            return snsapi_Base_Link;
        }

        public static void getUserInfo(string code)
        {
            //https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140842
            string link = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + wechatAppid + "&secret=" + wechatAppSecret + "&code=" + code + "&grant_type=authorization_code";
        }
    }
}
