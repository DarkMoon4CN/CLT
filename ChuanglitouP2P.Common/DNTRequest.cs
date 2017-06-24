using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// Request 操作类
    /// </summary>
    public class DNTRequest
    {




        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");

        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }


        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }

            return HttpContext.Current.Request.ServerVariables[strName].ToString();


        }



        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {

            }
            if (retVal == null)
                return "";

            return retVal;

        }



        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;


        }


        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }


        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }



        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }



        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool trueorfalse)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            if (trueorfalse)
            {
                return Utils.ChkSQL(HttpContext.Current.Request.QueryString[strName]);
            }
            else
            {
                return HttpContext.Current.Request.QueryString[strName];
            }
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }



        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }



        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }


        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }





        /// <summary>
        /// 获得指定Url参数的decimal类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.QueryString[strName], defValue);
        }



        /// <summary>
        /// 获得指定表单参数的Decimal类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.Form[strName], defValue);
        }


        /// <summary>
        /// 获得指定Url或表单参数的Decimal类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static decimal GetDecimal(string strName, decimal defValue)
        {
            if (GetQueryDecimal(strName, defValue) == defValue)
            {
                return GetFormDecimal(strName, defValue);
            }
            else
            {
                return GetQueryDecimal(strName, defValue);
            }
        }







        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP1()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (null == result || result == String.Empty || !Utils.IsIP(result))
            {
                return "0.0.0.0";
            }

            return result;

        }

        /// <summary>
        ///获得客户端IP 后来新改的   穿过代理服务器取远程用户真实IP地址

        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            //获得客户端IP 后来新改的
            // 穿过代理服务器取远程用户真实IP地址



            //string Ip = string.Empty;

            //Ip = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            //if (string.IsNullOrEmpty(Ip))
            //{
            //    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            //    {
            //        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            //        {
            //            if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
            //                Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
            //            else
            //                if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            //                    Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            //                else
            //                    Ip = "0.0.0.0";
            //        }
            //        else
            //            Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //    }
            //    else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            //    {
            //        Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            //    }
            //    else
            //    {
            //        Ip = "0.0.0.0";
            //    }
            //}
            //return Ip;

            //            在网站服务器上看到的IP值如下（PHP写法，如$_SERVER['REMOTE_ADDR']）：
            //REMOTE_ADDR：加速乐节点IP
            //HTTP_X_FORWARDED_FOR：网民电脑IP,代理IP1,代理IP2
            //HTTP_X_REAL_FORWARDED_FOR：网民电脑IP
            //HTTP_X_CONNECTING_IP：代理IP2
            //建议程序代码获取“HTTP_X_REAL_FORWARDED_FOR：网民电脑IP”进行测试
            string Ip = string.Empty;

            Ip = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            if (string.IsNullOrEmpty(Ip))
            {


                if (HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    else
                        if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                            Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        else
                            Ip = "0.0.0.0";
                }
                else
                    Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_FORWARDED_FOR"].ToString();

            }
            return Ip;



        }




        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }



        /// <summary>
        ///  返回body里的所有json数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string InputStream(Stream request)
        {
            Stream reqStream = request;
            string req = string.Empty;
            try
            {
                byte[] buffer = new byte[(int)reqStream.Length];
                reqStream.Read(buffer, 0, (int)reqStream.Length);
                req = System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reqStream != null)
                {
                    reqStream.Close();
                    reqStream.Dispose();
                    reqStream.Close();
                }
            }
            return req;
        }


        //		/// <summary>
        //		/// 保存上传的文件
        //		/// </summary>
        //		/// <param name="MaxAllowFileCount">最大允许的上传文件个数</param>
        //		/// <param name="MaxAllowFileSize">最大允许的文件长度(单位: KB)</param>
        //		/// <param name="AllowFileExtName">允许的文件扩展名, 以string[]形式提供</param>
        //		/// <param name="AllowFileType">允许的文件类型, 以string[]形式提供</param>
        //		/// <param name="Dir">目录</param>
        //		/// <returns></returns>
        //		public static Forum.AttachmentInfo[] SaveRequestFiles(int MaxAllowFileCount, int MaxAllowFileSize, string[] AllowFileExtName, string[] AllowFileType, string Dir)
        //		{
        //			int savefilecount = 0;
        //			
        //			int fcount = Math.Min(MaxAllowFileCount, HttpContext.Current.Request.Files.Count);
        //
        //			Forum.AttachmentInfo[] attachmentinfo = new Forum.AttachmentInfo[fcount];
        //			for(int i=0;i<fcount;i++)
        //			{
        //				string filename = HttpContext.Current.Request.Files[i].FileName;
        //				string fileextname = filename.Substring(filename.LastIndexOf("."));
        //				string filetype = HttpContext.Current.Request.Files[i].ContentType;
        //				int filesize = HttpContext.Current.Request.Files[i].ContentLength;
        //				// 判断 文件扩展名/文件大小/文件类型 是否符合要求
        //				if(Utils.InArray(fileextname, AllowFileExtName) && (filesize <= MaxAllowFileSize * 1024) && Utils.InArray(filetype, AllowFileType))
        //				{
        //
        //					HttpContext.Current.Request.Files[i].SaveAs(Dir + Utils.GetDateTime() + Environment.TickCount.ToString() + fileextname);
        //					attachmentinfo[savefilecount].Filename = filename;
        //					attachmentinfo[savefilecount].Filesize = filesize;
        //					attachmentinfo[savefilecount].Description = filetype;
        //					attachmentinfo[savefilecount].Filetype = fileextname;
        //					savefilecount++;
        //				}
        //			}
        //			return attachmentinfo;
        //			
        //		}





    }
}
