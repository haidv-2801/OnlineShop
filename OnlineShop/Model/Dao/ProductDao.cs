using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Product> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        // Lấy danh sách product theo category dùng viewmodel   
        public ProductCategoryViewModel ProductsByCate(long cateID, int takeNum)
        {
            ProductCategory cate = db.ProductCategories.Find(cateID);
            List<Product> products = db.Products.Where(x => x.CategoryID == cateID && x.Status).Take(takeNum).ToList();
            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel
            {
                Cate = cate,
                Products = products
            };
            return productCategoryViewModel;
        }

        // lấy danh sách theo id
        public List<ProductViewModel> ListByCategoryId(long categoryId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            // skip lấy từ bản ghi đến bản ghi nào
            // take lấy bn
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count();
            var model =
                (from a in db.Products
                 join b in db.ProductCategories on a.CategoryID equals b.ID
                 where a.CategoryID == categoryId
                 select new
                 {
                     CateMetaTitle = b.MetaTitle,
                     CateName = b.Name,
                     CreatedDate = a.CreatedDate,
                     ID = a.ID,
                     Images = a.Image,
                     Name = a.Name,
                     MetaTitle = a.MetaTitle,
                     Price = a.Price,
                     PromotionPrice = a.PromotionPrice
                 }).AsEnumerable().Select(x => new ProductViewModel()
                 {
                     CateMetaTitle = x.MetaTitle,
                     CateName = x.Name,
                     CreatedDate = x.CreatedDate,
                     ID = x.ID,
                     Images = x.Images,
                     Name = x.Name,
                     MetaTitle = x.MetaTitle,
                     Price = x.Price,
                     PromotionPrice = x.PromotionPrice
                 });
            // equals : trường làm column nối nhau
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.MetaTitle = entity.MetaTitle;
                product.PromotionPrice = entity.PromotionPrice;
                product.Description = entity.Description;
                product.Image = entity.Image;
                product.Detail = entity.Detail;
                product.CategoryID = entity.CategoryID;
                product.ModifiedBy = entity.ModifiedBy;
                product.Warranty = entity.Warranty;
                product.ModifiedDate = DateTime.Now;
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
            var product = db.Products.Find(id);
            product.Status = !product.Status; // true thành false, flase thành true
            db.SaveChanges();
            return product.Status;    // lấy kqua hiện tại
        }

        public bool Delete(long id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Product GetById(long id)
        {
            return db.Products.Find(id);
        }

        public long Create(Product entity)
        {
            long result = db.Products.Count(x => x.MetaTitle == entity.MetaTitle);
            if (result == 1)
            {
                return -1;
            }
            else
            {
                db.Products.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            // skip lấy từ bản ghi đến bản ghi nào
            // take lấy bn
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model =
               (from a in db.Products
                join b in db.ProductCategories
                on a.CategoryID equals b.ID
                where a.Name.Contains(keyword)
                select new
                {
                    CateMetaTitle = b.MetaTitle,
                    CateName = b.Name,
                    CreatedDate = a.CreatedDate,
                    ID = a.ID,
                    Images = a.Image,
                    Name = a.Name,
                    MetaTitle = a.MetaTitle,
                    Price = a.Price
                }).AsEnumerable().Select(x => new ProductViewModel()
                {
                    CateMetaTitle = x.MetaTitle,
                    CateName = x.Name,
                    CreatedDate = x.CreatedDate,
                    ID = x.ID,
                    Images = x.Images,
                    Name = x.Name,
                    MetaTitle = x.MetaTitle,
                    Price = x.Price
                });
            // equals : trường làm column nối nhau
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
            // take lấy ra bn bản ghi đầu tiên
        }
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
            // top hot != null & top hot > tgian hiện tại (còn hạn)
        }
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
            // khác sản phẩm đấy nhưng cùng danh mục (sp liên quan)
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }
    }
}
