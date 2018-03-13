using Data.Data;
using DataModel.Model.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class PositionDao
    {
        private readonly EFDbContext db;
        public PositionDao()
        {
            db = new EFDbContext();
        }
        public List<tbl_CV> load()
        {
            return db.tbl_CV.ToList();
        }
        public PositionModel detail(string ma)
        {
            var tbl = new PositionDao().load().Where(m => m.Ma_CV == ma).Select(m => new PositionModel
            {
                Ma_CV = m.Ma_CV,
                Ten_CV = m.Ten_CV,
                Display = m.Display
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(PositionModel model)
        {
            try
            {
                tbl_CV tbl = new tbl_CV();
                tbl.Ma_CV = model.Ma_CV;
                tbl.Ten_CV = model.Ten_CV;
                tbl.Display = model.Display;
                db.tbl_CV.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool Update(PositionModel model)
        {
            try
            {
                tbl_CV tbl = new tbl_CV();
                tbl.Ma_CV = model.Ma_CV;
                tbl.Ten_CV = model.Ten_CV;
                tbl.Display = model.Display;
                db.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool Delete(string ma)
        {
            try
            {
                var tbl = db.tbl_CV.Find(ma);
                db.tbl_CV.Remove(tbl);
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