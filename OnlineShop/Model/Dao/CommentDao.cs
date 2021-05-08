
using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CommentDao
    {
        OnlineShopDbContext db = null;
        public CommentDao()
        {
            db = new OnlineShopDbContext();
        }
        public IQueryable<Comment> GetAll()
        {
            return db.Comments.OrderBy(x => x.CreatedDate);
        }
        public IQueryable<CommentsVM> GetComments(long contentId)
        {
            IQueryable<CommentsVM> comments = db.Comments.Where(c => c.Content.ID == contentId)
                                                       .Select(c => new CommentsVM
                                                       {
                                                           ComID = c.Id,
                                                           CommentedDate = c.CreatedDate.Value,
                                                           CommentMsg = c.Text,
                                                           Users = new UserVM
                                                           {
                                                               UserID = c.User.ID,
                                                               Username = c.User.UserName,
                                                               imageProfile = c.User.Avatar,
                                                               Address = c.User.Address
                                                           }
                                                       }).AsQueryable();
            return comments;
        }
        public IQueryable<SubCommentsVM> GetSubComments(long ComID)
        {
            IQueryable<SubCommentsVM> subComments = db.Replies.Where(sc => sc.Comment.Id == ComID)
                                                                          .Select(sc => new SubCommentsVM
                                                                          {
                                                                              SubComID = sc.Id,
                                                                              CommentMsg = sc.Text,
                                                                              CommentedDate = sc.CreatedDate.Value,
                                                                              User = new UserVM
                                                                              {
                                                                                  UserID = sc.User.ID,
                                                                                  Username = sc.User.UserName,
                                                                                  imageProfile = sc.User.Avatar,
                                                                                  Address = sc.User.Address
                                                                              }
                                                                          }).AsQueryable();
            return subComments;
        }
    }
}
