using Data.Data;
using DataModel.Model.Commom;
using DataModel.Model.Systems;
using DataService.Dao.Applications;
using DataService.Dao.Commom;
using DataService.Dao.Systems;
using MVC6.Providers;
using OfficeOpenXml;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (System.IO.File.Exists(Server.MapPath("/Files/FilesReponse/" + model.LateId+".pdf")))
            {
               ViewBag.UrlFile= Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/FilesReponse/" + model.LateId + ".pdf";
            }
            else
            {
                ViewBag.UrlFile = "";
            }
            return View(model);
        }

        public ActionResult DetailCheck(long ma, int id)
        {
            CheckModel model = new CheckModel();

            model = new CheckDao().detail(ma, id);
            if (string.IsNullOrEmpty(model.TT_KT))
            {
                model.TT_KT = model.YC_KT;
            }
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
        public ActionResult SaveCheck(CheckModel model)
        {
            try
            {
                ModelState.Remove("YC_KT");
                ModelState.Remove("YC_SL");
                EFDbContext db = new EFDbContext();
                tbl_List_Check tbl = new tbl_List_Check();
                tbl = db.tbl_List_Check.Find(model.Id);
                tbl.CO = model.CO;
                tbl.CQ = model.CQ;
                tbl.MTR = model.MTR;
                tbl.SN = model.SN;
                tbl.PN = model.PN;
                tbl.Other = model.Other;
                tbl.Note_Other = model.Other.ToString() == "true" ? model.Note_Other : null;
                tbl.Result = model.Result;
                tbl.Reason = model.Reason;
                tbl.TT_KT = model.TT_KT;
                tbl.TT_SL = model.TT_SL;
                db.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Lấy nguyên nhân

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

        #endregion Lấy nguyên nhân

        #region Export excel

        public void ExportExcel(int ma)
        {
            using (FileStream template = System.IO.File.OpenRead(Server.MapPath("~/Files/Templates/PhieuKiemTra.xlsx")))
            {
                using (ExcelPackage package = new ExcelPackage(template))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet1"];
                    var tbl = new CheckDao().load(ma);
                    var model = new RequestDao().detail(ma);
                    sheet.Cells[2,5].Value = "Số " + model.FirstId + "- KTCL- " + model.Ma_BP;
                    int rowindex = 9;
                    int count = 1;
                    if (tbl.Count > 0)
                    {
                        if (tbl.Count > 3)
                        {
                            sheet.InsertRow(rowindex + 1, tbl.Count);
                        }
                       

                        //sheet.Cells["B10:AU10"].Copy(sheet.Cells["B11:AU" + (tbl.Count + rowindex + 1)]);
                        sheet.Cells[4, 4].Value = tbl[0].Ten_BP.ToString();
                        sheet.Cells[5, 4].Value = tbl[0].DiaDiem.ToString();
                        sheet.Cells[4, 10].Value = model.Hang_Muc.ToString();
                        sheet.Cells[5, 10].Value = string.IsNullOrEmpty(tbl[0].Date.ToString()) ? null : string.Format("{0:dd/MM/yyyy}", tbl[0].Date);
                        sheet.Cells[9, 1, 9, 36].Copy(sheet.Cells[10, 1, 9 + tbl.Count, 36]);
                        foreach (var item in tbl)
                        {
                            sheet.Cells[rowindex, 1].Value = count.ToString();
                            sheet.Cells[rowindex, 2].Value = item.DeXuat;
                            sheet.Cells[rowindex, 3].Value = item.HopDong;
                            sheet.Cells[rowindex, 4].Value = item.Ten_NCC;
                            sheet.Cells[rowindex, 5].Value = item.Ma_TB;
                            sheet.Row(rowindex).CustomHeight = true;
                            sheet.Cells[rowindex, 6].Value = item.YC_KT;
                            sheet.Cells[rowindex, 7].Value = item.TT_KT;
                            sheet.Cells[rowindex, 8].Value = item.YC_SL.ToString();
                            sheet.Cells[rowindex, 9].Value = item.TT_SL.ToString();
                            sheet.Cells[rowindex, 10].Value = item.DonVi;
                            sheet.Cells[rowindex, 11].Value = item.CO.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 12].Value = item.CQ.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 13].Value = item.MTR.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 14].Value = item.SN.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 15].Value = item.PN.ToString() == "True" ? "Có" : "Không";
                            sheet.Cells[rowindex, 16].Value = string.IsNullOrEmpty(item.Note_Other) ? "Không Có" : "Có";
                            sheet.Cells[rowindex, 17].Value = string.IsNullOrEmpty(item.Reason) ? "Đạt" : "Không Đạt";

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

        #endregion Export excel

        #region Gửi email

        // Forward Email
        [HttpPost]
        public ActionResult ForwardEmail(UpdateFile form)
        {
            bool status = new RequestDao().User_Edit(form.Id, form.name);
            if (!status) return Json(status, JsonRequestBehavior.AllowGet);
            string Cc = Request.Form["Cc"];
            string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("/Files/Templates/Approval.html"));

            var tbl = new Commom().respone(form.Id);

            tbl.Ghi_Chu = form.Ghi_Chu;
            tbl.Date_Autho = DateTime.Now;
            string name = new UserDao().load().Where(m => m.UserName.Equals(HttpContext.User.Identity.Name.ToString())).FirstOrDefault().FullName;
            tbl.Url = Request.Url.GetLeftPart(UriPartial.Authority) + "/Approvals/Detail?ma=" + tbl.Id;
            tbl.UrlFile = Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/" + tbl.LateId + ".pdf";

            string body = Engine.Razor.RunCompile(template, "checkkey", null, tbl);
            status = new Commom().SendEmail(form.email, "Chuyển tiếp thông báo kiểm tra từ " + name + " - " + tbl.LateId + "-" + tbl.Hang_Muc, body, Cc);

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        //Gửi Email phản hồi
        [HttpPost]
        public ActionResult SendEmail()
        {
            int ma = int.Parse(Request.Form["ma"]);
            string code = Request.Form["code"];
            string Cc = Request.Form["Cc"];
            HttpPostedFileBase file = null;

            var check = new CheckDao().load(ma);
            if (!check.Any(m => m.Result == null))
            {
                new CheckDao().SaveAutho(ma, code);
                bool result = false;

                //lấy thông tin  email
                string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("/Files/Templates/Reponse.html"));
                var tbl = new Commom().respone(ma);

                // Lưu file pdf
                if (Request.Files.Count > 0)
                {
                    file = Request.Files[0];
                    string extension = Path.GetExtension(file.FileName);
                    string filename = tbl.LateId.ToString() + extension;
                    if (System.IO.File.Exists(Server.MapPath("/Files/FilesReponse/" + filename)))
                    {
                        System.IO.File.Delete(Server.MapPath("/Files/FilesReponse/" + filename));
                    }
                    file.SaveAs(Server.MapPath("/Files/FilesReponse/" + filename));
                }

                switch (code)
                {
                    case "A":
                        tbl.Ket_Qua = "Đạt";
                        break;

                    case "R":
                        tbl.Ket_Qua = "Không đạt";
                        break;

                    default:
                        string ket_qua;
                        ket_qua = check.Count(m => m.Result == true).ToString() + "/" + check.Count;
                        tbl.Ket_Qua = "Đạt một phần (" + ket_qua + ")";
                        break;
                }

                tbl.Date_Autho = DateTime.Now;
                tbl.Url = Request.Url.GetLeftPart(UriPartial.Authority) + "/Requests/CheckList?ma=" + tbl.Id;
                tbl.UrlFile = Request.Url.GetLeftPart(UriPartial.Authority) + "/Files/FilesReponse/" + tbl.LateId + ".pdf";
                string body;
                var email = new UserDao().load().Where(m => m.UserName.Equals(tbl.User_Nhap)).FirstOrDefault();
                if (Engine.Razor.IsTemplateCached("templatekey", null))
                {
                    body = Engine.Razor.Run("templatekey", null, tbl);
                }
                else
                {
                    body = Engine.Razor.RunCompile(template, "templatekey", null, tbl);
                }

                // gửi email
                result = new Commom().SendEmail(email.Email, "Thông báo kết quả kiểm tra - " + tbl.LateId + "-" + tbl.Hang_Muc, body, Cc);
                return Json(new { status = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Gửi email
    }
}