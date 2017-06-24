using System.Web.Mvc;

namespace ChuangLiTouOpenApi.Areas.Callback
{
    public class CallbackAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Callback";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Callback_default",
                "Callback/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}