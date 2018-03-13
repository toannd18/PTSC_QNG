using Data.Data;
using DataModel.Model.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{

    public class RoleDao
    {
        private readonly EFDbContext db;
       public RoleDao()
        {
           db = new EFDbContext();
        }
        public List<AppRole> load()
        {
            return db.AppRoles.ToList();
        }
        public RoleModel detail(int ma)
        {
            var tbl = db.AppRoles.Where(m => m.Id.Equals(ma)).Select(m => new RoleModel
            {
                Id = m.Id,
                Role = m.Role,
                Description = m.Description
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(RoleModel model)
        {
            try
            {
                AppRole tbl = new AppRole();
                tbl.Role = model.Role;
                tbl.Description = model.Description;
                db.AppRoles.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(RoleModel model)
        {
            try
            {
                AppRole tbl = db.AppRoles.Find(model.Id);
                tbl.Role = model.Role;
                tbl.Description = model.Description;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete (int ma)
        {
            try
            {
                AppRole tbl = db.AppRoles.Find(ma);
                db.AppRoles.Remove(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool IsRole(string user, string role)
        {
            var tbl = (from r in db.AppRoles
                       join a in db.AppUserRoles on r.Id equals a.RoleId
                       join u in db.AppUsers on a.UserId equals u.Id
                       where u.UserName.Equals(user) && r.Role.Equals(role)
                       select r).ToList().Count;
            return tbl > 0 ? true : false;
        }
        public bool CheckExistRole(string Role)
        {
            return !db.AppRoles.Any(m => m.Role.Equals(Role));
        }
    }
}