using Model.Dao;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            /*       ViewBag.NewProducts = productDao.ListNewProduct(4);
                   ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(4);*/
            ViewBag.NewProducts = productDao.ProductsByCate(1, 3);
            ViewBag.ListFeatureProducts = productDao.ProductsByCate(2, 3);
            //set SEO title
            ViewBag.Title = ConfigurationManager.AppSettings["HomeTitle"];
            ViewBag.Keywords = ConfigurationManager.AppSettings["HomeKeyword"];
            ViewBag.Descriptions = ConfigurationManager.AppSettings["HomeDescriptions"];
            return View();
        }
        [ChildActionOnly]   // trên view con
                            // OutputCacheLocation :cache bên server hoặc client
                            // duration: (cache bn giây)
                            // partial view k support location
        [OutputCache(Duration = 3600 * 24, VaryByParam = "None")]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }
        [ChildActionOnly]   // trên view con
        //[OutputCache(Duration = 3600 * 24)]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);
            return PartialView(model);
        }
        [ChildActionOnly]   // trên view con
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }
        [ChildActionOnly]   // trên view con
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
        // new code
        [ChildActionOnly]
        public PartialViewResult Slide()
        {
            var model = new SlideDao().ListAll();
            return PartialView("~/Views/Shared/_slider.cshtml", model);
        }
        [ChildActionOnly]
        public PartialViewResult NavBarInner()
        {
            TempData["MenuNavBarInner"] = new MenuDao().ListByGroupId(1);
            var productCategories = new ProductCategoryDao().ListAll();
            return PartialView("~/Views/Shared/_navbar_inner.cshtml", productCategories);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderBottom()
        {
            var cartSize = 0;
    
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                var userSession = (UserLogin)Session[CommonConstants.USER_SESSION];
                cartSize = new CartDao().getSize(userSession.UserName);
            }
            else
            {
                if (Request.Cookies[Common.CommonConstants.CART_COOKIES] != null)
                {
                    var cart = Request.Cookies[Common.CommonConstants.CART_COOKIES];
                    cartSize = cart.Value.Split(',').Length;
                }
            }
            
            return PartialView("_header_bottom", cartSize.ToString());
        }
    }
}