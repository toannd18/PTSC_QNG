using Data.Data;
using DataModel.Model.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class DepartmentDao
    {
        private readonly EFDbContext db;
        public DepartmentDao()
        {
            db = new EFDbContext();
        }
        public List<tbl_BP> load()
        {
            return db.tbl_BP.OrderBy(m => m.Ma_BP).ToList();
        }
        public DepartmentModel detail(string ma)
        {
            var tbl = db.tbl_BP.Where(m => m.Ma_BP.Equals(ma)).Select(m => new DepartmentModel
            {
                Ma_BP = ma,
                Ten_BP = m.Ten_BP,
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(DepartmentModel model)
        {
            try
            {
                tbl_BP tbl = new tbl_BP();
                tbl.Ma_BP = model.Ma_BP;
                tbl.Ten_BP = model.Ten_BP;
                db.tbl_BP.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(DepartmentModel model)
        {
            try
            {
                tbl_BP tbl = db.tbl_BP.Find(model.Ma_BP);
                tbl.Ten_BP = model.Ten_BP;
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
            tbl_BP tbl = db.tbl_BP.Find(ma);
            db.tbl_BP.Remove(tbl);
            db.SaveChanges();
            return true;
        }
    }
}