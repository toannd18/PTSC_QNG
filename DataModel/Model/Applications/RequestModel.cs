using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataModel.Model.Applications
{
    public class RequestModel
    {
        public int Id { get; set; }
        public int FirstId { get; set; }
        [Display(Name = "Mã Yêu Cầu")]
        public string LateId { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Chọn Bộ Phận")]
        [Display(Name ="Bộ Phận Yêu Cầu")]
        public string Ma_BP { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Địa Điểm")]
        [Display(Name = "Địa Điểm")]
        public string Dia_Diem { get; set; }
        [StringLength(50,MinimumLength =0,ErrorMessage = "Nhập nhiều nhất 50 ký tự")]
        [Display(Name = "Hạng Mục")]
        public string Hang_Muc { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Nhập nhiều nhất 50 ký tự")]
        [Display(Name = "Hợp Đồng")]
        public string HopDong { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Đề Xuất")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Nhập ít nhất 6 và nhiều nhất 50 ký tự")]
        [Display(Name = "Đề Xuất")]
        public string DeXuat { get; set; }
     
        [Display(Name = "CO")]
        public bool CO { get; set; }
        [Display(Name = "CQ")]
        public bool CQ { get; set; }
        [Display(Name = "Khác")]
        public bool Other { get; set; }
        public string Note { get; set; }
        public string User_Nhap { get; set; }
        
        public System.DateTime Date_Nhap { get; set; }
        public string User_Edit { get; set; }
        public Nullable<System.DateTime> Date_Edit { get; set; }
        public string User_Autho { get; set; }
        public Nullable<System.DateTime> Date_Autho { get; set; }
        public string Status_Autho { get; set; }
        public string Note_Autho { get; set; }
        public string Ten_BP { get; set; }
        [Required(ErrorMessage ="Yêu cầu nhập ngày kiểm tra")]
        [Display(Name = "Ngày Kiểm Tra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> Date { get; set; }
        public string FullName { get; set; }
        [NotMapped]
        public HttpPostedFileBase uploafile { get; set; }
    }
}