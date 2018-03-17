using CommonLib;
using Domain.Models;
using GanXian.BLL;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class IndexController : BaseController
    {
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.PageType = "ProductsPage";
            ViewBag.ProjectUrl = base.projectURL;
            List<products2Tab> products2TabList = new List<products2Tab>();
            var resCache = CacheHelper.GetCache("homepageProductList");
            if (resCache != null) products2TabList = (List<products2Tab>) resCache;
            else {
                products2TabList = ProductsBiz.CreateNew().getHomePageShowProducts2Tab();
                if (products2TabList != null)
                {
                    var start = DateTime.Now;
                    var expiredDate = start.AddDays(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("homepageProductList", products2TabList, ts);
                }
            }
            return View(products2TabList);
        }
    }
}