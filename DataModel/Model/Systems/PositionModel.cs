using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class PositionModel
    {
        [Required(ErrorMessage = "Yêu Cầu Nhập Mã Chức Vụ")]
        public string Ma_CV { get; set; }
        
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Chức Vụ")]
        public string Ten_CV { get; set; }
        public Nullable<int> Display { get; set; }
        public bool isUpdate { get; set; }
        public PositionModel()
        {
            isUpdate = false;
        }
    }
}