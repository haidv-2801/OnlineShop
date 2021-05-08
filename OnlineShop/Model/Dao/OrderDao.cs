using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext db = null;
        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }

        public Order Find(long id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<Order> ListAllPaging(string searchString, int curIndex, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShipName.Contains(searchString) || x.ShipEmail.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(curIndex, pageSize);
        }
        public long Insert(Order order)
        {
            order.Status = true;
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }

        public bool Delete(long id)
        {
            try
            {
                var entity = db.Orders.Find(id);
                db.Orders.Remove(entity);
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
            var entity = db.Orders.Find(id);
            entity.Status = !entity.Status;
            db.SaveChanges();
            return (bool)entity.Status;
        }
    }
}
