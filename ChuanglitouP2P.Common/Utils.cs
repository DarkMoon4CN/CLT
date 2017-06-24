using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using ChuanglitouP2P.DBUtility;
using Microsoft.VisualBasic;
using ChuanglitouP2P.Model;
using System.Web.Script.Serialization;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.Mvc;
namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {


        private static Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);

        public static Regex RegexFont = new Regex(@"<font color=" + "\".*?\"" + @">([\s\S]+?)</font>", Utils.GetRegexCompiledOptions());

        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        private static string TemplateCookieName = string.Format("dnttemplateid_{0}_{1}_{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);




        /// <summary>
        /// 得到正则编译参数设置
        /// </summary>
        /// <returns></returns>
        public static RegexOptions GetRegexCompiledOptions()
        {
#if NET1
              return RegexOptions.Compiled.
#else
            return RegexOptions.None;
#endif
        }



        /// <summary>
        /// 返回字符串真实长度 1个汉字长度为 2
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;

        }


        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (stringarray == "" || stringarray == null)
            {
                return false;
            }

            str = str.ToLower();
            string[] stringArray = Utils.SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                //string t1 = str;
                //string t2 = stringArray[i];
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;

        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }


        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }


        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            //Regex r = null;
            Match m = null;

            //r = new Regex(@"(\r\n)",RegexOptions.IgnoreCase);
            for (m = RegexBr.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }


            return str;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }


        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }



        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }



        /// <summary>
        /// 以指定的ContentType输出指定文件文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">输出的文件名</param>
        /// <param name="filetype">将文件输出时设置的ContentType</param>
        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            Stream iStream = null;

            // 缓冲区为10k
            byte[] buffer = new Byte[10000];

            // 文件长度
            int length;

            // 需要读的数据长度
            long dataToRead;

            try
            {
                // 打开文件
                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


                // 需要读的数据长度
                dataToRead = iStream.Length;

                HttpContext.Current.Response.ContentType = filetype;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utils.UrlEncode(filename.Trim()).Replace("+", " "));

                while (dataToRead > 0)
                {
                    // 检查客户端是否还处于连接状态
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // 如果不再连接则跳出死循环
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    // 关闭文件
                    iStream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }










        /// <summary>
        /// int型转换为string型
        /// </summary>
        /// <returns>转换后的string类型结果</returns>
        public static string IntToStr(int intValue)
        {
            //
            return Convert.ToString(intValue);
        }
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }


        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }


        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {


            string myResult = p_SrcString;

            /*
            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (p_StartIndex >= p_SrcString.Length)
                {
                    return "";
                }
                else
                {
                    return p_SrcString.Substring(p_StartIndex,
                                                   ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }
            */

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string Spaces(int nSpaces)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nSpaces; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString1(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }

        /// <summary>
        /// 根据阿拉伯数字返回月份的名称(可更改为某种语言)
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }

        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }


        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回标准时间 
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00" || string.IsNullOrWhiteSpace(fDateTime))
            {
                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }


        public static string GetRealIP()
        {
            string ip = DNTRequest.GetIP();

            return ip;
        }

        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string mashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }


        /// <summary>
        /// 此方法用于确认用户输入的不是恶意信息
        /// </summary>
        /// <param name="text">用户输入信息</param>
        /// <param name="maxLength">输入的最大长度</param>
        /// <returns>处理后的输入信息</returns>
        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            //将网页中非法和有攻击性的符号替换掉，以防sql注入！返回正常数据
            text = Regex.Replace(text, "[\\s]{2,}", " ");	// 2个或以上的空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br> html换行符
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;   html空格符
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	// 任何其他的标签
            text = text.Replace("'", "''");// 单引号
            return text;
        }

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }


        /// <summary>
        /// 转换为静态html
        /// </summary>
        public void transHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fs;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }


        /// <summary>
        /// 转换为简体中文
        /// </summary>
        public static string ToSChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }

        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        public static string ToTChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        /// <summary>
        /// 替换html字符
        /// </summary>
        public static string EncodeHtml(string strHtml)
        {
            if (strHtml != "")
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }



        //public static string ClearHtml(string strHtml)
        //{
        //    if (strHtml != "")
        //    {

        //        r = Regex.Replace(@"<\/?[^>]*>", RegexOptions.IgnoreCase);
        //        for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
        //        {
        //            strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
        //        }
        //    }
        //    return strHtml;
        //}


        /// <summary>
        /// 进行指定的替换(脏字过滤)
        /// </summary>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = "";
            string text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }



        /// <summary>
        /// 获得伪静态页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append("-");
                    s.Append(i);
                    s.Append(expname);
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }


        /// <summary>
        /// 获得帖子的伪静态页码显示链接
        /// </summary>
        /// <param name="expname"></param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;
            int curPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                s.Append("<a href=\"");
                s.Append(url);
                s.Append("-");
                s.Append(i);
                s.Append(expname);
                s.Append("\">");
                s.Append(i);
                s.Append("</a>");
            }
            s.Append(t2);

            return s.ToString();
        }



        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, "page");
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, pagetag, null);
            //if (pagetag == "")
            //    pagetag = "page";
            //int startPage = 1;
            //int endPage = 1;

            //if(url.IndexOf("?") > 0)
            //{
            //    url = url + "&";
            //}
            //else
            //{
            //    url = url + "?";
            //}


            //string t1 = "<a href=\"" + url + "&" + pagetag + "=1" + "\">&laquo;</a>";
            //string t2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage + "\">&raquo;</a>";

            //if (countPage < 1) 
            //    countPage = 1;
            //if (extendPage < 3) 
            //    extendPage = 2;

            //if (countPage > extendPage)
            //{
            //    if (curPage - (extendPage / 2) > 0)
            //    {
            //        if (curPage + (extendPage / 2) < countPage)
            //        {
            //            startPage = curPage - (extendPage / 2);
            //            endPage = startPage + extendPage - 1;
            //        }
            //        else
            //        {
            //            endPage = countPage;
            //            startPage = endPage - extendPage + 1;
            //            t2 = "";
            //        }
            //    }
            //    else
            //    {
            //        endPage = extendPage;
            //        t1 = "";
            //    }
            //}
            //else
            //{
            //    startPage = 1;
            //    endPage = countPage;
            //    t1 = "";
            //    t2 = "";
            //}

            //StringBuilder s = new StringBuilder("");

            //s.Append(t1);
            //for (int i = startPage; i <= endPage; i++)
            //{
            //    if (i == curPage)
            //    {
            //        s.Append("<span>");
            //        s.Append(i);
            //        s.Append("</span>");
            //    }
            //    else
            //    {
            //        s.Append("<a href=\"");
            //        s.Append(url);
            //        s.Append(pagetag);
            //        s.Append("=");
            //        s.Append(i);
            //        s.Append("\">");
            //        s.Append(i);
            //        s.Append("</a>");
            //    }
            //}
            //s.Append(t2);

            //return s.ToString();
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <param name="anchor">锚点</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
        {
            if (pagetag == "")
                pagetag = "page";
            int startPage = 1;
            int endPage = 1;

            if (url.IndexOf("?") > 0)
            {
                url = url + "&";
            }
            else
            {
                url = url + "?";
            }

            string t1 = "<a href=\"" + url + "&" + pagetag + "=1";
            string t2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage;
            if (anchor != null)
            {
                t1 += anchor;
                t2 += anchor;
            }
            t1 += "\">&laquo;</a>";
            t2 += "\">&raquo;</a>";

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append(pagetag);
                    s.Append("=");
                    s.Append(i);
                    if (anchor != null)
                    {
                        s.Append(anchor);
                    }
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }


        /// <summary>
        /// 返回指定目录下的非 UTF8 字符集文件
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string Path)
        {
            //System.IO.StreamReader reader = null;
            StringBuilder filelist = new StringBuilder();
            DirectoryInfo Folder = new DirectoryInfo(Path);
            //System.IO.DirectoryInfo[] subFolders = Folder.GetDirectories(); 
            /*
            for (int i=0;i<subFolders.Length;i++) 
            { 
                FindNoUTF8File(subFolders[i].FullName); 
            }
            */
            FileInfo[] subFiles = Folder.GetFiles();
            for (int j = 0; j < subFiles.Length; j++)
            {
                if (subFiles[j].Extension.ToLower().Equals(".htm"))
                {
                    FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open, FileAccess.Read);
                    bool bUtf8 = IsUTF8(fs);
                    fs.Close();
                    if (!bUtf8)
                    {
                        filelist.Append(subFiles[j].FullName);
                        filelist.Append("\r\n");
                    }
                }
            }
            return Utils.SplitString(filelist.ToString(), "\r\n");

        }

        //0000 0000-0000 007F - 0xxxxxxx  (ascii converts to 1 octet!)
        //0000 0080-0000 07FF - 110xxxxx 10xxxxxx    ( 2 octet format)
        //0000 0800-0000 FFFF - 1110xxxx 10xxxxxx 10xxxxxx (3 octet format)

        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }

        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int SafeInt32(object objNum)
        {
            if (objNum == null)
            {
                return 0;
            }
            string strNum = objNum.ToString();
            if (IsNumeric(strNum))
            {

                if (strNum.ToString().Length > 9)
                {
                    if (strNum.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                }
                return Int32.Parse(strNum);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return Utils.MakeSureDirectoryPathExists(name);
        }

        /// <summary>
        /// 为脚本替换特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }


        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");

        }



        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = Utils.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }


            }
            return false;

        }

        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
            return AssemblyFileVersion.ProductName;
        }

        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {
            return AssemblyFileVersion.LegalCopyright;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }


        /// <summary>
        /// 读cookie数组值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string CookieName, string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                // return HttpContext.Current.Request.Cookies[strName].Value.ToString();

                return HttpContext.Current.Request.Cookies[CookieName].Values[strName].ToString();
            }

            return "";
        }



        public static string GetPhysicalPath(string virtualPath)
        {
            string basePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            basePath = basePath.Replace("bin/ChuanglitouP2P.Common.DLL", virtualPath).Replace("file:///", "");
            return basePath;
        }



        //HttpContext.Current.Request.Cookies["emailcode"].Values["email"].ToString()

        /// <summary>
        /// 得到论坛的真实路径
        /// </summary>
        /// <returns></returns>
        public static string GetTrueForumPath()
        {
            string forumPath = HttpContext.Current.Request.Path;
            if (forumPath.LastIndexOf("/") != forumPath.IndexOf("/"))
            {
                forumPath = forumPath.Substring(forumPath.IndexOf("/"), forumPath.LastIndexOf("/") + 1);
            }
            else
            {
                forumPath = "/";
            }
            return forumPath;

        }
        public static string GetQueryString(string qName, int intLen)
        {
            string str = "";
            if (HttpContext.Current.Request.Params[qName] != null && HttpContext.Current.Request.Params[qName] != "")
                str = Utils.InputText(HttpContext.Current.Request.Params[qName], intLen);
            return str;
        }
        /// <summary>
        /// 判断字符串是否是yy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }


        public static string RemoveALLHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }




        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }


        /// <summary>
        /// 过滤SQL入注标记
        /// </summary>
        /// <param name="NoHTML">包括HTML，脚本，数据库关键字，特殊字符的源码 </param>
        /// <returns>已经去除标记后的文字</returns>
        public static string CheckSQLHtml(string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            else
            {
                //过滤HTML中的不安全标签
                Htmlstring = Regex.Replace(Htmlstring, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);



                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                //  Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                //  Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "or", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "net", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "-", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "script", "", RegexOptions.IgnoreCase);

                //特殊的字符
                Htmlstring = Htmlstring.Replace("<", "");
                Htmlstring = Htmlstring.Replace(">", "");
                Htmlstring = Htmlstring.Replace("*", "");
                Htmlstring = Htmlstring.Replace("-", "");
                Htmlstring = Htmlstring.Replace("?", "");
                Htmlstring = Htmlstring.Replace("'", "''");
                Htmlstring = Htmlstring.Replace(",", "");
                Htmlstring = Htmlstring.Replace("/", "");
                Htmlstring = Htmlstring.Replace(";", "");
                Htmlstring = Htmlstring.Replace("*/", "");
                Htmlstring = Htmlstring.Replace("\r\n", "");
                Htmlstring = Htmlstring.Replace("/*", "");

                Htmlstring = Htmlstring.Replace("chr(13)", "");
                Htmlstring = Htmlstring.Replace("chr(10)", "");



                Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

                return Htmlstring;
            }
        }



        /// <summary>
        /// 过滤SQL保留/存路径用
        /// </summary>
        /// <param name="NoHTML">包括HTML，脚本，数据库关键字，特殊字符的源码 </param>
        /// <returns>已经去除标记后的文字</returns>
        public static string CheckSQL(string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            else
            {
                //过滤HTML中的不安全标签
                Htmlstring = Regex.Replace(Htmlstring, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);



                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "or", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "*", "", RegexOptions.IgnoreCase);
                // Htmlstring = Regex.Replace(Htmlstring, "-", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "script", "", RegexOptions.IgnoreCase);

                //特殊的字符
                Htmlstring = Htmlstring.Replace("<", "");
                Htmlstring = Htmlstring.Replace(">", "");
                Htmlstring = Htmlstring.Replace("*", "");
                // Htmlstring = Htmlstring.Replace("?", "");
                Htmlstring = Htmlstring.Replace("'", "''");
                Htmlstring = Htmlstring.Replace(",", "");
                //  Htmlstring = Htmlstring.Replace("/", "");
                Htmlstring = Htmlstring.Replace(";", "");
                Htmlstring = Htmlstring.Replace("*/", "");
                Htmlstring = Htmlstring.Replace("\r\n", "");
                Htmlstring = Htmlstring.Replace("/*", "");

                Htmlstring = Htmlstring.Replace("chr(13)", "");
                Htmlstring = Htmlstring.Replace("chr(10)", "");



                Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

                return Htmlstring;
            }
        }



        /// <summary>
        /// 将用户组Title中的font标签去掉
        /// </summary>
        /// <param name="title">用户组Title</param>
        /// <returns></returns>
        public static string RemoveFontTag(string title)
        {
            Match m = RegexFont.Match(title);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return title;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            return TypeParse.IsNumeric(Expression);
        }
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }

        public static bool IsDouble(object Expression)
        {
            return TypeParse.IsDouble(Expression);
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            return TypeParse.StrToBool(Expression, defValue);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {
            return TypeParse.StrToInt(Expression, defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return TypeParse.StrToFloat(strValue, defValue);
        }


        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object strValue, decimal defValue)
        {
            return TypeParse.StrToDecimal(strValue, defValue);
        }





        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            return TypeParse.IsNumericArray(strNumber);
        }


        public static string AdDeTime(int times)
        {
            string newtime = (DateTime.Now).AddMinutes(times).ToString();
            return newtime;

        }
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {

            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        public static bool IsRuleTip(Hashtable NewHash, string ruletype, out string key)
        {
            key = "";
            foreach (DictionaryEntry str in NewHash)
            {

                try
                {
                    string[] single = SplitString(str.Value.ToString(), "\r\n");

                    foreach (string strs in single)
                    {
                        if (strs != "")


                            switch (ruletype.Trim().ToLower())
                            {
                                case "email":
                                    if (IsValidDoEmail(strs.ToString()) == false)
                                        throw new Exception();
                                    break;

                                case "ip":
                                    if (IsIPSect(strs.ToString()) == false)
                                        throw new Exception();
                                    break;

                                case "timesect":
                                    string[] splitetime = strs.Split('-');
                                    if (Utils.IsTime(splitetime[1].ToString()) == false || Utils.IsTime(splitetime[0].ToString()) == false)
                                        throw new Exception();
                                    break;

                            }

                    }
                }
                catch
                {
                    key = str.Key.ToString();
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }


        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        /// <summary>
        /// 获取记录模板id的cookie名称
        /// </summary>
        /// <returns></returns>
        public static string GetTemplateCookieName()
        {
            return TemplateCookieName;
        }

        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }

        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }



        /// <summary>
        /// 返回会员等级
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string RetUseridentity(string i)
        {
            string str = "";

            switch (i)
            {
                case "0":
                    str = "普通";
                    break;
                case "1":
                    str = "vip";
                    break;
                case "2":
                    str = "黄金";
                    break;
                case "3":
                    str = "虚假";
                    break;
                case "4":
                    str = "渠道";
                    break;
                case "5":
                    str = "白金";
                    break;

                case "6":
                    str = "钻石";
                    break;

                case "7":
                    str = "钻石1";
                    break;

                case "8":
                    str = "钻石2";
                    break;

                default:
                    str = "未知";
                    break;
            }

            return str;

        }



        /// <summary>
        /// 返回两个日期之间的时间间隔
        /// </summary>
        /// <param name="Interval"> 返回类型 秒 ,分,小时,天等</param>
        /// <param name="StartDate">起始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <returns>返回两个日期之间的时间间隔</returns>

        public static long DateDiff(string Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;


            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case "Second":
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case "Minute":
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case "Hour":
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case "Day":
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case "Week":
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case "Month":
                    //lngDateDiffValue = (long)(TS.Days / 30);
                    //应取两个时间的月份之差(季度和年同理) 
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year) * 12 + (EndDate.Month - StartDate.Month);
                    break;
                case "Quarter":
                    //lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year) * 4 + (Quarter(EndDate) - Quarter(StartDate));
                    break;
                case "Year":
                    //lngDateDiffValue = (long)(TS.Days / 365);
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year);
                    break;
            }
            return (lngDateDiffValue);
        }


        /// <summary>
        /// 返回几小时间或几秒前的时差
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
                if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else
                    if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else
                        if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else
                            if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else
                                if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else
                                    if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else
                                        if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }




        /// <summary> 
        /// 取得某个日期是本年度的第几个季度. 
        /// </summary> 
        /// <param name="tDate"></param> 
        /// <returns></returns> 
        public static int Quarter(DateTime tDate)
        {
            switch (tDate.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;

                case 4:
                case 5:
                case 6:
                    return 2;

                case 7:
                case 8:
                case 9:
                    return 3;

                default:
                    return 4;
            }

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




        //加密方法  


        //注意：sKey输入密码的时候，必须使用英文字符，区分大小写，且字符数量是8个，不能多也不能少，否则出错。

        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中     
            //原来使用的UTF8编码，我改成Unicode编码了，不行     
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]   inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);     

            //建立加密对象的密钥和偏移量     
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法     
            //使得输入密码必须输入英文文本     
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write   the   byte   array   into   the   crypto   stream     
            //(It   will   end   up   in   the   memory   stream)     
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get   the   data   back   from   the   memory   stream,   and   into   a   string     
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format   as   hex     
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }


        //解密方法     
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put   the   input   string   into   the   byte   array     
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改     
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush   the   data   through   the   crypto   stream   into   the   memory   stream     
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get   the   decrypted   data   back   from   the   memory   stream     
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象     
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }



        public static void DrawImage()
        {
            // CreateImage img = new CreateImage();
            HttpContext.Current.Session["CheckCode"] = RndNum(4);
            CreateImages(HttpContext.Current.Session["CheckCode"].ToString());
        }
        /// <summary>
        /// 生成验证图片
        /// </summary>
        /// <param name="checkCode">验证字符</param>
        public static void CreateImages(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 25);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(font[findex], 10, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 3 + (i * 12), ii);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            HttpContext.Current.Response.ClearContent();
            //Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Jpeg";
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }


        /// <summary>
        /// 字符串单字用,号分隔组成新字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string strJoin(string str)
        {


            return string.Join(",", str.ToArray());

        }





        /// <summary>
        /// 生成随机的字母
        /// </summary>
        /// <param name="VcodeNum">生成字母的个数</param>
        /// <returns>string</returns>
        public static string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(',');
            string VNum = ""; //由于字符串很短，就不用StringBuilder了
            int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    // return RndNum(VcodeNum);
                    t = rand.Next(VcArray.Length);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }



        /// <summary>
        /// 生成随机的字母
        /// </summary>
        /// <param name="VcodeNum">生成字母的个数</param>
        /// <returns>string</returns>
        public static string RndNumChar(int VcodeNum)
        {
            //string Vchar = "0,1,2,3,4,5,6,7,8,9";
            // string Vchar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
            string Vchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
            string[] VcArray = Vchar.Split(',');
            string VNum = ""; //由于字符串很短，就不用StringBuilder了
            int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    //return RndNumChar(VcodeNum);
                    t = rand.Next(VcArray.Length);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }




        /// <summary>
        /// 检测上传文件安全 在上传文件时一定要用
        /// </summary>
        /// <param name="strPictureFilePath"></param>
        public static bool CheckPictureSafe(string strPictureFilePath)
        {
            bool ischeck = true;

            bool flag = true;
            if (!File.Exists(strPictureFilePath))
            {
                StringBuilder builder = new StringBuilder();
                try
                {
                    using (StreamReader reader = new StreamReader(strPictureFilePath))
                    {
                        string str;
                        while ((str = reader.ReadLine()) != null)
                        {
                            builder.Append(str + ",");
                        }
                        if (builder == null)
                        {
                            flag = false;
                            ischeck = true;
                        }
                        else
                        {
                            builder = builder.Replace("'", "''");
                            string[] strArray = "script|iframe|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=|include|filesystemobject|shell.application".Split(new char[] { '|' });
                            string[] strArray2 = strArray;
                            int index = 0;
                            while (index < strArray2.Length)
                            {
                                string str3 = strArray2[index];
                                flag = true;
                                break;
                            }
                        }
                        reader.Close();
                    }
                    if (flag)
                    {
                        File.Delete(strPictureFilePath);
                        ischeck = false;
                    }
                }
                catch (Exception exception)
                {
                    ischeck = false;
                    throw new Exception(exception.Message);

                }
            }

            return ischeck;
        }


        /// <summary>
        /// 清除指定缓存
        /// </summary>
        /// <param name="cachename"></param>
        public static void RemoveCache(string cachename)
        {

            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                if (key == cachename)
                {
                    _cache.Remove(key);
                }
            }

        }


        /// <summary>
        /// 返回提现单位字符
        /// </summary>
        /// <param name="un"></param>
        /// <returns></returns>
        public static string return_unit(int un)
        {

            switch (un)
            {
                case 0:
                    return "单笔(元)";
                case 1:
                    return "单笔百分比(%)";
                default:
                    return "未选选";
            }

        }


        /// <summary>
        /// 返回合同模版使用状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetUsestate(int i)
        {

            switch (i)
            {
                case 0:
                    return "未使用";
                case 1:
                    return "使用中";
                default:
                    return "未使用";
            }


        }





        public static string Getpayment_options(int i)
        {
            switch (i)
            {
                case 1:
                    return "按月等额本息";
                case 2:
                    return "按季等额本金";
                case 3:
                    return "每月还息，到期还本";
                case 4:
                    return "一次性还本付息";
                default:
                    return "未知";
            }
        }




        /// <summary>
        /// 获取项目图片类型
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string Gettype_picture(int i)
        {
            switch (i)
            {
                case 1:
                    return "基础材料";
                case 2:
                    return "担保材料 ";
                case 3:
                    return "现场图片";
                default:
                    return "未知";
            }
        }



        public static string GetTrbacekColor(int i)
        {
            string str = "";

            if (Convert.ToBoolean(i % 2))
            {
                str = "  style=\"background-color:#f2f2f2\"  ";
            }


            return str;

        }


        /// <summary>
        /// 返回借款期限单位组合
        /// </summary>
        /// <param name="Life_of_loan">借款期限</param>
        /// <param name="unit_day">单位</param>
        /// <returns></returns>
        public static string GetLife_of_loan(string Life_of_loan, string unit_day)
        {
            switch (unit_day)
            {
                case "1":
                    return Life_of_loan + "个月";
                case "3":
                    return Life_of_loan + "天 ";
                default:
                    return "未知";
            }

        }

        /// <summary>
        /// 返回借款期限单位组合
        /// </summary>
        /// <param name="Life_of_loan">借款期限</param>
        /// <param name="unit_day">单位</param>
        /// <returns></returns>
        public static string GetLife_of_loans(string Life_of_loan, string unit_day)
        {
            switch (unit_day)
            {
                case "1":
                    return Life_of_loan;
                case "3":
                    return Life_of_loan;
                default:
                    return "未知";
            }

        }

        /// <summary>
        /// 只返回单位
        /// </summary>
        /// <param name="unit_day"></param>
        /// <returns></returns>
        public static string getLifeUnit(string unit_day)
        {
            switch (unit_day)
            {
                case "1":
                    return "个月";
                case "3":
                    return "天 ";
                default:
                    return "未知";
            }

        }


        /// <summary>
        /// 获取后台登录管理员id
        /// </summary>
        /// <returns></returns>
        public static int GetAdmUserID()
        {
            int admid = 0;
            if (HttpContext.Current.Session["adminuserid"] != null)
            {

                admid = int.Parse(HttpContext.Current.Session["adminuserid"].ToString());

            }

            return admid;
        }




        public static string GetTender_state(string i)
        {
            switch (i)
            {
                case "-1":
                    return "录入待审";
                case "0":
                    return "初审审核中";
                case "1":
                    return "初审通过";
                case "2":
                    return "复审通过";
                case "3":
                    return "进行中(招标上线)";
                case "4":
                    return "满标(还款中)";
                case "5":
                    return "已还清";
                case "6":
                    return "初审未通过";
                case "7":
                    return "复审未通过";
                case "8":
                    return "流标";

                default:
                    return "录入待审";
            }


        }



        /// <summary>
        /// 获取汇付商户号
        /// </summary>
        /// <returns></returns>
        public static string GetMerId()
        {

            return ConfigurationManager.AppSettings["MerId"].ToString();
        }


        /// <summary>
        /// 获取汇付商户私钥
        /// </summary>
        /// <returns></returns>
        public static string GetMerPr()
        {

            return ConfigurationManager.AppSettings["MerPr"].ToString();
        }


        /// <summary>
        /// 获取汇付商户公钥
        /// </summary>
        /// <returns></returns>
        public static string GetPgPubk()
        {
            return ConfigurationManager.AppSettings["PgPubk"].ToString();
        }

        /// <summary>
        /// 获取汇付客户号
        /// </summary>
        /// <returns></returns>
        public static string GetMerCustID()
        {
            return ConfigurationManager.AppSettings["MerCustID"].ToString();
        }

        /// <summary>
        /// 获取担保公司客户号
        /// </summary>
        /// <returns></returns>
        public static string GetDanbaoCustID()
        {
            return ConfigurationManager.AppSettings["DanbaoCustID"].ToString();
        }
        /// <summary>
        /// 汇付接口返回网站绝对地址 不需要加 /
        /// </summary>
        /// <returns></returns>
        public static string GetRe_url(string urlfilename)
        {
            return ConfigurationManager.AppSettings["Re_url"].ToString() + urlfilename;
        }


        /// <summary>
        /// 返回网站绝对图片地址 不需要加 /
        /// </summary>
        /// <returns></returns>
        public static string GetImage_url(string urlfilename)
        {
            return ConfigurationManager.AppSettings["image_url"].ToString() + urlfilename;
        }


        /// <summary>
        /// 是否向用户收取取现手续费 true收取 false不收取
        /// </summary>
        /// <returns></returns>
        public static string GetIsFee()
        {
            return ConfigurationManager.AppSettings["IsFee"].ToString();
        }


        /// <summary>
        /// 汇付企业专属帐户
        /// </summary>
        /// <returns></returns>
        public static string GetMERDT()
        {
            return ConfigurationManager.AppSettings["MERDT"].ToString();
        }


        /// <summary>
        /// 投资人取现服务费
        /// </summary>
        /// <returns></returns>
        public static decimal GetinCashser()
        {
            decimal st = 0.00M;
            string str = ConfigurationManager.AppSettings["inCashser"].ToString();
            try
            {
                st = decimal.Parse(str);
            }
            catch
            {
                st = 0.00M;
            }

            return st;
        }

        /// <summary>
        /// 借款人取现服务费
        /// </summary>
        /// <returns></returns>
        public static decimal GetBoCashser()
        {
            decimal st = 0.00M;
            string str = ConfigurationManager.AppSettings["BoCashser"].ToString();

            try
            {
                st = decimal.Parse(str);
            }
            catch
            {
                st = 0.00M;
            }

            return st;
        }


        /// <summary>
        /// 获取汇付返回的用户号如cltwl_zxjtest001分格出_后半部分
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetUserSplit(string str)
        {
            string str1 = "";
            string[] us = str.Split('_', '）');

            if (us.Length >= 2)
            {
                str1 = us[1];
            }
            return str1;

        }


        /// <summary>
        /// 返回汇付远程接口地址
        /// </summary>
        /// <returns></returns>
        public static string GetChinapnrUrl()
        {
            return GetAppSetting("ChinapnrUrl");
        }

        /// <summary>
        /// 返回手续费收取功能（0关，1开）
        /// </summary>
        /// <returns></returns>
        public static string GetCostTaking()
        {
            return GetAppSetting("CostTaking");
        }

        /// <summary>
        /// 返回Weg.Config配置文件值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 验证页面来源是否为本站
        /// </summary>
        /// <returns></returns>
        public static bool GetPageUrlReferrer()
        {
            bool t = false;

            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                string url = ConfigurationManager.AppSettings["pageUrlReferrer"].ToString();
                string urls = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString();
                if (urls.Contains(urls))
                {
                    t = true;
                }
                else
                {
                    t = false;
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                t = false;
                HttpContext.Current.Response.End();
            }

            return t;
        }

        /// <summary>
        /// 用户登录操作
        /// </summary>

        public static bool CheckUserLogin()
        {
            bool t = false;
            if (GetUserIDCookies() > 0)
            {
                t = true;
            }
            return t;
        }


        /// <summary>
        /// 检查登录是否成功,成功返回id 不成功能直接返回登录页
        /// </summary>
        /// <returns></returns>
        public static int checkloginsession()
        {
            int uid = 0;
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + GetUserIDCookieslocahost().ToString());
            if (M_uid != null)
            {
                if (M_uid.codeno == getSessioncode())
                {
                    uid = M_uid.userid;
                }
                else
                {
                    // HttpContext.Current.Response.Redirect("/Signin/Index");
                    HttpContext.Current.Response.Write("<script>top.location.href='/Signin/Index';</script>");
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                //Referer
                //HttpContext.Current.Session["returnpage"] = HttpContext.Current.Request.UrlReferrer;
                if (HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/investconfirm/") >= 0) //如果存储的是投资页，需进行替换地址
                {
                    HttpContext.Current.Session["returnpage"] = HttpContext.Current.Request.Url.AbsoluteUri.Replace("invest_borrow/investconfirm/", "invest_borrow_") + ".html";
                }
                else
                {
                    HttpContext.Current.Session["returnpage"] = HttpContext.Current.Request.Url.AbsoluteUri;
                }

                // HttpContext.Current.Response.Redirect("/Signin/Index");
                HttpContext.Current.Response.Write("<script>top.location.href='/Signin/Index';</script>");
                HttpContext.Current.Response.End();
            }
            return uid;
        }



        /// <summary>
        /// 检查登录是否成功,成功返回id 不成功能直接返回登录页
        /// </summary>
        /// <returns></returns>
        public static int checkloginsessiontop()
        {
            int uid = 0;
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + GetUserIDCookieslocahost().ToString());
            if (M_uid != null)
            {
                if (M_uid.codeno == getSessioncode())
                {
                    uid = M_uid.userid;
                }
                else
                {
                    uid = 0;
                }
            }
            else
            {
            }
            return uid;
        }

        public static string GetUsrLoginModel()
        {

            string Usrname = "";
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + GetUserIDCookieslocahost().ToString());
            if (M_uid != null)
            {
                if (string.IsNullOrEmpty(M_uid.UsrName))
                    Usrname = M_uid.username;
                else
                    Usrname = M_uid.UsrName;

            }
            return Usrname;
        }

        public static string GetUsrNameSex()
        {
            string str = "";
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + GetUserIDCookieslocahost().ToString());
            if (M_uid != null)
            {
                string cachename = M_uid.userid.ToString() + "name";
                if (Utils.GeTThirdCacheName(cachename) == "")
                {
                    DataTable dt = DbHelperSQL.GET_DataTable_List("select top 1 realname  ,iD_number   FROM  hx_member_table  where  registerid=" + M_uid.userid);
                    if (dt.Rows.Count > 0)
                    {
                        string rname = dt.Rows[0]["realname"].ToString();
                        if (!string.IsNullOrWhiteSpace(rname))// != null && rname != "")
                        {
                            string sf = ChinaIDCard.Getgender(dt.Rows[0]["iD_number"].ToString());
                            if (sf != "")
                            {
                                str = str + rname.Substring(0, 1);
                                str = str + sf;
                            }
                            else
                            {
                                str = rname;
                            }
                        }
                        else
                        {
                            str = M_uid.username;
                        }
                    }
                    Utils.SetThirdCacheName(cachename, str, 30);
                }
                else
                {
                    str = Utils.GeTThirdCacheName(cachename);
                }
            }
            return str;
        }


        /// <summary>
        /// 写入登录
        /// </summary>
        /// <param name="mlogin"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        public static int LoginWriteSession(M_login mlogin, int remember)
        {
            int intreutrn = 0;
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + mlogin.userid.ToString());
            try
            {
                if (M_uid != null)
                {
                    intreutrn = M_uid.userid;
                    HttpCookie cok = new HttpCookie("loginuserInfo");
                    cok.Values.Add("username", DESEncrypt.Encrypt(M_uid.username, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("users_ID", DESEncrypt.Encrypt(M_uid.userid.ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    //cok.Domain = "chuanglitou.com";
                    cok.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Current.Response.AppendCookie(cok);
                    M_login loginb = new M_login();
                    loginb.userid = mlogin.userid;
                    loginb.username = mlogin.username;
                    loginb.codeno = mlogin.codeno;
                    loginb.UsrName = mlogin.UsrName;
                    DataCache.RemoveCache(CacheRemove._loginCachePrefix + M_uid.userid.ToString());
                    DataCache.SetCache(CacheRemove._loginCachePrefix + M_uid.userid.ToString(), loginb);
                }
                else
                {
                    DataCache.SetCache(CacheRemove._loginCachePrefix + mlogin.userid.ToString(), mlogin);
                    if (remember == 1)
                    {
                        HttpCookie cok = new HttpCookie("loginuserInfo");
                        cok.Values.Add("username", DESEncrypt.Encrypt(mlogin.username, ConfigurationManager.AppSettings["webp"].ToString()));
                        cok.Values.Add("users_ID", DESEncrypt.Encrypt(mlogin.userid.ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                        // cok.Domain = "chuanglitou.com";
                        cok.Expires = DateTime.Now.AddDays(7);
                        HttpContext.Current.Response.AppendCookie(cok);
                    }
                    else
                    {
                        HttpCookie cok = new HttpCookie("loginuserInfo");
                        cok.Values.Add("users_ID", DESEncrypt.Encrypt(mlogin.userid.ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                        cok.Expires = DateTime.Now.AddHours(7);
                        HttpContext.Current.Response.AppendCookie(cok);
                    }
                    intreutrn = mlogin.userid;
                }
            }
            catch
            {
                intreutrn = 0;
            }
            return intreutrn;
        }
        /// <summary>
        /// 退出操作
        /// </summary>
        /// <param name="userid"></param>
        public static void logout(int userid)
        {
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + userid.ToString());
            // HttpContext.Current.Request.Cookies["loginuserInfo"].Expires = DateTime.Now.AddDays(-1);
            // HttpContext.Current.Response.Cookies.Add(aCookie);
            if (M_uid != null && userid != 0)
            {
                DataCache.RemoveCache(M_uid.userid.ToString());
                HttpCookie cok = HttpContext.Current.Request.Cookies["loginuserInfo"];
                //cok.Values.Add("username", DESEncrypt.Encrypt("", ConfigurationManager.AppSettings["webp"].ToString()));
                // cok.Values.Add("users_ID", DESEncrypt.Encrypt("0", ConfigurationManager.AppSettings["webp"].ToString()));
                // cok.Domain = "chuanglitou.com";
                cok.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.AppendCookie(cok);
                //HttpContext.Current.Response.Cookies["loginuserInfo"].Expires = DateTime.Now.AddDays(-7);
                //HttpContext.Current.Response.Redirect("/");
                //HttpContext.Current.Response.Redirect("/index.html");
            }
            HttpContext.Current.Response.Write("<script> top.location.href = '/';</script>");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 邮件标记
        /// </summary>
        /// <param name="emailtype"></param>
        public static void writecookieseamil(string emailtype, string email)
        {
            HttpCookie cok = new HttpCookie("emailcode");
            cok.Values.Add("emailtype", emailtype);
            cok.Values.Add("email", email);
            cok.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.AppendCookie(cok);
        }

        /// <summary>
        /// 获取邮件验证cookies 邮箱
        /// </summary>
        /// <returns></returns>
        public static string getcookieseamilcheck()
        {
            string str = null;
            if (HttpContext.Current.Request.Cookies["emailcode"] != null)
            {
                str = HttpContext.Current.Request.Cookies["emailcode"].Values["email"].ToString();
            }
            else
            {
                str = "";
            }
            return str;
        }

        /// <summary>
        /// 获取邮件验证cookies
        /// </summary>
        /// <returns></returns>
        public static string getcookieseamil()
        {
            string str = null;
            if (HttpContext.Current.Request.Cookies["emailcode"] != null)
            {
                str = HttpContext.Current.Request.Cookies["emailcode"].Values["emailtype"].ToString();
            }
            else
            {
                str = "";
            }
            return str;
        }

        /// <summary>
        /// 登录时初始化数据
        /// </summary>
        /// <param name="username">登录用户名</param>
        /// <param name="users_ID">登录ID</param>
        /// <param name="isemail">是否通过邮件认证</param>
        /// <param name="authentication">是否通过身份认证</param>
        /// <param name="user_points">用户积分</param>
        public static bool loginwritecookies(string username, string users_ID, int remember)
        {
            bool t = false;
            try
            {
                if (remember == 1)
                {
                    HttpCookie cok = new HttpCookie("loginuserInfo");
                    cok.Values.Add("username", DESEncrypt.Encrypt(username, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("users_ID", DESEncrypt.Encrypt(users_ID.ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Expires = DateTime.Now.AddDays(7);
                    HttpContext.Current.Response.AppendCookie(cok);
                }
                else
                {
                    HttpCookie cok = new HttpCookie("loginuserInfo");
                    cok.Values.Add("username", DESEncrypt.Encrypt(username, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("users_ID", DESEncrypt.Encrypt(users_ID.ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Expires = DateTime.Now.AddHours(1);
                    HttpContext.Current.Response.AppendCookie(cok);
                }
                t = true;
            }
            catch
            {
                t = false;
            }
            return t;
        }


        /// <summary>
        /// 获取登录用户名
        /// </summary>
        /// <returns></returns>
        public static string GetUserNameCookies()
        {
            string str = "";
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + GetUserIDCookieslocahost().ToString());
            if (M_uid != null)
            {
                if (M_uid.codeno == getSessioncode())
                {
                    str = M_uid.username;
                }
                else
                {
                }
            }
            else
            {
                //HttpContext.Current.Response.Redirect("/login.html");
            }
            return str;
            /*
            string str = "";

            if (HttpContext.Current.Request.Cookies["loginuserInfo"] != null)
            {

                if (HttpContext.Current.Request.Cookies["loginuserInfo"].Values["username"] != null)
                {
                    str = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["loginuserInfo"].Values["username"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());
                }
                else
                {
                    str = "";
                }
            }

            return str;
        */
        }

        public static int GetCodeUid()
        {
            int uid = 0;
            if (HttpContext.Current.Request.Cookies["Invitation"] != null)
            {
                try
                {
                    uid = int.Parse(DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["Invitation"].Values["CodeUid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                }
                catch
                {
                    uid = 0;
                }
            }
            return uid;
        }

        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <returns></returns>
        public static string GetInvCode()
        {
            string InvCode = "";
            if (HttpContext.Current.Request.Cookies["Invitation"] != null)
            {
                InvCode = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["Invitation"].Values["InvCode"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());
            }
            return InvCode;
        }

        /// <summary>
        ///  获取cookie中的邀请码,以cookieName寻找
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetInvCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            var keyValue = new Dictionary<string, string>();
            if (cookie != null)
            {
                for (int i = 0; i < cookie.Values.Count; i++)
                {
                    string key = cookie.Values.Keys[i];
                    string value = DESEncrypt.Decrypt(cookie.Values[key].ToString(), ConfigurationManager.AppSettings["webp"].ToString());
                    keyValue.Add(key, value);
                }
            }
            return keyValue;
        }

        /// <summary>
        ///  设置cookie邀请码
        /// </summary>
        /// <param name="cookieName">例：channel</param>
        /// <param name="keyValue">例：Key:Invitedcode  Value:U50DB531</param>
        /// <param name="expires">单位:天 ,默认7天</param>
        /// <returns></returns>
        public static bool SetInvCookie(string cookieName, Dictionary<string, string> keyValue, int expires = 7)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                foreach (KeyValuePair<string, string> kv in keyValue)
                {
                    cookie.Values.Add(kv.Key, DESEncrypt.Encrypt(kv.Value, ConfigurationManager.AppSettings["webp"].ToString()));
                }
                cookie.Expires = DateTime.Now.AddDays(expires);
                HttpContext.Current.Response.AppendCookie(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取推广参数
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="vls"></param>
        /// <returns></returns>
        public static string GetExtensionInfor(string cookieName, string cookieValue)
        {
            string cv = "";

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                cv = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies[cookieName].Values[cookieValue], ConfigurationManager.AppSettings["webp"].ToString());

            }
            return cv;
        }



        /// <summary>
        /// 获取登录用户ID
        /// </summary>
        /// <returns></returns>
        public static int GetUserIDCookies()
        {
            int str = 0;

            if (HttpContext.Current.Request.Cookies["loginuserInfo"] != null)
            {
                try
                {
                    str = int.Parse(DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["loginuserInfo"].Values["users_ID"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                }
                catch
                {
                    str = 0;
                }
            }

            return str;

        }



        /// <summary>
        /// 获取登录用户ID
        /// </summary>
        /// <returns></returns>
        public static int GetUserIDCookieslocahost()
        {
            int str = 0;

            if (HttpContext.Current.Request.Cookies["loginuserInfo"] != null)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["loginuserInfo"].Values["users_ID"] != null)
                    {
                        str = int.Parse(DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["loginuserInfo"].Values["users_ID"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    }
                }
                catch
                {
                    str = 0;
                }
            }

            return str;

        }



        /// <summary>
        /// 获取登录用户本地验证码
        /// </summary>
        /// <returns></returns>
        public static string GetcodenoCookieslocahost()
        {
            string str = "0";

            if (HttpContext.Current.Request.Cookies["loginuserInfo"] != null)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["loginuserInfo"].Values["codeno"] != null)
                    {

                        str = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["loginuserInfo"].Values["codeno"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());
                    }
                }
                catch
                {
                    str = "0";
                }
            }

            return str;

        }


        /// <summary>
        /// 退出登录
        /// </summary>
        public static void logout()
        {
            HttpCookie cok = HttpContext.Current.Request.Cookies["loginuserInfo"];

            //  cok.Values.Add("username", DESEncrypt.Encrypt("", ConfigurationManager.AppSettings["webp"].ToString()));
            // cok.Values.Add("users_ID", DESEncrypt.Encrypt("0", ConfigurationManager.AppSettings["webp"].ToString()));

            cok.Expires = DateTime.Now.AddDays(-30);
            HttpContext.Current.Response.AppendCookie(cok);
            HttpContext.Current.Response.Redirect("/");
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="mlogin"></param>
        public static void AddLoginCache(string username, M_login mlogin)
        {
            FormsAuthentication.SetAuthCookie(username, true, FormsAuthentication.FormsCookiePath);
            var userData = SerializeHelper.Instance.JsonSerialize<M_login>(mlogin);//序列化用户实体
                                                                                   //保存身份信息
            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddHours(12), false, userData);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));//加密身份信息，保存至Cookie
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 用户中心显示还款状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetPayment_Status(string i)
        {
            switch (i)
            {
                case "0":
                    return "还款中";
                case "1":
                    return "已还款";
                default:
                    return "--";
            }

        }

        /// <summary>
        /// 隐藏手机号中间四位
        /// </summary>
        /// <param name="tempStr"></param>
        /// <returns></returns>
        public static string hidemobile(string tempStr)
        {
            return Regex.Replace(tempStr, @"(?im)(\d{3})(\d{4})(\d{4})", "$1****$3");//134****0555
            //return Regex.Replace(tempStr, @"(?im)(\d{2})(\d{7})(\d{2})", "$1***$3");//134****0555
        }

        /// <summary>
        ///隐藏用户中间位数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string hideuser(string username)
        {
            return Regex.Replace(username, @"(?im)(\w{2})(.*)(\w{2})", "$1***$3");//134****0555
        }

        /// <summary>
        /// 根据日期时间生成订单号
        /// </summary>
        /// <returns></returns>
        public static string Createcode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff") + RndNum(2);
        }

        /// <summary>
        /// 返回充值状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetRecharge_condition(string i)
        {
            switch (i)
            {
                case "0":
                    return "未支付成功";
                case "1":
                    return "支付成功";
                default:
                    return "未支付成功";
            }

        }

        /// <summary>
        /// 借款还款状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetRepayment_State(string i)
        {
            switch (i)
            {
                case "0":
                    return "未还款";
                case "1":
                    return "已还款";
                case "2":
                    return "逾期未还";
                case "3":
                    return "平台代还";
                default:
                    return "未还款";
            }

        }

        /// <summary>
        /// 连连支付取款状态
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEnumLLPayState(Int32 str)
        {
            EnumLLPayState typef = new EnumLLPayState();
            typef = (EnumLLPayState)Enum.ToObject(typeof(EnumLLPayState), str);
            return Enum.GetName(typeof(EnumLLPayState), str);
        }


        /// <summary>
        /// 返回取款状态
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEnumOrdIdState(Int32 str)
        {
            EnumOrdIdState typef = new EnumOrdIdState();
            typef = (EnumOrdIdState)Enum.ToObject(typeof(EnumOrdIdState), str);
            return Enum.GetName(typeof(EnumOrdIdState), str);
        }


        /// <summary>
        /// 返回资金状态名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEnumTypesFinanceName(Int32 str)
        {
            EnumTypesFinance typef = new EnumTypesFinance();
            typef = (EnumTypesFinance)Enum.ToObject(typeof(EnumTypesFinance), str);

            return Enum.GetName(typeof(EnumTypesFinance), str);

            /*
            EnumTypesFinance typef = new EnumTypesFinance();
            typef = (EnumTypesFinance)Enum.ToObject(typeof(EnumTypesFinance), 3);

           return  Enum.GetName(typeof(EnumTypesFinance), 3);
             * 
             * 
             */



        }


        /// <summary>
        /// 返回支付的返回代码  快捷充值和普通交易的代码是两套？？？
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetReturnCode(Int32 str)
        {


            EnumCode typef = new EnumCode();
            typef = (EnumCode)Enum.ToObject(typeof(EnumCode), str);

            return Enum.GetName(typeof(EnumCode), str);
        }



        /// <summary>
        /// 返回短信代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSMSType(Int32 str)
        {
            EnumSMSType typef = new EnumSMSType();
            typef = (EnumSMSType)Enum.ToObject(typeof(EnumSMSType), str);

            return Enum.GetName(typeof(EnumSMSType), str);
        }








        /// <summary>
        /// 将枚举类转换成datatble
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumList(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("参数不是枚举类型。");
            }
            DataTable table = new DataTable();
            try
            {
                table.Columns.Add("Text", typeof(string));
                table.Columns.Add("Value", typeof(string));
                Type attributeType = typeof(DescriptionAttribute);
                foreach (FieldInfo info in enumType.GetFields())
                {
                    if (info.FieldType.IsEnum)
                    {
                        DataRow row = table.NewRow();
                        row["Value"] = Convert.ToString((int)enumType.InvokeMember(info.Name, BindingFlags.GetField, null, null, null));
                        object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                        if (customAttributes.Length > 0)
                        {
                            DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                            row["Text"] = attribute.Description;
                        }
                        else
                        {
                            row["Text"] = info.Name;
                        }
                        table.Rows.Add(row);
                    }
                }
            }
            catch
            {
            }
            return table;
        }

        /// <summary>
        /// 将枚举类转换成List
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValues> GetEnumToList(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("参数不是枚举类型。");
            }

            List<KeyValues> list = new List<KeyValues>();
            try
            {
                Type attributeType = typeof(DescriptionAttribute);
                foreach (FieldInfo info in enumType.GetFields())
                {
                    if (info.FieldType.IsEnum)
                    {
                        KeyValues kv = new KeyValues();
                        kv.key = Convert.ToString((int)enumType.InvokeMember(info.Name, BindingFlags.GetField, null, null, null));
                        object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                        if (customAttributes.Length > 0)
                        {
                            DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                            kv.value = attribute.Description;
                        }
                        else
                        {
                            kv.value = info.Name;
                        }
                        list.Add(kv);
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        /// <summary>
        /// 获取登录验证校验码
        /// </summary>
        /// <returns></returns>
        public static string getSessioncode()
        {
            string str = null;

            if (HttpContext.Current.Session["codeno"] != null)
            {

                str = HttpContext.Current.Session["codeno"].ToString();

            }

            return str;

        }



        /// <summary>
        /// 设置登录验证校验码
        /// </summary>
        /// <returns></returns>
        public static string SetSessioncode()
        {

            HttpContext.Current.Session["codeno"] = HttpContext.Current.Session.SessionID;


            return HttpContext.Current.Session["codeno"].ToString();
        }


        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decoder(string str)
        {
            byte[] outputb = Convert.FromBase64String(str);
            string orgStr = Encoding.Default.GetString(outputb);
            return orgStr;

        }


        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encoder(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            string orgStr = Convert.ToBase64String(bytes);
            return orgStr;

        }




        /// <summary>
        /// 获取邮件模版类别 0邮件  1短信
        /// </summary>
        /// <param name="SEtype"></param>
        /// <returns></returns>
        public static DataTable GetMSMEmail(int SEtype)
        {
            DataTable dt = new DataTable();

            string sne = "dtmsmemail" + SEtype.ToString();
            dt = (DataTable)DataCache.GetCache(sne);
            try
            {
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    dt = DbHelperSQL.GET_DataTable_List("SELECT SmsEmailId,SmsEname,SEContext FROM hx_td_SMSEmail where SEtype=" + SEtype.ToString());

                    DataCache.SetCache(sne, dt);
                }
            }
            catch
            {

            }

            return dt;
        }


        /// <summary>
        /// 获取短信或邮件内容 获取邮件模版类别 0邮件  1短信
        /// </summary>
        /// <param name="SmsEmailId"></param>
        /// <returns></returns>
        public static string GetMSMEmailContext(int SmsEmailId, int SEtype)
        {
            string str = "";
            DataTable dt = GetMSMEmail(SEtype);

            DataRow[] yDR = dt.Select("SmsEmailId=" + SmsEmailId.ToString());
            foreach (DataRow dr in yDR)
            {


                str = dr[2].ToString();
            }

            return str;

        }

        /// <summary>
        /// 过滤所有html标签
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTML(string source)
        {

            try
            {
                string result;
                result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("'", " ");
                result = result.Replace("\t", string.Empty);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*td([^>])*>", "\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*br( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*li( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]*>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&nbsp;", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&bull;", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&lsaquo;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&rsaquo;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&trade;", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&frasl;", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @">", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&copy;", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&reg;", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&(.{2,6});", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\t)", "\t\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\r)", "\t\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\t)", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                string breaks = "\r\r\r";
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
                return result;
            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string ParseTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr,
               "<[^>]*>", "");
        }



        /// <summary>
        /// 获取首页友情链接图片 和文字链接  0 文字  1图片
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetLinks(int ttype, int top)
        {
            StringBuilder str = new StringBuilder();
            string str1;
            string sql = "";
            string CacheKey = "td_web_Links-" + ttype.ToString() + top.ToString();
            if (ttype == 0)
            {


                object objModel = DataCache.GetCache(CacheKey);
                if (objModel != null)
                {
                    str1 = objModel.ToString();

                }
                else
                {

                    DataTable dt = new DataTable();
                    sql = "select  top " + top.ToString() + " Linkname,LinkUrl from hx_td_Links where LinkType=" + ttype + " and Linkstate=0 order by Linkid desc";
                    dt = DbHelperSQL.GET_DataTable_List(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string linkName = dt.Rows[i]["Linkname"].ToString();
                        string relNofollow = (linkName == "理财资讯" || linkName == "网上投资理财") ? "" : "rel =\"nofollow\"";
                        str.Append("<a href=\"" + dt.Rows[i]["LinkUrl"].ToString() + "\" target=\"_blank\" " + relNofollow + ">" + linkName + "</a>");
                        // rel=\"nofollow\"
                        // str.Append("<li><a href=\"" + dt.Rows[i]["LinkUrl"].ToString() + "\" target=\"_blank\" >" + dt.Rows[i]["Linkname"].ToString() + "</a></li>");

                    }
                    int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                    DataCache.SetCache(CacheKey, str.ToString(), DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    str1 = str.ToString();
                    str.Clear();

                }


            }
            else
            {
                object objModel = DataCache.GetCache(CacheKey);
                if (objModel != null)
                {
                    str1 = objModel.ToString();

                }
                else
                {

                    DataTable dt = new DataTable();
                    sql = "select  top " + top.ToString() + " Linkname,LinkUrl,LinkLogo from hx_td_Links where LinkType=" + ttype + " and Linkstate=0 order by Linkid desc";
                    dt = DbHelperSQL.GET_DataTable_List(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str.Append("<li><a href=\"" + dt.Rows[i]["LinkUrl"].ToString() + "\" target=\"_blank\" rel=\"nofollow\"><img src=\"" + Utils.GetImage_url(dt.Rows[i]["LinkLogo"].ToString().Replace("//", "/")) + "\" width=\"53.19\" height=\"28.05\"  alt=\"" + dt.Rows[i]["Linkname"].ToString() + "\" /></a></li>");

                    }
                    int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                    DataCache.SetCache(CacheKey, str.ToString(), DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    str1 = str.ToString();
                    str.Clear();
                }


            }




            return str1;

        }


        /// <summary>
        /// 获取首页活动推荐广告
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTopcomm()
        {

            DataTable dt = new DataTable();
            string CacheKey = "td_web_Ad_type-4indextop";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel != null)
            {
                dt = (DataTable)objModel;
            }
            else
            {

                string sql = "select  top 1 AdPath,AdLink from hx_td_Ad where  AdState=0 and AdTypeId=4 order by AdTypeId desc";
                dt = DbHelperSQL.GET_DataTable_List(sql);

                int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                DataCache.SetCache(CacheKey, dt, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);

            }

            return dt;

        }


        /// <summary>
        /// 广告圆点
        /// </summary>
        /// <param name="adtypid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string IndexWebAd(int adtypid, int top)
        {


            StringBuilder str = new StringBuilder();

            DataTable dt = new DataTable();
            string sql = "select  top " + top.ToString() + " AdPath,AdLink from hx_td_Ad where  AdState=0 and AdTypeId=" + adtypid + " order by Adid desc";

            dt = DbHelperSQL.GET_DataTable_List(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    str.Append("<li class=\"on\"></li>");
                }
                else
                {
                    str.Append("<li></li>");
                }
            }

            return str.ToString();

        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="adtypid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string GetWebAD(int adtypid, int top)
        {
            string strad = "";


            string CacheKey = "td_web_Ad_type-" + adtypid.ToString() + "-" + top.ToString();
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel != null)
            {
                strad = objModel.ToString();

            }
            else
            {
                StringBuilder str = new StringBuilder();
                DataTable dt = new DataTable();
                string sql = "select  top " + top.ToString() + " AdPath,AdLink,AdName from hx_td_Ad where  AdState=0 and AdTypeId=" + adtypid + " order by Adid desc";
                dt = DbHelperSQL.GET_DataTable_List(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (adtypid == 1)
                    {
                        str.Append("<li style=\"background:url(" + Utils.GetImage_url(dt.Rows[i]["AdPath"].ToString()) + ") 50% 0 no-repeat;\"><a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" title=\"" + dt.Rows[i]["AdName"].ToString() + "\" style=\"width:100%; display:block;\" target=\"_blank\"></a></li>");

                    }
                    else if (adtypid == 2)
                    {
                        // str.Append("<div style=\"background:url(" + dt.Rows[i]["AdPath"].ToString() + ") no-repeat center center; width:100%; height:120px;\"><a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" style=\"width:100%; height:120px; display:block;\" target=\"_blank\"></a></div>");

                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" target=\"_blank\"><img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\" width=\"980\" height=\"165\"   title=\"" + dt.Rows[i]["AdName"].ToString() + "\" /></a>");

                    }
                    else if (adtypid == 3)
                    {
                        str.Append("<div style=\"width:100%; height:110px; background:url(" + dt.Rows[i]["AdPath"].ToString() + ") no-repeat center center;\"><a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" style=\"width:100%; height:120px; display:block;\"  title=\"" + dt.Rows[i]["AdName"].ToString() + "\"  target=\"_blank\"></a></div>");
                    }
                    else if (adtypid == 5)
                    {
                        //2016 09 28 更改将连接取消跳转
                        str.Append("<a href='javascript:void(0);'> <img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\" width=\"980\" height=\"203\"    title=\"" + dt.Rows[i]["AdName"].ToString() + "\"  /> </a>");
                    }
                    else if (adtypid == 6 || adtypid == 7)
                    {
                        //有链接跳转
                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\" width=\"980\" height=\"203\"    title=\"" + dt.Rows[i]["AdName"].ToString() + "\"  /> </a>");
                    }
                    else if (adtypid == 8)  //注册
                    {
                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\"   title=\"" + dt.Rows[i]["AdName"].ToString() + "\"    width=\"301\" height=\"370\" /> </a>");

                    }
                    else if (adtypid == 9) //登录
                    {
                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\" width=\"243\" height=\"289\"   title=\"" + dt.Rows[i]["AdName"].ToString() + "\" /> </a>");

                    }

                    else if (adtypid == 10) //登录
                    {
                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + dt.Rows[i]["AdPath"].ToString() + "\"    title=\"" + dt.Rows[i]["AdName"].ToString() + "\" /> </a>");

                    }

                    else if (adtypid == 11) //微信首页
                    {
                        str.Append("<li><a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + Utils.GetImage_url(dt.Rows[i]["AdPath"].ToString()).Replace("//A", "/A") + "\" width=\"100%\"  /> </a></li>");

                    }

                    else if (adtypid == 12 || adtypid == 13)
                    {
                        str.Append("<a href=\"" + dt.Rows[i]["AdLink"].ToString() + "\" > <img src=\"" + Utils.GetImage_url(dt.Rows[i]["AdPath"].ToString()).Replace("//A", "/A") + "\" width=\"100%\"  /> </a>");


                    }
                }
                int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                DataCache.SetCache(CacheKey, str.ToString(), DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                strad = str.ToString();

            }

            return strad;


        }


        /// <summary>
        /// 正则获取中间字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }




        /// <summary>
        /// 返回奖励状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetRewardState(string i)
        {
            string str = "";
            int n = int.Parse(i);
            switch (n)
            {
                case 0:
                    str = "未使用";
                    break;
                case 1:
                    str = "已使用";
                    break;
                case 2:
                    str = "已过期";
                    break;
                case 3:
                    str = "锁定中";
                    break;
                case 4:
                    str = "现金转账成功";
                    break;
                case 5:
                    str = "未转账";
                    break;
                default:
                    str = "未知";
                    break;
            }

            return str;
        }

        /// <summary>
        /// 连连充值状态
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetLLPayReState(int n)
        {
            string str = "";
            switch (n)
            {
                case 0:
                    str = "未支付";
                    break;
                case 1:
                    str = "充值成功";
                    break;
                case 2:
                    str = "充值失败";
                    break;

                default:
                    str = "";
                    break;
            }
            return str;

        }



        /// <summary>
        /// 连连取现汇付转账状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string Huizhuanzhong(int n)
        {
            string str = "";
            switch (n)
            {
                case 0:
                    str = "未转账";
                    break;
                case 1:        //利车贷
                    str = "转账成功";
                    break;
                case 2:        //投房贷
                    str = "转账失败";
                    break;

                default:
                    str = "";
                    break;
            }
            return str;
        }


        /// <summary>
        /// 返回首页及列表页不能贷款标的的样式 i =0 是推荐样式 1是列表样式 full 大于2加一灰色样式
        /// </summary>
        /// <param name="portype">货款类型</param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string RetPorCSS(string portype, int i, string full)
        {
            string str = "";

            string cssstr = "";

            if (i == 0)
            {
                cssstr = "re_project_box ";
            }
            else
            {
                cssstr = "project_columnh_item ";
            }

            int n = int.Parse(portype);
            switch (n)
            {
                case 2:        //创业贷
                    str = cssstr + "loan_geti";
                    break;
                case 3:        //利车贷
                    str = cssstr + "loan_che";
                    break;
                case 4:        //投房贷
                    str = cssstr + "loan_fang";
                    break;
                case 5:          //理财产品
                    str = cssstr + "loan_qiye"; ;
                    break;
                case 6:          //新手标
                    str = cssstr + "loan_novice"; ;
                    break;
                default:
                    str = cssstr + "loan_fang";
                    break;
            }


            int fulls = int.Parse(full);

            if (fulls > 2)
            {
                str = str + " full_scale";
            }



            return str;
        }



        /// <summary>fangjianmin 注释
        /// 调用远程接口 第一步注册时是传Uid 第二步第一次投资 Uit传的投资金额  step 1是第一步 2是第二步
        /// </summary>
        /// <param name="url"></param>
        public static string GetCoopAPI(string tid, string uid, int step = 1)
        {
            //string url = "http://mall.366dw.com/interface/reflection?tid=" + tid + "&step" + step.ToString() + "=" + uid + "";
            //WebRequest wRequest;
            //WebResponse wResponse;
            string r = "";
            //try
            //{
            //    wRequest = WebRequest.Create(url);
            //    wResponse = wRequest.GetResponse();
            //    Stream stream = wResponse.GetResponseStream();
            //    StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);
            //    r = reader.ReadToEnd();   //url返回的值 
            //    wResponse.Close();
            //}
            //catch
            //{
            //}
            //finally
            //{

            //}
            return r;
        }



        /// <summary>
        /// 提交参数
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <param name="dataEncode"></param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode, bool isApi = false)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                //
                // { "appId": 123456, "appSecret": "123456", "accessToken": "294b8a0fe2ffaa85243bdb2b4ff94fe9", "timeStamp": "20160524112856", "specialStamp": "sample string 4" }
                if (isApi)
                {
                    string appid = "123456";
                    string appSecret = "123456";
                    string timeStamp = DateTime.Now.TimeToTimeStamp() + "";
                    var dic = new Dictionary<string, string>
                        {
                            {"appId", appid},
                            {"appSecret", appSecret},
                            {"timeStamp",timeStamp}
                        };
                    var sign = HttpHelper.GetAccessToken(dic, "", "4A56BA9059D42BB07C72C9D368934FBD");
                    webReq.Headers.Add("appId", appid);
                    webReq.Headers.Add("appSecret", appSecret);
                    webReq.Headers.Add("accessToken", sign);
                    webReq.Headers.Add("timeStamp", timeStamp);
                }
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            LogInfo.WriteLog(ret);
            return ret;
        }




        /// <summary>
        /// 获取远程地址
        /// </summary>
        /// <param name="urlfilename"></param>
        /// <returns></returns>
        public static string GetRemote_url(string urlfilename)
        {
            return (ConfigurationManager.AppSettings["Remote_url"].ToString() + urlfilename);
        }


        /// <summary>
        /// 汇付企业专属帐户2
        /// </summary>
        /// <returns></returns>
        public static string GetMERDT2()
        {
            return ConfigurationManager.AppSettings["MERDT2"].ToString();
        }



        /// <summary>
        /// 手机转账接口返回 /
        /// </summary>
        /// <returns></returns>
        public static string GetActRemote_url(string urlfilename)
        {
            return ConfigurationManager.AppSettings["ActRemote_url"].ToString() + urlfilename;
        }



        #region 将DataTable转换为实体对象 + static List<T> DataTableFillObject<T>(DataTable dt) 
        /// <summary>
        /// 将DataTable转换为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableFillObject<T>(DataTable dt)
        {
            if (dt == null)
                return null;
            List<T> result = new List<T>();
            Type type = typeof(T);
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    T t = Activator.CreateInstance<T>();
                    PropertyInfo[] ps = type.GetProperties();
                    FieldInfo[] fs = type.GetFields();
                    foreach (PropertyInfo p in ps)
                    {
                        p.SetValue(t, row[p.Name], null);
                    }
                    foreach (FieldInfo f in fs)
                    {

                        f.SetValue(t, row[f.Name]);
                    }
                    result.Add(t);
                }
                catch { }
            }
            return result;
        }
        #endregion


        #region Model对象字符型安全过滤 +static object ValidateModelClass(object entity) 
        /// <summary>
        /// Model对象字符型安全过滤
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <returns></returns>
        public static object ValidateModelClass(object entity)
        {
            if (entity != null)
            {
                System.Type t = entity.GetType();
                System.Reflection.MemberInfo[] memberInfot = t.GetMembers();
                foreach (System.Reflection.MemberInfo var in memberInfot)
                {
                    if (var.MemberType == MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo propertyInfo = entity.GetType().GetProperty(var.Name);
                        if (propertyInfo.PropertyType.Name.ToLower() == "string")
                        {
                            try
                            {
                                string val = propertyInfo.GetValue(entity, null).ToString();
                                if (val.Length > 0)
                                {
                                    if (val.Contains("/") || val.Contains("-"))
                                    {
                                        propertyInfo.SetValue(entity, CheckSQL(val));
                                    }
                                    else
                                    {
                                        propertyInfo.SetValue(entity, CheckSQLHtml(val));
                                    }
                                }

                            }
                            catch
                            {
                                propertyInfo.SetValue(entity, "");
                            }

                        }
                    }

                }

            }
            return entity;
        }

        #endregion



        #region 由于win10系统获取到的时间会带上星期几 上午下午，用此方式重新定义一下格式便于测试+void SetSYSDateTimeFormat()
        /// <summary>
        /// 由于win10系统获取到的时间会带上星期几 上午下午，用此方式重新定义一下格式便于测试
        /// </summary>
        public static void SetSYSDateTimeFormat()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;

        }
        #endregion



        #region 将传入的字符串中间部分字符替换成特殊字符 +static string ReplaceWithSpecialChar(string value, int startLen = 4, int endLen = 4, char specialChar = '*') 
        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符如身份证号
        /// </summary>
        /// <param name="value">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="replaceChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        public static string ReplaceWithSpecialChar(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            try
            {
                int lenth = value.Length - startLen - endLen;
                string replaceStr = value.Substring(startLen, lenth);
                string specialStr = string.Empty;
                for (int i = 0; i < replaceStr.Length; i++)
                {
                    specialStr += specialChar;
                }
                value = value.Replace(replaceStr, specialStr);
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        #endregion




        #region 返回日期时间格式 static string GetDateFormat(DateTime dtime)
        /// <summary>
        /// 返回揭晓倒计时日期时间格式
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static string GetDateFormat(DateTime dtime)
        {
            return dtime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion


        #region 判断是否为日期时间 + bool IsDate(string strDate)
        /// <summary>
        /// 判断是否为日期时间
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion


        #region 获取时间格式至毫秒 + static string GetDateFF(DateTime dtime)
        /// <summary>
        /// 获取时间格式至毫秒
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static string GetDateFF(DateTime dtime)
        {
            string str = "";
            try
            {
                str = dtime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch
            {
                str = "";
            }

            return str;

        }

        #endregion

        #region 缓存，也用于防汇付第三方并发通知    服务器缓存 3分钟 +static void SetThirdCache(string CacheName)
        /// <summary>
        /// 缓存，也用于防汇付第三方并发通知    服务器缓存 3分钟
        /// </summary>
        /// <param name="CacheName"></param>
        public static void SetThirdCache(string CacheName)
        {
            long str = DateTime.Now.Ticks;
            SetThirdCacheName(CacheName, str, 3);
        }
        #endregion

        #region 服务器缓存 +static void SetThirdCache(string CacheName)
        /// <summary>
        ///  服务器缓存
        /// </summary>
        /// <param name="CacheName">缓存健值</param>
        /// <param name="value1">缓存value值</param>
        /// <param name="mm">缓存时间，默认3分钟</param>
        public static void SetThirdCacheName(string CacheName, object value1, int mm = 3)
        {
            string key = CacheName;
            if (HttpRuntime.Cache[key] == null)
            {
                HttpRuntime.Cache.Add(key, value1, null, DateTime.Now.AddMinutes(mm), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            }
        }
        #endregion

        #region 获取缓存汇付第三方并发通知 防并发 服务器缓存 3分钟 + static long GeTThirdCache(string CacheName)
        /// <summary>
        /// 缓存汇付第三方并发通知 防并发 服务器缓存 3分钟
        /// </summary>
        /// <param name="CacheName">用户ID+操作项id+订单号</param>
        /// <returns>返回0时说明没有缓存</returns>
        public static long GeTThirdCache(string CacheName)
        {
            long str = 0;
            string key = CacheName;
            if (HttpRuntime.Cache[key] == null)
            {
                return 0;
            }
            else
            {
                str = (long)HttpRuntime.Cache[key];
            }
            return str;
        }
        #endregion

        public static string GeTThirdCacheName(string CacheName)
        {
            string str = "";
            string key = CacheName;
            if (HttpRuntime.Cache[key] == null)
            {
                return "";
            }
            else
            {
                str = (string)HttpRuntime.Cache[key];
            }
            return str;
        }
        public static object GetThirdCacheObject(string CacheName)
        {
            return HttpRuntime.Cache[CacheName];
        }

        #region 获取投资进度百分比+static decimal GetPercentage(decimal borrowing_balance, decimal fundraising_amount)
        /// <summary>
        /// 获取投资进度百分比
        /// </summary>
        /// <param name="borrowing_balance">标的借款金额</param>
        /// <param name="fundraising_amount">已筹款金额</param>
        /// <returns></returns>
        public static decimal GetPercentage(decimal borrowing_balance, decimal fundraising_amount)
        {
            decimal Percentage = 0.00M;
            try
            {
                Percentage = fundraising_amount / borrowing_balance * 100;
            }
            catch
            {
                Percentage = 0.00M;
            }
            return decimal.Parse(Percentage.ToString("0.00"));
        }
        #endregion

        /// <summary>
        /// 获取投标差额
        /// </summary>
        /// <param name="borrowing_balance">标的借款金额</param>
        /// <param name="fundraising_amount">已筹款金额</param>
        /// <returns></returns>
        public static decimal GetDifference(decimal borrowing_balance, decimal fundraising_amount)
        {
            decimal Difference = 0.00M;
            try
            {
                Difference = borrowing_balance - fundraising_amount;
            }
            catch
            {
                Difference = 0.00M;
            }
            return Difference;
        }


        #region 判断奇偶数+static bool IsOdd(int n)
        /// <summary>
        /// 判断奇偶数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsOdd(int n)
        {
            return Convert.ToBoolean(n % 2);
        }
        #endregion


        #region 随机活动从数组中随机取一个字
        /// <summary>
        /// 从数组中随机取一个字
        /// </summary>
        /// <param name="str"></param>       
        /// <returns></returns>
        public static decimal GetActRandom(string str)
        {
            decimal amt = 0.00M;
            Random rand = new Random();
            string[] sArray = null;
            if (str.Length > 0)
            {
                sArray = Regex.Split(str, ",", RegexOptions.IgnoreCase);
            }
            int index = rand.Next(0, sArray.Length);
            try
            {
                amt = decimal.Parse(sArray[index].ToString());
            }
            catch
            {
                amt = 0.00M;
            }
            return amt;
        }
        #endregion





        #region 取得某月的第一天
        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }
        #endregion

        #region 取得某月的最后一天
        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }
        #endregion

        #region 取得上个月第一天
        //// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }
        #endregion

        #region 取得上个月的最后一天
        //// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }
        #endregion

        #region 活动网站活动状态+static string GetActState(int i)
        /// <summary>
        /// 活动网站活动状态
        /// </summary>
        /// <param name="i">状态id</param>
        /// <returns></returns>
        public static string GetActState(int i)
        {
            string str = "未上线";
            switch (i)
            {
                case -1:
                    str = "已删除";
                    break;
                case 0:
                    str = "未上线";
                    break;
                case 1:
                    str = "进行中(上线)";
                    break;
                case 2:
                    str = "结束(下线)";
                    break;
                case 3:
                    str = "停止";
                    break;
                default:
                    str = "未上线";
                    break;
            }

            return str;
        }
        #endregion




        #region 活动奖励类型
        /// <summary>
        /// 活动奖励类型
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetRewType(int i)
        {
            string str = "未上线";
            switch (i)
            {
                case 0:
                    str = "未知";
                    break;
                case 1:
                    str = "现金";
                    break;
                case 2:
                    str = "抵扣券";
                    break;
                case 3:
                    str = "加息券";
                    break;
                default:
                    str = "未知";
                    break;
            }

            return str;
        }


        #endregion



        #region 用户登录状态
        /// <summary>
        /// 用户登录状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string loginState(int i)
        {
            string str = "未上线";
            switch (i)
            {
                case 0:
                    str = "成功  ";
                    break;
                case 1:
                    str = "失败";
                    break;
                case 2:
                    str = "禁止登录";
                    break;

                default:
                    str = "失败";
                    break;
            }

            return str;
        }

        #endregion


        #region 用户登录来源
        /// <summary>
        /// 用户登录状态
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string loginsource(int i)
        {
            string str = "未知";
            switch (i)
            {
                case 0:
                    str = "PC";
                    break;
                case 1:
                    str = "微信";
                    break;
                case 2:
                    str = "android";
                    break;
                case 3:
                    str = "IOS";
                    break;

                default:
                    str = "未知";
                    break;
            }

            return str;
        }

        #endregion



        #region 微信用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UsrName"></param>
        /// <param name="AuthCookie"></param>
        public static void LoginWriteSession(string uid, string UsrName, bool AuthCookie = false)
        {

            string tempstr = uid + "|" + UsrName;

            string uids = DESEncrypt.Encrypt(tempstr, ConfigurationManager.AppSettings["webp"].ToString());
            //设置用户的 cookie 的值
            FormsAuthentication.SetAuthCookie(uids, true, FormsAuthentication.FormsCookiePath);
            //获取用户的 cookie 
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(uids, AuthCookie);
            //给用户的 cookie 的值加上 cookie 的域 和 过期日期
            //向客户端重写同名的 用户 cookie
            FormsAuthenticationTicket oldTicket = FormsAuthentication.Decrypt(cookie.Value);
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(1,
            oldTicket.Name,
            oldTicket.IssueDate,
            DateTime.Now.AddMonths(12),
            oldTicket.IsPersistent,
            oldTicket.UserData,
            FormsAuthentication.FormsCookiePath);
            //cookie.Domain = "jiajubuy.cn";
            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);


            /*
            FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, uids, DateTime.Now, DateTime.Now.AddYears(1), true, ""); 

            HttpCookie cookie = new HttpCookie(".CL2301");

            cookie.Value = FormsAuthentication.Encrypt(fat);

            cookie.Expires = fat.Expiration;

            cookie.Domain = "jiajubuy.cn";  // Highlight 

            HttpContext.Current.Response.Cookies.Add(cookie);
            */

        }
        #endregion

        #region 获取cookie 对象
        /// <summary>
        /// 获取cookie 对象
        /// </summary>
        /// <returns></returns>
        public static string[] getUserObject()
        {
            string[] dd = null;

            if (HttpContext.Current.User.Identity.Name != null && HttpContext.Current.User.Identity.Name.Length > 0)
            {
                string Usrname = HttpContext.Current.User.Identity.Name;

                Usrname = DESEncrypt.Decrypt(Usrname, ConfigurationManager.AppSettings["webp"].ToString());

                dd = Usrname.Split('|');
            }

            return dd;

        }
        #endregion



        #region 检查微信登录是否成功,成功返回id 
        /// <summary>
        /// 检查微信登录是否成功,成功返回id 
        /// </summary>
        /// <returns></returns>
        public static int checkwxloginsessiontop()
        {
            int uid = 0;

            string[] usrs = getUserObject();
            try
            {
                if (usrs.Length > 0)
                {
                    uid = int.Parse(usrs[0].ToString());
                }

            }
            catch
            {
                uid = 0;
            }
            return uid;
        }
        #endregion




        #region 获取微信登录用户名
        /// <summary>
        /// 获取微信登录用户名
        /// </summary>
        /// <returns></returns>
        public static string GetWXUserNameCookies()
        {
            string str = "";
            string[] usrs = getUserObject();
            try
            {
                if (usrs.Length > 0)
                {
                    str = usrs[1].ToString();
                }
            }
            catch
            {
                str = "";
            }
            return str;

        }
        #endregion







        #region 获取微信头部称呼
        /// <summary>
        /// 获取微信头部称呼
        /// </summary>
        /// <returns></returns>
        public static string GetWXUsrNameSex(Int64 uid)
        {
            string str = "";
            if (uid > 0)
            {
                string cachename = uid + "namewx";
                if (Utils.GeTThirdCacheName(cachename) == "")
                {
                    DataTable dt = DbHelperSQL.GET_DataTable_List("select top 1 username,realname  ,iD_number   FROM  hx_member_table  where  registerid=" + uid.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        string rname = dt.Rows[0]["realname"].ToString();
                        if (rname != null && rname != "")
                        {
                            string sf = ChinaIDCard.Getgender(dt.Rows[0]["iD_number"].ToString());
                            if (sf != "")
                            {
                                str = str + rname.Substring(0, 1);
                                str = str + sf;
                            }
                            else
                            {
                                str = dt.Rows[0]["username"].ToString();
                            }
                        }
                        else
                        {
                            str = dt.Rows[0]["username"].ToString();
                        }
                    }
                    Utils.SetThirdCacheName(cachename, str, 30);
                }
                else
                {
                    str = Utils.GeTThirdCacheName(cachename);
                }
            }
            return str;
        }
        #endregion



        #region 更新过期的优惠券
        /// <summary>
        /// 更新过期的优惠券
        /// </summary>
        public static void UpdateUserAct()
        {
            if (HttpContext.Current.Request.Cookies["yhj"] == null)
            {
                DbHelperSQL.RunSql("update hx_UserAct  set UseState=2   where convert(varchar(10),AmtEndtime,120)< convert(varchar(10),GETDATE(),120)  and UseState=0 ");
                HttpContext.Current.Response.Cookies["yhj"].Value = "update";
                HttpContext.Current.Response.Cookies["yhj"].Expires = DateTime.Now.AddDays(1);
            }




        }
        #endregion


        #region 渠道来源
        /// <summary>
        /// 渠道来源
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetChannelsource(int i)
        {
            string str = "";

            switch (i)
            {
                case 0:
                    str = "pc注册";
                    break;
                case 1:
                    str = "pc端好友邀请";
                    break;
                case 2:
                    str = "微信注册";
                    break;
                case 3:
                    str = "微信端好友邀请";
                    break;
                case 4:
                    str = "IOS客户端";
                    break;
                case 5:
                    str = "安卓客户端";
                    break;
                default:
                    str = "其它";
                    break;
            }

            return str;
        }

        #endregion

        /// <summary>
        /// 获取用户省信息
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetHashtable()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("11", "北京市");
            hashtable.Add("12", "天津市");
            hashtable.Add("13", "河北省");
            hashtable.Add("14", "山西省");
            hashtable.Add("15", "内蒙古自治区");
            hashtable.Add("21", "辽宁省");
            hashtable.Add("22", "吉林省");
            hashtable.Add("23", "黑龙江省");
            hashtable.Add("31", "上海市");
            hashtable.Add("32", "江苏省");
            hashtable.Add("33", "浙江省");
            hashtable.Add("34", "安徽省");
            hashtable.Add("35", "福建省");
            hashtable.Add("36", "江西省");
            hashtable.Add("37", "山东省");
            hashtable.Add("41", "河南省");
            hashtable.Add("42", "湖北省");
            hashtable.Add("43", "湖南省");
            hashtable.Add("44", "广东省");
            hashtable.Add("45", "广西壮族自治区");
            hashtable.Add("46", "海南省");
            hashtable.Add("50", "重庆市");
            hashtable.Add("51", "四川省");
            hashtable.Add("52", "贵州省");
            hashtable.Add("53", "云南省");
            hashtable.Add("54", "西藏自治区");
            hashtable.Add("61", "陕西省");
            hashtable.Add("62", "甘肃省");
            hashtable.Add("63", "青海省");
            hashtable.Add("64", "宁夏回族自治区");
            hashtable.Add("65", "新疆维吾尔自治区");
            return hashtable;
        }


        /// <summary>
        ///  2 个class实体验证数据是否一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newModel"></param>
        /// <param name="oldModel"></param>
        /// <returns></returns>
        public static bool CompareClass<T>(T newModel, T oldModel)
        {
            System.Reflection.PropertyInfo[] mPi = typeof(T).GetProperties();
            for (int i = 0; i < mPi.Length; i++)
            {
                System.Reflection.PropertyInfo pi = mPi[i];
                var oldValue = pi.GetValue(oldModel, null).ToString();
                var newValue = pi.GetValue(newModel, null).ToString();
                if (oldValue != newValue)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取标的持续时间。(单位：天)
        /// </summary>
        /// <param name="startTime">标的开始时间(时间格式:yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="endTime">标的结束时间(时间格式:yyyy-MM-dd HH:mm:ss)</param>
        /// <returns>持续多少天</returns>
        public static int GetTargetDurationDays(string startTime, string endTime)
        {
            if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                return -1;
            DateTime startT = Convert.ToDateTime(startTime.Split(' ')[0]);
            DateTime endT = Convert.ToDateTime(endTime.Split(' ')[0]);
            return Convert.ToInt32((endT - startT).TotalDays);
        }

        public static bool IsNewAppVersion(string clientVersion, string serverVersion)
        {
            if (clientVersion.Equals(serverVersion)) return false;
            clientVersion = clientVersion.Trim('.');
            serverVersion = serverVersion.Trim('.');
            string[] cVersion = clientVersion.Split('.');
            string[] sVersion = serverVersion.Split('.');
            var cLength = cVersion.Length;
            var sLength = sVersion.Length;
            var maxLength = sLength >= cLength ? sLength : cLength;
            for (int i = 0; i < maxLength; i++)
            {
                if (i < cLength && i < sLength)
                {
                    var cTemp = Convert.ToInt64(cVersion[i]);
                    var sTemp = Convert.ToInt64(sVersion[i]);
                    if (cTemp > sTemp)
                    { return false; }
                    if (cTemp < sTemp)
                    { return true; }
                }
                else if (cLength > sLength) return false;
            }
            return true;
        }

        /// <summary>
        /// 针对ChuanglitouP2P.BLL.EF.ActFacade类,提供的 移动设备来源匹配值.为了做到不同的移动设备，可以参与的活动也是不同的.
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public static string GetDevicePlatformCode(string deviceCode)
        {
            if (deviceCode == "123456")
                return EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.ios;
            else if (deviceCode == "654321")
                return EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.android;
            else
                return EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.all;

        }

        //public string GetSafeCodeIOS
        //{
        //    get
        //    {
        //        return Settings.Instance.GetCachingValue<string>("OpenApi_SafeCodeIOS", () =>
        //        {
        //            return DbHelperSQL.Re_String("select AppSafeCode from [ApplicationAuthorization] where AppId=123456");
        //        },1440);
        //    }
        //}
        //public string GetSafeCodeAndroid
        //{
        //    get
        //    {
        //        return Settings.Instance.GetCachingValue<string>("OpenApi_SafeCodeIOS", () =>
        //        {
        //            return DbHelperSQL.Re_String("select AppSafeCode from [ApplicationAuthorization] where AppId=654321");
        //        },1440);
        //    }
        //}

        /// <summary>
        /// 获取最大有效的金额
        /// <remark>当金额为10.00时，返回10；当金额为10.10时，返回10.1；当金额为10.101时，返回10.1</remark>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMaximumDecimalValue(Decimal? value)
        {
            if (value == null)
                return "0";
            else
            {
                decimal valueTemp = GetChinaMoney(value.Value);
                string valueString = valueTemp.ToString();
                string result = string.Empty;
                int zeroCount = 0;
                for (int i = valueString.Length - 1; i > 0; i--)
                {
                    var curChar = valueString[i];
                    if (curChar == '.')
                    {
                        zeroCount++;
                        break;
                    }
                    else if (Convert.ToInt32(curChar.ToString()) == 0)
                    {
                        zeroCount++;
                    }
                    else
                        break;
                }
                return valueString.Substring(0, valueString.Length - zeroCount);
            }
        }

        /// <summary>
        /// 获取中文方式下的货币显示方式
        /// <remark>默认保留2位小数。且使用四舍五入规则进位</remark>
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static Decimal GetChinaMoney(Decimal money)
        {
            return Math.Round(money, 2, MidpointRounding.AwayFromZero);
        }
    }
}
