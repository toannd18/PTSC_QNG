using Data.Data;
using DataModel.Model.Systems;
using DataModel.Model.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class CheckDao
    {
        private readonly EFDbContext db;
        public CheckDao()
        {
            db = new EFDbContext();
        }
        public List<CheckModel> FindAll()
        {
            var tbl = (from c in db.tbl_List_Check
                       join n in db.tbl_NCC on c.Ma_NCC equals n.Ma_NCC
                       
                       
                       join r in db.tbl_List_Request on c.RequestId equals r.Id
                       join d in db.tbl_BP on r.Ma_BP equals d.Ma_BP
                       select new CheckModel
                       {
                           Id = c.Id,
                           RequestId = c.RequestId,
                           Ma_NCC = c.Ma_NCC,
                           Ten_NCC = n.Ten_NCC,
                           Ma_TB = c.Ma_TB,
                          
                           YC_KT = c.YC_KT,
                           TT_KT = c.TT_KT,
                           YC_SL = c.YC_SL,
                           TT_SL = c.TT_SL,
                           DonVi = c.DonVi,
                           CO = c.CO,
                           CQ = c.CQ,
                           MTR = c.MTR,
                           SN = c.SN,
                           PN = c.PN,
                           Other = c.Other,
                           Note_Other = c.Note_Other,
                           Reason = c.Reason,
                           Result = c.Result,
                           User_Nhap = c.User_Nhap,
                           Date_Nhap = c.Date_Nhap,
                           Date_Edit = c.Date_Edit,
                           User_Edit = c.User_Edit,
                           DeXuat = r.DeXuat,
                           HopDong = r.HopDong,
                           DiaDiem = r.Dia_Diem,
                           Date = r.Date,
                           Ten_BP = d.Ten_BP,
                       }).ToList();
            return tbl;
        }
        public List<CheckModel> load(int id)
        {
            var tbl = (from c in db.tbl_List_Check
                       join n in db.tbl_NCC on c.Ma_NCC equals n.Ma_NCC
                       
                       where c.RequestId.Equals(id)
                       join r in db.tbl_List_Request on c.RequestId equals r.Id
                       join d in db.tbl_BP on r.Ma_BP equals d.Ma_BP
                       select new CheckModel
                       {
                           Id = c.Id,
                           RequestId = c.RequestId,
                           Ma_NCC = c.Ma_NCC,
                           Ten_NCC = n.Ten_NCC,
                           Ma_TB = c.Ma_TB,
                          
                           YC_KT = c.YC_KT,
                           TT_KT = c.TT_KT,
                           YC_SL = c.YC_SL,
                           TT_SL = c.TT_SL,
                           DonVi = c.DonVi,
                           CO = c.CO,
                           CQ = c.CQ,
                           MTR = c.MTR,
                           SN = c.SN,
                           PN = c.PN,
                           Other = c.Other,
                           Note_Other = c.Note_Other,
                           Reason = c.Reason,
                           Result = c.Result,
                           User_Nhap = c.User_Nhap,
                           Date_Nhap = c.Date_Nhap,
                           Date_Edit = c.Date_Edit,
                           User_Edit = c.User_Edit,
                           DeXuat=r.DeXuat,
                           HopDong=r.HopDong,
                           DiaDiem=r.Dia_Diem,
                           Date=r.Date,
                           Ten_BP=d.Ten_BP,
                       }).ToList();
            return tbl;
        }
        public  CheckModel detail(long ma, int id)
        {
            var tbl = new CheckDao().load(id).Where(m=>m.Id.Equals(ma)).FirstOrDefault();
            return tbl;
        }
        public bool Save (CheckModel model)
        {
            try
            {
                tbl_List_Check tbl = new tbl_List_Check();

                tbl.RequestId = model.RequestId;
                tbl.Ma_NCC = model.Ma_NCC;
                tbl.Ma_TB = model.Ma_TB;
                tbl.YC_KT = model.YC_KT;
                tbl.TT_KT = model.TT_KT;
                tbl.YC_SL = model.YC_SL;
                tbl.TT_SL = model.TT_SL;
                tbl.DonVi = model.DonVi;
                tbl.User_Nhap = model.User_Nhap;
                tbl.Date_Nhap = DateTime.Now;
                db.tbl_List_Check.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(CheckModel model)
        {
            try
            {
                tbl_List_Check tbl = new tbl_List_Check();
                tbl = db.tbl_List_Check.Find(model.Id);
                tbl.Ma_NCC = model.Ma_NCC;
                tbl.Ma_TB = model.Ma_TB;
                tbl.YC_KT = model.YC_KT;
                tbl.TT_KT = model.TT_KT;
                tbl.YC_SL = model.YC_SL;
                tbl.TT_SL = model.TT_SL;
                tbl.DonVi = model.DonVi;
                tbl.User_Edit = model.User_Nhap;
                tbl.Date_Edit = DateTime.Now;
               
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete (long ma)
        {
            var tbl = db.tbl_List_Check.Find(ma);
            if (new Commom.Commom().CheckPermission(tbl.RequestId))
            {
                return false;
            }
            if (tbl.Result != null)
            {
                return false;
            }
           
            db.tbl_List_Check.Remove(tbl);
            db.SaveChanges();
            return true;
        }
        public tbl_List_Check LoadReason(long ma)
        {
            return db.tbl_List_Check.Find(ma);        
         }
        public void SaveAutho(int ma,string result)
        {
            var tbl = db.tbl_List_Request.Find(ma);
            tbl.Status_Autho = result;
            tbl.Date_Autho = DateTime.Now;
            db.SaveChanges();
        }
    }
}