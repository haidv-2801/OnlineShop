using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên Đăng Nhập")]
        [Required(ErrorMessage = "Bạn Phải Nhập Tài Khoản")]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Bạn Phải Nhập Mật Khẩu")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}