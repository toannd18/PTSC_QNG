using Data.Data;
using DataModel.Model.Trade;
using System.Collections.Generic;
using System.Linq;

namespace DataService.Dao.Trade
{
    public class DeXuatNCCDao
    {
        private readonly EFDbContext db;
        private tbl_DG_NCC tbl;

        public DeXuatNCCDao()
        {
            db = new EFDbContext();
            tbl = new tbl_DG_NCC();
        }

        public IList<DGNCCModel> Load(int ma)

        {
            var model = (from n in db.tbl_NCC
                         join d in db.tbl_DG_NCC on n.Ma_NCC equals d.Ma_NCC
                         where d.DeXuatId==ma
                         select (new DGNCCModel
                         {
                             Id = d.Id,
                             DeXuatId = d.DeXuatId,
                             Ma_NCC = d.Ma_NCC,
                             DG_KT = d.DG_KT,
                             DG_TM = d.DG_TM,
                             DG = d.DG,
                             Ten_NCC = n.Ten_NCC
                           
                         })).OrderBy(m => m.Ma_NCC).ToList();
            return model;
        }

        public DGNCCModel Detail(int Id)
        {
            return db.tbl_DG_NCC.Where(m => m.Id == Id).Select(m => new DGNCCModel
            {
                Id = m.Id,
                DeXuatId = m.DeXuatId,
                Ma_NCC = m.Ma_NCC,
                DG_KT = m.DG_KT,
                DG_TM = m.DG_TM,
                DG = m.DG
            }).FirstOrDefault();
        }

        public bool SaveNCC(DGNCCModel m)
        {
            try
            {
                tbl.DeXuatId = m.DeXuatId;
                tbl.Ma_NCC = m.Ma_NCC;
                db.tbl_DG_NCC.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNCC(DGNCCModel m)
        {
            try
            {
                tbl = db.tbl_DG_NCC.Find(m.Id);
                tbl.DG = m.DG;
                tbl.DG_KT = m.DG_KT;
                tbl.DG_TM = m.DG_TM;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteNCC(int Id)
        {
            try
            {
                tbl = db.tbl_DG_NCC.Find(Id);

                db.tbl_DG_NCC.Remove(tbl);

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateDG(int ma, int data)
        {
            try
            {
                tbl = db.tbl_DG_NCC.Find(ma);
                tbl.DG = data;
                db.SaveChanges();
                return true;
            }catch
            {
                return false;
            }
           
        }
        public bool CheckExistNCC(int dx, string ma)
        {
            return !db.tbl_DG_NCC.Any(m => m.DeXuatId == dx && m.Ma_NCC == ma);
        }
    }
}