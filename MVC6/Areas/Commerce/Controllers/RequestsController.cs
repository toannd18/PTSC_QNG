using Data.Data;
using DataModel.Model.Trade;
using DataService.Dao.Systems;
using DataService.Dao.Trade;
using MVC6.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVC6.Areas.Commerce.Controllers
{
    [Authorize]
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
            var model = new DeXuatDao().LoadView();
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int Id)
        {
            ViewBag.User = new UserDao().load();
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

        // Cập nhật tình trạng

        [HttpPost]
        public ActionResult Update()
        {
            bool status;
            tbl_DeXuat tbl = new tbl_DeXuat();
            tbl.Id = int.Parse(Request.Form["Id"]);
            string test=Request.Form["Ngay_NT"];
            if (string.IsNullOrEmpty(Request.Form["Ngay_Ky"]))
            {
                tbl.Ngay_Ky = null;
            }
            else
            {
                tbl.Ngay_Ky = DateTime.Parse(Request.Form["Ngay_Ky"]);
            }
            if (string.IsNullOrEmpty(Request.Form["Ngay_TH"]))
            {
                tbl.Ngay_TH = null;
            }
            else
            {
                tbl.Ngay_TH = DateTime.Parse(Request.Form["Ngay_TH"]);
            }
            if (string.IsNullOrEmpty(Request.Form["Ngay_THTT"]))
            {
                tbl.Ngay_THTT = null;
            }
            else
            {
                tbl.Ngay_THTT = DateTime.Parse(Request.Form["Ngay_THTT"]);
            }
            if (string.IsNullOrEmpty(Request.Form["Ngay_NT"]))
            {
                tbl.Ngay_NT = null;
            }
            else
            {
                tbl.Ngay_NT = DateTime.Parse(Request.Form["Ngay_NT"]);
            }
            if (string.IsNullOrEmpty(Request.Form["Ngay_NT_QC"]))
            {
                tbl.Ngay_NT_QC = null;
            }
            else
            {
                tbl.Ngay_NT_QC = DateTime.Parse(Request.Form["Ngay_NT_QC"]);
            }
            tbl.Status = bool.Parse(Request.Form["Status"]);
            tbl.Ghi_Chu = Request.Form["Ghi_Chu"];
            status = new DeXuatDao().UpdateStatus(tbl);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        // Kiểm tra tồn tại của đề xuất
        public JsonResult CheckExistDX(string Ma,string Tieu_De)
        {
            bool status = new DeXuatDao().CheckExistDX(Ma, Tieu_De);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh sách đề xuất

        #region Danh sách kỹ thuật của đề xuất

        public ActionResult IndexDetail(int ma)
        {
            tbl_DeXuat_TM tm = new tbl_DeXuat_TM();
            tm = new DeXuatTMDao().load(ma).FirstOrDefault();
            ViewBag.TM = tm;
            ViewBag.User = new UserDao().load();
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
            if(!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
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
            if (!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
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
            if (!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            bool status = new DeXuatNCCDao().DeleteNCC(id);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectNCC()
        {
            if(!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
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

        public JsonResult CheckExistNCC(int DeXuatId, string Ma_NCC)
        {
            bool status = new DeXuatNCCDao().CheckExistNCC(DeXuatId, Ma_NCC);
            return Json(status);
        }

        [HttpPost]
        public ActionResult DGChung(int ma, int data)
        {
            if (!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            bool status = new DeXuatNCCDao().UpdateDG(ma, data);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh lựa chọn nhà cung cấp

        #region Tiêu chí thương mại

        public ActionResult ThuongMai(int dx)

        {
            if(!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name.ToString()))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
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