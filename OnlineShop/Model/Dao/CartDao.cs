using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class CartDao
    {
        private readonly OnlineShopDbContext db = null;
        public CartDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<Cart> ListAll()
        {
            return db.Carts.ToList();
        }

        public List<Cart> ListByUsername(string username)
        {
            return db.Carts.Where(x=>x.CustomerName==username).ToList();
        }

        public int getSize()
        {
            return db.Carts.Count();
        }

        public int getSize(string username)
        {
            return db.Carts.Count(x=>x.CustomerName==username);
        }

    }
}
