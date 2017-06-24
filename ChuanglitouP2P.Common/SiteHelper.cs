#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>PageHelper ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/22 22:23:57</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;

namespace ChuanglitouP2P.Common
{
    public static partial class PageHelper
    {
        public static MvcHtmlString ToHtml(this string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                return MvcHtmlString.Empty;
            else
                return MvcHtmlString.Create(inputText);
        }
        public static string Flash(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/static/flash/{0}?v={1}", fileName.TrimStart('/'), Settings.Instance.Guid));
        }
        public static string Image(this UrlHelper helper, string fileName)
        {
            return Settings.Instance.ImagesDomain + "/images/" + fileName;
            //return helper.Content(string.Format("~/images/{0}", fileName.TrimStart('/')));
        }
        public static string File(this UrlHelper helper, string fileName)
        {
            if (fileName.ToLowerInvariant().EndsWith(".css"))
            {
                //return helper.Content(string.Format("<link href=\"{2}/Static/css/{0}\" rel=\"stylesheet\" type=\"text/css\" />", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
                return helper.Content(string.Format("<link href=\"{2}/{0}?v={1}\" rel=\"stylesheet\" type=\"text/css\" />", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
            }
            //return helper.Content(string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{2}/Static/css/{0}\" ></script>", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
            return helper.Content(string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{2}/{0}?v={1}\" ></script>", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
        }

        public static string ApiFile(this UrlHelper helper, string fileName)
        {
            if (fileName.ToLowerInvariant().EndsWith(".css"))
            {
                return helper.Content(string.Format("<link href=\"{2}/Static/css/{0}?v={1}\" rel=\"stylesheet\" type=\"text/css\" />", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
            }
            return helper.Content(string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{2}/Static/js/{0}?v={1}\" ></script>", fileName.TrimStart('/'), Settings.Instance.SiteVersion, Settings.Instance.SiteDomain));
        }

        public static string GetWebUrl(this UrlHelper helper, string virPath)
        {
            string result = string.Empty;
            string[] temp = HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
            return result = temp[0] + "//" + temp[2] + "/" + virPath;
        }

        public static string Week(DateTime date)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            var week = weekdays[Convert.ToInt32(date.DayOfWeek)];
            return week;
        }
        #region 转换人民币大小金额

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string CmycurD(decimal num)
        {
            const string str1 = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字 
            var str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            var str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            string str4 = ((long)(num * 100)).ToString(CultureInfo.InvariantCulture);
            int j = str4.Length;
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                var str3 = str4.Substring(i, 1);    //从原num值中取出的值 
                int temp = Convert.ToInt32(str3);            //从原num值中取出的值 
                string ch1;    //数字的汉语读法 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
        /// <summary> 
        /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num) 
        /// </summary> 
        /// <param name="numstr">用户输入的金额，字符串形式未转成decimal</param> 
        /// <returns></returns> 
        public static string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "非数字形式！";
            }
        }
        #endregion
        #region 返回时间差
        public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            string dateDiff;
            try
            {
                TimeSpan ts = dateTime2 - dateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = dateTime1.Month.ToString(CultureInfo.InvariantCulture) + "月" + dateTime1.Day.ToString(CultureInfo.InvariantCulture) + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString(CultureInfo.InvariantCulture) + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString(CultureInfo.InvariantCulture) + "分钟前";
                    }
                }
            }
            catch (Exception e)
            {
                return "error";
            }
            return dateDiff;
        }
        public static string DateDiff(string dateTime1, string dateTime2)
        {
            try
            {
                var tempDate1 = DateTime.Parse(dateTime1);
                var tempDate2 = DateTime.Parse(dateTime2);
                return DateDiff(tempDate1, tempDate2);
            }
            catch (Exception e)
            {
                return "error";
            }
        }
        #endregion
    }
}
