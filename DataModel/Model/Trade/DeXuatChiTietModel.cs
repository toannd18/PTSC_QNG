using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Model.Trade
{
    public class DeXuatChiTietModel
    {
        public int Id { get; set; }
        public int DeXuatId { get; set; }
        [Required(ErrorMessage="Yêu cầu nhập tên hàng hóa")]
        [StringLength(50,ErrorMessage ="Bạn nhập quá 50 ký tự")]
        [Display(Name ="Tên")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả")]
        [Display(Name ="Mô tả")]
        public string Mo_Ta { get; set; }
      
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập đơn vị tính")]
        [Display(Name = "Đơn vị tính")]
        public string DVT { get; set; }

        [Display(Name = "Ghi chú")]
        public string Ghi_Chu { get; set; }
    }
}
