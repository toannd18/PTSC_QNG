using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataModel.Model.Trade
{
    public class DeXuatModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu nhập dữ liệu")]
        [StringLength(50,MinimumLength =5,ErrorMessage ="Ít nhất là 4 ký tự và nhiều nhất là 50 ký tự")]
        [Remote("CheckExistDX", "Requests",AdditionalFields = "Tieu_De", ErrorMessage = "Đề xuất này đã tồn tại")]
        [Display(Name ="Mã")]
        public string Ma { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập dữ liệu")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Ít nhất là 4 ký tự và nhiều nhất là 100 ký tự")]
        [Remote("CheckExistDX", "Requests", AdditionalFields = "Ma", ErrorMessage = "Đề xuất này đã tồn tại")]
        [Display(Name = "Tên Đề Xuất")]
        public string Tieu_De { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập dữ liệu")]
        [Display(Name = "Loại Đề Xuất")]
        public bool Kieu { get; set; }
        [Display(Name ="Người đề xuất")]
        public string Ten_Dx { get; set; }
        [Display(Name = "Trưởng BPYC")]
        public string Ten_Dx1 { get; set; }
        [Display(Name = "Trưởng BP QLHH")]
        public string Ten_Dx2 { get; set; }
        [Display(Name = "Người Phê Duyệt")]
        public string Ten_Dx3 { get; set; }
        [Display(Name = "Trưởng BPMH")]
        public string Ten_Dx4 { get; set; }
        [Display(Name = "Người thực hiện")]
        public string Ten_Dx5 { get; set; }
        [Display(Name ="Tổ trưởng")]
        public string Ten_Dg { get; set; }
        [Display(Name = "Tổ phó thương mại")]
        public string Ten_Dg1 { get; set; }
        [Display(Name = "Tổ phó kỹ thuật")]
        public string Ten_Dg2 { get; set; }
        [Display(Name = "Tổ viên thương mại")]
        public string Ten_Dg3 { get; set; }
        [Display(Name = "Tổ viên kỹ thuật")]
        public string Ten_Dg4 { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true)]
        [Display(Name ="Ngày đề xuất")]
        public DateTime? Ngay_Tao { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Gửi hồ sơ chào giá")]
        public DateTime? Ngay_Gui { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Phê duyệt đánh giá")]
        public DateTime? Ngay_PD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày ký hợp đồng")]
        public DateTime? Ngay_HD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày thực hiện hợp đồng")]
        public DateTime? Ngay_PHD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian nộp chào giá")]
        public DateTime? Ngay_Exp { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian đánh giá")]
        public DateTime? Ngay_Eval { get; set; }
    }
}
