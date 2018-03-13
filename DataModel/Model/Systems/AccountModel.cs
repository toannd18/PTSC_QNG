using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class AccountModel
    {
        [Required(ErrorMessage ="Yêu Cầu Nhập Tài Khoản",AllowEmptyStrings =false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Mật Khẩu", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}