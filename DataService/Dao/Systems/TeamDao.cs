using Data.Data;
using DataModel.Model.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class TeamDao
    {
        private readonly EFDbContext db;
        public TeamDao()
        {
            db = new EFDbContext();
        }
        public List<TeamModel> load()
        {
            var tbl = db.tbl_TO.Select(m => new TeamModel
            {
                Ma_TO = m.Ma_TO,
                Ten_TO = m.Ten_TO,
                Ma_BP = m.Ma_BP,
                Ten_BP = m.tbl_BP.Ten_BP
            }).ToList();
            return tbl;  
        }
        public TeamModel detail(string ma)
        {
            return new TeamDao().load().Where(m=>m.Ma_TO==ma).FirstOrDefault();
        }
        public bool Save (TeamModel model)
        {
            try
            {
                tbl_TO tbl = new tbl_TO();
                tbl.Ma_TO = model.Ma_TO;
                tbl.Ten_TO = model.Ten_TO;
                tbl.Ma_BP = model.Ma_BP;
                db.tbl_TO.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(TeamModel model)
        {
            try
            {
                tbl_TO tbl = new tbl_TO();
                tbl.Ma_TO = model.Ma_TO;
                tbl.Ten_TO = model.Ten_TO;
                tbl.Ma_BP = model.Ma_BP;
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
                var tbl = db.tbl_TO.Find(ma);
                db.tbl_TO.Remove(tbl);
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