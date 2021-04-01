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
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index(string Search, int page = 1, int pageSize = 2)
        {
            var dao = new CategoryDao();
            ViewBag.Search = Search;
            var model = dao.ListAllPaging(Search, page, pageSize);
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var currentCulture = Session[CommonConstants.CurrentCulture];
                model.Language = currentCulture.ToString();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                model.CreatedDate = DateTime.Now;
                var id = new CategoryDao().Create(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.InsertCategoryFailed);
                }
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var dao = new CategoryDao();
            var category = dao.GetByID(id);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category model)
        {

            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.ModifiedBy = session.UserName;
                model.ModifiedDate = DateTime.Now;
                var id = new CategoryDao().Edit(model);
                return RedirectToAction("Index");
                
            }



            return View();


        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int id)
        {
            new CategoryDao().Delete(id);
            SetAlert("Xóa danh mục thành công", "success");
            return RedirectToAction("Index");
        }
    }
}