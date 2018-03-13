using DataModel.Model.DailyReport;
using Data.Data;

using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace DataService.Dao.DailyReports
{
    public class DailyDetailDao
    {
        private readonly EFDbContext db;
        public DailyDetailDao()
        {
            db = new EFDbContext();
        }
        public IQueryable<DailyModel> load(int ma)
        {
            var tbl = (from dd in db.tbl_DailyDetail
                       join d in db.tbl_Daily on dd.DailyId equals d.Id
                       join j in db.tbl_Job on dd.JobId equals j.Id
                       where dd.DailyId == ma
                       select new DailyModel
                       {
                           Id = dd.Id,
                           FormTime = dd.FormTime,
                           ToTime = dd.ToTime,
                           Comment2 = dd.Comment2,
                           Content_Job = dd.Content_Job,
                           Method = dd.Method,
                           Result = dd.Result,
                           DailyId = d.Id,
                           JobId = dd.JobId,
                           Total_Job = d.Total_Job,
                           Comment1 = dd.Comment1,
                           Comment3 = dd.Comment3,
                           Level_1 =dd.Level_1,
                           Level_2=dd.Level_2,
                           Level_3 = dd.Level_3,
                           Ten_Job =j.Ten_Job
                       });
            return tbl;
        }
        public DailyModel detail(long mad, int ma)
        {
            var model = new DailyDetailDao().load(ma).Where(m => m.Id == mad).FirstOrDefault();
            return model;
        }
        public bool Save(DailyModel model)
        {
            try
            {
                
                tbl_DailyDetail tbl = new tbl_DailyDetail();
                tbl.FormTime = model.FormTime;
                tbl.ToTime = model.ToTime;
                tbl.Content_Job = model.Content_Job;
                tbl.Method = model.Method;
                tbl.Result = model.Result;
                tbl.JobId = model.JobId;
                tbl.DailyId = model.DailyId;

                db.tbl_DailyDetail.Add(tbl);
              
                db.SaveChanges();
                string Total_Job = Calculator(model.DailyId);
                if (!string.IsNullOrWhiteSpace(Total_Job))
                {
                    var cal = db.tbl_Daily.Find(model.DailyId);
                    cal.Total_Job = Total_Job;
                };
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(DailyModel model)
        {
            try
            {
                
                tbl_DailyDetail tbl = new tbl_DailyDetail();
                tbl.Id = model.Id;
                tbl.FormTime = model.FormTime;
                tbl.ToTime = model.ToTime;
                tbl.Content_Job = model.Content_Job;
                tbl.Method = model.Method;
                tbl.Result = model.Result;
                tbl.JobId = model.JobId;
                tbl.DailyId = model.DailyId;

                db.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
               
                db.SaveChanges();
                string Total_Job = Calculator(model.DailyId);
                if (!string.IsNullOrWhiteSpace(Total_Job))
                {
                    var cal = db.tbl_Daily.Find(model.DailyId);
                    cal.Total_Job = Total_Job;
                };
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(long ma)
        {
            try
            {
                
                var tbl = db.tbl_DailyDetail.Find(ma);
                db.tbl_DailyDetail.Remove(tbl);
                db.SaveChanges();
                string Total_Job = Calculator(tbl.DailyId);
                var cal = db.tbl_Daily.Find(tbl.DailyId);
                cal.Total_Job = Total_Job;
                
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string Calculator(int ma)
        {
            var cal = (from d in db.tbl_DailyDetail
                       join j in db.tbl_Job on d.JobId equals j.Id
                       where d.DailyId == ma
                       group d by new { d.JobId, j.Ten_Job } into g
                       select new
                       {
                           JobId = g.Key.JobId,
                           Ten_Job=g.Key.Ten_Job,
                           Sum=(g.Sum(m=> SqlFunctions.DateDiff("minute",m.FormTime,m.ToTime)))*100/480
                       }).ToList();
            string Total_Job = "";
            if (cal.Count > 0)
            {
                
                foreach (var item in cal)
                {
                    Total_Job = Total_Job + "- " + item.Ten_Job + "(" + item.Sum + "%)</br>";
                }

            }
            return Total_Job;
        }
        public List<string> SendRequest(int ma)
        {
           
                var user = (from u in db.AppUsers
                            join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                            
                            select new
                            {
                                FullName=u.FullName,
                                UserName = u.UserName,
                                Display = c.Display,
                                Ma_CV = c.Ma_CV,
                                Ma_BP = u.Ma_BP,
                                Ma_TO = u.Ma_TO,
                            });
                var tbl = db.tbl_Daily.Find(ma);
                var Sender = user.Where(m => m.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                List<string> Received;
                if (Sender.Display>2)
                {
                    Received = user.Where(m => m.Ma_BP == Sender.Ma_BP && m.Display > 3).Select(m=>m.UserName).ToList();
                    tbl.Status_Autho2 = true;
                    tbl.Status_Autho3 = true;
                  
                }
                else
                {
                    Received = user.Where(m => m.Display > 2 && m.Ma_TO == Sender.Ma_TO).Select(m=>m.UserName).ToList();
                    tbl.Status_Autho1 = true;
                }
                List<tbl_Notifications> messenges = new List<tbl_Notifications>();
                for(var i = 0; i < Received.Count(); i++)
                {
                    messenges.Add(new tbl_Notifications
                    {
                        SendId = Sender.UserName,
                        ReceiveId = Received[i],
                        Status = true,
                        Date = DateTime.Now,
                        Url = "/Daily/Dailys/Detail?ma=" + ma.ToString(),
                        Notification = "Nhật ký công việc ngày " + tbl.Date.ToString("dd/MM/yyyy") + " " + user.Where(m => m.UserName == tbl.UserName).FirstOrDefault().FullName,
                    });
                   
                }
               
                db.tbl_Notifications.AddRange(messenges);
                db.SaveChanges();
               
                return Received;
            
           
            
        }
        public bool Comment(long ma,string comment,int level,int check)
        {
            try
            {
               
                var tbl = db.tbl_DailyDetail.Find(ma);
                var model = db.tbl_Daily.Find(tbl.DailyId);
                if (check==3)
                {
                    tbl.Level_1 = level;
                    tbl.Comment1 = comment;
                    model.User_Autho1 = string.IsNullOrEmpty(model.User_Autho1) ? HttpContext.Current.User.Identity.Name : model.User_Autho1;
                  
                }
                else if (check==4)
                {
                    tbl.Level_2 = level;
                    tbl.Comment2 = comment;
                    model.User_Autho2 = string.IsNullOrEmpty(model.User_Autho2) ? HttpContext.Current.User.Identity.Name : model.User_Autho2;
                  
                }
                else
                {
                    tbl.Level_3 = level;
                    tbl.Comment3 = comment;
                    model.User_Autho3 = string.IsNullOrEmpty(model.User_Autho3) ? HttpContext.Current.User.Identity.Name : model.User_Autho3;
                    
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool CommentAll(int ma,string comment,int level, int check)
        {
            try
            {
                var tbl = db.tbl_DailyDetail.Where(m => m.DailyId == ma);
                var model = db.tbl_Daily.Find(ma);
                if (check==3)
                {
                    model.User_Autho1 = string.IsNullOrEmpty(model.User_Autho1)?HttpContext.Current.User.Identity.Name: model.User_Autho1;
                    foreach (var item in tbl)
                    {
                        if(item.Comment1 == null && item.Level_1 == null)
                        {
                            item.Comment1 = comment;
                            item.Level_1 = level;
                        }
                        
                    }
                }
                else if (check==4)
                {

                    model.User_Autho2 = string.IsNullOrEmpty(model.User_Autho2) ? HttpContext.Current.User.Identity.Name : model.User_Autho2;
                   
                    foreach (var item in tbl)
                    {
                        if(item.Comment2==null&& item.Level_2 == null)
                        {
                            item.Comment2 = comment;
                            item.Level_2 = level;
                        }
                       
                    }
                }
                else
                {
                    model.User_Autho3 = string.IsNullOrEmpty(model.User_Autho3) ? HttpContext.Current.User.Identity.Name : model.User_Autho3;
                  
                    foreach (var item in tbl)
                    {
                        if (item.Comment3 == null && item.Level_3 == null)
                        {
                            item.Comment3 = comment;
                            item.Level_3 = level;
                        }

                    }
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}