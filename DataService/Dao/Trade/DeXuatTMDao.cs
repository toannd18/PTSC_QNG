using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataService.Dao.Trade
{
    public class DeXuatTMDao
    {
        private readonly EFDbContext db;
        public DeXuatTMDao()
        {
            db = new EFDbContext();
        }
        public IList<tbl_DeXuat_TM> load(int dx)
        {
            return db.tbl_DeXuat_TM.Where(m => m.DeXuatId == dx).ToList();
        }
        public bool Save(tbl_DeXuat_TM model)
        {
            try
            {
                db.Entry(model).State = model.Id == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
