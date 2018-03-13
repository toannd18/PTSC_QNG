using Data.Data;

using MVC6.Providers;
using OfficeOpenXml;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Model.Applications;
using DataService.Dao.Applications;
using DataService.Dao.Systems;
using DataService.Dao.Commom;
using DataModel.Model.Commom;
using DataModel.Model.Systems;

namespace MVC6.Controllers
{
    [Authorize]
    public class ApprovalsController : Controller
    {
        
        // GET: Approvals
        public ActionResult Index()
        {
            if (!new PermissionAttribute().PermissionAuthorization("Read", "REQUEST"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult Load()

        {
            string user = HttpContext.User.Identity.Name;
            var data = new ApprovalDao().load(user);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int ma)
        {
           
            var model = new RequestDao().detail(ma);
            return View(model);
        }
        public ActionResult DetailCheck(long ma,int id)
        {
          
            CheckModel model = new CheckModel();
            
            model = new CheckDao().detail(ma, id);
            if (!model.CO.HasValue)
                model.CO = false;
            if (!model.CQ.HasValue)
                model.CO = false;
            if (!model.MTR.HasValue)
                model.MTR = false;
            if (!model.SN.HasValue)
                model.SN = false;
            if (!model.PN.HasValue)
                model.PN = false;
            if (!model.Other.HasValue)
                model.Other = false;
            if (!model.Result.HasValue)
                model.Result = false;
            return PartialView("_DetailApproval", model);
        }
        [HttpPost]
        public ActionResult SaveCheck(SaveApprovalModel model)
        {
            try
            {
                EFDbContext db = new EFDbContext();
                tbl_List_Check tbl = new tbl_List_Check();
                tbl = db.tbl_List_Check.Find(model.Id);
                tbl.CO = model.CO;
                tbl.CQ = model.CQ;
                tbl.MTR = model.MTR;
                tbl.SN = model.SN;
                tbl.PN = model.PN;
                tbl.Other = model.Other;
                tbl.Note_Other = model.Other.ToString()=="true"?model.Note_Other:null;
                tbl.Result = model.Result;
                tbl.Reason = model.Reason;
                db.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
          


        }
        [HttpPost]
        public ActionResult SendEmail()
        {
            int ma = int.Parse(Request.Form["ma"]);
            string code = Request.Form["code"];
            HttpPostedFileBase file = null;
            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];
                string extension = Path.GetExtension(file.FileName);
                string filename = ma.ToString() + extension;
                if (System.IO.File.Exists(Server.MapPath("/Files/FilesReponse/" + filename)))
                {
                    System.IO.File.Delete(Server.MapPath("/Files/FilesReponse/" + filename));
                }
                file.SaveAs(Server.MapPath("/Files/FilesReponse/" + filename));
            }
          
            var check = new CheckDao().load(ma);
            if (!check.Any(m=>m.Result==null))
            {
                new CheckDao().SaveAutho(ma, code);
                bool result = false;
                //lấy thông tin  email
                string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Files/Templates/Reponse.html"));
                var tbl = new Commom().respone(ma);
                tbl.Date_Autho = DateTime.Now.ToString("dd/MM/yyyy");
                tbl.Url = Request.Url.GetLeftPart(UriPartial.Authority)+ "/Requests/CheckList?ma=" + tbl.Id;
                tbl.UrlFile = Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/FilesReponse/" + ma.ToString() + ".pdf";
                var email = new UserDao().load().Where(m => m.UserName.Equals(tbl.User_Nhap)).FirstOrDefault();
                string body = Engine.Razor.RunCompile(template, "templatekey", null, tbl);
                // gửi email
                result = new Commom().SendEmail(email.Email, "Mail Phản Hồi Thông Báo Kiểm Tra - " + tbl.Hang_Muc, body);
                return Json(new { status = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoadReason(long ma)
        {
            string reason = new CheckDao().LoadReason(ma).Reason;
            return Json(new { reason = reason }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadNote(long ma)
        {
            string reason = new CheckDao().LoadReason(ma).Note_Other;
            return Json(new { reason = reason }, JsonRequestBehavior.AllowGet);
        }
        public void ExportExcel(int ma)
        {
            using (FileStream template = System.IO.File.OpenRead(Server.MapPath("/Reports/PhieuKiemTra.xlsx")))
            {
                using (ExcelPackage package = new ExcelPackage(template))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet1"];
                    var tbl = new CheckDao().load(ma);
                    int rowindex = 10;
                    int count = 1;
                    if (tbl.Count > 0)
                    {
                        sheet.InsertRow(rowindex + 1, tbl.Count);
                    
                        //sheet.Cells["B10:AU10"].Copy(sheet.Cells["B11:AU" + (tbl.Count + rowindex + 1)]);
                        sheet.Cells[5, 9].Value = tbl[0].Ten_BP.ToString();
                        sheet.Cells[6, 9].Value = tbl[0].DiaDiem.ToString();
                        sheet.Cells[6, 26].Value = string.IsNullOrEmpty(tbl[0].Date.ToString()) ? null : string.Format("{0:dd/MM/yyyy}", tbl[0].Date);
                        foreach (var item in tbl)
                        {
                            sheet.Cells[rowindex, 2,rowindex,47].Copy(sheet.Cells[rowindex+1,2,rowindex+1,47]);
                            sheet.Cells[rowindex, 2].Value = count.ToString();
                            sheet.Cells[rowindex, 4].Value = item.DeXuat;
                            sheet.Cells[rowindex, 7].Value = item.HopDong;
                            sheet.Cells[rowindex, 9].Value = item.Ten_NCC;
                            sheet.Cells[rowindex, 12].Value = item.Ten_TB;
                            sheet.Cells[rowindex, 18].Value = item.YC_KT;
                            sheet.Cells[rowindex, 22].Value = item.TT_KT;
                            sheet.Cells[rowindex, 26].Value = item.YC_SL.ToString();
                            sheet.Cells[rowindex, 28].Value = item.TT_SL.ToString();
                            sheet.Cells[rowindex, 30].Value = item.DonVi;
                            sheet.Cells[rowindex, 32].Value = item.CO.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 34].Value = item.CQ.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 36].Value = item.MTR.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 38].Value = item.SN.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 40].Value = item.PN.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 42].Value = string.IsNullOrEmpty(item.Note_Other) ? "Không Có" : "Có";
                            sheet.Cells[rowindex, 44].Value = string.IsNullOrEmpty(item.Reason) ? "Đạt" : "Không Đạt";

                            rowindex++;
                            count++;
                        }
                       
                    }
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=results.xlsx");
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
                
            }
        }
        [HttpPost]
        public ActionResult ForwardEmail(UpdateFile form)
        {
          
            bool status = new RequestDao().User_Edit(form.Id, form.name);
            if (!status) return Json(status, JsonRequestBehavior.AllowGet);
            string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Files/Templates/Approval.html"));
            var tbl = new Commom().respone(form.Id);
            tbl.Date_Autho = DateTime.Now.ToString("dd/MM/yyyy");
            tbl.FullName = new UserDao().load().Where(m => m.UserName.Equals(HttpContext.User.Identity.Name.ToString())).FirstOrDefault().FullName;
            tbl.Url = Request.Url.GetLeftPart(UriPartial.Authority) + "/Approvals/Detail?ma=" + tbl.Id;
            tbl.UrlFile = Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/" + form.Id + ".pdf";
            string body = Engine.Razor.RunCompile(template,"checkkey",null,tbl);
            status = new Commom().SendEmail(form.email, "Mail Thông Báo Kiểm Tra - " + tbl.Hang_Muc, body);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}