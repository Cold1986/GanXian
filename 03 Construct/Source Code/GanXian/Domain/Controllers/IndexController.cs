using Domain.Models;
using GanXian.BLL;
using GanXian.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.PageType = "ProductsPage";
            List<products2Tab> products2TabList = products2TabList = ProductsBiz.CreateNew().getHomePageShowProducts2Tab();
            return View(products2TabList);
        }
    }
}