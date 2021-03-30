using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        // CRUD
        public bool Update(ProductCategory entity)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(entity.ID);
                productCategory.Name = entity.Name;
                productCategory.MetaTitle = entity.MetaTitle;
                productCategory.ModifiedBy = entity.ModifiedBy;
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.MetaKeywords = entity.MetaKeywords;
                productCategory.MetaDescriptions = entity.MetaDescriptions;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var product = db.ProductCategories.Find(id);
            product.Status = !product.Status; // true thành false, flase thành true
            db.SaveChanges();
            return product.Status;    // lấy kqua hiện tại
        }

        public bool Delete(long id)
        {
            try
            {
                var product = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ProductCategory GetById(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public long Create(ProductCategory entity)
        {
            long result = db.ProductCategories.Count(x => x.MetaTitle == entity.MetaTitle);
            if (result == 1)
            {
                return -1;
            }
            else
            {
                db.ProductCategories.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }
    }
}
