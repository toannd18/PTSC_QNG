using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class JobDao
    {
        private readonly EFDbContext db;
        public JobDao()
        {
            db = new EFDbContext();
        }
        public List<tbl_Job> GetWithUser()
        {
            var user = db.AppUsers.Where(m => m.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault();
            if (string.IsNullOrEmpty(user.Ma_TO))
             {
                var tbl = db.tbl_Job.Where(m => m.Ma_BP == user.Ma_BP).ToList();
                return tbl;
            }
            else
            {
                var tbl = db.tbl_Job.Where(m => m.Ma_TO == user.Ma_TO).ToList();
                return tbl;
            }
            
        }
    }
}