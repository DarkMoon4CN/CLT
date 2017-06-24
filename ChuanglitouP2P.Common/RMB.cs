using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
 
      
        /// <summary>
        /// 实现将人民币的数字形式转换为中文格式，最大支持9兆兆。
        /// </summary>
        public  class RMB
        {
            #region 内部常量
            private static string RMBUppercase = "零壹贰叁肆伍陆柒捌玖";
           
            private static string RMBUnitChar = "元拾佰仟万拾佰仟亿拾佰仟兆拾佰仟万拾佰仟亿拾佰仟兆"; //人民币整数位对应的标志 
            private const decimal MaxNumber = 9999999999999999999999999.99m;
            private const decimal MinNumber = -9999999999999999999999999.99m;
            private static char[] cDelim = { '.' }; //小数分隔标识
            #endregion



            /// <summary>
            /// 格式化货币三位逗号分隔
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public static string DecimalFormat(decimal num)
            {
                string s = "";

                s = string.Format("{0:N}", num);
                 return s;
            
            }


            #region Convert
            #region Convert <decimal>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="digital"></param>
            /// <returns></returns>
            public static string Convert(decimal number)
            {
                bool NegativeFlag = false;
                decimal RMBNumber;

                CheckNumberLimit(number);

                RMBNumber = Math.Round(number, 2);    //将四舍五入取2位小数
                if (RMBNumber == 0)
                {
                    return "零元整";
                }
                else if (RMBNumber < 0)  //如果是负数
                {
                    NegativeFlag = true;
                    RMBNumber = Math.Abs(RMBNumber);           //取绝对值
                }
                else
                {
                    NegativeFlag = false;
                }

                string buf = "";                           // 存放返回结果 
                string strDecPart = "";                    // 存放小数部分的处理结果 
                string strIntPart = "";                    // 存放整数部分的处理结果 
                string[] tmp = null;
                string strDigital = RMBNumber.ToString();

                tmp = strDigital.Split(cDelim, 2); // 将数据分为整数和小数部分 


                if (RMBNumber >= 1m) // 大于1时才需要进行整数部分的转换
                {
                    strIntPart = ConvertInt(tmp[0]);
                }

                if (tmp.Length > 1) //分解出了小数
                {
                    strDecPart = ConvertDecimal(tmp[1]);
                }
                else  //没有小数肯定是为整
                {
                    strDecPart = "整";
                }

                if (NegativeFlag == false) //是否负数
                {
                    buf = strIntPart + strDecPart;
                }
                else
                {
                    buf = "负" + strIntPart + strDecPart;
                }
                return buf;
            }
            #endregion

            #region Convert <double>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Convert(double number)
            {
                decimal dec;
                try
                {
                    dec = System.Convert.ToDecimal(number);
                }
                catch
                {
                    throw new RMBException("不能转成标准的decimal类型:" + number.ToString());
                }
                return Convert(dec);
            }
            #endregion

            #region Convert <float>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Convert(float number)
            {
                decimal dec;
                try
                {
                    dec = System.Convert.ToDecimal(number);
                }
                catch
                {
                    throw new RMBException("不能转成标准的decimal类型:" + number.ToString());
                }
                return Convert(dec);
            }
            #endregion

            #region Convert <int>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Convert(int number)
            {
                decimal dec;
                dec = System.Convert.ToDecimal(number);
                return Convert(dec);
            }
            #endregion

            #region Convert <long>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Convert(long number)
            {
                decimal dec;
                dec = System.Convert.ToDecimal(number);
                return Convert(dec);
            }
            #endregion

            #region Convert <string>
            /// <summary>
            /// 转换成人民币大写形式
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Convert(string number)
            {
                decimal dec;
                try
                {
                    dec = System.Convert.ToDecimal(number, null);
                }
                catch
                {
                    throw new RMBException("不能转成标准的decimal类型:" + number);
                }
                return Convert(dec);
            }
            #endregion
            #endregion

            #region MaxSupportNumber
            /// <summary>
            /// 支持的最大转换数
            /// </summary>
            public static decimal MaxSupportNumber
            {
                get
                {
                    return MaxNumber;
                }
            }
            #endregion

            #region MinSupportNumber
            /// <summary>
            /// 支持的最小数
            /// </summary>
            public static decimal MinSupportNumber
            {
                get
                {
                    return MinNumber;
                }
            }
            #endregion

            #region 内部函数
            #region ConvertInt
            private static string ConvertInt(string intPart)
            {
                string buf = "";
                int length = intPart.Length;
                int curUnit = length;

                // 处理除个位以上的数据 
                string tmpValue = "";                    // 记录当前数值的中文形式 
                string tmpUnit = "";                    // 记录当前数值对应的中文单位 
                int i;
                for (i = 0; i < length - 1; i++, curUnit--)
                {
                    if (intPart[i] != '0')
                    {
                        tmpValue = DigToCC(intPart[i]);
                        tmpUnit = GetUnit(curUnit - 1);
                    }
                    else
                    {
                        // 如果当前的单位是"万、亿"，则需要把它记录下来 
                        if ((curUnit - 1) % 4 == 0)
                        {
                            tmpValue = "";
                            tmpUnit = GetUnit(curUnit - 1);//

                        }
                        else
                        {
                            tmpUnit = "";

                            // 如果当前位是零，则需要判断它的下一位是否为零，再确定是否记录'零' 
                            if (intPart[i + 1] != '0')
                            {

                                tmpValue = "零";

                            }
                            else
                            {
                                tmpValue = "";
                            }
                        }
                    }
                    buf += tmpValue + tmpUnit;
                }


                // 处理个位数据 
                if (intPart[i] != '0')
                    buf += DigToCC(intPart[i]);
                buf += "元";

                return CombinUnit(buf);
            }
            #endregion

            #region ConvertDecimal
            /// <summary>
            /// 小数部分的处理
            /// </summary>
            /// <param name="decPart">需要处理的小数部分</param>
            /// <returns></returns>
            private static string ConvertDecimal(string decPart)
            {
                string buf = "";
                int i = decPart.Length;

                //如果小数点后均为零
                if ((decPart == "0") || (decPart == "00"))
                {
                    return "整";
                }

                if (decPart.Length > 1) //2位
                {
                    if (decPart[0] == '0') //如果角位为零0.01
                    {
                        buf = DigToCC(decPart[1]) + "分"; //此时不可能分位为零
                    }
                    else if (decPart[1] == '0')
                    {
                        buf = DigToCC(decPart[0]) + "角整";
                    }
                    else
                    {
                        buf = DigToCC(decPart[0]) + "角";
                        buf += DigToCC(decPart[1]) + "分";
                    }
                }
                else //1位
                {
                    buf += DigToCC(decPart[0]) + "角整";
                }
                return buf;
            }
            #endregion

            #region GetUnit
            /// <summary>
            /// 获取人民币中文形式的对应位置的单位标志
            /// </summary>
            /// <param name="n"></param>
            /// <returns></returns>
            private static string GetUnit(int n)
            {
                return RMBUnitChar[n].ToString();
            }
            #endregion

            #region DigToCC
            /// <summary>
            /// 数字转换为相应的中文字符 ( Digital To Chinese Char )
            /// </summary>
            /// <param name="c">以字符形式存储的数字</param>
            /// <returns></returns>
            private static string DigToCC(char c)
            {
                return RMBUppercase[c - '0'].ToString();
            }
            #endregion

            #region CheckNumberLimit
            private static void CheckNumberLimit(decimal number)
            {
                if ((number < MinNumber) || (number > MaxNumber))
                {
                    throw new RMBException("超出可转换的范围");
                }
            }
            #endregion

            #region CombinUnit
            /// <summary>
            /// 合并兆亿万词
            /// </summary>
            /// <param name="rmb"></param>
            /// <returns></returns>
            private static string CombinUnit(string rmb)
            {
                if (rmb.Contains("兆亿万"))
                {
                    return rmb.Replace("兆亿万", "兆");
                }
                if (rmb.Contains("亿万"))
                {
                    return rmb.Replace("亿万", "亿");
                }
                if (rmb.Contains("兆亿"))
                {
                    return rmb.Replace("兆亿", "兆");
                }
                return rmb;
            }
            #endregion
            #endregion


            /// <summary>
            /// 以万为单位显示 t 为真四舍五入 t=false 不进行四舍五入
            /// </summary>
            /// <param name="number">金额</param>
            /// <param name="dec">小数位数</param>
            /// <param name="t">是否四舍五入</param>
            /// <returns></returns>
            public static string GetWebConvertdisp(decimal number,int dec,bool t)
            {
                string str = "";
                decimal deci=10000M;
                if (number < deci)
                {

                    if (dec == 0)
                    {
                        str = number.ToString("0");
                    }
                    else
                    {
                        if (t ==true)
                        {
                            str = GetDecimal(number, dec, true).ToString();
                        }
                        else
                        {
                            str = GetDecimal(number, dec, false).ToString();
                        }
                    
                    }
                }
                else if(number >= deci)
                {

                    decimal df = number / deci;

                 //   string strDecPart = "";                    // 存放小数部分的处理结果 
                                // 存放整数部分的处理结果 
                    string[] tmp = null;
                    string strDigital = df.ToString();

                    tmp = strDigital.Split(cDelim, 2); // 将数据分为整数和小数部分 

                    if (tmp.Length > 1) //分解出了小数
                    {
                        // strDecPart = ConvertDecimal(tmp[1]);


                        if (decimal.Parse(tmp[1]) > 0)
                        {
                            str = df.ToString("0.00") + "万";
                        }
                        else
                        {
                            str = df.ToString("0") + "万";
                        }
                        
                    }
                    else
                    {
                        str = df.ToString("0") + "万";
                    }
                }



                return str;
            
            
            }


            /// <summary>
            /// 去掉金额后面的小数据取被10整除的整数
            /// </summary>
            /// <param name="dec"></param>
            /// <returns></returns>
            public static decimal GetInt10(decimal dec)
            {
                decimal temdec = 0.00M;

                int ff = (int)dec / 10;
                int cc = ff * 10;

                temdec = decimal.Parse(cc.ToString());
                
                return temdec;

            }




            #region GetDecimal
            /// <summary>
            /// 获取几位小数点 bool t为真则四舍五入返回小数点位数,t为假则不进行四舍五入返回小数位数,默认为真
            /// </summary>
            /// <param name="decimal">金额</param>
            /// <param name="n">保留小数点位数</param>
            ///  <param name="n">bool t为真则四舍五入返回小数点位数,t为假则不进行四舍五入返回小数位数,默认为真</param>
            /// <returns>返回字符串</returns>
            public static decimal GetDecimal(decimal rmb=0.00m, int n = 2, bool t = true)
            {
                decimal dec=0.00M ;

                if (t)
                {
                    dec = Math.Round(rmb, n, MidpointRounding.AwayFromZero);
                }
                else
                {                    
                    string[] tmp = null;
                    string strDigital = rmb.ToString();
                    tmp = strDigital.Split(cDelim, 2); // 将数据分为整数和小数部分 
                    if (tmp.Length > 1) //分解出了小数
                    {
                        if (tmp[1].Length <= n)
                        {
                            dec = rmb;
                        }
                        else
                        {
                            string dec1 = tmp[1].ToString().Substring(0, n);
                            strDigital = tmp[0].ToString() + "." + dec1;
                            dec = decimal.Parse(strDigital);
                        }

                    }
                    else
                    {
                        dec = rmb;
                    }
                }
                return dec;
            }
            #endregion




        }




          






        #region RMBException
        /// <summary>
        /// 人民币转换的错误
        /// </summary>
        public class RMBException : System.Exception
        {
            public RMBException(string msg)
                : base(msg)
            {
            }
        }
        #endregion


    } 

