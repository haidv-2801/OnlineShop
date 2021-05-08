using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.ViewModel
{
    public class PostsVM
    {
        public long PostID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
    }

    public class CommentsVM
    {
        public long ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public PostsVM Posts { get; set; }
        public UserVM Users { get; set; }
    }

    public class UserVM
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        public string imageProfile { get; set; }
        public string Address { get; set; }
    }

    public class SubCommentsVM
    {
        public long SubComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public CommentsVM Comment { get; set; }
        public UserVM User { get; set; }
    }


}