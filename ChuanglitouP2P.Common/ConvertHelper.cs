using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.Common
{
    public static class ConvertHelper
    {
        public static T ParseValue<T>(object text, T defaultValue)
        {
            if (text == null || text.ToString() == "")
            {
                return defaultValue;
            }

            try
            {
                if (typeof(T) == typeof(String))
                    return (T)(object)(text.ToString());
                if (typeof(T) == typeof(DateTime))
                    return (T)(object)DateTime.Parse(text.ToString());
                if (typeof(T) == typeof(long))
                    return (T)(object)long.Parse(text.ToString());
                if (typeof(T) == typeof(int))
                    return (T)(object)int.Parse(text.ToString());
                if (typeof(T) == typeof(double))
                    return (T)(object)double.Parse(text.ToString());
                if (typeof(T) == typeof(decimal))
                    return (T)(object)decimal.Parse(text.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                if (typeof(T) == typeof(decimal?))
                    return (T)(object)decimal.Parse(text.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                if (typeof(T) == typeof(int?))
                    return (T)(object)int.Parse(text.ToString());
                if (typeof(T) == typeof(Guid))
                    return (T)(object)(new Guid(text.ToString()));
                if (typeof(T) == typeof(bool))
                    return (T)(object)(bool.Parse(text.ToString()));
                if (typeof(T) == typeof(bool?))
                {
                    bool b;
                    if (!bool.TryParse(text.ToString(), out b)) return defaultValue;
                    return (T)(object)b;
                }
                if (typeof(T) == typeof(Boolean))
                    return (T)(object)(Boolean.Parse(text.ToString()));

                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), text.ToString());
                }
            }

            catch (Exception e)
            {

                LoggerHelper.Error("Error parsing value '" + text + "' to a " + typeof(T).Name, e);

                return defaultValue;
            }


            return defaultValue;
        }

        public static T GetEntityFromQueryString<T>(string queryIdKey, bool throwOnError)
        {
            int id;
            object result = null;

            if (String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[queryIdKey]))
            {
                result = null;
            }
            else if (!Int32.TryParse(HttpContext.Current.Request.QueryString[queryIdKey], out id))
            {
                result = null;
            }
            else
            {
                LogInfo.WriteLog("GetEntityFromQueryString:对象" + typeof(T).Name + "没有被执行");
            }


            if (throwOnError)
            {
                LogInfo.WriteLog(typeof(T).Name + " 应该在URL中设置成" + queryIdKey + "=(value)");
            }

            return (T)result;
        }

        public static T GetQueryValue<T>(String key, T defaultValue)
        {
            T value;
            if (HttpContext.Current.Request[key] == null)
            {

                LogInfo.WriteLog("缺少参数: " + key);

                return defaultValue;
            }

            try
            {
                value = ParseValue(HttpContext.Current.Request[key], defaultValue);
            }
            catch
            {

                LogInfo.WriteLog("参数格式错误: " + key + " 应该是 " + typeof(T).Name);

                return defaultValue;
            }

            return value;
        }


        public static IQueryable<T> GetEntitiesFromQueryString<T>(string queryIDKey, bool throwOnError)
        {
            object result = null;

            if (String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[queryIDKey]))
            {
                result = null;
            }
            else
            {
                var ds = new List<int>();

                foreach (String strId in HttpContext.Current.Request.QueryString[queryIDKey].Split(new[] { ',' }))
                {
                    int id;
                    if (Int32.TryParse(strId, out id))
                        ds.Add(id);
                }

                LogInfo.WriteLog("GetEntityFromQueryString: 对象" + typeof(T).Name + "没有被执行");
            }


            if (result == null && throwOnError)
            {
                LogInfo.WriteLog(typeof(T).Name + " that should be set in the URL as " + queryIDKey +
                                   "=(value)");
            }

            return (IQueryable<T>)result;
        }

        /// <summary>
        /// xiezh add
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="defaultKey"></param>
        /// <param name="showFieldText"></param>
        /// <param name="isFilter">是不为检索下拉框,是的话去除html</param>
        /// <returns></returns>

        public static string RemoveHtml(string htmlstring)
        {
            //删除脚本   
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");
            htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();

            return htmlstring;
        }


        public static void FilePathCheck(string path, string fileExtensionWithDot)
        {
            if (path == null || path.Contains("..") || path.Contains("/") || !path.EndsWith(fileExtensionWithDot))
            {
                LogInfo.WriteLog("FilePathCheck: " + path);
            }
        }


        public static T GetEntityFromId<T>(string variable, bool throwOnError)
        {
            object result = null;
            if (throwOnError)
            {
                LogInfo.WriteLog(typeof(T).Name + " that should be set in [" + variable + "]");
            }

            return (T)result;
        }
    }
}