﻿using System.Web.Mvc;

namespace Domain.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.PageType = "ProductsPage";
            return View();
        }
    }
}