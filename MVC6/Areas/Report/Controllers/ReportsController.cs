using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Data.Data;
using System.Data.Entity.SqlServer;

using DataModel.Model.Reports;
using DataService.Dao.ExportReports;
using DataService.Dao.Systems;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace MVC6.Areas.Report.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        
        // GET: Report/Reports
        public ActionResult ExportDaily(int ma)
        {
            ViewBag.Id = ma;
            return View();
        }

        public ActionResult DocumentViewerPartial(int ma)
        {
            ViewBag.Id = ma;
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         join u in db.AppUsers on d.UserName equals u.UserName
                         join u1 in db.AppUsers on d.User_Autho1 equals u1.UserName
                         join u2 in db.AppUsers on d.User_Autho2 equals u2.UserName
                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                         join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO
                         where d.Id == ma
                         select new ExportDailyModel
                         {
                             DailyId = d.Id,
                             Id = dd.Id,
                             FullName = u.FullName,
                             FullName1 = u1.FullName,
                             FullName2 = u2.FullName,
                             Date = d.Date,
                             FromTime = dd.FormTime,
                             ToTime = dd.ToTime,
                             Total = d.Total_Job,
                             Content = dd.Content_Job,
                             Method = dd.Method,
                             Result = dd.Result,
                             Comment1 = dd.Comment1,
                             Comment2 = dd.Comment2,
                             Ten_TO = t.Ten_TO,
                             Ten_CV = c.Ten_CV
                         }).ToList();
            foreach (var item in model)
            {
                item.Total = item.Total.Replace("</br>", "\n");
            }
            return PartialView("_DocumentViewerPartial", model);
        }

        public ActionResult DocumentViewerPartialExport(int ma)
        {
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         join u in db.AppUsers on d.UserName equals u.UserName
                         join u1 in db.AppUsers on d.User_Autho1 equals u1.UserName
                         join u2 in db.AppUsers on d.User_Autho2 equals u2.UserName
                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                         join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO
                         where d.Id == ma
                         select new ExportDailyModel
                         {
                             DailyId = d.Id,
                             Id = dd.Id,
                             FullName = u.FullName,
                             FullName1 = u1.FullName,
                             FullName2 = u2.FullName,
                             Date = d.Date,
                             FromTime = dd.FormTime,
                             ToTime = dd.ToTime,
                             Total = d.Total_Job,
                             Content = dd.Content_Job,
                             Method = dd.Method,
                             Result = dd.Result,
                             Comment1 = dd.Comment1,
                             Comment2 = dd.Comment2,
                             Ten_TO = t.Ten_TO,
                             Ten_CV = c.Ten_CV
                         }).ToList();
            XtraReportDaily report = new XtraReportDaily();
            report.DataSource = model;
            return DocumentViewerExtension.ExportTo(report);
        }
        public ActionResult SearchLevel()
        {
            var user = new ExportReportDao().GetLevelUser(HttpContext.User.Identity.Name);
            if (!string.IsNullOrEmpty(user.Ma_TO))
            {
                ViewBag.Check = 1;
            }
            else
            {
                ViewBag.Check = 0;
                user.Ma_BP = user.Ma_BP == "BGD" ? "" : user.Ma_BP;
                ViewBag.MaTo = new SelectList(new TeamDao().load().Where(m => m.Ma_BP.Contains(user.Ma_BP)).ToList(),"Ma_TO","Ten_TO");
            }
            
            return View();
        }
        public ActionResult SearchLoad(DateTime FromTime,DateTime ToTime,string Ma_TO)
        {
            var user = new ExportReportDao().GetLevelUser(HttpContext.User.Identity.Name);
                       
                Ma_TO = Ma_TO == "undefined"?user.Ma_TO: Ma_TO;

                user.Ma_BP = user.Ma_BP == "BGD" ? "" : user.Ma_BP;
            List<LevelReportModel> data = new List<LevelReportModel>();
            if (user.Display == 1)
            {
                data = new ExportReportDao().ReprotLevel(FromTime, ToTime, user.Ma_BP, Ma_TO).Where(m => m.UserName == user.UserName).ToList();
            }
           else
                data = new ExportReportDao().ReprotLevel(FromTime, ToTime, user.Ma_BP, Ma_TO);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportGridDaily(DateTime FromTime,DateTime Totime)
        {
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         where d.UserName == HttpContext.User.Identity.Name && d.Date >= FromTime && d.Date <= Totime
                         select new
                         {
                             Thoi_Gian = d.Date,
                             Noi_Dung = dd.Content_Job,
                             Phuong_Phap = dd.Method,
                             Ket_Qua = dd.Result,
                             Danh_Gia_TT = "Mức " + dd.Level_1,
                             Y_Kien_TT=dd.Comment1,
                             Danh_Gia_PT = "Mức " + dd.Level_2,
                             Y_Kien_PT = dd.Comment2,
                             Danh_Gia_TP = "Mức " + dd.Level_3,
                             Y_Kien_TP = dd.Comment3,

                         }).ToList().Select(m=>new {
                             Thoi_Gian = m.Thoi_Gian.ToString("dd/MM/yyyy"),
                             Noi_Dung = m.Noi_Dung,
                             Phuong_Phap = m.Phuong_Phap,
                             Ket_Qua = m.Ket_Qua,
                             Danh_Gia_TT = m.Danh_Gia_TT,
                             Y_Kien_TT = m.Y_Kien_TT,
                             Danh_Gia_PT = m.Danh_Gia_PT,
                             Y_Kien_PT = m.Y_Kien_PT,
                             Danh_Gia_TP =m.Danh_Gia_TP,
                             Y_Kien_TP=m.Y_Kien_TP
                         }).ToList();
           
            var grid = new GridView();
            grid.DataSource = model;
            grid.DataBind();
            string FileName = "Từ " + FromTime.ToString("dd/MM/yyyy") + " Tới " + Totime.ToString("dd/MM/yyyy") + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename="+FileName);
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
    }
}