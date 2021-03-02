using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Mời nhập userName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mời nhập passWord")]
        public string Password { get; set; }
        public bool RememberMe { set; get; }
    }
}