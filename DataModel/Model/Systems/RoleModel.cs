using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataModel.Model.Systems
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Nhập Role",AllowEmptyStrings =false)]
        [Remote("CheckExist","Roles",ErrorMessage ="Mã Đã Tồn Tại")]
        public string Role { get; set; }
        public string Description { get; set; }
    }
}