using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.ViewModel
{
    public class ProductCategoryViewModel
    {
        public ProductCategory Cate { get; set; }
        public List<Product> Products { get; set; }
    }
}