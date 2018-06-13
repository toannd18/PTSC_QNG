
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
        public List<ExportCustomer> ExportSupplier(int dx)
        {
            var model = (from t in db.tbl_DeXuat
                         join ncc in db.tbl_DG_NCC on t.Id equals ncc.DeXuatId
                         join n in db.tbl_NCC on ncc.Ma_NCC equals n.Ma_NCC
                         where t.Id == dx
                         select new ExportCustomer
                         {
                             Ma = t.Ma,
                             Tieu_De = t.Tieu_De,
                             Kieu = t.Kieu,
                             DG = t.Ten_Dg,
                             DG1 = t.Ten_Dg1,
                             DG2 = t.Ten_Dg2,
                             DG3 = t.Ten_Dg3,
                             Ngay_Tao = t.Ngay_Tao,
                             Ngay_Gui = t.Ngay_Gui,
                             Ngay_Exp = t.Ngay_Exp,
                             Ngay_Eval = t.Ngay_Eval,
                             Ngay_PD = t.Ngay_PD,
                             Ngay_HD = t.Ngay_HD,
                             Ngay_PHD = t.Ngay_PHD,
                             Ten = n.Ten_NCC,
                             Ma_NCC = n.Ma_NCC,
                             Tel = n.Tel,
                             Fax = n.Fax,
                             Attn = n.Attn,
                             Email = n.Email,
                             Dia_Chi = n.Dia_Chi,
                             DG_Chung=ncc.DG,
                             DG_TM=ncc.DG_TM,
                             DG_KT=ncc.DG_KT
                         }).OrderBy(m=>m.Ma_NCC).ToList();
            return model;
        }
       
    }
    public class LevelUser
    {
        public string UserName { get; set; }
        public string Ma_BP { get; set; }
        public string Ma_TO { get; set; }
        public int? Display { get; set; }
    }
    public class ExportCustomer
    {
        public string Ma { get; set; }
        public string Tieu_De { get; set; }
        public bool Kieu { get; set; }
        public string DG { get; set; }
        public string DG1 { get; set; }
        public string DG2 { get; set; }
        public string DG3 { get; set; }
        public DateTime? Ngay_Tao { get; set; }
        public DateTime? Ngay_Gui { get; set; }
        public DateTime? Ngay_Exp { get; set; }
        public DateTime? Ngay_Eval { get; set; }
        public DateTime? Ngay_PD { get; set; }
        public DateTime? Ngay_HD { get; set; }
        public DateTime? Ngay_PHD { get; set; }
        public string Ten { get; set; }
        public string Ma_NCC { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Attn { get; set; }
        public string Email { get; set; }
        public int? DG_Chung { get; set; }
        public int? DG_TM { get; set; }
        public bool? DG_KT { get; set; }
        public string Dia_Chi { get; set; }

    }
}