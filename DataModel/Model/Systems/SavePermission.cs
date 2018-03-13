using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class SavePermission
    {
        public int RoleId { get; set; }
        public string CanRead { get; set; }
        public string CanUpdate { get; set; }
        public string CanCreate { get; set; }
        public string CanDelete { get; set; }
    }
}