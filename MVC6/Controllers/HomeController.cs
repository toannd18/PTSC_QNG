using DataService.Dao.Trade;
using MVC6.Notification_Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC6.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetNotification()
        {
            var user = HttpContext.User.Identity.Name;
            var data=   NotificationService.GetNotification(user).Where(m=>m.Status==true).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notification()
        {
            var user = HttpContext.User.Identity.Name;
            var data = NotificationService.GetNotification(user);
            return View(data);
        }
       
    }
}