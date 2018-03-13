using System.Web.Mvc;

namespace MVC6.Areas.Daily
{
    public class DailyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Daily";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Daily_default",
                "Daily/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
              
            );
        }
    }
}