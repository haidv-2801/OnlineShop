using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string GroupID { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
    }
}