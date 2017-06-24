using System.Web.Mvc;

namespace ChuangLiTouOpenApi.Areas.UserAuthentication
{
    public class UserAuthenticationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserAuthentication";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UserAuthentication_default",
                "UserAuthentication/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}