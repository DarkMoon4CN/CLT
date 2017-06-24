#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>HttpHelper ©2013 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2013/3/21 17:14:42</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ChuanglitouP2P.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// Post方式调用API.请求参数为JSON.
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="objParam">请求参数集合.</param>
        /// <param name="apiUrl">请求地址.</param>
        /// <param name="contentType">内容格式 默认 application/json </param>
        /// <returns></returns>
        /// 创建者：解志辉
        /// ------------------------------------
        public static T Post<T>(string apiUrl, object objParam, string contentType = "application/json") where T : new()
        {
            string param = JsonHelper.ObjectToJson(objParam);
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            byte[] bs = Encoding.UTF8.GetBytes(param);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = bs.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bs, 0, bs.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                string res = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return JsonHelper.JsonToObject<T>(res);
            }
            catch (WebException ex)
            {
                return new T();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cASHPAY_URL"></param>
        /// <param name="reqJson"></param>
        /// <returns></returns>
        public static string Post(string cASHPAY_URL, string reqJson)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(cASHPAY_URL));
            http.Accept = "application/json";
            http.ContentType = "application/json;charset=utf-8";
            http.Method = "POST";


            //ASCIIEncoding encoding = new ASCIIEncoding();


            // Byte[] bytes = encoding.GetBytes(reqJson);

            byte[] bytes = Encoding.UTF8.GetBytes(reqJson);
            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            return content;
        }
        public static string Post(string apiUrl, NameValueCollection objParam, string contentType = "application/json")
        {

            using (var client = new WebClient())
            {
                byte[] result = client.UploadValues(apiUrl, "POST", objParam);
                var retStr = Encoding.UTF8.GetString(result);
                return (retStr);

            }





            //string param = JsonHelper.ObjectToJson(objParam);
            //var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            //byte[] bs = Encoding.UTF8.GetBytes(param);
            //request.Method = "POST";
            //request.ContentType = contentType;
            //request.ContentLength = bs.Length;
            //using (Stream requestStream = request.GetRequestStream())
            //{
            //    requestStream.Write(bs, 0, bs.Length);
            //}
            //try
            //{
            //    var response = (HttpWebResponse)request.GetResponse();
            //    var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            //    string res = sr.ReadToEnd();
            //    sr.Close();
            //    response.Close();
            //    return (res);
            //}
            //catch (WebException ex)
            //{
            //    return "";
            //}
        }

        /// <summary>
        /// Gets方式调用API.
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// 创建者：解志辉
        /// ------------------------------------
        public static T Get<T>(string url) where T : new()
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1000 * 60 * 15;
            request.Method = "GET";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string returnValue = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return JsonHelper.JsonToObject<T>(returnValue);
            }
            catch (WebException ex)
            {

                return new T();
            }
            catch (Exception ex)
            {

                return new T();
            }
        }
        public static string Get(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1000 * 60 * 15;
            request.Method = "GET";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string returnValue = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return returnValue;
            }
            catch (WebException ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            return "";
        }

        /// <summary>
        /// 移除html元素
        /// </summary>
        /// <param name="htmlstring">The htmlstring.</param>
        /// <returns></returns>
        /// 创建者：解志辉
        /// ------------------------------------
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

            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", "");
            var htmlEncode = HttpContext.Current.Server.HtmlEncode(htmlstring);
            if (htmlEncode != null)
                htmlstring = htmlEncode.Trim();

            return htmlstring;
        }
        /// <summary>
        /// 删除注释
        /// </summary>
        /// <param name="htmlstring">原字符串.</param>
        /// <returns></returns>
        /// 创建者：解志辉
        /// ------------------------------------
        public static string RemoveNotes(string htmlstring)
        {
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            return htmlstring;
        }
        /// <summary>
        /// 移除table元素.
        /// </summary>
        /// <param name="htmlstring">The htmlstring.</param>
        /// <returns></returns>
        /// 创建者：解志辉
        /// ------------------------------------
        public static string RemoveTable(string htmlstring)
        {
            //删除TABLE 
            htmlstring = htmlstring.Replace("</tr>", "<br/>");
            htmlstring = Regex.Replace(htmlstring, @"</?table[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"</?tr[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"</?td[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"</?th[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"</?BLOCKQUOTE[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"</?tbody[^>]*>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<style[^\s]*", "", RegexOptions.IgnoreCase);
            return htmlstring;

        }
        /// <summary>
        /// 字典排序 升序.
        /// </summary>
        /// <param name="tdic"> </param>
        /// <returns>key1:value1|key2:key2</returns>
        /// 创建者：解志辉
        /// ------------------------------------
        private static string GetSignValue(object tdic)
        {
            var dic = ToMap(tdic);
            var vle = "";
            if (dic.Count > 0)
            {
                var lst = new List<KeyValuePair<string, string>>(dic);
                lst.Sort((s1, s2) => String.Compare(s1.Key, s2.Key, StringComparison.Ordinal));
                vle = lst.Aggregate(vle, (current, kvp) => current + (kvp.Key + ":" + kvp.Value + "|"));
            }
            return vle.TrimEnd('|');
        }
        /// <summary>
        ///
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToMap(Object o)
        {
            var map = new Dictionary<string, string>();
            try
            {
                map = o as Dictionary<string, string>;

                if (map == null)
                {
                    map = new Dictionary<string, string>();
                    Type t = o.GetType();

                    PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (PropertyInfo p in pi)
                    {
                        MethodInfo mi = p.GetGetMethod();

                        if (mi != null && mi.IsPublic)
                        {
                            var vl = (string)mi.Invoke(o, null);
                            if (vl != null)
                                map.Add(p.Name, vl);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("ToMap:" + ex.ToString());
            }

            return map;


        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <param name="safeCode">The safe code.</param>
        /// <returns></returns>
        public static string GetAccessToken(object dic, string data, string safeCode = "")
        {
            var v = GetSignValue(dic);
            if (!string.IsNullOrEmpty(safeCode))
            {
                v += "|" + safeCode;
            }
#if (HELP)
            data = string.Empty;
#endif
            v += data == string.Empty ? "" : "|" + data;
            LoggerHelper.Error("请求拼装串:" + v);
            return EncryptHelper.Md5Encrypt(v);
        }
    }
}
