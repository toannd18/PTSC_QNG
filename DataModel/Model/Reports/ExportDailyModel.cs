using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Reports
{
    public class ExportDailyModel
    {
        public int DailyId { get; set; }
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Ten_CV { get; set; }
        public string Ten_TO { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string Total { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public string Result { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string FullName1 { get; set; }
        public string FullName2 { get; set; }
    }
}