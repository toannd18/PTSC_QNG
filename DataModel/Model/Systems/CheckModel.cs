using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class CheckModel
    {
        public long Id { get; set; }
        public int RequestId { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Chọn Nhà Cung Cấp",AllowEmptyStrings =false)]
        [Display(Name ="Tên Nhà Cung Cấp")]
        public string Ma_NCC { get; set; }
        public string Ten_NCC { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Loại Thiết Bị", AllowEmptyStrings = false)]
        [Display(Name = "Tên Hàng Hóa")]
        public string Ma_TB { get; set; }

        public string Ten_TB { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Thông Số Kỹ Thuật", AllowEmptyStrings = false)]
        [Display(Name = "Kỹ Thuật Yêu Cầu")]
        public string YC_KT { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Thông Số Kỹ Thuật", AllowEmptyStrings = false)]
        [Display(Name = "Kỹ Thuật Thực Tế")]
        public string TT_KT { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Số Lượng", AllowEmptyStrings = false)]
        [Display(Name = "Số Lượng Yêu Cầu")]
        public int YC_SL { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Số Lượng", AllowEmptyStrings = false)]
        [Display(Name = "Số Lượng Thực Tế")]
        public int TT_SL { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Đơn Vị", AllowEmptyStrings = false)]
        [Display(Name = "Đơn Vị")]
        public string DonVi { get; set; }
        public Nullable<bool> CO { get; set; }
        public Nullable<bool> CQ { get; set; }
        public Nullable<bool> MTR { get; set; }
        public Nullable<bool> SN { get; set; }
        public Nullable<bool> PN { get; set; }
        [Display(Name = "Khác")]
        public Nullable<bool> Other { get; set; }
        [Display(Name = "Ghi Chú")]
        public string Note_Other { get; set; }
        [Display(Name = "Kết Luận")]
        public Nullable<bool> Result { get; set; }
        [Display(Name = "Lý Do")]
        public string Reason { get; set; }
        public string User_Nhap { get; set; }
        public Nullable<System.DateTime> Date_Nhap { get; set; }
        public string User_Edit { get; set; }
        public Nullable<System.DateTime> Date_Edit { get; set; }
        public string DeXuat { get; set; }
        public string HopDong { get; set; }
        public string Ten_BP { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string DiaDiem { get; set; }
     
    }
}