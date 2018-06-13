using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Dao.Trade
{
    public class DGTMDao
    {
        private readonly EFDbContext db;
        public DGTMDao()
        {
            db = new EFDbContext();
        }
        public List<DGTMModel> Load(int dx)
        {
            var model = (from t in db.tbl_DeXuat_KT
                         join t1 in db.tbl_DG_KT on t.Id equals t1.DG_KT_Id into g
                         from t1 in g.DefaultIfEmpty()
                         join n in db.tbl_DG_NCC on t1.DG_NCC_Id equals n.Id into g1
                         from n in g1.DefaultIfEmpty()
                         where t.DeXuatId == dx
                         select new
                         {
                             Ma = t.Id,
                             Ten = t.Ten,
                             MoTa = t.Mo_Ta,
                             NCC = n.Ma_NCC,
                             So_luong = t.SoLuong,
                             DVT = t.DVT,
                             Don_Gia = t1.Don_Gia.HasValue?t1.Don_Gia:0,
                             Ma_DG = t1.Id,
                             Tien = (t1.Don_Gia.HasValue ? t1.Don_Gia : 0) * (t.SoLuong),
                         }).GroupBy(m => new { m.Ma }).Select(m => new DGTMModel
                         {
                             Ma = m.Key.Ma,
                             Ten = m.Select(t => t.Ten).FirstOrDefault(),
                             Mo_Ta = m.Select(t => t.MoTa).FirstOrDefault(),
                             So_luong = m.Select(t => t.So_luong).FirstOrDefault(),
                             DVT = m.Select(t => t.DVT).FirstOrDefault(),
                             subjectTM = m.Select(t => new SubjectTM
                             {
                                 NCC = t.NCC,
                                 Don_Gia = t.Don_Gia,
                                 Tien = t.Tien,
                                 Ma_DG = t.Ma_DG,
                             }).ToList()
                         }).ToList();
            return model;
        }
        public List<DG_TMView> LoadTM(int dx)
        {
            var model = (from t in db.tbl_DG_TM
                        join d in db.tbl_DG_NCC on t.DG_NCC_Id equals d.Id
                        where t.DeXuatId==dx
                        select new DG_TMView {
                            Ma_NCC=d.Ma_NCC,
                            Hieu_Luc=t.Hieu_Luc,
                            Thoi_Gian = t.Thoi_Gian,
                            Dia_Diem = t.Dia_Diem,
                            Dieu_Kien = t.Dieu_Kien,
                            BH = t.BH,
                            Che_Do = t.Che_Do,
                            Ghi_Chu=t.Ghi_Chu,
                            Van_Chuyen=t.Van_Chuyen
                        }).ToList();
            return model;
        }
        public List<tbl_DG_TM> DetailTM(int dx)
        {
            var model = db.tbl_DG_TM.Where(m => m.DeXuatId == dx).OrderBy(m => m.DG_NCC_Id).ToList();
            return model;
        }
        public bool SaveTM(tbl_DG_TM tbl)
        {
            try
            {
                db.Entry(tbl).State = tbl.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }catch
            {
                return false;
            }
        }
    }
    public class DGTMModel
    {
        public int Ma { get; set; }
        public string Ten { get; set; }
        public string Mo_Ta { get; set; }
        public string DVT { get; set; }
        public int So_luong { get; set; }

        public List<SubjectTM> subjectTM{get;set;}
    }
    public class SubjectTM
    {
        public int Ma_DG { get; set; }
        public int? Don_Gia { get; set; }
        public int? Tien { get; set; }
        public string NCC { get; set; }
    }
    public class DG_TMView
    {
        public string Ma_NCC { get; set; }
        public string Hieu_Luc { get; set; }
        public string Thoi_Gian { get; set; }
        public string Dia_Diem { get; set; }
        public string Dieu_Kien { get; set; }
        public string BH { get; set; }
        public string Che_Do { get; set; }
        public bool Van_Chuyen { get; set; }

        public string Ghi_Chu
        {
            get; set;
        }
    }
}
