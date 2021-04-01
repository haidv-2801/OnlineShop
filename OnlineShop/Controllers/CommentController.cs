using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CommentController : Controller
    {
        private OnlineShopDbContext db;
        // GET: ChatRoom
        [HttpGet]
        public ActionResult GetUsers()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUsers(string username)
        {
            /*User user = db.Users.Where(x=>x.UserName.Equals(username)).FirstOrDefault();
            if (user != null)
            {
                Session["USER_SESSION"] = user.ID;
                return RedirectToAction("GetPosts");
            }*/
            ViewBag.Msg = "Username does not exist !";
            return View();
        }
        public PartialViewResult GetComments(int postId)
        {
            IQueryable<Comment> comments = db.Comments.Where(c => c.Content.ID == postId)
                                     .Select(c => new Comment
                                     {
                                         Id = c.Id,
                                         CreatedDate = c.CreatedDate.Value,
                                         Text = c.Text,
                                         User = new User
                                         {
                                             ID = c.User.ID,
                                             UserName = c.User.UserName,
                                             Avatar = c.User.Avatar
                                         }
                                     }).AsQueryable();

            return PartialView("~/Views/Shared/_MyComments.cshtml", comments);
        }
    }
}