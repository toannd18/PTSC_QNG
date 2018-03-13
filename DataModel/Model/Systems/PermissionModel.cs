using DataModel.Model.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class PermissionModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FunctionId { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public RoleModel AppRole { get; set; }
        public FunctionModel Function { get; set; }

    }
}