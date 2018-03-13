using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Applications
{
    public class ResponeModel
    {
        public int Id { get; set; }
        public string LateId { get; set; }
        public string Ten_BP { get; set; }
        public string Dia_Diem { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string FullName { get; set; }
        public string User_Nhap { get; set; }
        public string Date_Autho { get; set; }
        public string Url { get; set; }
        public string UrlFile { get; set; }
        public string Hang_Muc { get; set; }
    }
}