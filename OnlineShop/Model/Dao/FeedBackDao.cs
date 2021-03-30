using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class FeedBackDao
    {
        OnlineShopDbContext db = null;
        public FeedBackDao()
        {
            db = new OnlineShopDbContext();
        }

        public bool ChangeStatus(long id)
        {
            var feedback = db.FeedBacks.Find(id);
            feedback.Status = !feedback.Status; // true thành false, flase thành true
            db.SaveChanges();
            return feedback.Status;    // lấy kqua hiện tại
        }

        public bool Update(FeedBack entity)
        {
            try
            {
                var FeedBack = db.FeedBacks.Find(entity.ID);
                FeedBack.Name = entity.Name;
                FeedBack.Phone = entity.Phone;
                FeedBack.Address = entity.Address;
                FeedBack.Email = entity.Email;
                FeedBack.Content = entity.Content;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<FeedBack> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<FeedBack> model = db.FeedBacks;
            // phân trang theo ngày tạo
            if (!string.IsNullOrEmpty(searchString))
            {
                // exst string find -> where để nối tìm kiếm
                // tìm kiếm theo name
                model = model.Where(x => x.Name.Contains(searchString));
            }
            // nếu cần tìm kiếm thêm thì else if
            // cần sort page ngay r ms phân trang
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public FeedBack GetById(long id)
        {
            return db.FeedBacks.Find(id);
        }
    }
}
