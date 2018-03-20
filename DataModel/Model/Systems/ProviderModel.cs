using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
	public class ProviderModel
	{
        [Required(ErrorMessage ="Yêu Cầu Nhập Mã Nhà Cung Cấp",AllowEmptyStrings =false)]
        [StringLength(10,MinimumLength =1,ErrorMessage ="Yêu Cầu Nhập Không Quá 10 Ký Tự")]
        [Display(Name ="Mã Nhà Cung Cấp")]
        public string Ma_NCC { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Nhà Cung Cấp", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 100 Ký Tự")]
        [Display(Name = "Tên Nhà Cung Cấp")]
        public string Ten_NCC { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Địa Chỉ", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 100 Ký Tự")]
        public string Dia_Chi { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 20 Ký Tự")]
        public string Tel { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 20 Ký Tự")]
        public string Fax { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 50 Ký Tự")]
        public string Attn { get; set; }

        [EmailAddress(ErrorMessage ="Yêu Cầu Phải Là Địa Chỉ Email")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 50 Ký Tự")]
        public string Email { get; set; }
        [Display(Name ="Loại hình hàng hóa")]
        public string Hang_Hoa { get; set; }
        [Display(Name = "Loại hình dịch vụ")]
        public string Dich_Vu { get; set; }
        public string Diem { get; set; }
        public bool IsUpdate { get; set; }
    }
}