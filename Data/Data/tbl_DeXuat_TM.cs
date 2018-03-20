using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Data.Data
{
    public class tbl_DeXuat_TM
    {
        [Key]
        public int Id { get; set; }
        public int DeXuatId { get; set; }
        [Display(Name = "Đồng tiền chào giá")]
        public string Loai_Tien { get; set; }
        [Display(Name = "Hiệu lực chào giá")]
        public string Hieu_Luc { get; set; }
        [Display(Name = "Thời gian bảo hành")]
        public string Thoi_Gian { get; set; }

        [Display(Name = "Địa điểm giao hàng")]
        public string Dia_Diem { get; set; }

        [Display(Name = "Điều kiện thanh toán")]
        public string Dieu_Kien { get; set; }

        [Display(Name = "Chính sách bảo hành")]
        public string BH { get; set; }

        [Display(Name = "Chế độ hậu mãi")]
        public string Che_Do { get; set; }

        [Display(Name = "Tiêu chí khác")]
        public string Ghi_Chu { get; set; }
    }
}
