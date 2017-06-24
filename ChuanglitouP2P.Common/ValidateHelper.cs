using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    public class ValidateHelper
    {
        /// <summary>
        ///  验证电话号码
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");

        }


        /// <summary>
        ///  验证手机号码
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})|(17[0-9]{1})|(14[0-9]{1}))+\d{8})$");

        }



        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"\\w{1,}@\\w{1,}\\.\\w{1,}");
        }

        /// <summary>
        /// 验证邮政编码
        /// </summary>
        /// <param name="str_postcode">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPostCode(string str_postcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postcode, @"\d{6}");
        }
    }
}
