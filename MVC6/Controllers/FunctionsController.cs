
using Data.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;

using DataModel.Model.Systems;
using DataService.Dao.Systems;
using DataModel.Model.Applications;
using DataModel.Model.Commom;

namespace MVC6.Controllers
{
    [Authorize]
    public class FunctionsController : Controller
    {
        // GET: Functions
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData()
        {
            ViewBag.TreeView = new FunctionDao().load();
            return PartialView("_TreeView");
        }
        public ActionResult Detail(string ma)
        {
            ViewBag.Parent = new SelectList(new FunctionDao().load(), "Id", "Name");
            FunctionModel model = new FunctionModel();
            if(ma!=null)
            {
            model = new FunctionDao().detail(ma);
            }
            return PartialView("_DetailFunction", model);
        }
        [HttpPost]
        public ActionResult Save(FunctionModel model)
        {
            ViewBag.Parent = new SelectList(new FunctionDao().load(), "Id", "Name");
            bool status;
            if (ModelState.IsValid)
            {
                if (model.IsUpdate)
                {
                    status = new FunctionDao().Update(model);
                }
                else
                    status = new FunctionDao().Save(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            else
                return PartialView("_DetailFunction", model);

        }
        [HttpPost]
        public ActionResult Delete(string ma)
        {
            bool status = new FunctionDao().Delete(ma);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadPermission(string functionID)
        {
            
            List<PermissionModel> permissions = new List<PermissionModel>();
            var role = new RoleDao().load();
            var lispermission = new PermissionDao().GetByFunctionId(functionID);
            if (lispermission.Count == 0)
            {
                foreach (var item in role)
                {
                    permissions.Add(new PermissionModel()
                    {
                        RoleId = item.Id,
                        CanCreate = false,
                        CanDelete = false,
                        CanRead = false,
                        CanUpdate = false,
                        AppRole = new RoleModel()
                        {
                            Id = item.Id,
                            Description = item.Description,
                            Role = item.Role,
                        }
                    });
                }
            }
            else
            {
                foreach(var item in role)
                {
                    if (!lispermission.Any(m => m.RoleId.Equals(item.Id)))
                     {
                        permissions.Add(new PermissionModel()
                        {
                            RoleId = item.Id,
                            CanCreate = false,
                            CanDelete = false,
                            CanRead = false,
                            CanUpdate = false,
                            AppRole = new RoleModel()
                            {
                                Id = item.Id,
                                Description = item.Description,
                                Role = item.Role,
                            }
                        });
                    }
                    else
                    {
                        var add = lispermission.Where(m => m.RoleId.Equals(item.Id)).Select(m => new PermissionModel
                        {
                            RoleId = item.Id,
                            CanCreate = m.CanCreate,
                            CanDelete = m.CanDelete,
                            CanRead = m.CanRead,
                            CanUpdate = m.CanUpdate,
                            AppRole = new RoleModel()
                            {
                                Id = item.Id,
                                Description = item.Description,
                                Role = item.Role,
                            }
                        }).ToList();
                        permissions.AddRange(add);
                    }

                }
            }
            return PartialView("_DetailPermission", permissions);
        }
        [HttpPost]
        public ActionResult SavePermission(string data,string FunctionId)
        {
            var model = new JavaScriptSerializer().Deserialize<List<SavePermission>>(data);
            PermissionDao _permissiondao = new PermissionDao();
            _permissiondao.DeleteAll(FunctionId);
            Permission permission = null;
            try
            {
                foreach (var item in model)
                {
                    permission = new Permission();
                    permission.RoleId = item.RoleId;
                    permission.FunctionId = FunctionId;
                    permission.CanCreate = Boolean.Parse(item.CanCreate);
                    permission.CanDelete = Boolean.Parse(item.CanDelete);
                    permission.CanRead = Boolean.Parse(item.CanRead);
                    permission.CanUpdate = Boolean.Parse(item.CanUpdate);
                    _permissiondao.Save(permission);

                }
                var functions = new FunctionDao().GetAllWithParent(FunctionId);
                if (functions.Any())
                {
                    foreach (var item in functions)
                    {
                        _permissiondao.DeleteAll(item.Id);
                        foreach (var p in model)
                        {
                            var childPermission = new Permission();
                            childPermission.FunctionId = item.Id;
                            childPermission.RoleId = p.RoleId;
                            childPermission.CanCreate = Boolean.Parse(p.CanCreate);
                            childPermission.CanDelete = Boolean.Parse(p.CanDelete);
                            childPermission.CanRead = Boolean.Parse(p.CanRead);
                            childPermission.CanUpdate = Boolean.Parse(p.CanUpdate);
                            _permissiondao.Save(childPermission);
                        }
                    }
                }
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [ChildActionOnly]
        public PartialViewResult GetAllMenu()
        {
            var user = HttpContext.User.Identity.Name;
            List<FunctionViewModel> model = new List<FunctionViewModel>();
            bool check = new RoleDao().IsRole(user, "admin");
            if (check)
            {
                model = new FunctionDao().load();
            }
            else
                model = new FunctionDao().GetAllWithPermission(user);
            return PartialView(model);
        }
    }
}