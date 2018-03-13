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
    public class PositionsController : Controller
    {
        // GET: Positions
        
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "POSITION"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult Load()
        {
            var data = new PositionDao().load();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string ma)
        {
            PositionModel model = new PositionModel();
            
            if (!string.IsNullOrWhiteSpace(ma))
            {
                if (!new PermissionAttribute().PermissionAuthorization("Update", "POSITION"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                
                model = new PositionDao().detail(ma);
                model.isUpdate = true;
            }
            else
            {
                if (!new PermissionAttribute().PermissionAuthorization("Create", "POSITION"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return PartialView("_DetailPosition", model);
        }
        [HttpPost]
        public ActionResult Save(PositionModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.isUpdate)
                {
                    status = new PositionDao().Update(model);
                }
                else
                {
                    status = new PositionDao().Save(model);
                }
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_DetailPosition", model);
        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            if (!new PermissionAttribute().PermissionAuthorization("Delete", "POSITION"))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            bool status = new PositionDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}