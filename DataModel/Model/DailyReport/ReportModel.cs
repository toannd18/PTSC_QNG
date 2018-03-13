using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataModel.Model.DailyReport
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Ten_CV { get; set; }
        public string Ten_BP { get; set; }
        public string Ten_TO { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Yêu cầu nhập thời gian")]
        public System.DateTime Date { get; set; }
        public string Total_Job { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string User_Autho1 { get; set; }
        public string FullName_Autho1 { get; set; }
        public string User_Autho2 { get; set; }
        
        public string FullName_Autho2 { get; set; }
        public string User_Autho3 { get; set; }

        public string FullName_Autho3 { get; set; }
        public string Comment3 { get; set; }
        public bool Status_Autho1 { get; set; }
        public bool Status_Autho2 { get; set; }

        public bool Status_Autho3 { get; set; }
        public string Ma_To { get; set; }
        public string Ma_BP { get; set; }
        public ReportModel()
        {
            Date = System.DateTime.Today;
        }
    }
}