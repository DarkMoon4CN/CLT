using System;
using System.Globalization;
using System.Web;
using ChuanglitouP2P.Common.Log;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class LogInfo
    {
        public static void Info(object message)
        {
            LoggerProvider.Instance.Info(message);
        }

        public static String GenerateAdditionalInfo()
        {
            try
            {
                if (HttpContext.Current == null) return "";

                HttpRequest req = HttpContext.Current.Request;
                string infos = "";

                if (req.UserAgent != null)
                {
                    if (req.UserAgent.Contains("Sogou web spider"))
                        return "";
                    if (req.UserAgent.Contains("Googlebot"))
                        return "";
                    if (req.UserAgent.Contains("Baiduspider"))
                        return "";
                    if (req.UserAgent.Contains("YodaoBot"))
                        return "";
                    if (req.UserAgent.Contains("Yahoo! Slurp"))
                        return "";
                    if (req.UserAgent.Contains("Sosospider"))
                        return "";
                }

                if (req.Path != null)
                    if (req.Path.Contains("ScriptResource") ||
                        req.Path.Contains("WebResource")
                        )
                        return string.Empty;


                if (req != null)
                {
                    infos = string.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            "请求地址: {0}<br/>" +
                            "来源网址: {1}<br/>" +
                            "主机IP: {2}<br/>" +
                            "用户浏览器: {3}<br/>" +
                            "请求方式: {4}<br/>" +
                            "登录用户: {5}",
                            req.Url.AbsoluteUri,
                           req.UrlReferrer == null ? "(null)" : req.UrlReferrer.AbsoluteUri,
                           DNTRequest.GetIP() ?? "(null)",
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

        public static void WriteLog(string message)
        {
            if (String.IsNullOrEmpty(message))
                LoggerProvider.Instance.Error(GenerateAdditionalInfo());
            else
                LoggerProvider.Instance.Error(GenerateAdditionalInfo() + @"<br \>" +Settings.Instance.RemoveScript(message));
        }

        public static void WriteLog(string message, Exception exception)
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