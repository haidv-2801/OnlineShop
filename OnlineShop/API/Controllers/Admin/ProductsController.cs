using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Model.EF;

namespace API.Controllers.Admin
{
    public class ProductsController : ApiController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        // GET: api/Products
        [HttpGet]
        public IHttpActionResult GetProducts(string search)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(search))
            {
                model = model.Where(x => x.Name.Contains(search));
            }
            model.OrderByDescending(x => x.CreatedDate);
            return Ok(model);
        }

        // GET: api/Products/5
        [HttpGet]
        public IHttpActionResult GetProduct(long id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        // POST: api/Products
        [HttpPost]
        public IHttpActionResult PostProduct(Product entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long result = db.Products.Count(x => x.MetaTitle == entity.MetaTitle);
            if (result == 1)
            {
                return NotFound();
            }
            else
            {
                db.Products.Add(entity);
                db.SaveChanges();
                return Ok(entity.ID);
            }
        }
        // PUT: api/Products/5
        [HttpPut]
        public IHttpActionResult PutProduct(long id, Product entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entity.ID)
            {
                return BadRequest();
            }

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
            return Ok(true);
        }

        
        // DELETE: api/Products/5
        [HttpDelete]
        public IHttpActionResult DeleteProduct(long id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }
    }
}