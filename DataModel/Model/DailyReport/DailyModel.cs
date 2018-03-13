using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.DailyReport
{
    public class DailyModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu chọn thời gian")]
        public System.TimeSpan FormTime { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn thời gian")]
        public System.TimeSpan ToTime { get; set; }
        public string Comment1 { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nội dung công việc")]
        public string Content_Job { get; set; }
        public string Method { get; set; }
        [StringLength(250,ErrorMessage ="Không được nhập quá 250 ký tự")]
        public string Result { get; set; }
        public int DailyId { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn hạng mục công việc")]
        public int JobId { get; set; }
        public string Total_Job { get; set; }
        public string Comment2 { get; set; }
        public int? Level_1 { get; set; }
        public int? Level_2 { get; set; }
        public string Comment3 { get; set; }
        public int? Level_3 { get; set; }
        public string Ten_Job { get; set; }
        public DailyModel()
        {
            Level_1 = 3;
            Level_2 = 3;
        }
       
    }
}