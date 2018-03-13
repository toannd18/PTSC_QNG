using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Reports
{
    public class LevelReportModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Ten_To { get; set; }
        public int? Level_1 { get; set; }
        public int? Level_2 { get; set; }
        public int? Level_3 { get; set; }
        public int? Level_4 { get; set; }
        public int? Level_5 { get; set; }
    }
}