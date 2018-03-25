using CommonLib;
using Domain.Attribute;
using Domain.Models;
using GanXian.BLL;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Domain.Controllers
{
    [Authorization]//如果将此特性加在Controller上，那么访问这个Controller里面的方法都需要验证用户登录状态
    public class AdminController : Controller
    {
        public LogHelper _Apilog = new LogHelper("ApiLog");
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> login(AdminLoginViewModels model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            if (model.Account == "Admin" && model.Password == "Good2018@")
            {
                HttpContext.Session["LoginedUser"] = "adminUser";
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "账号或密码错误");
            }

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("product", "Admin");
        }

        public ActionResult product(string searchString, int searchStatus = 99)
        {
            var ProductList = ProductsBiz.CreateNew().getProductList();
            if (!string.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(w => (!string.IsNullOrEmpty(w.remark) && w.remark.Contains(searchString))
                                                                        || w.productName.Contains(searchString)).ToList();
            }
            if (searchStatus != 99)
            {
                ProductList = ProductList.Where(w => w.status.Equals(searchStatus)).ToList();
            }
            return View(ProductList);
        }

        [HttpPost]
        public JsonResult updateProduct(products product)
        {
            string res = "false";
            try
            {
                if (product != null && product.productId != 0)
                {
                    products DBProduct = ProductsBiz.CreateNew().getProductById(product.productId);
                    if (product.status != null)
                    {
                        DBProduct.status = product.status;
                    }
                    var DBres = ProductsBiz.CreateNew().updateProductById(DBProduct);
                    if (DBres)
                    {
                        res = "success";
                    }

                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("更新产品信息异常: " + e.Message);
            }
            return Json(res);
        }
    }
}