using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MVC6.Notification_Hub.Startup))]

namespace MVC6.Notification_Hub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
         typeof(IUserIdProvider), () => new HubUserProvider());
            app.MapSignalR();
        }
    }
}
