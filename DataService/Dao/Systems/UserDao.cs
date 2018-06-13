using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Data;
using DataModel.Model.Systems;


namespace DataService.Dao.Systems
{
    public class UserDao
    {
        private EFDbContext db;
        public UserDao()
        {
            db = new EFDbContext();
        }
        public List<AppUser> load()
        {
            var model = db.AppUsers.ToList();
            return model;
        }
        public List<AppUserModel> loadview()
        {
            var model = (from a in db.AppUsers
                         join t in db.tbl_TO on a.Ma_TO equals t.Ma_TO into g1
                         from t in g1.DefaultIfEmpty()
                         join b in db.tbl_BP on a.Ma_BP equals b.Ma_BP
                         select new AppUserModel
                         {
                             Id = a.Id,
                             UserName = a.UserName,
                             FullName = a.FullName,
                             Email = a.Email,
                             Avatar = a.Avatar,
                             Ten_To = t.Ten_TO,
                             Ten_BP = b.Ten_BP
                         }).ToList();
            return model;
        }
        public AppUserModel detail(int ma)
        {
            var tbl = db.AppUsers.Where(m => m.Id.Equals(ma)).Select(m => new AppUserModel
            {
                Id = m.Id,
                UserName = m.UserName,
                FullName = m.FullName,
                Address = m.Address,
                Avatar = m.Avatar,
                BrithDay = m.BrithDay,
                Status = m.Status,
                Gender = m.Gender,
                Email = m.Email,
                PasswordHash = m.PasswordHash,
                ConfirmPassword = m.PasswordHash,
                Ma_BP=m.Ma_BP,
                Ma_CV=m.Ma_CV,
                Ma_TO=m.Ma_TO,
            }).FirstOrDefault();
            return tbl;
        }
        public bool Save(AppUserModel model)
        {
            try
            {
                AppUser tbl = new AppUser();
                tbl.Address = model.Address;
                tbl.Avatar = model.Avatar;
                tbl.BrithDay = model.BrithDay;
                tbl.Email = model.Email;
                tbl.FullName = model.FullName;
                tbl.Gender = model.Gender;
                tbl.PasswordHash = model.PasswordHash;
                tbl.UserName = model.UserName;
                tbl.Status = true;
                tbl.Ma_BP = model.Ma_BP;
                tbl.Ma_CV = model.Ma_CV;
                tbl.Ma_TO = model.Ma_TO;
                db.AppUsers.Add(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool Upload(AppUserModel model)
        {
            try
            {
                var tbl = db.AppUsers.Find(model.Id);
                tbl.Address = model.Address;
                tbl.Avatar = model.Avatar;
                tbl.BrithDay = model.BrithDay;
                tbl.Email = model.Email;
                tbl.FullName = model.FullName;
                tbl.Gender = model.Gender;
                tbl.UserName = model.UserName;
                tbl.Ma_BP = model.Ma_BP;
                tbl.Ma_CV = model.Ma_CV;
                tbl.Ma_TO = model.Ma_TO;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int Id)
        {
            try
            {
                var tbl = db.AppUsers.Find(Id);
                db.AppUsers.Remove(tbl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool CheckExistUser(string UserName)
        {
            return !db.AppUsers.Any(m => m.UserName.Equals(UserName));
        }
        public List<FindName> ListName(string name)
        {
            var tbl = db.AppUsers.AsNoTracking().Where(m => (m.FullName.ToLower().Contains(name.ToLower())||m.UserName.ToLower().Contains(name.ToLower()))&& m.Status == true).Select(x => new FindName
            {
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email
            }).ToList();
            return tbl;
        }
        public RoleUserModel DetailRole(int Id)
        {
            var tbl = (from a in db.AppUserRoles
                       join r in db.AppRoles on a.RoleId equals r.Id
                       join u in db.AppUsers on a.UserId equals u.Id
                       where u.Id == Id
                       select new RoleUserModel
                       {
                           Id = a.Id,
                           UserId = Id,
                           UserName = u.UserName,
                           Role = r.Role,
                           RoleId = r.Id
                       }).FirstOrDefault();
            if (tbl != null)
            {
                return tbl;
            
            }
            RoleUserModel model = new RoleUserModel();
            model.UserId = Id;
            return model;
        }
        public bool SaveRole(RoleUserModel model)
        {
            try
            {
                AppUserRole tbl = new AppUserRole();

            if (model.Id > 0)
                {
                    tbl = db.AppUserRoles.Find(model.Id);
                    tbl.RoleId = model.RoleId;
                    tbl.UserId = model.UserId;
                    db.SaveChanges();
                }
             else
                {
                    tbl.RoleId = model.RoleId;
                    tbl.UserId = model.UserId;
                    db.AppUserRoles.Add(tbl);
                    db.SaveChanges();
                }
              return true;
            }
            catch{
                return false;
            }
            
        }
        
    }
    public class FindName{
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
    public class UserViewModel
    {

    }
}