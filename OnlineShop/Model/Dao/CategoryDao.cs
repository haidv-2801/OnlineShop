using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public long Create(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }
        public long Edit(Category model)
        {
            var category = db.Categories.Find(model.ID);
            category.Name = model.Name;
            category.MetaTitle = model.MetaTitle;
            category.MetaKeywords = model.MetaKeywords;
            category.MetaDescriptions = model.MetaDescriptions;
            category.DisplayOrder = model.DisplayOrder;
            category.SeoTitle = model.SeoTitle;
            category.ShowOnHome = model.ShowOnHome;
            category.Status = model.Status;
            category.Language = model.Language;
            db.SaveChanges();

            return category.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }
        }
        public IEnumerable<Category> ListAllPaging(string Search, int page, int pageSize)
        {

            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(Search))
            {
                model = model.Where(x => x.Name.Contains(Search));//tìm trong cả tên và username . sắp xếp giảm dần || x.Name.Contains(Search)
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);//cài nuget pagelist
        }
        public Category GetByID(long id)
        {
            return db.Categories.Find(id);
        }
    }
}
