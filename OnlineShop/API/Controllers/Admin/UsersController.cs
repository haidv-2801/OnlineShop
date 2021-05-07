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
    public class UsersController : ApiController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        // GET: api/Users
        [HttpGet]
        public IHttpActionResult GetUsers(string search)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(search))
            {
                model = model.Where(x => x.UserName.Contains(search) || x.Name.Contains(search));
            }
            model.OrderByDescending(x => x.CreatedDate);
            return Ok(model);
        }

        // GET: api/Users/5
        [HttpGet]
        public IHttpActionResult GetUser(long id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut]
        public IHttpActionResult PutUser(long id, User entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entity.ID)
            {
                return BadRequest();
            }

            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                if (string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Users
        [HttpPost]
        public IHttpActionResult PostUser(User entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long result = db.Users.Count(x => x.UserName == entity.UserName);
            if (result == 1)
            {
                return NotFound();
            }
            else
            {
                db.Users.Add(entity);
                db.SaveChanges();
            }
            return Ok(entity.ID);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public IHttpActionResult DeleteUser(long id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }
    }
}