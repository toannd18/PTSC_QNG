using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Commom
{
    public class FunctionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public string ParentId { get; set; }
        public bool Status { get; set; }
        public string IconCss
        {
            get; set;
        }
        public ICollection<FunctionViewModel> Functions1 { get; set; }
    }
}