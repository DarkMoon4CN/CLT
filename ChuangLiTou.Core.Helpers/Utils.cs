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
using System.Net;
using System.Globalization;
using System.Threading;

namespace ChuangLiTou.Core.Helpers
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {


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



    }


}
