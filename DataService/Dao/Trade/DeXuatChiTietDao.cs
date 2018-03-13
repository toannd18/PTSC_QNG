using Data.Data;
using DataModel.Model.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Dao.Trade
{
    public class DeXuatChiTietDao
    {
        private readonly EFDbContext db;
        private tbl_DeXuat_KT tbl;
        public DeXuatChiTietDao()
        {
            db = new EFDbContext();
            tbl = new tbl_DeXuat_KT();
        }
        public IList<tbl_DeXuat_KT> Load(int DxId)
        {
            return db.tbl_DeXuat_KT.Where(m => m.DeXuatId == DxId).ToList();
        }
        public DeXuatChiTietModel Detail(int Id)
        {
            var model = db.tbl_DeXuat_KT.Where(m => m.Id == Id).Select(m => new DeXuatChiTietModel
            {
                Id = m.Id,
                DeXuatId = m.DeXuatId,
                Ten = m.Ten,
                Mo_Ta = m.Mo_Ta,
                SoLuong = m.SoLuong,
                DVT = m.DVT,
                Ghi_Chu=m.Ghi_Chu,
             
            }).FirstOrDefault();
            return model;
        }
        public bool Save(DeXuatChiTietModel m)
        {
            try
            {
                tbl.DeXuatId = m.DeXuatId;
                tbl.Ten = m.Ten;
                tbl.Mo_Ta = m.Mo_Ta;
                tbl.SoLuong = m.SoLuong;
                tbl.DVT = m.DVT;
                tbl.Ghi_Chu = m.Ghi_Chu;
              

                db.tbl_DeXuat_KT.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(DeXuatChiTietModel m)
        {
            try
            {
                
                tbl = db.tbl_DeXuat_KT.Find(m.Id);
                tbl.Ten = m.Ten;
                tbl.Mo_Ta = m.Mo_Ta;
                tbl.SoLuong = m.SoLuong;
                tbl.DVT = m.DVT;
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
                tbl = db.tbl_DeXuat_KT.Find(Id);

                db.tbl_DeXuat_KT.Remove(tbl);
                db.SaveChanges();
                return true;
            } catch
            {
                return false;
            }
        }
    }
}
