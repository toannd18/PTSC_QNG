using Data.Data;
using DataModel.Model.Applications;
using EASendMail;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DataService.Dao.Commom
{
    public class Commom
    {
        public bool SendEmail(string toEmail, string subject, string emailbody, string Cc)
        {
            try
            {
                string senderEmail = "ITServiceDesk.qn@ptsc.com.vn";
                string senderPass = "*khong#di";
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set sender email address, please change it to yours
                oMail.From = senderEmail;

                // Set recipient email address, please change it to yours
                oMail.To = toEmail;

                // Set email subject
                oMail.Subject = subject;

                // Set email Cc
                if (!string.IsNullOrEmpty(Cc))
                {
                    string[] sMailCc;
                    sMailCc = Cc.Split(';');
                    for (int i = 0; i < sMailCc.Length; i++)
                    {
                        oMail.Cc.Add(new MailAddress(sMailCc[i]));
                    }
                }

                // Set email body
                oMail.HtmlBody = emailbody;

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("omail.ptsc.com.vn");

                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                oServer.User = senderEmail;
                oServer.Password = senderPass;

                // Set 465 SMTP port
                oServer.Port = 465;

                // Enable SSL connection
                oServer.ConnectType = SmtpConnectType.ConnectDirectSSL;
                oSmtp.SendMail(oServer, oMail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ResponeModel respone(int RequestId)
        {
            EFDbContext db = new EFDbContext();
            var tbl = (from r in db.tbl_List_Request
                       where r.Id.Equals(RequestId)
                       join u in db.AppUsers on r.User_Nhap equals u.UserName
                       join t in db.AppUsers on r.User_Autho equals t.UserName
                       join d in db.tbl_BP on r.Ma_BP equals d.Ma_BP
                       select new ResponeModel
                       {
                           Id = r.Id,
                           LateId = r.LateId,
                           Ten_BP = d.Ten_BP,
                           Dia_Diem = r.Dia_Diem,
                           Date = r.Date,
                           FullName = u.FullName,
                           FullName_1 = t.FullName,
                           User_Nhap = r.User_Nhap,
                           Hang_Muc = r.Hang_Muc
                       }).FirstOrDefault();
            return tbl;
        }

        // Kiểm tra  xem đã phe duyệt chưa trong daily diary
        public bool CheckPermission(int RequestId)
        {
            EFDbContext db = new EFDbContext();
            var tbl = db.tbl_List_Request.Find(RequestId);
            return tbl.Status_Autho == "A" ? true : false;
        }

        public string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] result = md5.Hash;

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }
            return str.ToString();
        }

        public int ChangePassword(string OldPassword, string NewPassword)
        {
            EFDbContext db = new EFDbContext();
            var tbl = db.AppUsers.Where(m => m.UserName == HttpContext.Current.User.Identity.Name && m.PasswordHash == OldPassword).FirstOrDefault();
            if (tbl == null)
            {
                return 0;
            }
            tbl.PasswordHash = NewPassword;
            db.SaveChanges();
            return 1;
        }

        // Tạo mã tự động cho phiếu yêu cầu
        public int CreatRequestId(string Ma_BP, DateTime date)
        {
            EFDbContext db = new EFDbContext();
            int ma;
            var data = db.tbl_List_Request.Where(m => m.Ma_BP == Ma_BP && m.Date.Value.Year == date.Year).ToList();
            if (data.Count == 0)
            {
                ma = 1;
            }
            else
            {
                ma = data.Max(m => m.FirstId) + 1;
            }

            return ma;
        }
    }
}