using Data.Data;
using DataModel.Model.Applications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class RequestDao
    {
        private readonly EFDbContext db;
        public RequestDao()
        {
            db = new EFDbContext();
        }
        public List<RequestModel> load(string user)
        {
            List<RequestModel> tbl = new List<RequestModel>();
            var sub = (from a in db.AppUsers
                        join p in db.tbl_CV on a.Ma_CV equals p.Ma_CV
                     select new
                     {
                         UserName = a.UserName,
                         FullName = a.FullName,
                         Ma_TO = a.Ma_TO,
                         Ma_BP = a.Ma_BP,
                         Display = p.Display
                     });
            if (string.IsNullOrEmpty(user))
            {
                tbl = (from l in db.tbl_List_Request
                       join d in db.tbl_BP on l.Ma_BP equals d.Ma_BP
                       join u in sub on l.User_Nhap equals u.UserName
                       select new RequestModel
                       {
                           Id = l.Id,
                           FirstId=l.FirstId,
                           LateId=l.LateId,
                           Ma_BP = l.Ma_BP,
                           Ten_BP = d.Ten_BP,
                           Dia_Diem = l.Dia_Diem,
                           Hang_Muc = l.Hang_Muc,
                           HopDong = l.HopDong,
                           DeXuat = l.DeXuat,
                           CO = l.CO,
                           CQ = l.CQ,
                           Other = l.Other,
                           Note = l.Note,
                           User_Nhap = l.User_Nhap,
                           Date_Nhap = l.Date_Nhap,
                           User_Edit = l.User_Edit,
                           Date_Edit = l.Date_Edit,
                           User_Autho = l.User_Autho,
                           Date_Autho = l.Date_Autho,
                           Status_Autho = l.Status_Autho,
                           Note_Autho = l.Note_Autho,
                           Date = l.Date,
                           FullName=u.FullName
                       }).ToList();
            }
            else
            {
                if (sub.ToList().Exists(m => m.UserName == user && string.IsNullOrWhiteSpace(m.Ma_TO)))
                {
                    tbl = (from l in db.tbl_List_Request
                           join d in db.tbl_BP on l.Ma_BP equals d.Ma_BP
                           join u in sub on l.User_Nhap equals u.UserName
                           where (u.Ma_BP == sub.Where(m => m.UserName == user).Select(m => m.Ma_BP).FirstOrDefault() &&
                           u.Display < sub.Where(m => m.UserName == user).Select(m => m.Display).FirstOrDefault()) ||
                           l.User_Nhap == user
                           select new RequestModel
                           {
                               Id = l.Id,
                               FirstId = l.FirstId,
                               LateId = l.LateId,
                               Ma_BP = l.Ma_BP,
                               Ten_BP = d.Ten_BP,
                               Dia_Diem = l.Dia_Diem,
                               Hang_Muc = l.Hang_Muc,
                               HopDong = l.HopDong,
                               DeXuat = l.DeXuat,
                               CO = l.CO,
                               CQ = l.CQ,
                               Other = l.Other,
                               Note = l.Note,
                               User_Nhap = l.User_Nhap,
                               Date_Nhap = l.Date_Nhap,
                               User_Edit = l.User_Edit,
                               Date_Edit = l.Date_Edit,
                               User_Autho = l.User_Autho,
                               Date_Autho = l.Date_Autho,
                               Status_Autho = l.Status_Autho,
                               Note_Autho = l.Note_Autho,
                               Date = l.Date,
                               FullName = u.FullName
                           }).ToList();
                }
                else
                {
                    tbl = (from l in db.tbl_List_Request
                           join d in db.tbl_BP on l.Ma_BP equals d.Ma_BP
                           join u in sub on l.User_Nhap equals u.UserName
                           where (u.Ma_TO == sub.Where(m => m.UserName == user).Select(m => m.Ma_TO).FirstOrDefault() &&
                           u.Display < sub.Where(m => m.UserName == user).Select(m => m.Display).FirstOrDefault()) ||
                           l.User_Nhap == user
                           select new RequestModel
                           {
                               Id = l.Id,
                               FirstId = l.FirstId,
                               LateId = l.LateId,
                               Ma_BP = l.Ma_BP,
                               Ten_BP = d.Ten_BP,
                               Dia_Diem = l.Dia_Diem,
                               Hang_Muc = l.Hang_Muc,
                               HopDong = l.HopDong,
                               DeXuat = l.DeXuat,
                               CO = l.CO,
                               CQ = l.CQ,
                               Other = l.Other,
                               Note = l.Note,
                               User_Nhap = l.User_Nhap,
                               Date_Nhap = l.Date_Nhap,
                               User_Edit = l.User_Edit,
                               Date_Edit = l.Date_Edit,
                               User_Autho = l.User_Autho,
                               Date_Autho = l.Date_Autho,
                               Status_Autho = l.Status_Autho,
                               Note_Autho = l.Note_Autho,
                               Date = l.Date,
                               FullName=u.FullName
                           }).ToList();
                }
            }
            
            return tbl;
        }
        public RequestModel detail(int ma)
        {
            var tbl = db.tbl_List_Request.Where(m => m.Id.Equals(ma)).Select(l => new RequestModel
            {
                Id = l.Id,
                FirstId = l.FirstId,
                LateId = l.LateId,
                Ma_BP = l.Ma_BP,
                Dia_Diem = l.Dia_Diem,
                Hang_Muc = l.Hang_Muc,
                HopDong = l.HopDong,
                DeXuat = l.DeXuat,
                CO = l.CO,
                CQ = l.CQ,
                Other = l.Other,
                Note = l.Note,
                User_Nhap = l.User_Nhap,
                Date_Nhap = l.Date_Nhap,
                User_Edit = l.User_Edit,
                Date_Edit = l.Date_Edit,
                User_Autho = l.User_Autho,
                Date_Autho = l.Date_Autho,
                Status_Autho = l.Status_Autho,
                Note_Autho = l.Note_Autho,
                Date = l.Date
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(RequestModel model)
        {
            try
            {
                tbl_List_Request tbl = new tbl_List_Request();
                tbl.LateId = model.LateId;
                tbl.FirstId = model.FirstId;
                tbl.CO = model.CO;
                tbl.CQ = model.CQ;
                tbl.Date = model.Date;
                tbl.DeXuat = model.DeXuat;
                tbl.Dia_Diem = model.Dia_Diem;
                tbl.Hang_Muc = model.Hang_Muc;
                tbl.HopDong = model.HopDong;
                tbl.Ma_BP = model.Ma_BP;
                tbl.Note = model.Note;
                tbl.Other = model.Other;
                tbl.User_Nhap = model.User_Nhap;
                tbl.Date_Nhap = DateTime.Now;
                db.tbl_List_Request.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool Update(RequestModel model)
        {
            try
            {
                tbl_List_Request tbl = db.tbl_List_Request.Find(model.Id);
                tbl.CO = model.CO;
                tbl.CQ = model.CQ;
                tbl.Date = model.Date;
                tbl.DeXuat = model.DeXuat;
                tbl.Dia_Diem = model.Dia_Diem;
                tbl.Hang_Muc = model.Hang_Muc;
                tbl.HopDong = model.HopDong;
                tbl.Ma_BP = model.Ma_BP;
                tbl.Note = model.Note;
                tbl.Other = model.Other;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool Delete(int ma)
        {
            var tbl = db.tbl_List_Request.Find(ma);
            if (tbl.Status_Autho!=null)
            {
                return false;
            }
            db.tbl_List_Request.Remove(tbl);
            db.SaveChanges();
            return true;
        }
        public bool User_Autho(int ma,string name)
        {
            try
            {
                var tbl = db.tbl_List_Request.Find(ma);
                tbl.User_Autho = name;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
          
        }
        public bool User_Edit(int ma,string name)
        {
            try
            {
                var tbl = db.tbl_List_Request.Find(ma);
                tbl.User_Edit = name;
                tbl.Date_Edit = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}