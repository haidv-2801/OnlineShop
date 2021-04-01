using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.Dao
{
    public class ContentDao
    {
        OnlineShopDbContext db = null;

        public ContentDao()
        {
            db = new OnlineShopDbContext();
        }
        public Content GetById(long id)
        {
            return db.Contents.Find(id);
        }
        public long Insert(Content entity)
        {
            long result = db.Contents.Count(x => x.MetaTitle == entity.MetaTitle);
            if (result == 1)
            {
                return -1;
            }
            else
            {
                db.Contents.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }
        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);
                content.Name = entity.Name;
                content.Code = entity.Code;
                content.MetaTitle = entity.MetaTitle;
                content.Description = entity.Description;
                content.Image = entity.Image;
                content.CategoryID = entity.CategoryID;
                content.ModifiedBy = entity.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Content ViewDetail(int id)
        {
            // tìm kiếm theo khóa có thể dùng singleordefault
            return db.Contents.Find(id);
        }
        public long Create(Content content)
        {

            // xử lý alias
            if (!string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();
            // xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);
                    // insert to tag table
                    if (!existedTag)
                    {
                        this.insertTag(tagId, tag);
                    }
                    // insert to content tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }
        public long Edit(Content content)
        {
            // xử lý alias
            if (!string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            db.SaveChanges();
            // xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);
                    // insert to tag table
                    if (!existedTag)
                    {
                        this.insertTag(tagId, tag);
                    }
                    // insert to content tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange
                (db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public void insertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x =>
                         new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            // phân trang theo ngày tạo
            if (!string.IsNullOrEmpty(searchString))
            {
                // exst string find -> where để nối tìm kiếm
                // tìm kiếm theo 2 khả năng username hoặc name
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            // nếu cần tìm kiếm thêm thì else if
            // cần sort page ngay r ms phân trang
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        // không sử dụng tìm kiếm
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            // nếu cần tìm kiếm thêm thì else if
            // cần sort page ngay r ms phân trang
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             ID = a.ID
                         }).AsEnumerable().Select(
                x => new Content()
                {
                    Name = x.Name,
                    MetaTitle = x.MetaTitle,
                    Image = x.Image,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    ID = x.ID
                });
            // nếu cần tìm kiếm thêm thì else if
            // cần sort page ngay r ms phân trang
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
