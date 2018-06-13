using Data.Data;
using DataModel.Model.Trade;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Trade
{
    public class DeXuatDao
    {
        public readonly EFDbContext db;

        public DeXuatDao()
        {
            db = new EFDbContext();
        }

        public IList<tbl_DeXuat> Load()
        {
            return db.tbl_DeXuat.ToList();
        }

        public DeXuatModel Detail(int Id)
        {
            var model = db.tbl_DeXuat.Where(m => m.Id == Id).Select(m => new DeXuatModel
            {
                Id = m.Id,
                Ma = m.Ma,
                Tieu_De = m.Tieu_De,
                Kieu = m.Kieu,
                Ten_Dx = m.Ten_Dx,
                Ten_Dx1 = m.Ten_Dx1,
                Ten_Dx2 = m.Ten_Dx2,
                Ten_Dx3 = m.Ten_Dx3,
                Ten_Dx4 = m.Ten_Dx4,
                Ten_Dx5 = m.Ten_Dx5,
                Ten_Dg = m.Ten_Dg,
                Ten_Dg1 = m.Ten_Dg1,
                Ten_Dg2 = m.Ten_Dg2,
                Ten_Dg3 = m.Ten_Dg3,
                Ten_Dg4 = m.Ten_Dg4,
                Ngay_Tao = m.Ngay_Tao,
                Ngay_Gui = m.Ngay_Gui,
                Ngay_PD = m.Ngay_PD,
                Ngay_HD = m.Ngay_HD,
                Ngay_PHD = m.Ngay_PHD,
                Ngay_Eval = m.Ngay_Eval,
                Ngay_Exp = m.Ngay_Exp
            }).FirstOrDefault();

            return model;
        }

        public bool Save(DeXuatModel m)
        {
            try
            {
                tbl_DeXuat tbl = new tbl_DeXuat();
                tbl.Id = m.Id;
                tbl.Ma = m.Ma;
                tbl.Tieu_De = m.Tieu_De;
                tbl.Kieu = m.Kieu;
                tbl.Ten_Dx = m.Ten_Dx;
                tbl.Ten_Dx1 = m.Ten_Dx1;
                tbl.Ten_Dx2 = m.Ten_Dx2;
                tbl.Ten_Dx3 = m.Ten_Dx3;
                tbl.Ten_Dx4 = m.Ten_Dx4;
                tbl.Ten_Dx5 = m.Ten_Dx5;
                tbl.Ten_Dg = m.Ten_Dg;
                tbl.Ten_Dg1 = m.Ten_Dg1;
                tbl.Ten_Dg2 = m.Ten_Dg2;
                tbl.Ten_Dg3 = m.Ten_Dg3;
                tbl.Ten_Dg4 = m.Ten_Dg4;
                tbl.Ngay_Tao = m.Ngay_Tao;
                tbl.Ngay_Gui = m.Ngay_Gui;
                tbl.Ngay_PD = m.Ngay_PD;
                tbl.Ngay_HD = m.Ngay_HD;
                tbl.Ngay_PHD = m.Ngay_PHD;
                tbl.Ngay_Eval = m.Ngay_Eval;
                tbl.Ngay_Exp = m.Ngay_Exp;
                db.tbl_DeXuat.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DeXuatModel m)
        {
            try
            {
                tbl_DeXuat tbl = new tbl_DeXuat();
                tbl = db.tbl_DeXuat.Find(m.Id);

                tbl.Ma = m.Ma;
                tbl.Tieu_De = m.Tieu_De;
                tbl.Kieu = m.Kieu;
                tbl.Ten_Dx = m.Ten_Dx;
                tbl.Ten_Dx1 = m.Ten_Dx1;
                tbl.Ten_Dx2 = m.Ten_Dx2;
                tbl.Ten_Dx3 = m.Ten_Dx3;
                tbl.Ten_Dx4 = m.Ten_Dx4;
                tbl.Ten_Dx5 = m.Ten_Dx5;
                tbl.Ten_Dg = m.Ten_Dg;
                tbl.Ten_Dg1 = m.Ten_Dg1;
                tbl.Ten_Dg2 = m.Ten_Dg2;
                tbl.Ten_Dg3 = m.Ten_Dg3;
                tbl.Ten_Dg4 = m.Ten_Dg4;
                tbl.Ngay_Tao = m.Ngay_Tao;
                tbl.Ngay_Gui = m.Ngay_Gui;
                tbl.Ngay_PD = m.Ngay_PD;
                tbl.Ngay_HD = m.Ngay_HD;
                tbl.Ngay_PHD = m.Ngay_PHD;
                tbl.Ngay_Eval = m.Ngay_Eval;
                tbl.Ngay_Exp = m.Ngay_Exp;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateStatus(tbl_DeXuat m)
        {
            try
            {
                tbl_DeXuat tbl = new tbl_DeXuat();
                tbl = db.tbl_DeXuat.Find(m.Id);
                tbl.Ngay_Ky = m.Ngay_Ky;
                tbl.Ngay_TH = m.Ngay_TH;
                tbl.Ngay_THTT = m.Ngay_THTT;
                tbl.Ngay_NT = m.Ngay_THTT;
                tbl.Ngay_NT_QC = m.Ngay_NT_QC;
                tbl.Status = m.Status;
                tbl.Ghi_Chu = m.Ghi_Chu;
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
                tbl_DeXuat tbl = db.tbl_DeXuat.Find(Id);
                db.tbl_DeXuat.Remove(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<DeXuatViewModel> LoadView()
        {
            List<DeXuatViewModel> tbl = new List<DeXuatViewModel>();
            string username = HttpContext.Current.User.Identity.Name;
            var user = (from a in db.AppUsers
                        where a.UserName == username
                        join cv in db.tbl_CV on a.Ma_CV equals cv.Ma_CV
                        select new
                        {
                            username = username,
                            capdo = cv.Display,
                            Ma_BP = a.Ma_BP
                        }).FirstOrDefault();
            //var hd = new DeXuatHDDao().load();

            tbl = (from dx in db.tbl_DeXuat

                   join u1 in db.AppUsers on dx.Ten_Dx equals u1.UserName into g1
                   from sub1 in g1.DefaultIfEmpty()
                   join u2 in db.AppUsers on dx.Ten_Dx5 equals u2.UserName into g2
                   from sub2 in g2.DefaultIfEmpty()
                   join c in db.tbl_CV on sub2.Ma_CV equals c.Ma_CV
                   join hd in db.tbl_DeXuat_HD on dx.Id equals hd.DeXuatId into g3
                   from sub3 in g3.DefaultIfEmpty()
                   join ncc in db.tbl_NCC on sub3.Ma_NCC equals ncc.Ma_NCC into g4
                   from sub4 in g4.DefaultIfEmpty()
                   where (sub2.Ma_BP == user.Ma_BP && user.capdo > c.Display && user.capdo > 2) || dx.Ten_Dx5 == username || dx.Ten_Dg == username || dx.Ten_Dg1 == username || dx.Ten_Dg2 == username || dx.Ten_Dg3 == username
                   select new DeXuatViewModel
                   {
                       Id = dx.Id,
                       Ma = dx.Ma,
                       Tieu_De = dx.Tieu_De,
                       Ngay_Tao = dx.Ngay_Tao,
                       Kieu = dx.Kieu,
                       FullName_Dx = sub1.FullName,
                       FullName_TH = sub2.FullName,
                       Ngay_Ky = dx.Ngay_Ky,
                       Ngay_NT = dx.Ngay_NT,
                       Ngay_NT_QC = dx.Ngay_NT_QC,
                       Ngay_TH = dx.Ngay_TH,
                       Ngay_THTT = dx.Ngay_THTT,
                       Status = dx.Status,
                       Ghi_Chu = dx.Ghi_Chu,
                       Sohd = sub3.So_HD > 0 ? (sub3.So_HD + "-" + sub3.Date.Year.ToString() + "-PTSC-QNG-M" + (dx.Kieu ? "HH" : "DV")) : "",
                       TenNCC = sub4.Ten_NCC
                   }).ToList();

            return tbl;
        }

        public bool CheckExistDX(string Ma, string Tieu_De)
        {
            return !db.tbl_DeXuat.Any(m => m.Ma == Ma && m.Tieu_De == Tieu_De);
        }

        public bool PermissionDx(string user)
        {
            return db.tbl_DeXuat.Any(m => m.Ten_Dx5 == user || m.Ten_Dg == user || m.Ten_Dg1 == user || m.Ten_Dg2 == user || m.Ten_Dg3 == user || m.Ten_Dg4 == user);
        }

        public bool PermissionTM(string user)
        {
            return db.tbl_DeXuat.Any(m => m.Ten_Dx5 == user || m.Ten_Dg == user || m.Ten_Dg1 == user || m.Ten_Dg3 == user);
        }
    }
}