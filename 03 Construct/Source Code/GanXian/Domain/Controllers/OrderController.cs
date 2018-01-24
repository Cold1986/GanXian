using CommonLib;
using GanXian.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class OrderController : BaseController
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderList(string code)
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
            List<GanXian.Model.UserOrderListInfo> userOrderList = new List<GanXian.Model.UserOrderListInfo>();
            try
            {
                userOrderList = OrderBiz.CreateNew().getUserOrderListInfo(userOpenId);
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("OrderController 的OrderList 异常：" + e.Message);
            }
            ViewBag.PageName = "我的订单";
            return View(userOrderList);
        }
    }
}