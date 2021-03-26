using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AboutDao
    {
        OnlineShopDbContext db = null;
        public AboutDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<About> GetComponentByMetaTitle(string compString)
        {
            return db.Abouts.Where(x => x.MetaTitle.Contains(compString)).ToList();
        }
    }
}
