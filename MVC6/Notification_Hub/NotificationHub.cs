using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Data.Data;

namespace MVC6.Notification_Hub
{
    public class NotificationHub : Hub
    {
        public static void Send(List<string> username)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            for(var i = 0; i < username.Count; i++)
            {
                context.Clients.User(username[i]).displayStatus();
            }
           
        }
    }
    public static class NotificationService
    {
        
        public static List<tbl_Notifications> GetNotification(string user)
        {
            EFDbContext db = new EFDbContext();
           
            return db.tbl_Notifications.Where(m => m.ReceiveId == user).OrderByDescending(m=>m.Date).ToList();
        }
        public static void RemoveNotification(string ma,string user)
        {
            EFDbContext db = new EFDbContext();
            var model = db.tbl_Notifications.Where(m => m.ReceiveId == user && m.Url == ma);
            foreach(var item in model)
            {
               item.Status=false;
            }
            db.SaveChanges();
        }
        //static readonly string connString = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;

        //internal static SqlCommand command = null;
        //internal static SqlDependency dependency = null;
        //public static List<Notification> GetNotification()
        //{
        //    try
        //    {
        //        var messenges = new List<Notification>();
        //        string Sql_command = @"SELECT [Id],[SendId],[ReceiveId],[Url],[Notification] FROM [EFDbContext_Demo].[dbo].[Notification]";
        //        using (SqlConnection con = new SqlConnection(connString))
        //        {
        //            con.Open();

        //            using (command = new SqlCommand(Sql_command, con))
        //            {
        //                command.Notification = null;
        //                dependency = new SqlDependency(command);
        //                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);


        //                if (con.State == System.Data.ConnectionState.Closed)
        //                    con.Open();
        //                var read = command.ExecuteReader();
        //                while (read.Read())
        //                {
        //                    messenges.Add(new Notification
        //                    {
        //                        Id = (long)read["Id"],
        //                        SendId = (string)read["SendId"],
        //                        ReceiveId = (string)read["ReceiveId"],
        //                        Url = (string)read["Url"],

        //                        Notification1 = (string)read["Notification"]
        //                    });
        //                }
        //            }
        //        }
        //        return messenges;
        //    }

        //    catch
        //    {
        //        return null;
        //    }
        //}
        //private static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{

        //    if (e.Type == SqlNotificationType.Change)
        //    {
        //        dependency.OnChange -= dependency_OnChange;
        //        NotificationHub.Send("test");
        //    }
        //}
    }
    public class HubUserProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.Name;
        }
    }

}