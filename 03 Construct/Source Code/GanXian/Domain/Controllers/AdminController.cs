using CommonLib;
using Domain.Attribute;
using Domain.Models;
using GanXian.BLL;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
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
            if (UserBiz.CreateNew().isAdminUser(model.Account, model.Password))
            {
                HttpContext.Session["LoginedUser"] = "adminUser";
                HttpContext.Session["LoginedUserName"] = model.Account;
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
            AdminProductViewModels adminproduct = new AdminProductViewModels();
            ProductsBiz Pbiz = ProductsBiz.CreateNew();
            var ProductList = Pbiz.getProductList();
            //根据名称或备注搜索产品信息
            if (!string.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(w => (!string.IsNullOrEmpty(w.remark) && w.remark.Contains(searchString))
                                                                        || w.productName.Contains(searchString)).ToList();
            }
            //根据状态搜索产品
            if (searchStatus != 99)
            {
                ProductList = ProductList.Where(w => w.status.Equals(searchStatus)).ToList();
            }
            adminproduct.productList = ProductList;
            //页面产品标签绑定
            var tablist = Pbiz.getAllTab();
            List<CheckBoxListInfo> infos = new List<CheckBoxListInfo>();
            foreach (tablist item in tablist)
            {
                infos.Add(new CheckBoxListInfo(item.tabId.ToString(), item.typeName, false));
            }
            //ViewData["CheckBoxListOfTablist"] = infos;
            adminproduct.checkBoxList = infos;
            return View(adminproduct);
        }


        public ActionResult userList()
        {
            List<users> lusers = UserBiz.CreateNew().getUserList();
            return View(lusers);
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
                        CacheHelper.RemoveAllCache();
                    }

                }
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("更新产品信息异常: " + e.Message);
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult GetProdInfo(int productId)
        {
            products products = new products();
            var resCache = CacheHelper.GetCache("product_" + productId.ToString());
            if (resCache != null) products = (products)resCache;
            else {
                products = ProductsBiz.CreateNew().getProductById(productId, false);
                if (products != null)
                {
                    var start = DateTime.Now;
                    var expiredDate = start.AddDays(1);
                    TimeSpan ts = expiredDate - start;
                    CacheHelper.SetCache("product_" + productId.ToString(), products, ts);
                }
            }
            return Json(products);
        }

        [HttpPost]
        public JsonResult GetProd2tabs(int productId)
        {
            string tabs = string.Empty;
            var products2Tab = ProductsBiz.CreateNew().getProduct2Tabs(productId);
            if (products2Tab != null)
            {
                foreach (var item in products2Tab)
                {
                    if (item.status == 1) tabs += item.tabId + ",";
                }
            }

            return Json(tabs);

        }

        [HttpPost]
        public JsonResult CreateOrUploadProds(FormCollection collection)
        {
            string r_productId = collection["r_productId"];//编号
            string r_product = collection["r_product"];//产品名称
            string r_size = collection["r_size"];//规格
            string r_price = collection["r_price"];//原价
            string r_oPrice = collection["r_oPrice"];//折后价
            string r_cost = collection["r_cost"];//成本价
            string r_APN = collection["r_APN"];//产地
            string r_weight = collection["r_weight"];//净重
            string r_condition = collection["r_condition"];//存放条件
            string r_remark = collection["r_remark"];//产品描述
            string main_pic = "pic1";// Request.Params["main_pic"];//主图图片
            string tablist = Request.Params["tablist"];

            products prod = new products();
            ProductsBiz productBiz = ProductsBiz.CreateNew();

            if (!string.IsNullOrEmpty(r_productId))
            {
                prod = productBiz.getProductById(Convert.ToInt32(r_productId), false);
            }
            prod.productName = r_product.Trim();
            prod.specs = r_size.Trim();
            prod.originalPrice = Convert.ToDecimal(r_price.Trim());
            prod.discountedPrice = Convert.ToDecimal(r_oPrice.Trim());
            if (!string.IsNullOrEmpty(r_cost)) prod.cost = Convert.ToDecimal(r_cost.Trim());
            prod.origin = r_APN.Trim();
            prod.nw = r_weight.Trim();
            prod.storageCondition = r_condition.Trim();
            prod.remark = r_remark.Trim();
            prod.showPic = main_pic.Trim();


            HttpPostedFileBase file1 = Request.Files["file1"];
            string res1 = savefiles(file1, r_product);
            if (!string.IsNullOrEmpty(res1))
            {
                this.saveThumbnail(file1, res1);
                prod.pic1 = "/img/" + res1;
            }
            HttpPostedFileBase file2 = Request.Files["file2"];
            string res2 = savefiles(file2, r_product);
            if (!string.IsNullOrEmpty(res2))
            {
                prod.pic2 = "/img/" + res2;
            }
            HttpPostedFileBase file3 = Request.Files["file3"];
            string res3 = savefiles(file3, r_product);
            if (!string.IsNullOrEmpty(res3))
            {
                prod.pic3 = "/img/" + res3;
            }
            HttpPostedFileBase file4 = Request.Files["file4"];
            string res4 = savefiles(file4, r_product);
            if (!string.IsNullOrEmpty(res4))
            {
                prod.pic4 = "/img/" + res4;
            }

            HttpPostedFileBase file5 = Request.Files["file5"];
            string res5 = savefiles(file5, r_product);
            if (!string.IsNullOrEmpty(res5))
            {
                prod.pic5 = "/img/" + res5;
            }
            HttpPostedFileBase filePicDetail1 = Request.Files["picDetail1"];
            string picDetail1 = savefiles(filePicDetail1, r_product);
            if (!string.IsNullOrEmpty(picDetail1))
            {
                prod.picDetail1 = "/img/" + picDetail1;
            }
            HttpPostedFileBase filePicDetail2 = Request.Files["picDetail2"];
            string picDetail2 = savefiles(filePicDetail2, r_product);
            if (!string.IsNullOrEmpty(picDetail2))
            {
                prod.picDetail2 = "/img/" + picDetail2;
            }
            HttpPostedFileBase filePicDetail3 = Request.Files["picDetail3"];
            string picDetail3 = savefiles(filePicDetail3, r_product);
            if (!string.IsNullOrEmpty(picDetail3))
            {
                prod.picDetail3 = "/img/" + picDetail3;
            }
            HttpPostedFileBase filePicDetail4 = Request.Files["picDetail4"];
            string picDetail4 = savefiles(filePicDetail4, r_product);
            if (!string.IsNullOrEmpty(picDetail4))
            {
                prod.picDetail4 = "/img/" + picDetail4;
            }

            try
            {
                //新增
                if (string.IsNullOrEmpty(r_productId))
                {
                    r_productId = productBiz.insertProduct(prod, tablist);
                }
                else
                {
                    productBiz.updateProductById(prod, tablist);
                }
                var prodInfo = productBiz.getProductById(Convert.ToInt32(r_productId), false);

                productchangelog changelog = new productchangelog();
                if (HttpContext.Session["LoginedUserName"] != null) changelog.Operator = HttpContext.Session["LoginedUserName"].ToString();
                changelog.LogTime = System.DateTime.Now;
                changelog.productId = prodInfo.productId;
                changelog.productName = prodInfo.productName;
                changelog.specs = prodInfo.specs;
                changelog.originalPrice = prodInfo.originalPrice;
                changelog.discountedPrice = prodInfo.discountedPrice;
                changelog.discountedExpiredDate = prodInfo.discountedExpiredDate;
                changelog.cost = prodInfo.cost;
                changelog.pic1 = prodInfo.pic1;
                changelog.pic2 = prodInfo.pic2;
                changelog.pic3 = prodInfo.pic3;
                changelog.pic4 = prodInfo.pic4;
                changelog.pic5 = prodInfo.pic5;
                changelog.picDetail1 = prodInfo.picDetail1;
                changelog.picDetail2 = prodInfo.picDetail2;
                changelog.picDetail3 = prodInfo.picDetail3;
                changelog.picDetail4 = prodInfo.picDetail4;
                changelog.showPic = prodInfo.showPic;
                changelog.origin = prodInfo.origin;
                changelog.nw = prodInfo.nw;
                changelog.storageCondition = prodInfo.storageCondition;
                changelog.remark = prodInfo.remark;
                changelog.createDate = prodInfo.createDate;
                changelog.status = prodInfo.status;
                changelog.column1 = prodInfo.column1;
                changelog.column2 = prodInfo.column2;

                productBiz.insertProductChangeLog(changelog);
                CacheHelper.RemoveAllCache();
                return new JsonResult { Data = new { _code = 100, _msg = "上传成功" }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("新增或更新产品信息异常: " + e.Message);
                return new JsonResult { Data = new { _code = 200, _msg = "上传失败" }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }


        }

        private string savefiles(HttpPostedFileBase file, string prodName)
        {
            string fileNewName = string.Empty;
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(file.FileName);
                fileNewName = prodName + "_" + System.DateTime.Now.Ticks.ToString() + fileExt; // Guid.NewGuid() + fileExt;
                string fileSaveDir = Server.MapPath("~/img");
                if (!Directory.Exists(fileSaveDir))
                {
                    Directory.CreateDirectory(fileSaveDir);
                }
                file.SaveAs(Path.Combine(fileSaveDir, fileNewName));
                Thread.Sleep(50);
            }
            return fileNewName;
        }

        /// <summary>
        /// 保存缩略图，用于分享朋友圈
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileNewName"></param>
        private void saveThumbnail(HttpPostedFileBase file, string fileNewName)
        {
            //生成缩略图保存  
            System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
            int width = image.Width;
            int height = image.Height;

            int max = 100;
            if (width > max || height > max)
            {
                try
                {
                    System.Drawing.Image newPic; //定义新位图对象  
                    if (width > height)
                    {
                        newPic = new Bitmap(image, max, height * max / width); //缩放  
                    }
                    else
                    {
                        newPic = new Bitmap(image, width * max / height, max); //缩放  
                    }
                    string fileSaveDir = Server.MapPath("~/Thumbnail");
                    if (!Directory.Exists(fileSaveDir))
                    {
                        Directory.CreateDirectory(fileSaveDir);
                    }
                    newPic.Save(Path.Combine(fileSaveDir, fileNewName));
                    Thread.Sleep(50);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 订单管理页面
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult order(string orderNo, string createDate, string receiver, string expressNo, string status)
        {
            List<GanXian.Model.UserOrderListInfo> userOrderList = new List<GanXian.Model.UserOrderListInfo>();
            try
            {
                salesslip slipCondition = new salesslip();

                if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
                {
                    slipCondition.status = Convert.ToInt32(status);
                }
                if (!string.IsNullOrEmpty(orderNo))
                {
                    slipCondition.salesNo = orderNo.ToLower();
                }
                if (!string.IsNullOrEmpty(receiver))
                {
                    slipCondition.receiver = receiver.ToLower();
                }
                if (!string.IsNullOrEmpty(expressNo))
                {
                    slipCondition.expressNo = expressNo.ToLower();
                }
                if (!string.IsNullOrEmpty(createDate))
                {
                    slipCondition.createDate = Convert.ToDateTime(createDate);
                }

                userOrderList = OrderBiz.CreateNew().getOrderListInfoByCondition(slipCondition);
            }
            catch (Exception e)
            {
                _Apilog.WriteLog("AdminController order 异常：" + e.Message);
            }
            ViewBag.status = status;
            ViewBag.orderNo = orderNo;
            ViewBag.createDate = createDate;
            ViewBag.receiver = receiver;
            ViewBag.expressNo = expressNo;

            ViewBag.ProjectUrl = System.Configuration.ConfigurationSettings.AppSettings["projectUrl"];
            return View(userOrderList);
        }

        [HttpPost]
        public JsonResult ChangePrice(string type, string salesNo, decimal price)
        {
            string res = "success";
            try
            {
                if (type == "changeAmount")
                {
                    OrderBiz.CreateNew().adminChangeAmount(salesNo, price);
                }
                else if (type == "changeExpress")
                {
                    OrderBiz.CreateNew().adminChangePostage(salesNo, price);
                }
            }
            catch (Exception e)
            {
                res = "fail";
                _Apilog.WriteLog("AdminController ChangePrice 异常：" + e.Message);
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult ChangeStatusReturn(string salesNo)
        {
            string res = "success";
            try
            {
                OrderBiz.CreateNew().adminReturnProd(salesNo);
            }
            catch (Exception e)
            {
                res = "fail";
                _Apilog.WriteLog("AdminController ChangeStatus 异常：" + e.Message);
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult deliverProds(string salesNo, string expressNo)
        {
            string res = "success";
            try
            {
                OrderBiz.CreateNew().deliverProds(salesNo, expressNo);
            }
            catch (Exception e)
            {
                res = "fail";
                _Apilog.WriteLog("AdminController deliverProds 异常：" + e.Message);
            }
            return Json(res);
        }
    }
}