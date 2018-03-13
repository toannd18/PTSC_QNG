using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.Applications
{
    public class SaveApprovalModel
    {
        public long Id { get; set; }
      
        public Nullable<bool> CO { get; set; }
        public Nullable<bool> CQ { get; set; }
        public Nullable<bool> MTR { get; set; }
        public Nullable<bool> SN { get; set; }
        public Nullable<bool> PN { get; set; }
        [Display(Name = "Khác")]
        public Nullable<bool> Other { get; set; }
        [Display(Name = "Ghi Chú")]
        public string Note_Other { get; set; }
        [Display(Name = "Kết Luận")]
        public Nullable<bool> Result { get; set; }
        [Display(Name = "Lý Do")]
        public string Reason { get; set; }
    }
}