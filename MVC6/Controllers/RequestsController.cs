using DataModel.Model.Applications;
using DataModel.Model.Commom;
using DataModel.Model.Systems;
using DataService.Dao.Commom;
using DataService.Dao.ExportReports;
using DataService.Dao.Systems;
using EASendMail;
using MVC6.Providers;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVC6.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        #region Danh sách phiếu yêu cầu

        // GET: Requests
        public ActionResult Require()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "REQUEST"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult LoadRequire()
        {
            string name = HttpContext.User.Identity.Name;
            var model = new RequestDao().load(name);
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailRequire(int ma)
        {
            RequestModel model = new RequestModel();
            if (ma > 0)
            {
                if (new Commom().CheckPermission(ma))
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                if (!new PermissionAttribute().PermissionAuthorization("Update", "REQUEST"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                model = new RequestDao().detail(ma);
            }
            else
            {
                if (!new PermissionAttribute().PermissionAuthorization("Create", "REQUEST"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var user = new ExportReportDao().GetLevelUser(HttpContext.User.Identity.Name);
                model.FirstId = GetRequestId(user.Ma_BP);
                model.LateId = model.FirstId.ToString() + "-Y4CKT-" + user.Ma_BP;
                model.Ma_BP = user.Ma_BP;
            }
            ViewBag.Depart = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
            return PartialView("_DetailRequire", model);
        }

        public JsonResult Change_Id(string ma)
        {
            int count = GetRequestId(ma);
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequire(RequestModel data)
        {
            if (ModelState.IsValid)
            {
                data.User_Nhap = HttpContext.User.Identity.Name;
                bool status;
                if (data.Id > 0)
                {
                    status = new RequestDao().Update(data);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = new RequestDao().Save(data);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.Depart = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
            return PartialView("_DetailRequire", data);
        }

        public ActionResult DeleteRequire(int ma)
        {
            if (new Commom().CheckPermission(ma))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            if (!new PermissionAttribute().PermissionAuthorization("Delete", "REQUEST"))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            bool status = new RequestDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh sách phiếu yêu cầu

        #region Danh sách thiêt bị nghiệm thu

        public ActionResult CheckList(int ma)
        {
            string name = HttpContext.User.Identity.Name;
            var model = new RequestDao().load(name).Where(m => m.Id.Equals(ma)).FirstOrDefault();
            return View(model);
        }

        public ActionResult LoadCheck(int id)
        {
            var model = new CheckDao().load(id);
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailCheck(long ma, int id)
        {
            if (new Commom().CheckPermission(id))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            ViewBag.Provider = new SelectList(new ProviderDao().load(), "Ma_NCC", "Ten_NCC");
            
            CheckModel model = new CheckModel();
            model.RequestId = id;
            if (ma > 0)
            {
                model = new CheckDao().detail(ma, id);
            }

            return PartialView("_DetailCheck", model);
        }

        [HttpPost]
        public ActionResult SaveCheck(CheckModel data)
        {
            ModelState.Remove("TT_KT");
            ModelState.Remove("TT_SL");
            if (ModelState.IsValid)
            {
                data.User_Nhap = HttpContext.User.Identity.Name;

                bool status;
                if (data.Id > 0)
                {
                    status = new CheckDao().Update(data);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = new CheckDao().Save(data);
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.Provider = new SelectList(new ProviderDao().load(), "Ma_NCC", "Ten_NCC");
            
            return PartialView("_DetailCheck", data);
        }

        [HttpPost]
        public ActionResult DeleteCheck(long ma)
        {
            bool status = false;
            status = new CheckDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        #endregion Danh sách thiêt bị nghiệm thu

        public JsonResult ListEmail(string a)
        {
            var data = new UserDao().ListName(a);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendEmail(UpdateFile form)
        {
            if (new Commom().CheckPermission(form.Id))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            bool status = new RequestDao().User_Autho(form.Id, form.name);
            if (!status) return Json(status, JsonRequestBehavior.AllowGet);

            var template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("/Files/Templates/Approval.html"));

            var tbl = new Commom().respone(form.Id);
            if (form.file != null)
            {
                string filename = tbl.LateId.ToString();
                string extension = Path.GetExtension(form.file.FileName);
                filename = filename + extension;
                if (System.IO.File.Exists(Server.MapPath("/Files/" + filename)))
                {
                    System.IO.File.Delete(Server.MapPath("/Files/" + filename));
                }
                form.file.SaveAs(Server.MapPath("/Files/" + filename));
            }
            string Cc = Request.Form["Cc"];
            tbl.Date_Autho = DateTime.Now;

            tbl.Url = Request.Url.GetLeftPart(UriPartial.Authority) + "/Approvals/Detail?ma=" + tbl.Id;
            tbl.UrlFile = Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/" + tbl.LateId + ".pdf";
            string body = Engine.Razor.RunCompile(template, "key", null, tbl);
            status = new Commom().SendEmail(form.email, "Thông báo kiểm tra - " + tbl.LateId + "-" + tbl.Hang_Muc, body, Cc);

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
           
        }

        private int GetRequestId(string Ma_BP)
        {
            DateTime date = DateTime.Now;
            return new Commom().CreatRequestId(Ma_BP, date);
        }
    }
}