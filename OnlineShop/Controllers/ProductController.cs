using Model.Dao;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();
        // GET: Product
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(search, page, pageSize);

            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        public ActionResult Category(long cateId, int? page)
        {
            var category = new CategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            if (page == null) page = 1;

            var links = (from l in db.Products
                         select l).Where(x => x.CategoryID == cateId).OrderBy(x => x.CreatedDate);
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }
        // cache trên từng id khác nhau
        // có thể sử dụng với config trong web.config
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
            return View(product);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            });
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 2)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model.ToPagedList(page, pageSize));
        }
    }
}
