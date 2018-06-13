using Data.Data;

using DataModel.Model.DailyReport;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Dao.DailyReports
{
    public class DailyReportDao
    {
        private readonly EFDbContext db;

        public DailyReportDao()
        {
            db = new EFDbContext();
        }

        public IQueryable<ReportModel> load()
        {
            var tbl = (from d in db.tbl_Daily
                       join u1 in db.AppUsers on d.UserName equals u1.UserName
                       join u2 in db.AppUsers on d.User_Autho1 equals u2.UserName into gu2
                       from sub2 in gu2.DefaultIfEmpty()
                       join u3 in db.AppUsers on d.User_Autho2 equals u3.UserName into gu3
                       from sub3 in gu3.DefaultIfEmpty()
                       join u4 in db.AppUsers on d.User_Autho3 equals u4.UserName into gu4
                       from sub4 in gu4.DefaultIfEmpty()
                       select new ReportModel
                       {
                           Id = d.Id,
                           UserName = d.UserName,
                           FullName = u1.FullName,
                           Date = d.Date,
                           Total_Job = d.Total_Job,
                           User_Autho1 = sub2.UserName,
                           FullName_Autho1 = sub2.FullName,
                           User_Autho2 = sub3.UserName,
                           FullName_Autho2 = sub3.FullName,
                           User_Autho3 = sub4.UserName,
                           FullName_Autho3 = sub4.FullName,
                           Status_Autho1 = d.Status_Autho1,
                           Status_Autho2 = d.Status_Autho2,
                           Status_Autho3 = d.Status_Autho3,
                           Comment1 = d.Comment1,
                           Comment2 = d.Comment2
                       });
            return tbl;
        }

        public bool Save(ReportModel model)
        {
            try
            {
                tbl_Daily tbl = new tbl_Daily();
                tbl.Date = model.Date;
                tbl.UserName = HttpContext.Current.User.Identity.Name;

                db.tbl_Daily.Add(tbl);

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ReportModel model)
        {
            try
            {
                tbl_Daily tbl = new tbl_Daily();
                tbl = db.tbl_Daily.Find(model.Id);
                tbl.Date = model.Date;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int ma)
        {
            try
            {
                tbl_Daily tbl = new tbl_Daily();
                tbl = db.tbl_Daily.Find(ma);

                db.tbl_Daily.Remove(tbl);

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ReportModel detail(int ma)
        {
            var tbl = (from d in db.tbl_Daily
                       join u in db.AppUsers on d.UserName equals u.UserName
                       join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                       join b in db.tbl_BP on u.Ma_BP equals b.Ma_BP
                       join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO into gr
                       from sub in gr.DefaultIfEmpty()
                       where d.Id == ma
                       select new ReportModel()
                       {
                           Id = d.Id,
                           Date = d.Date,
                           UserName = d.UserName,
                           FullName = u.FullName,
                           Ten_BP = b.Ten_BP,
                           Ten_TO = sub.Ten_TO,
                           Ten_CV = c.Ten_CV,
                       }).FirstOrDefault();
            return tbl;
        }

        public IEnumerable<ReportModel> GetAllWithUser(string Ma_TO)
        {
            var user = (from u in db.AppUsers
                        join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                        where u.UserName == HttpContext.Current.User.Identity.Name
                        select new
                        {
                            UserName = u.UserName,
                            Display = c.Display,
                            Ma_CV = c.Ma_CV,
                            Ma_BP = u.Ma_BP,
                            Ma_TO = u.Ma_TO,
                        }).FirstOrDefault();
            IQueryable<ReportModel> report;
            if (string.IsNullOrEmpty(Ma_TO))
                report = (from d in db.tbl_Daily
                          join u1 in db.AppUsers on d.UserName equals u1.UserName
                          join u2 in db.AppUsers on d.User_Autho1 equals u2.UserName into gu2
                          from sub2 in gu2.DefaultIfEmpty()
                          join u3 in db.AppUsers on d.User_Autho2 equals u3.UserName into gu3
                          from sub3 in gu3.DefaultIfEmpty()
                          select new ReportModel
                          {
                              Id = d.Id,
                              UserName = d.UserName,
                              FullName = u1.FullName,
                              Date = d.Date,
                              Total_Job = d.Total_Job,
                              User_Autho1 = sub2.UserName,
                              FullName_Autho1 = sub2.FullName,
                              User_Autho2 = sub3.UserName,
                              FullName_Autho2 = sub3.FullName,
                              Status_Autho1 = d.Status_Autho1,
                              Status_Autho2 = d.Status_Autho2,
                              Comment1 = d.Comment1,
                              Comment2 = d.Comment2
                          });
            else
                report = (from d in db.tbl_Daily
                          join u1 in db.AppUsers on d.UserName equals u1.UserName
                          join u2 in db.AppUsers on d.User_Autho1 equals u2.UserName into gu2
                          from sub2 in gu2.DefaultIfEmpty()
                          join u3 in db.AppUsers on d.User_Autho2 equals u3.UserName into gu3
                          from sub3 in gu3.DefaultIfEmpty()
                          where u1.Ma_TO == Ma_TO
                          select new ReportModel
                          {
                              Id = d.Id,
                              UserName = d.UserName,
                              FullName = u1.FullName,
                              Date = d.Date,
                              Total_Job = d.Total_Job,
                              User_Autho1 = sub2.UserName,
                              FullName_Autho1 = sub2.FullName,
                              User_Autho2 = sub3.UserName,
                              FullName_Autho2 = sub3.FullName,
                              Status_Autho1 = d.Status_Autho1,
                              Status_Autho2 = d.Status_Autho2,
                              Comment1 = d.Comment1,
                              Comment2 = d.Comment2
                          });

            IEnumerable<ReportModel> tbl;
            if (string.IsNullOrEmpty(user.Ma_TO))
            {
                tbl =
                                         from u in db.AppUsers
                                         join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                                         join r in report on u.UserName equals r.UserName
                                         where c.Display < user.Display && r.Status_Autho2 == true && u.Ma_BP == user.Ma_BP
                                         select r;
            }
            else
                tbl =
                             from u in db.AppUsers
                             join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                             join r in report on u.UserName equals r.UserName
                             where c.Display < user.Display && r.Status_Autho2 == true && u.Ma_TO == user.Ma_TO
                             select r;

            return tbl;
        }

        public IEnumerable<ReportModel> GetAllTest(string Ma_TO, string Ma_BP)
        {
            var user = (from u in db.AppUsers
                        join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                        where u.UserName == HttpContext.Current.User.Identity.Name
                        select new
                        {
                            UserName = u.UserName,
                            Display = c.Display,
                            Ma_CV = c.Ma_CV,
                            Ma_BP = u.Ma_BP,
                            Ma_TO = u.Ma_TO,
                        }).FirstOrDefault();
            IEnumerable<ReportModel> tbl;
            tbl = (from d in db.tbl_Daily
                   join u1 in db.AppUsers on d.UserName equals u1.UserName
                   join j in db.tbl_CV on u1.Ma_CV equals j.Ma_CV
                   join u2 in db.AppUsers on d.User_Autho1 equals u2.UserName into gu2
                   from sub2 in gu2.DefaultIfEmpty()
                   join u3 in db.AppUsers on d.User_Autho2 equals u3.UserName into gu3
                   from sub3 in gu3.DefaultIfEmpty()
                   join u4 in db.AppUsers on d.User_Autho3 equals u4.UserName into gu4
                   from sub4 in gu4.DefaultIfEmpty()
                   where (!string.IsNullOrEmpty(user.Ma_TO) ? d.Status_Autho1 == true : d.Status_Autho2 == true) && j.Display < user.Display
                   select new ReportModel
                   {
                       Id = d.Id,
                       UserName = d.UserName,
                       FullName = u1.FullName,
                       Date = d.Date,
                       Total_Job = d.Total_Job,
                       User_Autho1 = sub2.UserName,
                       FullName_Autho1 = sub2.FullName,
                       User_Autho2 = sub3.UserName,
                       FullName_Autho2 = sub3.FullName,
                       User_Autho3 = sub4.UserName,
                       FullName_Autho3 = sub4.FullName,
                       Status_Autho1 = d.Status_Autho1,
                       Status_Autho2 = d.Status_Autho2,
                       Status_Autho3 = d.Status_Autho3,
                       Comment1 = d.Comment1,
                       Comment2 = d.Comment2,

                       Ma_BP = u1.Ma_BP,
                       Ma_To = u1.Ma_TO
                   });
            if (user.Ma_BP != "BGD")
            {
                Ma_BP = user.Ma_BP;
            }
            if (!string.IsNullOrEmpty(user.Ma_TO))
            {
                Ma_TO = user.Ma_TO;
            }
            tbl = tbl.Where(m => m.Ma_BP == Ma_BP && (Ma_TO==""?1==1:m.Ma_To==Ma_TO));
            return tbl;
        }

        public bool CheckPermission(int ma)
        {
            var user = (from u in db.AppUsers
                        join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                        where u.UserName == HttpContext.Current.User.Identity.Name
                        select new
                        {
                            UserName = u.UserName,
                            Display = c.Display,
                            Ma_CV = c.Ma_CV,
                            Ma_BP = u.Ma_BP,
                            Ma_TO = u.Ma_TO,
                        }).FirstOrDefault();
            if (user.Display > 2)
            {
                if (db.tbl_DailyDetail.Any(m => m.DailyId == ma && m.Level_2 != null))
                {
                    return true;
                }
                return false;
            }
            var tbl = db.tbl_Daily.Find(ma);
            return tbl.Status_Autho2;
        }
    }
}