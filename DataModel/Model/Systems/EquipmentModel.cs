using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class EquipmentModel
    {
        [Required(ErrorMessage = "Yêu Cầu Nhập Mã Thiết Bị", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 10 Ký Tự")]
        [Display(Name = "Mã Thiết Bị")]
        public string Ma_TB { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Nhà Cung Cấp", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 10 Ký Tự")]
        [Display(Name = "Nhà Cung Cấp")]
        public string Ma_NCC { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Thiết Bị", AllowEmptyStrings = false)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 70 Ký Tự")]
        [Display(Name = "Tên Thiết Bị")]
        public string Ten_TB { get; set; }
        [Display(Name = "Công Suất")]
        public string Cong_Suat { get; set; }
        public bool IsUpdate { get; set; }
        public string Ten_NCC { get; set; }
    }
}