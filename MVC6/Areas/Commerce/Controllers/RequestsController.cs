using Data.Data;
using DataModel.Model.Trade;
using DataService.Dao.Systems;
using DataService.Dao.Trade;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVC6.Areas.Commerce.Controllers
{
    public class RequestsController : Controller
    {
        #region Danh sách đề xuất

        // GET: Commerce/Requests
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load()
        {
            var model = new DeXuatDao().Load();
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int Id)
        {
            DeXuatModel model = new DeXuatModel();
            if (Id > 0)
            {
                model = new DeXuatDao().Detail(Id);
            }

            return PartialView("_DetailTradeRequest", model);
        }

        [HttpPost]
        public ActionResult Save(DeXuatModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DetailTradeRequest", model);
            }
            bool status;
            if (model.Id > 0)
            {
                status = new DeXuatDao().Update(model);
            }
            else
            {
                status = new DeXuatDao().Save(model);
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ma)
        {
            bool status = new DeXuatDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        // Kiểm tra tồn tại của đề xuất
        public JsonResult CheckExistDX(string ma)
        {
            bool status = new DeXuatDao().CheckExistDX(ma);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh sách đề xuất

        #region Danh sách kỹ thuật của đề xuất

        public ActionResult IndexDetail(int ma)
        {
            tbl_DeXuat_TM tm = new tbl_DeXuat_TM();
            tm = new DeXuatTMDao().load(ma).FirstOrDefault();
            ViewBag.TM = tm;
            var model = new DeXuatDao().Detail(ma);
            return View(model);
        }

        public ActionResult LoadDetail(int ma)
        {
            var model = new DeXuatChiTietDao().Load(ma);
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetail(int ma)
        {
            DeXuatChiTietModel model = new DeXuatChiTietModel();
            if (ma > 0)
            {
                model = new DeXuatChiTietDao().Detail(ma);
            }
            return PartialView("_DetailMarterial", model);
        }

        [HttpPost]
        public ActionResult SaveDetail(DeXuatChiTietModel model)
        {
            bool status;
            if (model.Id > 0)
            {
                status = new DeXuatChiTietDao().Update(model);
            }
            else
            {
                status = new DeXuatChiTietDao().Save(model);
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDetail(int ma)
        {
            bool status = new DeXuatChiTietDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh sách kỹ thuật của đề xuất

        #region Danh lựa chọn nhà cung cấp

        public ActionResult LoadNCC(int dx)
        {
            var model = new DeXuatNCCDao().Load(dx);
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DetailNCC(DGNCCModel model)
        {
            bool status;
            status = new DeXuatNCCDao().SaveNCC(model);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteNCC(int id)
        {
            bool status = new DeXuatNCCDao().DeleteNCC(id);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectNCC()
        {
            return PartialView("_ListNCC");
        }

        public ActionResult ListNCC(int ma)
        {
            List<tbl_NCC> model = new List<tbl_NCC>();
            if (ma == 0)
            {
                model = new ProviderDao().load();
            }
            else if (ma == 1)
            {
                model = new ProviderDao().load().Where(m => m.Hang_Hoa != null).ToList();
            }
            else
            {
                model = new ProviderDao().load().Where(m => m.Dich_Vu != null).ToList();
            }
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh lựa chọn nhà cung cấp

        #region Tiêu chí thương mại
        public ActionResult ThuongMai(int dx)

        {
            tbl_DeXuat_TM model = new tbl_DeXuat_TM();
     
            model = new DeXuatTMDao().load(dx).FirstOrDefault();
            return PartialView("_DetailCommerce", model);
        }
        [HttpPost]
        public ActionResult SaveTM(tbl_DeXuat_TM model)
        {
            bool status = new DeXuatTMDao().Save(model);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
      
        #endregion Tiêu chí thương mại
    }
}