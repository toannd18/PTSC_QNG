using Data.Data;
using DataModel.Model.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class EquipmentDao
    {
        private readonly EFDbContext db;
        public EquipmentDao()
        {
            db = new EFDbContext();
        }
        public List<EquipmentModel> load()
        {
            var tbl = (from e in db.tbl_TB
                       join p in db.tbl_NCC on e.Ma_NCC equals p.Ma_NCC
                       select new EquipmentModel
                       {
                           Ma_TB = e.Ma_TB,
                           Ma_NCC = e.Ma_NCC,
                           Cong_Suat = e.Cong_Suat,
                           Ten_TB = e.Ten_TB,
                           Ten_NCC = p.Ten_NCC
                       }).ToList();
            return tbl;
        }
        public EquipmentModel detail(string ma)
        {
            var tbl = db.tbl_TB.Where(m => m.Ma_TB.Equals(ma)).Select(m => new EquipmentModel
            {
                Ma_TB = ma,
                Ma_NCC = m.Ma_NCC,
                Ten_TB=m.Ten_TB,
                Cong_Suat=m.Cong_Suat
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(EquipmentModel model)
        {
            try
            {
                tbl_TB tbl = new tbl_TB();
                tbl.Ma_TB = model.Ma_TB;
                tbl.Ten_TB = model.Ten_TB;
                tbl.Ma_NCC = model.Ma_NCC;
                tbl.Cong_Suat = model.Cong_Suat;
                db.tbl_TB.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(EquipmentModel model)
        {
            try
            {
                tbl_TB tbl = db.tbl_TB.Find(model.Ma_TB);
                tbl.Ten_TB = model.Ten_TB;
                tbl.Ma_NCC = model.Ma_NCC;
                tbl.Cong_Suat = model.Cong_Suat;
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
            tbl_TB tbl = db.tbl_TB.Find(ma);
            db.tbl_TB.Remove(tbl);
            db.SaveChanges();
            return true;
        }
    }
}
