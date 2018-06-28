using System.ComponentModel.DataAnnotations;

namespace Data.Data
{
    public class tbl_DG_TM
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public int DG_NCC_Id { get; set; }

        public int DeXuatId { get; set; }
        [Display(Name ="Hiệu lực")]
        public string Hieu_Luc { get; set; }
        [Display(Name = "Thời gian")]
        public string Thoi_Gian { get; set; }
        [Display(Name = "Địa điểm")]
        public string Dia_Diem { get; set; }
        [Display(Name = "Điều kiện")]
        public string Dieu_Kien { get; set; }
        [Display(Name = "Bảo hành")]
        public string BH { get; set; }
        [Display(Name = "Chế độ")]
        public string Che_Do { get; set; }
        [Display(Name = "Vận chuyển")]
        public bool Van_Chuyen { get; set; }
        [Display(Name = "Khác")]
        public string Ghi_Chu
        {
            get; set;
        }
        public tbl_DG_TM()
        {
            Van_Chuyen = true;
        }
    }
}