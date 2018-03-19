using Domain.Attribute;
using Domain.Models;
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
            return RedirectToAction("Index", "Admin");
        }
    }
}