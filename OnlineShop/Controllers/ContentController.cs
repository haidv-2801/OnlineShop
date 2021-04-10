using Model.Dao;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var model = new ContentDao().ListAllPaging(page, pageSize);
            return View(model);
        }
        public ActionResult Detail(long id)
        {
            var model = new ContentDao().GetById(id);
            PostsVM post = new PostsVM
            {
                PostID = model.ID,
                Name = model.Name,
                Message = model.Description,
                PostedDate = (DateTime)model.CreatedDate,
                Author = model.CreatedBy,
                Image = model.Image
            };

            ViewBag.Tags = new ContentDao().ListTag(id);
            return View(post);
        }
        public PartialViewResult ContentTag(long id)
        {
            var model = new ContentDao().ListTag(id);
            return PartialView("~/Views/Shared/_filter_tag.cshtml", model);
        }
        public ActionResult Tag(string tagId, int page = 1, int pageSize = 10)
        {
            var model = new ContentDao().ListAllByTag(tagId, page, pageSize);
            ViewBag.Tag = new ContentDao().GetTag(tagId);
            return View(model);
        }
    }
}