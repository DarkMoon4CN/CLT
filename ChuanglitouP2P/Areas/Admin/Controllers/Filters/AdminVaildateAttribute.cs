using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChuanglitouP2P.Areas.Admin.Controllers.Filters
{
    /// <summary>
    /// 后台登录和权限验证
    /// isLimit判断是否需要权利处理 需要给 true  不做权限给false
    /// isLogin判断是否权限登录 给 true 后台都需要的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminVaildateAttribute : ActionFilterAttribute
    {
        public bool IsLimit { get; set; }

        public bool IsLogin { get; set; }

        public AdminVaildateAttribute()
        {
            this.IsLimit = true;
            this.IsLogin = true;
        }

        /// <summary>
        /// isLimit判断是否需要权利处理 需要给 true  不做权限给false
        /// isLogin判断是否权限登录 给 true 后台都需要的
        /// </summary>
        /// <param name="isLimit">判断是否需要权利处理：需要给 true  不做权限给false</param>
        /// <param name="isLogin">判断是否权限登录 给 true 后台都需要的 </param>
        public AdminVaildateAttribute(bool isLimit, bool isLogin = true)
        {
            this.IsLimit = isLimit;
            this.IsLogin = isLogin;
        }

        /// <summary>
        /// 在所有Action执行前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            var userid = Utils.GetAdmUserID();

            if (IsLogin)
            {   //登录验证
                if (userid < 1)
                {   //没有登录
                    Redirect(filterContext, "/admin/Login/LoginOut");
                    return;
                }
            }

            if (IsLimit)
            {   //权限验证
                if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
                {   //无权限

                    var content = new ContentResult
                    {
                        Content = "<script type='text/javascript'>alert(\"您没有操作权限\");history.back();</script>"
                    };
                    filterContext.Result = content;
                    return;

                }
            }
        }

        /// <summary>
        /// 处理页面跳转
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="url"></param>
        private void Redirect(ActionExecutingContext filterContext, string url)
        {
            if (filterContext.IsChildAction)
            {
                filterContext.HttpContext.Response.Redirect(url, true);
            }
            else
            {
                filterContext.Result = new RedirectResult(url);
            }
        }
    }
}