using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Commom
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage ="Yêu cầu nhập mật khẩu mới")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Yêu cầu nhập ít nhất 6 và nhiều nhất là 20 ký tự")]
        public string NewPassWord { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập xác nhận mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Yêu cầu nhập ít nhất 6 và nhiều nhất là 20 ký tự")]
        [Compare("NewPassWord",ErrorMessage ="Không trùng với mật khẩu mới")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nhập mật khẩu cũ")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Yêu cầu nhập ít nhất 6 và nhiều nhất là 20 ký tự")]
        public string OldPassword { get; set; }
    }
}