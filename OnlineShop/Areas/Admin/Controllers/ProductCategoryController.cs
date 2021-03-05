using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                var culture = Session[CommonConstants.CurrentCulture];
                model.CreatedDate = DateTime.Now;
                new ProductCategoryDao().Create(model);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDao();
            var product = dao.GetById(id);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var result = dao.Update(model);
                if (result)
                {
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhật Thể Loại Sản Phẩm K Thành Công");
                }
            }
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new ProductCategoryDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            // trả ra đối tg có thuộc tính status
            return Json(new
            {
                status = result
            });
        }
    }
}