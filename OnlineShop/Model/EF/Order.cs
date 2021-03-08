namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [Key]
        public long ID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage ="Không được trống")]
        [Display(Name ="Mã khách hàng")]
        
        public long? CustomerID { get; set; }

        [Display(Name ="Tên khách hàng")]
        [Required(ErrorMessage = "Không được trống")]
        [StringLength(50)]
        public string ShipName { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Không được trống")]
        [StringLength(50)]
        public string ShipMobile { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage ="Không được trống")]
        [StringLength(50)]
        public string ShipAddress { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Sai định dạng email")]
        [StringLength(50)]
        public string ShipEmail { get; set; }

        public bool? Status { get; set; }
    }
}
