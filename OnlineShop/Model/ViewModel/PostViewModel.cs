using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PostViewModel
    {
        public int PostID { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
    }
    public class CommentsVM
    {
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public PostViewModel Posts { get; set; }
        public UserVM Users { get; set; }
    }

    public class UserVM
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string imageProfile { get; set; }
    }

    public class SubCommentsVM
    {
        public int SubComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public CommentsVM Comment { get; set; }
        public UserVM User { get; set; }
    }
}
