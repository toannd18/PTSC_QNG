using DataModel.Model.DailyReport;
using DataService.Dao.DailyReports;
using DataService.Dao.Systems;
using MVC6.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC6.Areas.Daily.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        
        // GET: Daily/Reports
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "DAILYREPORT"))
            {
                return RedirectToAction("Login", "Account",new {Area="" });
            }
            return View();
        }
        public ActionResult Load()
        {
            string user = HttpContext.User.Identity.Name;
            var data = new DailyReportDao().load().Where(m => m.UserName == user).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Chi tiết nhật ký
        public ActionResult Detail(int ma)
        {
            ReportModel model = new ReportModel();
            
            if (ma > 0)
            {
                if (new DailyReportDao().CheckPermission(ma))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (!new PermissionAttribute().PermissionAuthorization("Update", "DAILYREPORT"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                model = new DailyReportDao().load().Where(m => m.Id == ma).FirstOrDefault();
            }
            else
            {
              
                if (!new PermissionAttribute().PermissionAuthorization("Create", "DAILYREPORT"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return PartialView("_DetailDaily", model);
        }

        [HttpPost]
        // Lưu nhật ký
        public ActionResult Save(ReportModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.Id > 0)
                {
                    status = new DailyReportDao().Update(model);
                }
                else
                    status = new DailyReportDao().Save(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_DetailDaily", model);
        }

        [HttpPost]
                // Xóa Nhật ký
        public ActionResult Delete(int ma)
        {
            if (new DailyReportDao().CheckPermission(ma))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (!new PermissionAttribute().PermissionAuthorization("Delete", "DAILYREPORT"))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            bool status = new DailyReportDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailyDetail(int ma)
        {
            var model = new DailyReportDao().detail(ma);
            return View(model);
        }
        public ActionResult LoadDaily(int ma)
        {
            var data = new DailyDetailDao().load(ma);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailReport(long mad, int ma)
        {
            if (new DailyReportDao().CheckPermission(ma))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            DailyModel model = new DailyModel();
            if (mad > 0)
            {
                model = new DailyDetailDao().detail(mad, ma);
            }
            else
            {
                model.DailyId = ma;
            }
            ViewBag.Job = new SelectList(new JobDao().GetWithUser(), "Id", "Ten_Job");
            return PartialView("_DetailDailyReport", model);
        }
        [HttpPost]
        public ActionResult SaveDaily(DailyModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.Id > 0)
                {
                    status = new DailyDetailDao().Update(model);
                }
                else
                {
                    status = new DailyDetailDao().Save(model);
                }
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Job = new SelectList(new JobDao().GetWithUser(), "Id", "Ten_Job");
            return PartialView("_DetailDailyReport", model);
        }
        [HttpPost]
        public ActionResult DeleteDaily(long ma,int mad)
        {
            if (new DailyReportDao().CheckPermission(mad))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            bool status = new DailyDetailDao().Delete(ma);
            return Json(new { status = status },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendRequest(int ma)
        {
            try
            {
                if (new DailyReportDao().CheckPermission(ma))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                List<string> received = new DailyDetailDao().SendRequest(ma);
                bool status = received.Count < 0 ? false : true;
                if (status)
                {
                    Notification_Hub.NotificationHub.Send(received);
                }

                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}