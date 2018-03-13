
using Data.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Model.Systems;
using DataService.Dao.Systems;

namespace MVC6.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
  
        //
        // GET: /Roles/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load()
        {

            var model = new RoleDao().load();
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int ma)
        {
            RoleModel tbl = new RoleModel();
            if (ma > 0)
            {
                tbl = new RoleDao().detail(ma);
                return PartialView("_DetailRole",tbl);
            }
            else
            {
                return PartialView("_DetailRole", tbl);
            }
        }
        [HttpPost]
        public ActionResult Save(RoleModel model)
        {
            bool status;
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    status = new RoleDao().Update(model);
                }
                else
                    status = new RoleDao().Save(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            else
                return PartialView("_DetailRole", model);
        }
        [HttpPost]
        public ActionResult Delete(int ma)
        {
            bool status = new RoleDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckExist(string Role)
        {
            bool status = new RoleDao().CheckExistRole(Role);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}