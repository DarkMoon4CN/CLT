using System.Web.Mvc;
using System.Web;
using System.Web.Security;
using ChuanglitouP2P.Common;

namespace WeiXin.Controllers
{
    /// <summary>
    ///     创利投 属性过滤器 验证请求权限
    /// </summary>
    public class ActAuth : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var from = filterContext.RequestContext.HttpContext.Request.QueryString["RedirectUrl"];

                var url = filterContext.RequestContext.HttpContext.Request.Url == null
                            ? string.Empty
                            : filterContext.RequestContext.HttpContext.Request.Url.OriginalString;
                if (!string.IsNullOrEmpty(from))
                {
                    url = HttpUtility.UrlDecode(from);
                }
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (filterContext.RequestContext.HttpContext.Request.UrlReferrer != null)
                        url = filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString();
                    else
                    {
                        url = Settings.Instance.SiteDomain;
                    }
                }
                var content = new ContentResult
                {
                    Content =
                        string.Format(
                            "<script type='text/javascript'>window.location.href='{0}?RedirectUrl={1}';</script>",
                            FormsAuthentication.LoginUrl, HttpUtility.UrlEncode(url))
                };
                filterContext.Result = content;
            }
        }
    }
}