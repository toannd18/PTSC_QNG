using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Dao
{
    public class Account
    {
        private readonly EFDbContext db;
        public Account()
        {
            db = new EFDbContext();
        }
        public bool Login(string username,string password)
        {
            return  db.AppUsers.Where(m => m.UserName.Equals(username) && m.PasswordHash.Equals(password) && m.Status == true).Count()>0?true:false;
        }
    }
}
