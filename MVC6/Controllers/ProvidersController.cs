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
    public class ProvidersController : Controller
    {
       
        // GET: Providers
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load()
        {
            var data = new ProviderDao().load();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string ma)
        {
            ProviderModel model = new ProviderModel();
            if (!string.IsNullOrEmpty(ma))
            {
                model = new ProviderDao().detail(ma);
                model.IsUpdate = true;
            }
 
            return PartialView("_DetailProvider", model);
        }
        [HttpPost]
        public ActionResult Save(ProviderModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (model.IsUpdate)
                {
                    status = new ProviderDao().Update(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = new ProviderDao().Save(model);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
            }
           return PartialView("_DetailProvider", model);
        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            bool status = false;
            status = new ProviderDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}