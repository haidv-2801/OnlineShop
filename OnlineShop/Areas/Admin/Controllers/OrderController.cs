using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index(string searchString, int curIndex = 1, int pageSize = 10)
        {
            var model = new OrderDao().ListAllPaging(searchString, curIndex, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new OrderDao().Delete(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreatedDate = DateTime.Now;
                order.Status = true;
                new OrderDao().Insert(order);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Thêm không thành công.");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {

            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var model = new OrderDao().Find(id);
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new { status = result });
        }
    }
}