using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class UserController : Controller
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: User
        public ActionResult Index()
        {
            _Apilog.WriteLog("call back test");
            return View();
        }

        public ActionResult callback(string code)
        {
            _Apilog.WriteLog("回掉成功" + code);
            return Redirect("index/index");
            
        }

        public ActionResult GetOpenId2()
        {
            string test = "";

            //http://qydev.weixin.qq.com/wiki/index.php?title=OAuth%E9%AA%8C%E8%AF%81%E6%8E%A5%E5%8F%A3

            //https://open.weixin.qq.com/connect/oauth2/authorize?appid=gh_3c367cf1a722&redirect_uri=&response_type=code&scope=snsapi_base&state=1
            //https://open.weixin.qq.com/connect/oauth2/authorize?appid="+wx_appid+"&redirect_uri="+api.wx_reg+"&response_type=code&scope=snsapi_base,snsapi_userinfo&state=1,0#wechat_redirect
            try
            {
                string callback = "http://www.genoforce.net/ganxian/user/callback";
                callback = System.Web.HttpUtility.UrlEncode(callback);
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx981611f61bfd96c2&redirect_uri=" + callback + "&state=testbycold&response_type=code&scope=snsapi_base&state=1#wechat_redirect";

                //https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx981611f61bfd96c2&redirect_uri=http%3a%2f%2fwww.genoforce.net%2fganxian%2fuser%2fcallback&response_type=code&scope=snsapi_base&state=1#wechat_redirect
                //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx520c15f417810387&redirect_uri=https%3A%2F%2Fchong.qq.com%2Fphp%2Findex.php%3Fd%3D%26c%3DwxAdapter%26m%3DmobileDeal%26showwxpaytitle%3D1%26vb2ctag%3D4_2030_5_1194_60&response_type=code&scope=snsapi_base&state=123#wechat_redirect";
                _Apilog.WriteLog(url);
                return Redirect(url);
                test = RequestHelper.SendRequest(url, "");
                _Apilog.WriteLog(test);
            }
            catch (Exception e)
            {
                test = e.Message;
                _Apilog.WriteLog(e.Message);
            }
            return View();
        }
    }
}