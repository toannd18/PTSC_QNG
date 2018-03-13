using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class FunctionModel
    {
        [Required(ErrorMessage = "Yêu Cầu Nhập Mã Chức Năng")]
        public string Id { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Nhập Tên")]
        public string Name { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public string ParentId { get; set; }
        public bool Status { get; set; }
        public string IconCss { get; set; }
        public bool IsUpdate { get; set; }

    }
}