using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class TeamModel
    {
        [Required(ErrorMessage ="Yêu Cầu Nhập Mã Tổ")]
        public string Ma_TO { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Tổ")]
        public string Ten_TO { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Phòng Ban")]
        public string Ma_BP { get; set; }
        public string Ten_BP { get; set; }
        public bool isUpdate { get; set; }
        public TeamModel()
        {
            isUpdate = false;
        }
    }
}