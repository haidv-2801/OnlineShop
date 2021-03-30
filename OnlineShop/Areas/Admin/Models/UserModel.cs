using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class UserModel
    {
        public long ID { get; set; }
        [Required(ErrorMessage = "Mời nhập userName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mời nhập passWord")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mời nhập Tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mời nhập Địa Chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Mời nhập Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mời nhập Điện Thoại")]
        public string Phone { get; set; }
        public bool Status { get; set; }
    }
}