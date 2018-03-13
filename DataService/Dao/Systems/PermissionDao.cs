using Data.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class PermissionDao
    {
        private readonly EFDbContext db;
        public PermissionDao()
        {
            db = new EFDbContext();
        }
        public List<Permission> GetByFunctionId(string functionId)
        {
            return db.Permissions.Where(m => m.FunctionId.Equals(functionId)).ToList();
        }
        public void DeleteAll(string functionId)
        {
            var tbl = db.Permissions.Where(m => m.FunctionId.Equals(functionId)).ToList();
            if (tbl != null)
            {
                foreach (Permission obj in tbl)
                {
                    db.Permissions.Remove(obj);

                }
                db.SaveChanges();
            }
            

        }
        public void Save(Permission model)
        {
            db.Permissions.Add(model);
            db.SaveChanges();
        }
        public void SaveChange()
        {
            db.SaveChanges();
        }
    }
}