using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataModel.Model.Trade
{
    public class DeXuatHDModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn mã hợp đồng")]
        [Display(Name = "Mã đề xuất")]
        [Remote("CheckExistDxHd", "Contractions", ErrorMessage ="Đề xuất này đã tồn tại hợp đồng")]
        public int? DeXuatId { get; set; }

        public string TenDeXuat { get; set; }

        [Display(Name = "Mã số hợp đồng")]
        public int So_HD { get; set; }

        public string Ten_HD { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày ký hợp đồng")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn nhà cung cấp")]
        [Display(Name = "Tên nhà cung cấp")]
        public string Ma_NCC { get; set; }

        public string Ten_NCC { get; set; }

        [Range(0, 10, ErrorMessage = "Thang điểm là 10")]
        [Display(Name = "Đánh giá chất lương")]
        public int Chat_luong { get; set; }

        [Range(0, 10, ErrorMessage = "Thang điểm là 10")]
        [Display(Name = "Đánh giá tiến độ")]
        public int Tien_Do { get; set; }

        [Range(0, 10, ErrorMessage = "Thang điểm là 10")]
        [Display(Name = "Đánh giá giá cả")]
        public int Gia_Ca { get; set; }

        [Range(0, 10, ErrorMessage = "Thang điểm là 10")]
        [Display(Name = "Đánh giá thái độ")]
        public int Thai_Do { get; set; }

        [Display(Name = "Xếp loại")]
        public string Diem { get; set; }

        public string Ten_TH { get; set; }
        public DeXuatHDModel()
        {
            Chat_luong = Tien_Do = Gia_Ca = Thai_Do = 0;
            
           
        }
    }
}