using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        OnlineShopDbContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDbContext();
        }
        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x =>x.Status == true);
        }
        public List<Contact> GetListActiveContact()
        {
            return db.Contacts.Where(x=>x.Status == true).ToList();
        }
        public int InsertFeedBack(FeedBack fb)
        {
            db.FeedBacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
        public IEnumerable<Contact> ListAllPaging(string Search, int page, int pageSize)
        {

            IQueryable<Contact> model = db.Contacts;
            if (!string.IsNullOrEmpty(Search))
            {
                model = model.Where(x => x.Content.Contains(Search));//tìm trong cả tên và username . sắp xếp giảm dần || x.Name.Contains(Search)
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);//cài nuget pagelist
        }

        public long Create(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
            return contact.ID;
        }
        public long Edit(Contact model)
        {
            var contact = db.Contacts.Find(model.ID);
            contact.Content = model.Content;
            contact.Status = model.Status;
            db.SaveChanges();

            return contact.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var contact = db.Contacts.Find(id);
            contact.Status = !contact.Status;
            db.SaveChanges();
            return contact.Status;
        }
        public Contact GetByID(long id)
        {
            return db.Contacts.Find(id);
        }
    }
}
