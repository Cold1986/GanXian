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
        public ActionResult Index(string orderId, string code)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("OrderList");//查不到销售单,跳转至订单列表页面
            }

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

            GanXian.Model.UserOrderListInfo userOrderList = new GanXian.Model.UserOrderListInfo();
            try
            {
                userOrderList = OrderBiz.CreateNew().getUserOrderListInfo(userOpenId, orderId).FirstOrDefault();
                if (userOrderList == null)
                {
                    return RedirectToAction("OrderList");//查不到销售单,跳转至订单列表页面
                }
                else
                {
                    ViewBag.totalPrice = userOrderList.amount + userOrderList.postage;
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("OrderController Index 异常：" + e.Message);
            }
            ViewBag.PageName = "订单详情";
            return View(userOrderList);
        }

        /// <summary>
        /// 获取用户订单列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="status">0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除取消订单 5 预付款</param>
        /// <returns></returns>
        public ActionResult OrderList(string code, string status)
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
                //to do.. 判断有无预付款状态，需要更新成0或1
                if (status.ToLower() != "all")
                {
                    userOrderList = userOrderList.Where(x => x.status == Convert.ToInt32(status)).ToList();
                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("OrderController 的OrderList 异常：" + e.Message);
            }
            ViewBag.PageName = "我的订单";
            return View(userOrderList);
        }

        [HttpPost]
        public JsonResult deleteOrder(string orderId)
        {
            string res = "fail";
            string userOpenId = base.getUserOpenIdFromCookie();

            if (!string.IsNullOrEmpty(userOpenId))
            {
                try
                {
                    res = "success";
                    OrderBiz.CreateNew().delOrder(userOpenId, orderId);
                }
                catch (Exception e)
                {
                    res = "fail";
                    _Apilog.WriteLog("OrderController/deleteOrder 异常： " + e.Message);
                }
            }
            return Json(res);
        }
    }
}