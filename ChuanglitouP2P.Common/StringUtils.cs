using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 处理String的类
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 处理字符串，对显示字符串稍作加密，比身份证号，手机号、银行卡号等，只显示前几位和最后几位，其余部分均显示*
        /// </summary>
        /// <param name="str"></param>
        /// <param name="frontLength">前几位</param>
        /// <param name="backLength">后几位</param>
        /// <returns></returns>
        public static string MaskStr(string str, int frontLength = 3, int backLength = 3)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (frontLength >= str.Length)
                {
                    frontLength = str.Length;
                }
                sb.Append(str.Substring(0, frontLength));
                for (int i = 0; i < str.Length - frontLength - backLength; i++)
                {
                    sb.Append("*");
                }
                if (backLength >= str.Length)
                {
                    backLength = str.Length;
                }
                sb.Append(str.Substring(str.Length - backLength));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 处理邮箱，对显示邮箱稍作加密，比如abcd@163.com，处理之后变为a***@163.com
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static string MaskMail(string mail)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(mail))
            {
                Match match = Regex.Match(mail, @"^(\w)(\w*)@(.+)$");
                string first = match.Groups[1].Value;
                string second = match.Groups[2].Value;
                string domain = match.Groups[3].Value;


                sb.Append(first);
                for (int i = 0; i < second.Length; i++)
                {
                    sb.Append("*");
                }
                sb.Append("@");
                sb.Append(domain);

            }
            return sb.ToString();
        }

        /// <summary>
        /// 高效率的C#截取指定长度字符串，大于指定长度的，在末尾显示指定字符，默认为"..."
        /// 备注：C#中字符串截断本没有那么麻烦，问题就出在string.Substring()这个方法将中文也按一个字符计算，导致我们在实际应用中截取字符串（中英文组合）后的“长度”不一致。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="endShow"></param>
        /// <returns></returns>
        public static string CutStr(string str, int len, string endShow = "...")
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                int strLen = str.Length;

                if (len < strLen)
                {
                    #region 计算长度
                    int tempCutLen = 0;
                    while (tempCutLen < len && tempCutLen < strLen)
                    {
                        //每遇到一个中文，则将目标长度减一。
                        if ((int)str[tempCutLen] > 128) { len--; }
                        tempCutLen++;
                    }
                    #endregion

                    if (tempCutLen < strLen && tempCutLen > 0)
                    {
                        str = string.Format("{0}{1}", str.Substring(0, tempCutLen), endShow);
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 将数字转换为 x 位数,不够前面补0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string TwoDigits(int val, int x = 2)
        {
            if (x > 1)
            {
                int temp = Convert.ToInt32("1".PadRight(x - 1, '0'));
                if (val < temp)
                    return val.ToString().PadLeft(x, '0');
            }
            return val.ToString();
        }
    }
}
