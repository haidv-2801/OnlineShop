using Model.Dao;
using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CommentController : Controller
    {
        OnlineShopDbContext db = new OnlineShopDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult GetComments(int postId)
        {
            var model = new CommentDao().GetComments(postId);

            return PartialView("~/Views/Shared/_MyComments.cshtml", model);
        }
        [HttpPost]
        public ActionResult AddComment(CommentsVM comment, int postId)
        {
            //bool result = false;
            Comment commentEntity = null;
            var userSess = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (userSess == null) return RedirectToAction("Login", "User", new { postId = postId, status = false });
            else
            {
                var userId = userSess.UserID;
                var user = db.Users.FirstOrDefault(u => u.ID == userId);
                var post = db.Contents.FirstOrDefault(p => p.ID == postId);

                if (comment != null)
                {
                    commentEntity = new Comment
                    {
                        Text = comment.CommentMsg,
                        CreatedDate = comment.CommentedDate,
                    };

                    if (user != null && post != null)
                    {
                        post.Comments.Add(commentEntity);
                        user.Comments.Add(commentEntity);

                        db.SaveChanges();
                        //result = true;
                    }
                }
                return RedirectToAction("GetComments", "Comment", new { postId = postId, status = true });
            }

        }

        [HttpGet]
        public PartialViewResult GetSubComments(int ComID)
        {
            var model = new CommentDao().GetSubComments(ComID);

            return PartialView("~/Views/Shared/_MySubComments.cshtml", model);
        }
        [HttpPost]
        public ActionResult AddSubComment(SubCommentsVM subComment, int ComID)
        {
            Reply subCommentEntity = null;
            var userSess = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (userSess == null) return RedirectToAction("Login", "User", new { ComID = ComID, status = false });
            else
            {
                var userId = userSess.UserID;
                var user = db.Users.FirstOrDefault(u => u.ID == userId);
                var comment = db.Comments.FirstOrDefault(p => p.Id == ComID);

                if (subComment != null)
                {
                    subCommentEntity = new Reply
                    {
                        Text = subComment.CommentMsg,
                        CreatedDate = subComment.CommentedDate,
                    };

                    if (user != null && comment != null)
                    {
                        comment.Replies.Add(subCommentEntity);
                        user.Replies.Add(subCommentEntity);

                        db.SaveChanges();
                        //result = true;
                    }
                }
                return RedirectToAction("GetSubComments", "Comment", new { ComID = ComID, status = true });
            }
        }
    }
}