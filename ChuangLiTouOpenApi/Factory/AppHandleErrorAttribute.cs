using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Factory
{
    /// <summary>
    /// 错误日志（Controller发生异常时会执行这里）
    /// </summary>
    public class AppHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //使用log4net或其他记录错误消息
            Exception Error = filterContext.Exception;
            string Message = Error.ToString();//错误信息
            string Url = HttpContext.Current.Request.RawUrl;//错误发生地址
            LoggerHelper.Error(Url + "" + Message);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("/Error/Index");//跳转至错误提示页面
        }
    }
}