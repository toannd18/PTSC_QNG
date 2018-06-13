using Data.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Dao.Trade
{
    public class DGKTDao
    {
        private readonly EFDbContext db;
        public DGKTDao()
        {
            db = new EFDbContext();
        }
        public List<DGKTModel> Load(int dx)
        {
            var model = (from t in db.tbl_DeXuat_KT
                         join t1 in db.tbl_DG_KT on t.Id equals t1.DG_KT_Id into g
                         from t1 in g.DefaultIfEmpty()
                         join n in db.tbl_DG_NCC on t1.DG_NCC_Id equals n.Id into g1
                         from n in g1.DefaultIfEmpty()
                         where t.DeXuatId==dx
                         select new
                         {
                             Ma = t.Id,
                             Ten = t.Ten,
                             MoTa = t.Mo_Ta,
                             NCC = n.Ma_NCC,
                             So_luong = t.SoLuong,
                             DVT = t.DVT,
                             Ten_DG = t1.Ten,
                             MoTa_DG = t1.Mo_Ta,
                             DG= n.Ma_NCC == null?false:t1.DG,
                             Ghi_Chu=t1.Ghi_Chu
                         }).GroupBy(m => new { m.Ma }).Select(m => new DGKTModel
                         {
                             Ma = m.Key.Ma,
                             Ten = m.Select(t => t.Ten).FirstOrDefault(),
                             MoTa = m.Select(t => t.MoTa).FirstOrDefault(),
                             So_luong = m.Select(t => t.So_luong).FirstOrDefault(),
                             DVT = m.Select(t => t.DVT).FirstOrDefault(),
                             subject = m.Select(t => new Subject { NCC = t.NCC, Ten_DG = t.Ten_DG, MoTa_DG = t.MoTa_DG, DG=t.DG, Ghi_Chu=t.Ghi_Chu }).ToList()
                         }).ToList();
            return model;
        }
        public List<tbl_DG_KT> Detail(int dg_kt)
        {
            var model = db.tbl_DG_KT.Where(m => m.DG_KT_Id == dg_kt).ToList();
            return model;
        }
        public bool Save(tbl_DG_KT model)
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
        public bool SaveDG(int ma,int DG)
        {
            try
            {
                var model = db.tbl_DG_KT.Find(ma);
                model.Don_Gia = DG;
                db.SaveChanges();
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
                tbl_DG_KT tbl = db.tbl_DG_KT.Find(Id);
                db.tbl_DG_KT.Remove(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


    public class DGKTModel
    {
        public int Ma { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public string DVT { get; set; }
        public int So_luong { get; set; }
        public List<Subject> subject { get; set; }

    }
    public class Subject
    {
        public string NCC { get; set; }
        public string Ten_DG { get; set; }
        public string MoTa_DG { get; set; }
        public bool DG { get; set; }
        public string Ghi_Chu { get; set; }

    }

}
