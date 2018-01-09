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
            return View();
        }

        /// <summary>
        /// 用户收货地址页
        /// </summary>
        /// <returns></returns>
        public ActionResult Addresslist()
        {
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "管理收货地址";
            return View();
        }

        /// <summary>
        /// 用户添加新地址
        /// </summary>
        /// <returns></returns>
        public ActionResult Addressnew()
        {
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "添加新地址";
            return View();
        }


        /// <summary>
        /// 用户管理收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult Addressedit()
        {
            ViewBag.FooterType = "custom";
            ViewBag.PageName = "管理收货地址";
            return View();
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

    }
}