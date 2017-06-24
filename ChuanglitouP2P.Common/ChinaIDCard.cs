using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 身份证操作类
    /// </summary>
    public  class ChinaIDCard
    {
        #region 18位身份证号码验证


        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
        public static bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        #endregion

        #region 15位身份证号码验证


        /// <summary>  
        /// 15位身份证号码验证  
        /// </summary>  
        public static bool CheckIDCard15(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }
        #endregion

        #region 身份证验证
        
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="ic"></param>
        /// <param name="gender"></param>
        /// <param name="age"></param>
        /// <param name="birth"></param>
        public static void GetInfo(string ic, out string gender, out int age, out string birth)
        {
            birth = "";
            age = 1;

            string sex = "";
            var myDate = DateTime.Now;
            var month = myDate.Month;
            var day = myDate.Day;
            if (CheckIDCard18(ic))
            {
                if (ic.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
                {
                    birth = ic.Substring(6, 4) + "-" + ic.Substring(10, 2) + "-" + ic.Substring(12, 2);
                    sex = ic.Substring(14, 3);
                    age = myDate.Year - int.Parse(ic.Substring(6, 4)) - 1;
                    if (int.Parse(ic.Substring(10, 2)) < month || int.Parse(ic.Substring(10, 2)) == month && int.Parse(ic.Substring(12, 2)) <= day)
                    {
                        age++;
                    }
                }
            }

            if (CheckIDCard15(ic))
            {
                if (ic.Length == 15)
                {
                    birth = "19" + ic.Substring(6, 2) + "-" + ic.Substring(8, 2) + "-" + ic.Substring(10, 2);
                    sex = ic.Substring(12, 3);
                    age = myDate.Year - int.Parse("19" + ic.Substring(6, 2)) - 1;
                    if (int.Parse(ic.Substring(6, 2)) < month || int.Parse(ic.Substring(8, 2)) == month && int.Parse(ic.Substring(10, 2)) <= day)
                    {
                        age++;
                    }
                }

            }
            gender = int.Parse(sex) % 2 == 0 ? "女" : "男";//1代表男性，2代表女性     
        }
        #endregion



        #region 获取身份证性别
        /// <summary>
        /// 获取身份证性别
        /// </summary>
        /// <param name="ic"></param>
        /// <returns></returns>

        public static string Getgender(string ic)
        {
            string gender = "先生";
            string sex = "";

            if (ic.Length > 0)
            {
                if (CheckIDCard18(ic))
                {
                    if (ic.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
                    {
                        sex = ic.Substring(14, 3);
                    }
                }

                if (CheckIDCard15(ic))
                {
                    if (ic.Length == 15)
                    {
                        sex = ic.Substring(12, 3);

                    }

                }
                try
                {
                    gender = int.Parse(sex) % 2 == 0 ? "女士" : "先生";  //1代表男性，2代表女性  
                }catch
                {
                    gender = "";
                }
                 
            }

            
            return gender;

        } 

        #endregion



    }
}
