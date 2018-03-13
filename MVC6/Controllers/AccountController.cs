using DataService.Dao;
using Data.Data;
using DataService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataModel.Model.Systems;
using DataService.Dao.Commom;
using DataModel.Model.Commom;

namespace MVC6.Controllers
{
    public class AccountController : Controller
    {
       
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountModel model, string ReturnUrl = "")
        {
            ReturnUrl = Convert.ToString(Request.QueryString["ReturnUrl"]);
            if (ModelState.IsValid)
            using (EFDbContext db= new EFDbContext())
            {
                    model.Password = new Commom().MD5Hash(model.Password);
                    bool check = new DataService.Dao.Account().Login(model.Name, model.Password);
                if (check)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, model.Remember);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    {
                        ModelState.AddModelError("", "Mật Khẩu hoặc Tài Khoản Không Chính Xác");
                    }
                }
           
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ChangePassword()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.NewPassWord = new Commom().MD5Hash(model.NewPassWord);
                model.OldPassword = new Commom().MD5Hash(model.OldPassword);
                int result = new Commom().ChangePassword(model.OldPassword, model.NewPassWord);
                if (result == 0)
                {
                    ViewBag.Notify = "Mật khẩu không đúng";
                    ViewBag.Type = "alert alert-danger";
                }
                else
                {
                    ViewBag.Notify = "Thay đổi mật khẩu thành công";
                    ViewBag.Type = "alert alert-success";
                }
                return View();
            }
            return View(model);
        }
      
    }
}