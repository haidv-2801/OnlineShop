using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        public ActionResult Index(string Search, int page = 1, int pageSize = 2)
        {
            var dao = new ContactDao();
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
        public ActionResult Create(Contact model)
        {
            if (ModelState.IsValid)
            {
                

                new ContactDao().Create(model);
                return RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContactDao();
            var contact = dao.GetByID(id);

            return View(contact);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Contact model)
        {
            if (ModelState.IsValid)
            {

                new ContactDao().Edit(model);
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            new ContactDao().Delete(id);
            SetAlert("Xóa liên hệ thành công", "success");
            return RedirectToAction("Index");
        }
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContactDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}