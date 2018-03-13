using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.Model.Systems
{
    public class RoleUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}