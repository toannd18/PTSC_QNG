
using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel.Model.Reports;

namespace DataService.Dao.ExportReports
{
    public class ExportReportDao
    {
        private readonly EFDbContext db;
        public ExportReportDao()
        {
            db = new EFDbContext();
        }
        public List<LevelReportModel> ReprotLevel(DateTime FromTime, DateTime ToTime,string Ma_BP,string Ma_TO)
        {
            List<LevelReportModel> tbl = new List<LevelReportModel>();
            tbl = (from dd in (from m in db.tbl_DailyDetail
                    select new
                    {
                        DailyId = m.DailyId,
                        Level = m.Level_3.HasValue ? m.Level_3 : m.Level_2
                    })
                  join d in db.tbl_Daily on dd.DailyId equals d.Id
                  join u in db.AppUsers on d.UserName equals u.UserName
                  join t in db.tbl_TO on u.Ma_TO equals t.Ma_TO
                  where d.Date>=FromTime && d.Date<=ToTime && u.Ma_BP.Contains(Ma_BP) && u.Ma_TO.Contains(Ma_TO)
                  group new { dd, d, u,t } by new { d.UserName } into g
                  select new LevelReportModel
                  {
                      UserName=g.FirstOrDefault().u.UserName,
                      FullName = g.FirstOrDefault().u.FullName,
                      Ten_To= g.FirstOrDefault().t.Ten_TO,
                      Level_1 = g.Where(m => m.dd.Level == 1).Count(),
                      Level_2 = g.Where(m => m.dd.Level == 2).Count(),
                      Level_3 = g.Where(m => m.dd.Level == 3).Count(),
                      Level_4 = g.Where(m => m.dd.Level == 4).Count(),
                      Level_5 = g.Where(m => m.dd.Level == 5).Count(),

                  }).ToList();
            return tbl;
        }
        public LevelUser GetLevelUser(string UserName)
        {
            LevelUser user = (from u in db.AppUsers
                              join c in db.tbl_CV on u.Ma_CV equals c.Ma_CV
                              where u.UserName == UserName
                              select new LevelUser
                              {
                                  UserName = u.UserName,
                                  Ma_BP = u.Ma_BP,
                                  Ma_TO = u.Ma_TO,
                                  Display = c.Display
                              }).FirstOrDefault();
            return user;
        }
    }
    public class LevelUser
    {
        public string UserName { get; set; }
        public string Ma_BP { get; set; }
        public string Ma_TO { get; set; }
        public int? Display { get; set; }
    }
}