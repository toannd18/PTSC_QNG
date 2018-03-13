
using DataModel.Model.Systems;
using DataService.Dao.Systems;
using MVC6.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC6.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
  
        // GET: Teams
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "TEAM"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult Load()
        {
            var data = new TeamDao().load();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string ma)
        {
            TeamModel model = new TeamModel();
            if (!string.IsNullOrWhiteSpace(ma))
            {
                if (!new PermissionAttribute().PermissionAuthorization("Update", "TEAM"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                model = new TeamDao().detail(ma);
                model.isUpdate = true;
            }
            else
            {
                if (!new PermissionAttribute().PermissionAuthorization("Create", "TEAM"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.Dept = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
            return PartialView("_DetailTeam", model);
        }
        [HttpPost]
        public ActionResult Save(TeamModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.isUpdate)
                {
                    status = new TeamDao().Update(model);
                }
                else
                {
                    status = new TeamDao().Save(model);
                }
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Dept = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
            return PartialView("_DetailTeam", model);
        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            if (!new PermissionAttribute().PermissionAuthorization("Delete", "TEAM"))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            bool status = new TeamDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);

        }
    }
    
}