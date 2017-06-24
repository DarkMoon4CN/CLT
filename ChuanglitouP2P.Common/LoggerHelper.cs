#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>HttpHelper ©2013 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2013/3/21 16:14:42</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion
using System;
using System.Web;

using ChuanglitouP2P.Common.Log;
namespace ChuanglitouP2P.Common
{
    public class LoggerHelper
    {
        public static void Info(object message)
        {
            LoggerProvider.Instance.Info(GenerateAdditionalInfo() + @"<br \>" + Settings.Instance.RemoveScript(message.ToString()));
        }

        public static String GenerateAdditionalInfo()
        {
            try
            {

                if (HttpContext.Current == null) return "";

                HttpRequest req = HttpContext.Current.Request;
                string infos = "";


                if (req.Path != null)
                    if (req.Path.Contains("ScriptResource") ||
                        req.Path.Contains("WebResource")
                        )
                        return string.Empty;

                if (req != null)
                {
                    infos = string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        "请求地址: {0} <BR/>" +
                        "来源网址: {1}<BR/>" +
                        "主机IP: {2}<BR/>" +
                        "用户浏览器: {3}<BR/>" +
                        "请求方式: {4}<BR/>" +
                        "登录用户: {5}<BR/>",
                        req.Url.AbsoluteUri,
                        req.UrlReferrer == null ? "(null)" : req.UrlReferrer.AbsoluteUri,
                       Settings.Instance.ClientIp2 ?? "(null)",
                        req.UserAgent ?? "(null)",
                        req.HttpMethod,
                        string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? "(null)" : HttpContext.Current.User.Identity.Name
                        );
                }

                return infos;
            }
            catch (Exception)
            {
                return "(exception when trying to get more info)";
            }
        }

        public static void Error(string message)
        {
            if (String.IsNullOrEmpty(message))
                LoggerProvider.Instance.Error(GenerateAdditionalInfo());
            else
            
            LoggerProvider.Instance.Error(GenerateAdditionalInfo() + @"<br \>" + Settings.Instance.RemoveScript(message));
        }

        public static void Error(string message, Exception exception)
        {
            if (String.IsNullOrEmpty(message))
                LoggerProvider.Instance.Error(GenerateAdditionalInfo(), exception);
            else
                LoggerProvider.Instance.Error(GenerateAdditionalInfo() + @"<br />" + Settings.Instance.RemoveScript(message), exception);
        }
        public static void Warning(string message)
        {
            if (String.IsNullOrEmpty(message))
                LoggerProvider.Instance.Warning(GenerateAdditionalInfo());
            else
                LoggerProvider.Instance.Warning(GenerateAdditionalInfo() + @"<br \>" + Settings.Instance.RemoveScript(message));
        }

        public static void Warning(string message, Exception exception)
        {
            if (String.IsNullOrEmpty(message))
                LoggerProvider.Instance.Warning(GenerateAdditionalInfo(), exception);
            else
                LoggerProvider.Instance.Warning(GenerateAdditionalInfo() + @"<br />" + Settings.Instance.RemoveScript(message), exception);
        }
    }
}
