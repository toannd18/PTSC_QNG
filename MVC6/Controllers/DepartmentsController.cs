using DataModel.Model.Systems;
using DataService.Dao.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC6.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
       
        // GET: Departments
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load()
        {
            var data = new DepartmentDao().load();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string ma)
        {
            DepartmentModel model = new DepartmentModel();
            if (!string.IsNullOrEmpty(ma))
            {
                model = new DepartmentDao().detail(ma);
                model.IsUpdate = true;
            }

            return PartialView("_DetailDepartment", model);
        }
        [HttpPost]
        public ActionResult Save(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.IsUpdate)
                {
                    status = new DepartmentDao().Update(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = new DepartmentDao().Save(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
            }
            return PartialView("_DetailDepartment", model);
        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            bool status = false;
            status = new DepartmentDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}