using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Data;

using System.IO;

using MVC6.Providers;
using DataModel.Model.Systems;
using DataService.Dao.Systems;
using DataService.Dao.Commom;

namespace MVC6.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        
        // GET: Users
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "USER"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult LoadData()
        {
            var data = new UserDao().loadview();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int ma)
        {

            AppUserModel tbl = new AppUserModel();
            if (ma > 0)
            {
                tbl = new UserDao().detail(ma);
            }
            if (string.IsNullOrEmpty(tbl.Ma_BP))
            {
                ViewBag.Team = new SelectList(new TeamDao().load(), "Ma_TO", "Ten_TO");
            }
            else
            {
                ViewBag.Team = new SelectList(new TeamDao().load().Where(m => m.Ma_BP == tbl.Ma_BP), "Ma_TO", "Ten_TO");
            }
            Bind();
            return PartialView("_DetailUser", tbl);
        }      
        [HttpPost]
        public ActionResult Save(AppUserModel model)
        {
            bool status;
            if (model.Id > 0)
            {
                if(ModelState["PasswordHash"]!=null && ModelState["ConfirmPassword"] != null)
                {
                    ModelState["PasswordHash"].Errors.Clear();
                    ModelState["ConfirmPassword"].Errors.Clear();
                }
            }
            if (ModelState.IsValid)
            {
                
                if (model.uploadanh != null)
                {
                    string filename = model.UserName.ToString();
                    string extension = Path.GetExtension(model.uploadanh.FileName);
                    filename = filename + extension;
                    model.Avatar = "/images/" + filename;
                    if (System.IO.File.Exists(Server.MapPath(model.Avatar)))
                    {
                        System.IO.File.Delete(Server.MapPath(model.Avatar));
                    }
                    model.uploadanh.SaveAs(Server.MapPath(model.Avatar));
                }
                if (model.Id > 0)
                {
                    
                    status = new UserDao().Upload(model);
                }
                else
                {
                    model.PasswordHash = new Commom().MD5Hash(model.PasswordHash);
                    status = new UserDao().Save(model);
                }
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Ma_BP))
            {
                ViewBag.Team = new SelectList(new TeamDao().load(), "Ma_TO", "Ten_TO");
            }
            else
            {
                ViewBag.Team = new SelectList(new TeamDao().load().Where(m => m.Ma_BP == model.Ma_BP), "Ma_TO", "Ten_TO");
            }
            Bind();
            return PartialView("_DetailUser", model);
        }
        [HttpPost]
        public ActionResult Delete(int ma)
        {
            bool status = new UserDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckExist(string UserName)
        {
            bool status = new UserDao().CheckExistUser(UserName);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailRoleUser(int ma)
        {
            ViewBag.User = new SelectList(new UserDao().load(), "Id", "UserName");
            ViewBag.Role = new SelectList(new RoleDao().load(), "Id", "Role");
            var model = new UserDao().DetailRole(ma);
            return PartialView("_DetailRoleUser", model);
        }
        [HttpPost]
        public ActionResult SaveRole(RoleUserModel model)
        {
            bool status = new UserDao().SaveRole(model);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        private void Bind()
        {
            ViewBag.Dept = new SelectList(new DepartmentDao().load(), "Ma_BP", "Ten_BP");
            
            ViewBag.Position = new SelectList(new PositionDao().load(), "Ma_CV", "Ten_CV");
            List<SelectListItem> SelYN = new List<SelectListItem>();
           
            SelYN.Add(new SelectListItem
            {
                Text = "Nam",
                Value = true.ToString()
            });
            SelYN.Add(new SelectListItem
            {
                Text = "Nữ",
                Value = false.ToString()
            });

            ViewBag.Sex = SelYN;
        }
        public ActionResult ChangeBP(string ma)
        {
            var data = new TeamDao().load().Where(m => m.Ma_BP == ma);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}