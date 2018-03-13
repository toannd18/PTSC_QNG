using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class DepartmentModel
    {
        [Required(ErrorMessage = "Yêu Cầu Nhập Mã Phòng Ban", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 10 Ký Tự")]
        [Display(Name = "Mã Phòng Ban")]
        public string Ma_BP { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Phòng Ban", AllowEmptyStrings = false)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Yêu Cầu Nhập Không Quá 70 Ký Tự")]
        [Display(Name = "Tên Phòng Ban")]
        public string Ten_BP { get; set; }

        public bool IsUpdate { get; set; }
    }
}