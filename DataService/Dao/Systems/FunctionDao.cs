using Data.Data;
using DataModel.Model.Commom;
using DataModel.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.Systems
{
    public class FunctionDao
    {
        private readonly EFDbContext db;
            public FunctionDao()
        {
            db = new EFDbContext();
        }
        public List<FunctionViewModel> load()
        {
            var tbls = db.Functions.Select(x => new FunctionViewModel {
                Id = x.Id,
                Name=x.Name,
                Url=x.Url,
                DisplayOrder=x.DisplayOrder,
                ParentId=x.ParentId,
                Status=x.Status,
                IconCss=x.IconCss
            }).ToList();
            var parents = tbls.Where(m => m.ParentId == null).ToList();
            foreach(var parent in parents)
            {
                parent.Functions1 = tbls.Where(x => x.ParentId == parent.Id).OrderBy(x=>x.DisplayOrder).ToList();
            }
            return parents.OrderBy(x=>x.DisplayOrder).ToList();
        }
        public FunctionModel detail(string ma)
        {
            var tbl = db.Functions.Where(m => m.Id.Equals(ma)).Select(m => new FunctionModel
            {
                Id = m.Id,
                Name = m.Name,
                Url = m.Url,
                DisplayOrder = m.DisplayOrder,
                ParentId = m.ParentId,
                Status = m.Status,
                IconCss = m.IconCss
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(FunctionModel model)
        {
            try
            {
                Function tbl = new Function();
                tbl.Id = model.Id;
                tbl.Name = model.Name;
                tbl.Url = model.Url;
                tbl.DisplayOrder = model.DisplayOrder;
                tbl.ParentId = model.ParentId;
                tbl.Status = model.Status;
                tbl.IconCss = model.IconCss;
                db.Functions.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update (FunctionModel model)
        {
            try
            {
                Function tbl = db.Functions.Where(m => m.Id.Equals(model.Id)).FirstOrDefault();
                tbl.Name = model.Name;
                tbl.Url = model.Url;
                tbl.DisplayOrder = model.DisplayOrder;
                tbl.ParentId = model.ParentId;
                tbl.Status = model.Status;
                tbl.IconCss = model.IconCss;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(string ma)
        {
            try
            {
                Function tbl = db.Functions.Where(m => m.Id.Equals(ma)).FirstOrDefault();
                db.Functions.Remove(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Function> GetAllWithParent(string ParentId)
        {
            return db.Functions.Where(m => m.ParentId.Equals(ParentId)).ToList();
        }
        public List<FunctionViewModel> GetAllWithPermission(string user)
        {
           var tbl = (from f in db.Functions
                                  join p in db.Permissions on f.Id equals p.FunctionId
                                  join r in db.AppRoles on p.RoleId equals r.Id
                                  join a in db.AppUserRoles on r.Id equals a.RoleId
                                  join u in db.AppUsers on a.UserId equals u.Id
                                  where u.UserName.Equals(user) && p.CanRead==true
                                  select new FunctionViewModel {
                                      Id = f.Id,
                                      Name = f.Name,
                                      Url = f.Url,
                                      DisplayOrder = f.DisplayOrder,
                                      ParentId = f.ParentId,
                                      Status = f.Status,
                                      IconCss = f.IconCss
                                  } ).ToList();
            var parents = tbl.Where(m => m.ParentId == null).ToList();
            foreach (var parent in parents)
            {
                parent.Functions1 = tbl.Where(x => x.ParentId == parent.Id).OrderBy(x => x.DisplayOrder).ToList();
            }
            return parents.OrderBy(x => x.DisplayOrder).ToList();
           
        }
       
    }
}