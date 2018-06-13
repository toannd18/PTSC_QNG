using Data.Data;
using DataModel.Model.Reports;
using DataService.Dao.ExportReports;
using DataService.Dao.Systems;
using DataService.Dao.Trade;
using DevExpress.Web.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVC6.Areas.Report.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        // GET: Report/Reports
        public ActionResult ExportDaily(int ma)
        {
            ViewBag.Id = ma;
            return View();
        }

        public ActionResult DocumentViewerPartial(int ma)
        {
            ViewBag.Id = ma;
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         join u in db.AppUsers on d.UserName equals u.UserName
                         join u1 in db.AppUsers on d.User_Autho1 equals u1.UserName
                         join u2 in db.AppUsers on d.User_Autho2 equals u2.UserName
                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                         join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO
                         where d.Id == ma
                         select new ExportDailyModel
                         {
                             DailyId = d.Id,
                             Id = dd.Id,
                             FullName = u.FullName,
                             FullName1 = u1.FullName,
                             FullName2 = u2.FullName,
                             Date = d.Date,
                             FromTime = dd.FormTime,
                             ToTime = dd.ToTime,
                             Total = d.Total_Job,
                             Content = dd.Content_Job,
                             Method = dd.Method,
                             Result = dd.Result,
                             Comment1 = dd.Comment1,
                             Comment2 = dd.Comment2,
                             Ten_TO = t.Ten_TO,
                             Ten_CV = c.Ten_CV
                         }).ToList();
            foreach (var item in model)
            {
                item.Total = item.Total.Replace("</br>", "\n");
            }
            return PartialView("_DocumentViewerPartial", model);
        }

        public ActionResult DocumentViewerPartialExport(int ma)
        {
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         join u in db.AppUsers on d.UserName equals u.UserName
                         join u1 in db.AppUsers on d.User_Autho1 equals u1.UserName
                         join u2 in db.AppUsers on d.User_Autho2 equals u2.UserName
                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                         join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO
                         where d.Id == ma
                         select new ExportDailyModel
                         {
                             DailyId = d.Id,
                             Id = dd.Id,
                             FullName = u.FullName,
                             FullName1 = u1.FullName,
                             FullName2 = u2.FullName,
                             Date = d.Date,
                             FromTime = dd.FormTime,
                             ToTime = dd.ToTime,
                             Total = d.Total_Job,
                             Content = dd.Content_Job,
                             Method = dd.Method,
                             Result = dd.Result,
                             Comment1 = dd.Comment1,
                             Comment2 = dd.Comment2,
                             Ten_TO = t.Ten_TO,
                             Ten_CV = c.Ten_CV
                         }).ToList();
            XtraReportDaily report = new XtraReportDaily();
            report.DataSource = model;
            return DocumentViewerExtension.ExportTo(report);
        }

        public ActionResult SearchLevel()
        {
            var user = new ExportReportDao().GetLevelUser(HttpContext.User.Identity.Name);
            if (!string.IsNullOrEmpty(user.Ma_TO))
            {
                ViewBag.Check = 1;
            }
            else
            {
                ViewBag.Check = 0;
                user.Ma_BP = user.Ma_BP == "BGD" ? "" : user.Ma_BP;
                ViewBag.MaTo = new SelectList(new TeamDao().load().Where(m => m.Ma_BP.Contains(user.Ma_BP)).ToList(), "Ma_TO", "Ten_TO");
            }

            return View();
        }

        public ActionResult SearchLoad(DateTime FromTime, DateTime ToTime, string Ma_TO)
        {
            var user = new ExportReportDao().GetLevelUser(HttpContext.User.Identity.Name);

            Ma_TO = Ma_TO == "undefined" ? user.Ma_TO : Ma_TO;

            user.Ma_BP = user.Ma_BP == "BGD" ? "" : user.Ma_BP;
            List<LevelReportModel> data = new List<LevelReportModel>();
            if (user.Display == 1)
            {
                data = new ExportReportDao().ReprotLevel(FromTime, ToTime, user.Ma_BP, Ma_TO).Where(m => m.UserName == user.UserName).ToList();
            }
            else
                data = new ExportReportDao().ReprotLevel(FromTime, ToTime, user.Ma_BP, Ma_TO);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Report cho báo cáo công việc
        public ActionResult ExportGridDaily(DateTime FromTime, DateTime Totime)
        {
            EFDbContext db = new EFDbContext();
            var model = (from dd in db.tbl_DailyDetail
                         join d in db.tbl_Daily on dd.DailyId equals d.Id
                         where d.UserName == HttpContext.User.Identity.Name && d.Date >= FromTime && d.Date <= Totime
                         select new
                         {
                             Thoi_Gian = d.Date,
                             Noi_Dung = dd.Content_Job,
                             Phuong_Phap = dd.Method,
                             Ket_Qua = dd.Result,
                             Danh_Gia_TT = dd.Level_1.HasValue ? ("Mức " + dd.Level_1) : "",
                             Y_Kien_TT = dd.Comment1,
                             Danh_Gia_PT = dd.Level_2.HasValue ? ("Mức " + dd.Level_2) : "",
                             Y_Kien_PT = dd.Comment2,
                             Danh_Gia_TP = dd.Level_3.HasValue ? ("Mức " + dd.Level_3) : "",
                             Y_Kien_TP = dd.Comment3,
                         }).ToList().Select(m => new
                         {
                             Thoi_Gian = m.Thoi_Gian,
                             Noi_Dung = m.Noi_Dung,
                             Phuong_Phap = m.Phuong_Phap,
                             Ket_Qua = m.Ket_Qua,
                             Danh_Gia_TT = m.Danh_Gia_TT,
                             Y_Kien_TT = m.Y_Kien_TT,
                             Danh_Gia_PT = m.Danh_Gia_PT,
                             Y_Kien_PT = m.Y_Kien_PT,
                             Danh_Gia_TP = m.Danh_Gia_TP,
                             Y_Kien_TP = m.Y_Kien_TP
                         }).ToList();
            string documentName = "Từ " + FromTime.ToString("dd/MM/yyyy") + " Tới " + Totime.ToString("dd/MM/yyyy");
            documentName = string.Format("Bao cao nhat ky cong viec {0}.xlsx", documentName);
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/Bao cao nhat ky cong viec.xlsx");
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet1"];
                    int i = 0;

                    foreach (var item in model)
                    {
                        sheet.Cells[2 + i, 1, 2 + i, 10].Copy(sheet.Cells[3 + i, 1, 3 + i, 10]);
                        sheet.Cells[2 + i, 1].Value = item.Thoi_Gian;
                        sheet.Cells[2 + i, 2].Value = item.Noi_Dung;
                        sheet.Cells[2 + i, 3].Value = item.Phuong_Phap;
                        sheet.Cells[2 + i, 4].Value = item.Ket_Qua;
                        sheet.Cells[2 + i, 5].Value = item.Danh_Gia_TT;
                        sheet.Cells[2 + i, 6].Value = item.Y_Kien_TT;
                        sheet.Cells[2 + i, 7].Value = item.Danh_Gia_PT;
                        sheet.Cells[2 + i, 8].Value = item.Y_Kien_PT;
                        sheet.Cells[2 + i, 9].Value = item.Danh_Gia_TP;
                        sheet.Cells[2 + i, 10].Value = item.Y_Kien_TP;
                        i++;
                    }
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }

            return View();
        }

        #region Export to Excel

        public void EvalueTrade(int dx)
        {
          
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/Sample Ho So.xlsx");

            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet sheet_1 = package.Workbook.Worksheets["Danh gia NCC"];
                    ExcelWorksheet sheet_2 = package.Workbook.Worksheets["TBE"];
                    ExcelWorksheet sheet_3 = package.Workbook.Worksheets["CBE"];

                    var _supplier = new ExportReportDao().ExportSupplier(dx);
                    var _dexuat = new DeXuatDao().Detail(dx);
                    var _dgkt = new DGKTDao().Load(dx);
                    var _dgtm = new DGTMDao().Load(dx);
                    var _tm = new DGTMDao().LoadTM(dx);
                    var _user = new UserDao().load();
                    sheet_1.Cells[8, 1].Value = "V/v: " + _dexuat.Tieu_De;
                    sheet_1.Cells[12, 1].Value = "- Căn cứ đề xuất số " + _dexuat.Ma;
                    sheet_1.Cells[13, 1].Value = "- Căn cứ Kế hoạch mua Hàng hóa đã được phê duyệt ngày " + _dexuat.Ngay_Gui;
                    sheet_1.Cells[37, 3].Value = _supplier.First(g => g.DG_Chung == _supplier.Min(g1 => g1.DG_Chung)).Ten;
                    sheet_1.Cells[38, 3].Value = _dexuat.Tieu_De;

                    sheet_2.Cells[27, 3].Value = _user.Where(u => u.UserName == _dexuat.Ten_Dg4).Select(u => u.FullName).FirstOrDefault();
                    sheet_2.Cells[27, 6].Value = _user.Where(u => u.UserName == _dexuat.Ten_Dg2).Select(u => u.FullName).FirstOrDefault();

                    sheet_3.Cells[36, 3].Value = _user.Where(u => u.UserName == _dexuat.Ten_Dg3).Select(u => u.FullName).FirstOrDefault();
                    sheet_3.Cells[36, 7].Value = _user.Where(u => u.UserName == _dexuat.Ten_Dg1).Select(u => u.FullName).FirstOrDefault();

                    int rowIndex = 0;
                    int i = 1;
                    int t = _supplier.Count;
                    int rowInsert = (t - 1) * 4;
                    int index_kt = 0, index_tm = 0;
                    int TongTien = 0;
                    int count_kt = _dgkt.Count;
                    if (t > 3)
                    {
                        sheet_2.InsertColumn(7, (t - 3) * 2);
                        sheet_3.InsertColumn(7, (t - 3) * 2);
                    }

                    sheet_1.InsertRow(22, rowInsert);
                    sheet_1.InsertRow(26 + rowInsert, t - 1);
                    sheet_2.InsertRow(19, count_kt - 1);
                    sheet_3.InsertRow(20, count_kt - 1);
                    if (count_kt > 1)
                    {
                        sheet_2.Cells[18, 1, 18, 6].Copy(sheet_2.Cells[19, 1, 17 + count_kt, 6]);
                        sheet_3.Cells[19, 1, 19, 6].Copy(sheet_3.Cells[20, 1, 18 + count_kt, 6]);
                    }

                    foreach (var item in _supplier)
                    {
                        if (i < t)
                        {
                            sheet_1.Cells[18 + rowIndex, 1, 21 + rowIndex, 5].Copy(sheet_1.Cells[18 + rowIndex + 4, 1, 21 + rowIndex + 4, 5]);
                            sheet_1.Cells[25 + rowInsert, 1, 25 + rowInsert, 5].Copy(sheet_1.Cells[25 + i + rowInsert, 1, 25 + i + rowInsert, 5]);
                            sheet_2.Cells[16, 5 + (i - 1) * 2, 18 + count_kt, 6 + (i - 1) * 2].Copy(sheet_2.Cells[16, 7 + (i - 1) * 2, 18 + count_kt, 8 + (i - 1) * 2]);
                            sheet_3.Cells[16, 5 + (i - 1) * 2, 28 + count_kt, 6 + (i - 1) * 2].Copy(sheet_3.Cells[16, 7 + (i - 1) * 2, 28 + count_kt, 8 + (i - 1) * 2]);
                        }

                        sheet_2.Cells[11 + i, 2].Value = sheet_2.Cells[16, 5 + (i - 1) * 2, 16, 6 + (i - 1) * 2].Value
                        = sheet_3.Cells[11 + i, 2].Value = sheet_3.Cells[16, 5 + (i - 1) * 2, 16, 6 + (i - 1) * 2].Value
                        = item.Ma_NCC;
                        sheet_2.Cells[11 + i, 3].Value = sheet_3.Cells[11 + i, 3].Value = item.Ten;
                        // foreach đánh giá ky thuật
                        foreach (var item_kt in _dgkt)
                        {
                            sheet_2.Cells[18 + index_kt, 1].Value = index_kt + 1;
                            sheet_2.Cells[18 + index_kt, 2].Value = item_kt.Ten + "\n" + item_kt.MoTa;
                            sheet_2.Cells[18 + index_kt, 3].Value = item_kt.DVT;
                            sheet_2.Cells[18 + index_kt, 4].Value = item_kt.So_luong;

                            foreach (var item_kt1 in item_kt.subject)
                            {
                                if (item_kt1.NCC == item.Ma_NCC)
                                {
                                    sheet_2.Cells[18 + index_kt, 5 + (i - 1) * 2].Value = item_kt1.Ten_DG + "\n" + item_kt1.MoTa_DG;
                                    sheet_2.Cells[18 + index_kt, 6 + (i - 1) * 2].Value = item_kt1.DG ? "Đạt" : "Không Đạt" + item_kt1.Ghi_Chu == null ? "" : ("\n" + item_kt1.Ghi_Chu);
                                    break;
                                }
                            }
                            index_kt++;
                        }

                        // foreach đánh giá thương mại
                        foreach (var item_tm in _dgtm)
                        {
                            sheet_3.Cells[19 + index_tm, 1].Value = index_tm + 1;
                            sheet_3.Cells[19 + index_tm, 2].Value = item_tm.Ten + "\n" + item_tm.Mo_Ta;
                            sheet_3.Cells[19 + index_tm, 3].Value = item_tm.DVT;
                            sheet_3.Cells[19 + index_tm, 4].Value = item_tm.So_luong;

                            foreach (var item_tm1 in item_tm.subjectTM)
                            {
                                if (item_tm1.NCC == item.Ma_NCC)
                                {
                                    sheet_3.Cells[19 + index_tm, 5 + (i - 1) * 2].Value = item_tm1.Don_Gia;
                                    sheet_3.Cells[19 + index_tm, 6 + (i - 1) * 2].Value = item_tm1.Tien;
                                    if (item.DG_Chung == 1)
                                    {
                                        TongTien += item_tm1.Tien.Value;
                                    }

                                    break;
                                }
                            }
                            index_tm++;
                        }
                        var _itemtm = _tm.Where(tm1 => tm1.Ma_NCC == item.Ma_NCC).FirstOrDefault();

                        if (item.DG_Chung == 1)
                        {
                            sheet_1.Cells[39 + rowInsert + t, 3].Value = _itemtm.Thoi_Gian;
                        }
                        index_kt = 0;
                        index_tm = 0;

                        sheet_2.Cells[18 + count_kt, 5 + (i - 1) * 2, 18 + count_kt, 6 + (i - 1) * 2].Value = item.DG_KT.HasValue ? (item.DG_KT.Value ? "Đạt" : "Không Đạt") : "";

                        sheet_3.Cells[28 + count_kt, 5 + (i - 1) * 2, 28 + count_kt, 6 + (i - 1) * 2].Value = item.DG_TM;
                        sheet_3.Cells[20 + count_kt, 5 + (i - 1) * 2, 20 + count_kt, 6 + (i - 1) * 2].Formula = "Sum(" + sheet_3.Cells[19, 6 + (i - 1) * 2].Address + ":" + sheet_3.Cells[18 + count_kt, 6 + (i - 1) * 2].Address + ")*10%";
                        sheet_3.Cells[21 + count_kt, 5 + (i - 1) * 2, 21 + count_kt, 6 + (i - 1) * 2].Formula = "Sum(" + sheet_3.Cells[19, 6 + (i - 1) * 2].Address + ":" + sheet_3.Cells[18 + count_kt, 6 + (i - 1) * 2].Address + ")+" + sheet_3.Cells[20 + count_kt, 5 + (i - 1) * 2, 20 + count_kt, 6 + (i - 1) * 2].Address;
                        if (_itemtm != null)
                        {
                            sheet_3.Cells[19 + count_kt, 5 + (i - 1) * 2, 19 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Van_Chuyen ? "Bao gồm phí vận chuyển" : "Không bao gồm phí vận chuyển";
                            sheet_3.Cells[22 + count_kt, 5 + (i - 1) * 2, 22 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Dia_Diem;
                            sheet_3.Cells[23 + count_kt, 5 + (i - 1) * 2, 23 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Thoi_Gian;
                            sheet_3.Cells[24 + count_kt, 5 + (i - 1) * 2, 24 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Dieu_Kien;
                            sheet_3.Cells[25 + count_kt, 5 + (i - 1) * 2, 25 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.BH;
                            sheet_3.Cells[26 + count_kt, 5 + (i - 1) * 2, 26 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Che_Do;
                            sheet_3.Cells[27 + count_kt, 5 + (i - 1) * 2, 27 + count_kt, 6 + (i - 1) * 2].Value = _itemtm.Hieu_Luc;
                        }

                        sheet_1.Cells[30 + rowInsert + t, 2, 34 + rowInsert + t, 2].Copy(sheet_1.Cells[30 + rowInsert + t, 2 + i, 34 + rowInsert + t, 2 + i]);
                        sheet_1.Cells[30 + rowInsert + t, 2 + i].Value = sheet_1.Cells[24 + i + rowInsert, 4].Value = item.Ma_NCC;
                        sheet_1.Cells[31 + rowInsert + t, 2 + i].Value = item.DG_KT.HasValue ? (item.DG_KT.Value ? "Đạt" : "Không Đạt") : "";
                        sheet_1.Cells[32 + rowInsert + t, 2 + i].Value = item.DG_TM;
                        sheet_1.Cells[33 + rowInsert + t, 2 + i].Value = item.DG_Chung;

                        sheet_1.Cells[18 + rowIndex, 1].Value = sheet_1.Cells[24 + i + rowInsert, 1].Value = i;
                        sheet_1.Cells[18 + rowIndex, 2, 21 + rowIndex, 2].Value = sheet_1.Cells[24 + i + rowInsert, 2].Value = item.Ten;
                        sheet_1.Cells[18 + rowIndex, 4].Value = item.Tel;
                        sheet_1.Cells[18 + rowIndex + 1, 4].Value = item.Fax;
                        sheet_1.Cells[18 + rowIndex + 2, 4].Value = item.Attn;
                        sheet_1.Cells[18 + rowIndex + 3, 4].Value = item.Email;
                        sheet_1.Cells[18 + rowIndex, 5, 21 + rowIndex, 5].Value = item.Dia_Chi;
                        i++;
                        rowIndex += 4;
                    }
                    Double sub_Total;
                    sub_Total = TongTien * 0.1;
                    TongTien = Convert.ToInt32(sub_Total) + TongTien;
                    sheet_1.Cells[38 + rowInsert + t, 3].Value = TongTien.ToString("0,###") + " VNĐ (đã bao gồm thuế VAT)";
                    string documentName = string.Format("Danh gia nha cung cap duoi 30tr de xuat {0}.xlsx", _dexuat.Ma);
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void EvaluePrice(int dx)
        {
            string documentname = string.Format("Thu moi chao gia de xuat {0}.xlsx", dx);
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/Sample Chao gia.xlsx");
            string tieude;
            var _tm = new DeXuatTMDao().load(dx).FirstOrDefault();
            var _user = new UserDao().load();
            var _dxkt = new DeXuatChiTietDao().Load(dx);
            int count = _dxkt.Count;
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    var _supplier = new ExportReportDao().ExportSupplier(dx);

                    foreach (var item in _supplier)
                    {
                        string thoigian = item.Ngay_Exp.HasValue ? item.Ngay_Exp.Value.ToString("dd/MM/yyyy") : "";
                        int i = 0;
                        var _sheet = package.Workbook.Worksheets.Add("Thu moi chao gia " + item.Ten, package.Workbook.Worksheets["Thu moi chao gia"]);
                        thoigian = "Thời gian nộp hồ sơ chào giá: Trước 17h ngày " + thoigian
                            + "(Bản gốc hoặc bằng fax hoặc bản Scan gửi từ địa chỉ email chính thức của Quý Công ty).";
                        tieude = "Công ty Cổ phần Dịch vụ Dầu khí Quảng Ngãi PTSC (PTSC Quảng Ngãi) hiện có nhu cầu mua " + item.Tieu_De
                            + " phục vụ hoạt động sản xuất kinh doanh của Công ty";
                        _sheet.InsertRow(39, count - 1);
                        _sheet.Cells[38 + count, 1, 38 + count, 9].Copy(_sheet.Cells[39, 1, 38 + count, 9]);

                        foreach (var item1 in _dxkt)
                        {
                            i++;
                            _sheet.Cells[38 + i, 2].Value = i;
                            _sheet.Cells[38 + i, 3, 38 + i, 6].Value = item1.Ten + ": \n" + item1.Mo_Ta;
                            _sheet.Cells[38 + i, 7].Value = item1.DVT;
                            _sheet.Cells[38 + i, 8].Value = item1.SoLuong;
                            _sheet.Row(38 + i).Height = 54;
                        }
                        i = 0;
                        _sheet.Cells[23, 1].Value = thoigian;
                        _sheet.Cells[10, 1].Value = tieude;
                        _sheet.Cells[13, 1].Value = _sheet.Cells[13, 1].Value + " " + _tm.Dia_Diem;
                        _sheet.Cells[14, 1].Value = _sheet.Cells[14, 1].Value + " bằng " + _tm.Loai_Tien;
                        _sheet.Cells[15, 1].Value = _sheet.Cells[15, 1].Value + " " + _tm.Hieu_Luc;
                        _sheet.Cells[16, 1].Value = _sheet.Cells[16, 1].Value + " " + _tm.Thoi_Gian;
                        _sheet.Cells[18, 1].Value = _user.Where(m => m.UserName == item.DG).Select(m => m.FullName + "- Email:" + m.Email);
                        if (!string.IsNullOrEmpty(item.DG2))
                        {
                            _sheet.Cells[19, 1].Value = _user.Where(m => m.UserName == item.DG1).Select(m => m.FullName + "- Email:" + m.Email);
                        }
                        if (!string.IsNullOrEmpty(item.DG2))
                        {
                            _sheet.InsertRow(20, 1);
                            _sheet.Cells[18, 1, 18, 9].Copy(_sheet.Cells[20, 1, 20, 9]);
                            _sheet.Cells[20, 1].Value = _user.Where(m => m.UserName == item.DG2).Select(m => m.FullName + "- Email:" + m.Email);
                            i++;
                        }
                    }
                    package.Workbook.Worksheets["Thu moi chao gia"].Hidden = eWorkSheetHidden.VeryHidden;
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentname);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void EvaluePlan(int dx)
        {
            string documentName = string.Format("Lap ke hoach mua sam {0}.xlsx", dx);
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/Sample Lap Ke Hoach.xlsx");
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["KH MUA HÀNG"];
                    var _dexuat = new DeXuatDao().Detail(dx);
                    var _supplier = new ExportReportDao().ExportSupplier(dx);
                    var _user = new UserDao().load();
                    int rowIndex = 0;
                    int i = 1;
                    int m = 0;

                    sheet.Cells[7, 1].Value = "V/v: " + _dexuat.Tieu_De;
                    sheet.Cells[10, 1].Value = "- Căn cứ đề xuất số " + _dexuat.Ma;
                    sheet.Cells[14, 1].Value = _dexuat.Tieu_De;
                    sheet.Cells[20, 3].Value = ": " + (_dexuat.Ngay_Gui.HasValue ? _dexuat.Ngay_Gui.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[21, 3].Value = ": " + (_dexuat.Ngay_Exp.HasValue ? _dexuat.Ngay_Exp.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[22, 3].Value = ": " + (_dexuat.Ngay_Eval.HasValue ? _dexuat.Ngay_Eval.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[23, 3].Value = ": " + (_dexuat.Ngay_PD.HasValue ? _dexuat.Ngay_PD.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[24, 3].Value = ": " + (_dexuat.Ngay_HD.HasValue ? _dexuat.Ngay_HD.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[25, 3].Value = ": " + (_dexuat.Ngay_PHD.HasValue ? _dexuat.Ngay_PHD.Value.ToString("dd/MM/yyyy") : "");
                    sheet.Cells[27, 1].Value = "1.";
                    sheet.Cells[27, 2].Value = _user.Where(k => k.UserName == _dexuat.Ten_Dg).Select(k => k.FullName + "- Email:" + k.Email) + ": Tổ trưởng";

                    sheet.Cells[28, 1].Value = "2.";
                    sheet.Cells[28, 2].Value = _user.Where(k => k.UserName == _dexuat.Ten_Dg1).Select(k => k.FullName + "- Email:" + k.Email) + ": Tổ phó thuong mại";

                    sheet.Cells[29, 1].Value = "3.";
                    sheet.Cells[29, 2].Value = _user.Where(k => k.UserName == _dexuat.Ten_Dg2).Select(k => k.FullName + "- Email:" + k.Email) + ": Tổ phó kỹ thuật";

                    if (!string.IsNullOrEmpty(_dexuat.Ten_Dg3))
                    {
                        m++;
                        sheet.InsertRow(30, 1);
                        sheet.Cells[29, 1, 29, 5].Copy(sheet.Cells[30, 1, 30, 5]);
                        sheet.Cells[30, 1].Value = "4.";
                        sheet.Cells[30, 2].Value = _user.Where(k => k.UserName == _dexuat.Ten_Dg3).Select(k => k.FullName + "- Email:" + k.Email) + ": Tổ viên thương mại";
                    }
                    if (!string.IsNullOrEmpty(_dexuat.Ten_Dg4))
                    {
                        m++;
                        sheet.InsertRow(31, 1);
                        sheet.Cells[29, 1, 29, 5].Copy(sheet.Cells[31, 1, 31, 5]);
                        sheet.Cells[31, 1].Value = "5.";
                        sheet.Cells[31, 2].Value = _user.Where(k => k.UserName == _dexuat.Ten_Dg4).Select(k => k.FullName + "- Email:" + k.Email) + ": Tổ viên kỹ thuật";
                    }

                    int t = _supplier.Count;
                    int rowInsert = (t - 1) * 4;
                    foreach (var item in _supplier)
                    {
                        if (i < t)
                        {
                            sheet.Cells[37 + m + rowIndex, 1, 40 + m + rowIndex, 5].Copy(sheet.Cells[37 + m + rowIndex + 4, 1, 40 + m + rowIndex + 4, 5]);
                        }
                        sheet.Cells[37 + m + rowIndex, 1].Value = i;
                        sheet.Cells[37 + m + rowIndex, 2, 40 + m + rowIndex, 2].Value = item.Ten;
                        sheet.Cells[37 + m + rowIndex, 4].Value = item.Tel;
                        sheet.Cells[37 + m + rowIndex + 1, 4].Value = item.Fax;
                        sheet.Cells[37 + m + rowIndex + 2, 4].Value = item.Attn;
                        sheet.Cells[37 + m + rowIndex + 3, 4].Value = item.Email;
                        sheet.Cells[37 + m + rowIndex, 5, 37 + m + rowIndex, 5].Value = item.Dia_Chi;
                        i++;
                        rowIndex += 4;
                    }

                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void EvalueTrade1(int dx)
        {
            
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/Sample Ho So 1.xlsx");
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet sheet_1 = package.Workbook.Worksheets["Danh gia NCC"];

                    var _supplier = new ExportReportDao().ExportSupplier(dx);
                    var _dexuat = new DeXuatDao().Detail(dx);
                    var _tm = new DGTMDao().LoadTM(dx);
                    var _dgtm = new DGTMDao().Load(dx);
                    sheet_1.Cells[8, 1].Value = "V/v: " + _dexuat.Tieu_De;
                    sheet_1.Cells[12, 1].Value = "- Căn cứ đề xuất số " + _dexuat.Ma;
                    sheet_1.Cells[13, 1].Value = "- Căn cứ Kế hoạch mua Hàng hóa đã được phê duyệt ngày " + _dexuat.Ngay_Gui.Value.ToString("dd/mm/yyy");
                    sheet_1.Cells[29, 3].Value = _supplier.First(g => g.DG_Chung == _supplier.Min(g1 => g1.DG_Chung)).Ten;
                    sheet_1.Cells[30, 3].Value = _dexuat.Tieu_De;
                    int rowIndex = 0;
                    int t = _supplier.Count;
                    int rowInsert = (t - 1) * 4;
                    int i = 1;
                    int TongTien = 0;
                   
                    sheet_1.InsertRow(22, rowInsert);
                    
                    
                    foreach (var item in _supplier)
                    {
                        if (i < t)
                        {
                            sheet_1.Cells[17 + rowIndex, 1, 20 + rowIndex, 5].Copy(sheet_1.Cells[17 + rowIndex + 4, 1, 20 + rowIndex + 4, 5]);
                            sheet_1.Cells[23 + rowInsert, 3, 26 + rowInsert, 3].Copy(sheet_1.Cells[23 + rowInsert, 3 + i, 26 + rowInsert, 3 + i]);
                        }

                        // foreach đánh giá thương mại
                        foreach (var item_tm in _dgtm)
                        {
                            foreach (var item_tm1 in item_tm.subjectTM)
                            {
                                if (item_tm1.NCC == item.Ma_NCC)
                                {
                                  
                                    if (item.DG_Chung == 1)
                                    {
                                        TongTien += item_tm1.Tien.Value;
                                    }

                                    break;
                                }
                            }
                        }
                        var _itemtm = _tm.Where(tm1 => tm1.Ma_NCC == item.Ma_NCC).FirstOrDefault();
                        if (item.DG_Chung == 1)
                        {
                           
                            sheet_1.Cells[32 + rowInsert, 3].Value = _itemtm.Thoi_Gian;
                        }
                      
                        sheet_1.Cells[23 + rowInsert, 2 + i].Value = item.Ma_NCC;
                        sheet_1.Cells[24 + rowInsert, 2 + i].Value = item.DG_KT.HasValue ? (item.DG_KT.Value ? "Đạt" : "Không Đạt") : "";
                        sheet_1.Cells[25 + rowInsert , 2 + i].Value = item.DG_TM;
                        sheet_1.Cells[26 + rowInsert, 2 + i].Value = item.DG_Chung;

                        sheet_1.Cells[17 + rowIndex, 1].Value = i;
                        sheet_1.Cells[17 + rowIndex, 2, 20 + rowIndex, 2].Value  = item.Ten;
                        sheet_1.Cells[17 + rowIndex, 4].Value = item.Tel;
                        sheet_1.Cells[17 + rowIndex + 1, 4].Value = item.Fax;
                        sheet_1.Cells[17 + rowIndex + 2, 4].Value = item.Attn;
                        sheet_1.Cells[17 + rowIndex + 3, 4].Value = item.Email;
                        sheet_1.Cells[17 + rowIndex, 5, 21 + rowIndex, 5].Value = item.Dia_Chi;
                        i++;
                        rowIndex += 4;
                    }
                    Double sub_Total;
                    sub_Total = TongTien * 0.1;
                    TongTien = Convert.ToInt32(sub_Total) + TongTien;
                    sheet_1.Cells[31 + rowInsert, 3].Value = TongTien.ToString("0,###") + " VNĐ (đã bao gồm thuế VAT)";
                    string documentName = string.Format("Danh gia nha cung cap duoi 30tr de xuat {0}.xlsx", _dexuat.Ma);
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        // Report cho phiếu nghiệm thu
        public void ExportSoTheoDoi()
        {
            string documentName = string.Format("QN-QUA-PR06-FM03 So theo doi {0}.xlsx", DateTime.Now.ToShortDateString());
            string templateDocument = HttpContext.Server.MapPath("~/Files/Templates/QN-QUA-PR06-FM03 So theo doi.xlsx");
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["So theo doi"];
                    ExcelWorksheet sheet_1 = package.Workbook.Worksheets["So theo doi chi tiet"];

                    var model = new RequestDao().load("").Where(m => m.User_Autho != null).ToList();
                    var tbl = new CheckDao().FindAll().OrderBy(m => m.DeXuat).ToList();
                    int i = 1;
                    foreach (var item in model)
                    {
                        sheet.Cells[3 + i, 1, 3 + i, 8].Copy(sheet.Cells[4 + i, 1, 4 + 1, 8]);
                        sheet.Cells[3 + i, 1].Value = i;
                        sheet.Cells[3 + i, 2].Value = item.LateId;
                        sheet.Cells[3 + i, 3].Value = item.DeXuat;
                        sheet.Cells[3 + i, 4].Value = item.HopDong;

                        sheet.Cells[3 + i, 5].Value = item.Date;
                        sheet.Cells[3 + i, 6].Value = item.Date_Autho;
                        sheet.Cells[3 + i, 7].Value = item.Hang_Muc;
                        sheet.Cells[3 + i, 8].Value = item.FullName_1;
                        switch (item.Status_Autho)
                        {
                            case "A":
                                sheet.Cells[3 + i, 9].Value = "Đạt";
                                break;

                            case "R":
                                sheet.Cells[3 + i, 9].Value = "Không đạt";
                                break;

                            case "W":
                                sheet.Cells[3 + i, 9].Value = "Đạt một phần";
                                break;

                            default:
                                sheet.Cells[3 + i, 9].Value = "Đang chờ";
                                break;
                        }
                        i++;
                    }
                    i = 1;
                    foreach (var item1 in tbl)
                    {
                        sheet_1.Cells[3 + i, 1, 3 + i, 10].Copy(sheet_1.Cells[4 + i, 1, 4 + 1, 10]);
                        sheet_1.Cells[3 + i, 1].Value = i;
                        sheet_1.Cells[3 + i, 2].Value = item1.DeXuat;
                        sheet_1.Cells[3 + i, 3].Value = item1.HopDong;
                        sheet_1.Cells[3 + i, 4].Value = item1.Ma_TB;
                        sheet_1.Cells[3 + i, 5].Value = item1.TT_KT;
                        sheet_1.Cells[3 + i, 6].Value = item1.TT_SL;
                        sheet_1.Cells[3 + i, 7].Value = item1.Date;
                        sheet_1.Cells[3 + i, 8].Value = model.Where(m => m.Id == item1.RequestId).Select(m => m.Date_Autho);
                        sheet_1.Cells[3 + i, 9].Value = item1.Result.HasValue ? (item1.Result.Value ? "Đạt" : "Không đạt") : "Chưa đánh giá";
                        sheet_1.Cells[3 + i, 10].Value = item1.Reason;
                    }
                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.AddHeader("content-disposition",
                              "attachment;filename=" + documentName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        #endregion Export to Excel
    }
}