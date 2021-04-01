using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            ViewBag.About = new AboutDao().GetComponentByMetaTitle("about-us");
            ViewBag.Founder = new AboutDao().GetComponentByMetaTitle("founder");
            return View();
        }
    }
}