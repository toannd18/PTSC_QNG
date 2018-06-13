using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Commom
{
    public class UpdateFile
    {
        public string FullName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int Id { get; set; }
        public string Ghi_Chu { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}