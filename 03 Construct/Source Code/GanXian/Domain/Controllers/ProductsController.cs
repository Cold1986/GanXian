using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Category()
        {
            ViewBag.PageName = "产品列表";
            return View();
        }

        public ActionResult Shopcart()
        {
            ViewBag.PageName = "购物车";
            return View();
        }
    }
}