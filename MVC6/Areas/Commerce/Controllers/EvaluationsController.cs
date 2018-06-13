using Data.Data;
using DataModel.Model.Trade;
using DataService.Dao.Trade;
using System.Linq;
using System.Web.Mvc;

namespace MVC6.Areas.Commerce.Controllers
{
    [Authorize]
    public class EvaluationsController : Controller
    {
        // GET: Commerce/Evaluations
        public ActionResult IndexSpec(int dx)
        {
            if(!new DeXuatDao().PermissionDx(HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            var model = new DGKTDao().Load(dx);
            ViewBag.DeXuat = dx;
            ViewBag.NCC = new DeXuatNCCDao().Load(dx);
            return View(model);
        }

        #region Đánh giá kỹ thuật

        public ActionResult LoadSpec(int dg_kt, int dx)
        {
            ViewBag.NCC = new SelectList(new DeXuatNCCDao().Load(dx), "Id", "Ten_NCC");
            tbl_DG_KT model = new tbl_DG_KT();
            model = new DGKTDao().Detail(dg_kt).FirstOrDefault();
            return PartialView("_DetailDGKT", model);
        }

        public ActionResult DetailSpec(int NCC, int dg_kt)
        {
            tbl_DG_KT model = new tbl_DG_KT();
            model = new DGKTDao().Detail(dg_kt).Where(m => m.DG_NCC_Id == NCC).FirstOrDefault();
            if (model == null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSpec(tbl_DG_KT model)
        {
            bool status;
            status = new DGKTDao().Save(model);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSpec(int Id)
        {
            bool status;
            status = new DGKTDao().Delete(Id);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadKt(int dx)
        {
            var tbl = new DeXuatNCCDao().Load(dx);
            ViewBag.NCC = new SelectList(tbl, "Ma_NCC", "Ten_NCC");
            return PartialView("_DetailNCC_KT", tbl.FirstOrDefault());
        }

        public ActionResult Detail(int dx, string ma_ncc)
        {
            var tbl = new DeXuatNCCDao().Load(dx).Where(m => m.Ma_NCC == ma_ncc).FirstOrDefault();
            ViewBag.NCC = new SelectList(new DeXuatNCCDao().Load(dx), "Ma_NCC", "Ten_NCC");
            return PartialView("_DetailNCC_KT", tbl);
        }

        public ActionResult SaveNCCKT(DGNCCModel model)
        {
            bool status;
            status = new DeXuatNCCDao().UpdateNCC(model);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        #endregion Đánh giá kỹ thuật

        #region Đánh giá thương mai

        public ActionResult IndexCom(int dx)
        {
            if (!new DeXuatDao().PermissionTM(HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            var model = new DGTMDao().Load(dx);
            ViewBag.DeXuat = dx;
            ViewBag.NCC = new DeXuatNCCDao().Load(dx);
            ViewBag.DGTM = new DGTMDao().LoadTM(dx);
            return View(model);
        }

        public ActionResult LoadTM(int dg_kt, int dx)
        {
            ViewBag.NCC = new SelectList(new DeXuatNCCDao().Load(dx), "Id", "Ten_NCC");
            tbl_DG_KT model = new tbl_DG_KT();
            model = new DGKTDao().Detail(dg_kt).FirstOrDefault();
            return PartialView("_DetailDG", model);
        }

        public ActionResult DetailTM(int NCC, int dg_kt)
        {
            tbl_DG_KT model = new tbl_DG_KT();
            model = new DGKTDao().Detail(dg_kt).Where(m => m.DG_NCC_Id == NCC).FirstOrDefault();
            if (model==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDG(int ma, int dg)
        {
            bool status;
            status = new DGKTDao().SaveDG(ma, dg);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDGTM(int dx)
        {
            ViewBag.NCC = new SelectList(new DeXuatNCCDao().Load(dx), "Id", "Ten_NCC");
            var tbl = new DGTMDao().DetailTM(dx).FirstOrDefault();
            return PartialView("_DetailDGTM", tbl);
        }

        public ActionResult DetailDGTM(int NCC, int dx)
        {
            var tbl = new DGTMDao().DetailTM(dx).Where(m => m.DG_NCC_Id == NCC).FirstOrDefault();
            if (tbl ==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(tbl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveTM(tbl_DG_TM tbl)
        {
            bool status = new DGTMDao().SaveTM(tbl);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        #endregion Đánh giá thương mai
    }
}