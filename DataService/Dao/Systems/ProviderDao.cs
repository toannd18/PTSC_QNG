using Data.Data;
using DataModel.Model.Systems;
using System.Collections.Generic;
using System.Linq;

namespace DataService.Dao.Systems
{
    public class ProviderDao
    {
        private readonly EFDbContext db;

        public ProviderDao()
        {
            db = new EFDbContext();
        }

        public List<tbl_NCC> load()
        {
            return db.tbl_NCC.OrderBy(m => m.Ma_NCC).ToList();
        }

        public ProviderModel detail(string ma)
        {
            var tbl = db.tbl_NCC.Where(m => m.Ma_NCC.Equals(ma)).Select(m => new ProviderModel
            {
                Ma_NCC = ma,
                Ten_NCC = m.Ten_NCC,
                Dia_Chi = m.Dia_Chi,
                Tel = m.Tel,
                Fax = m.Fax,
                Attn = m.Attn,
                Email = m.Email,
                Hang_Hoa = m.Hang_Hoa,
                Dich_Vu = m.Dich_Vu,
                Diem = m.Diem
            }).FirstOrDefault();
            return tbl;
        }

        public bool Save(ProviderModel model)
        {
            try
            {
                tbl_NCC tbl = new tbl_NCC();
                tbl.Ma_NCC = model.Ma_NCC;
                tbl.Ten_NCC = model.Ten_NCC;
                tbl.Dia_Chi = model.Dia_Chi;
                tbl.Tel = model.Tel;
                tbl.Fax = model.Fax;
                tbl.Attn = model.Attn;
                tbl.Email = model.Email;
                tbl.Hang_Hoa = model.Hang_Hoa;
                tbl.Dich_Vu = model.Dich_Vu;
                db.tbl_NCC.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ProviderModel model)
        {
            try
            {
                tbl_NCC tbl = db.tbl_NCC.Find(model.Ma_NCC);
                tbl.Ten_NCC = model.Ten_NCC;
                tbl.Dia_Chi = model.Dia_Chi;
                tbl.Tel = model.Tel;
                tbl.Fax = model.Fax;
                tbl.Attn = model.Attn;
                tbl.Email = model.Email;
                tbl.Hang_Hoa = model.Hang_Hoa;
                tbl.Dich_Vu = model.Dich_Vu;
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
            tbl_NCC tbl = db.tbl_NCC.Find(ma);
            db.tbl_NCC.Remove(tbl);
            db.SaveChanges();
            return true;
        }
    }
}