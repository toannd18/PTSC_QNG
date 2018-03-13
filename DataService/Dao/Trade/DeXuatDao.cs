using Data.Data;
using DataModel.Model.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Tieu_De=m.Tieu_De,
                Kieu=m.Kieu,
                Ten_Dx=m.Ten_Dx,
                Ten_Dx1 = m.Ten_Dx1,
                Ten_Dx2 = m.Ten_Dx2,
                Ten_Dx3 = m.Ten_Dx3,
                Ten_Dx4 = m.Ten_Dx4,
                Ten_Dx5 = m.Ten_Dx5,
                Ten_Dg=m.Ten_Dg,
                Ten_Dg1 = m.Ten_Dg1,
                Ten_Dg2 = m.Ten_Dg2,
                Ten_Dg3 = m.Ten_Dg3,
                Ngay_Tao=m.Ngay_Tao,
                Ngay_Gui=m.Ngay_Gui,
                Ngay_PD=m.Ngay_PD,
                Ngay_HD =m.Ngay_HD,
                Ngay_PHD = m.Ngay_PHD,
                Ngay_Eval=m.Ngay_Eval,
                Ngay_Exp=m.Ngay_Exp
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

        public bool CheckExistDX(string Ma)
        {
            return !db.tbl_DeXuat.Any(m => m.Ma == Ma);
        }
    }
}
