using CommonLib;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {

            test();

            ViewBag.PageType = "HomePage";
            return View();
        }

        private void test()
        {

            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT * FROM Products WHERE productId=@ID";
                Products user = conn.Query<Products>(sqlCommandText, new { ID = 2 }).FirstOrDefault();
            }
        }

        public class Products
        {
            public int productId { get; set; }//自增主键
            public string productName { get; set; }
            public string specs { get; set; }

        }
    }
}