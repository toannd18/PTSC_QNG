
using Data.Data;
using DataService.Dao.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC6.Providers
{
    public class PermissionAttribute
    {
        private readonly EFDbContext db;
        public PermissionAttribute()
        {
            db = new EFDbContext();
        }
        public bool PermissionAuthorization(string action,string function)
        {
            if(!new RoleDao().IsRole(HttpContext.Current.User.Identity.Name, "admin"))
            {
                var permission = (from p in db.Permissions
                                  join a in db.AppUserRoles on p.RoleId equals a.RoleId
                                  join u in db.AppUsers on a.UserId equals u.Id
                                  where u.UserName == HttpContext.Current.User.Identity.Name
                                  select p).ToList();
                if (permission.Count > 0)
                {
                    if (!permission.Exists(x => x.FunctionId == function && x.CanRead) && action == "Read")
                    {
                        return false;
                    }
                    else if (!permission.Exists(x => x.FunctionId == function && x.CanUpdate) && action == "Update")
                    {
                        return false;

                    }
                    else if (!permission.Exists(x => x.FunctionId == function && x.CanCreate) && action == "Create")
                    {
                        return false;
                    }
                    else if (!permission.Exists(x => x.FunctionId == function && x.CanDelete) && action == "Delete")
                    {
                        return false;
                    }
                
                }
                else
                    return false;
                return true;
            }
            return true;
        }
     
        
    }
}