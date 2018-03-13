using Data.Data;
using DataModel.Model.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Applications
{
    public class ApprovalDao
    {
        private readonly EFDbContext db;
        public ApprovalDao()
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
            if (sub.ToList().Exists(m => m.UserName == user && string.IsNullOrWhiteSpace(m.Ma_TO)))
            {
                tbl = (from l in db.tbl_List_Request
                       join d in db.tbl_BP on l.Ma_BP equals d.Ma_BP
                       join u in sub on l.User_Nhap equals u.UserName
                       where (u.Ma_BP == sub.Where(m => m.UserName == user).Select(m => m.Ma_BP).FirstOrDefault() &&
                       u.Display < sub.Where(m => m.UserName == user).Select(m => m.Display).FirstOrDefault()) ||
                       l.User_Autho == user|| l.User_Edit==user
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
                       l.User_Edit == user||l.User_Autho==user
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
            return tbl;
        }
      
    }
}