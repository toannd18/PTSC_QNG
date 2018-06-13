using Data.Data;
using DataModel.Model.Trade;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataService.Dao.Trade
{
    public class DeXuatHDDao
    {
        private readonly EFDbContext db;

        public DeXuatHDDao()
        {
            db = new EFDbContext();
        }

        public List<DeXuatHDModel> load()
        {
            List<DeXuatHDModel> tbl = new List<DeXuatHDModel>();
            tbl = (from hd in db.tbl_DeXuat_HD
                   join dx in db.tbl_DeXuat on hd.DeXuatId equals dx.Id
                   join m in db.tbl_NCC on hd.Ma_NCC equals m.Ma_NCC
                   join u in db.AppUsers on dx.Ten_Dx5 equals u.UserName
                   select new DeXuatHDModel
                   {
                       Id = hd.Id,
                       DeXuatId = dx.Id,
                       TenDeXuat = dx.Ma,
                       So_HD = hd.So_HD,
                       Ten_HD = hd.So_HD + "-" + hd.Date.Year.ToString() + "-PTSC-QNG-M" + (dx.Kieu ? "HH" : "DV"),
                       Date = hd.Date,
                       Ma_NCC = m.Ma_NCC,
                       Ten_NCC = m.Ten_NCC,
                       Diem=hd.Diem,
                       Chat_luong=hd.Chat_luong,
                       Gia_Ca=hd.Gia_Ca,
                       Tien_Do=hd.Tien_Do,
                       Thai_Do=hd.Thai_Do,
                       Ten_TH=u.FullName
                   }).ToList();

            return tbl;
        }
        public tbl_DeXuat_HD Detail(int Id)
        {
            var model = db.tbl_DeXuat_HD.Find(Id);
            return model;
        }
        public bool Save(DeXuatHDModel tbl)
        {
            try
            {
                tbl_DeXuat_HD model = new tbl_DeXuat_HD();
                model.Id = tbl.Id;
                model.So_HD = tbl.So_HD;
                model.DeXuatId = tbl.DeXuatId.Value;
                model.Date = tbl.Date;
                model.Chat_luong = tbl.Chat_luong;
                model.Gia_Ca = model.Gia_Ca;
                model.Thai_Do = tbl.Thai_Do;
                model.Tien_Do = tbl.Tien_Do;
                model.Ma_NCC = tbl.Ma_NCC;
                model.Diem = tbl.Diem;
                db.Entry(model).State = model.Id == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                Tinh(model.Ma_NCC);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int Id)
        {
            try
            {
                var model = db.tbl_DeXuat_HD.Find(Id);
                db.tbl_DeXuat_HD.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetHD(DateTime date)
        {
            int so = db.tbl_DeXuat_HD.Where(m => m.Date.Year == date.Year).Count();
            return so + 1;
        }
        public void Tinh(string Ma_NCC)
        {
            var tbl = (from h in db.tbl_DeXuat_HD
                      where h.Ma_NCC == Ma_NCC
                      group h by h.Ma_NCC into g
                      select new
                      {
                          Ma_NCC = g.Key,
                          CountA = g.Count(m => m.Diem == "A"),
                          CountB = g.Count(m => m.Diem == "B"),
                          CountC = g.Count(m => m.Diem == "C"),
                          Count = g.Count()
                      }).FirstOrDefault();
            var model = db.tbl_NCC.Find(Ma_NCC);
            model.Time = tbl.Count;
            if (tbl.CountC > 0 || tbl.CountB>1)
            {
                
                model.Diem = "C";
                db.SaveChanges();
            }
            else if(tbl.CountB==1)
            {
                
                model.Diem = "B";
                db.SaveChanges();
            }
            else if (tbl.Count == tbl.CountA)
            {
                model.Diem = "A";
                db.SaveChanges();
            }
            
        }

        public bool CheckExistDxHd(int dx)
        {
            return db.tbl_DeXuat_HD.Any(m => m.DeXuatId == dx);
        }
    }
}