using Data.Data;
using DataModel.Model.Trade;
using DataService.Dao.Trade;
using MVC6.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC6.Areas.Commerce.Controllers
{
    [Authorize]
    public class ContractionsController : Controller
    {
        // GET: Commerce/Contractions
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "CONTRACT"))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            return View();
        }
        public JsonResult Load()
        {
            var tbl = new DeXuatHDDao().load();
            return Json(new { data = tbl }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int Id)
        {
            DeXuatHDModel model = new DeXuatHDModel();
            if (Id > 0)
            {
                model = new DeXuatHDDao().load().Where(m=>m.Id==Id).FirstOrDefault();
            }
            else
            {
                model.Date = DateTime.Now;
                model.So_HD = new DeXuatHDDao().GetHD(model.Date);
                model.Ten_HD = model.So_HD + "-" + model.Date.Year + "-PTSC-QNG";
            }
            return PartialView("_DetailHD", model);

        }
        [HttpPost]
        public ActionResult Save(DeXuatHDModel model)
        {
            bool status;
            status = new DeXuatHDDao().Save(model);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            bool status;
            status = new DeXuatHDDao().Delete(Id);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        // Load Autocomplete
        public JsonResult LoadDeXuat(string ma)
        {
            var data = new DeXuatDao().LoadView().Where(m => m.Ma.ToUpper().Contains(ma.ToUpper())).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectSupplier(int dx)
        {
            var model = new DeXuatNCCDao().Load(dx).OrderBy(m => m.DG).First();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHD(DateTime date)
        {
            int so = new DeXuatHDDao().GetHD(date);
            return Json(so, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckExistDxHd(int DeXuatId)
        {
            bool status = new DeXuatHDDao().CheckExistDxHd(DeXuatId);
            return Json(!status,JsonRequestBehavior.AllowGet);
        }
    }
}