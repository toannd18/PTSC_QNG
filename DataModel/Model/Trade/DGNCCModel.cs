using System.ComponentModel.DataAnnotations;

namespace DataModel.Model.Trade
{
    public class DGNCCModel
    {
        public int Id { get; set; }
        public int DeXuatId { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn nhà cung cấp", AllowEmptyStrings = false)]
        [Display(Name = "Nhà cung cấp")]
        public string Ma_NCC { get; set; }

        [Display(Name = "Đánh giá kỹ thuật")]
        public string Ten_NCC { get; set; }

        public bool? DG_KT { get; set; }

        [Display(Name = "Đánh giá thương mại")]
        public int? DG_TM { get; set; }

        [Display(Name = "Đề xuất lựa chọn")]
        public int? DG { get; set; }
    }
}