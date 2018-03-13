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
    public class EquipmentsController : Controller
    {
    
        // GET: Equipments
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load()
        {
            var data = new EquipmentDao().load();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string ma)
        {
            ViewBag.Provider = new SelectList(new ProviderDao().load(), "Ma_NCC", "Ten_NCC");
            EquipmentModel model = new EquipmentModel();
            if (!string.IsNullOrEmpty(ma))
            {
                model = new EquipmentDao().detail(ma);
                model.IsUpdate = true;
            }

            return PartialView("_DetailEquipment", model);
        }
        [HttpPost]
        public ActionResult Save(EquipmentModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.IsUpdate)
                {
                    status = new EquipmentDao().Update(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = new EquipmentDao().Save(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
            }
            return PartialView("_DetailEquipment", model);
        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            bool status = false;
            status = new EquipmentDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}