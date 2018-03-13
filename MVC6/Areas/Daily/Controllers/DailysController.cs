
using Data.Data;
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
    public class DailysController : Controller
    {
       
        // GET: Daily/Dailys
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "DAILYDAIRY"))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            var user = new UserDao().load().Where(m => m.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            ViewBag.Check = 0;
            if (string.IsNullOrEmpty(user.Ma_TO))
            {
                if (user.Ma_BP == "BGD")
                {
                    ViewBag.Dept = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
                    ViewBag.TO = new SelectList(new TeamDao().load(), "Ma_TO", "Ten_TO");
                    ViewBag.Check = 1;
                }
                else
                {
                    ViewBag.TO = new SelectList(new TeamDao().load().Where(m => m.Ma_BP == user.Ma_BP).ToList(), "Ma_TO", "Ten_TO");
                    ViewBag.Check = 2;
                }
            }
           
            return View();
        }
        public ActionResult Load(string Ma_TO,string Ma_BP)
        {
            var data = new DailyReportDao().GetAllTest(Ma_TO,Ma_BP).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int ma)
        {
            string user = HttpContext.User.Identity.Name;
            string url= "/Daily/Dailys/Detail?ma=" + ma.ToString();
            EFDbContext db = new EFDbContext();
            var check = (from u in db.AppUsers
                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                         where u.UserName==user
                         select new
                         {
                             Ma_To = u.Ma_TO,
                             Display = c.Display
                         }).FirstOrDefault();
            ViewBag.Check = check.Ma_To;
            ViewBag.CV = check.Display;
            var model = new DailyReportDao().detail(ma);
            Notification_Hub.NotificationService.RemoveNotification(url, user);
            return View(model);
        }
        public ActionResult LoadDaily(int ma)
        {
            var data = new DailyDetailDao().load(ma);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Comment(long ma, string comment,int level,int check)
        {
            
            bool status = false;
           
            status = new DailyDetailDao().Comment(ma, comment, level,check);
 
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CommentDetail(long mad, int ma,int check)
        {
            if (check == 3)
            {
                if (new DailyReportDao().CheckPermission(ma))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var model = new DailyDetailDao().detail(mad, ma);
                model.Level_1=model.Level_1.HasValue?model.Level_1 : 3;
                return PartialView("_CommentTeam", model);
            }
            else if(check==4)
            {
                

                var model = new DailyDetailDao().detail(mad, ma);
                model.Level_2 = model.Level_2.HasValue ? model.Level_2 : 3;
               
                return PartialView("_CommentLead", model);
            }
            else
            {
                var model = new DailyDetailDao().detail(mad, ma);
                model.Level_3 = model.Level_3.HasValue ? model.Level_3 : 3;

                return PartialView("_CommentLead 2", model);
            }
        }
        [HttpPost]
        public ActionResult CommentAll(int ma,string comment, int level,int check)
        {
            bool status = false;
            if (check > 3)
                status = new DailyDetailDao().CommentAll(ma, comment, level, check);
            else
            {
                if (new DailyReportDao().CheckPermission(ma))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                status = new DailyDetailDao().CommentAll(ma, comment, level, check);
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadTeam(string Ma_BP)
        {
            var data = new TeamDao().load().Where(m => m.Ma_BP.Contains(Ma_BP)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}