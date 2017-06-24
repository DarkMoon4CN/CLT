using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    public static class StaticExtensionHelper
    {
        #region InvestTermEnum
        /// <summary>
        /// 获取InvestTermEnum类型枚举的值，以字符串展示形式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ValueToString(this InvestTermEnum obj)
        {
            return Enum.GetName(typeof(InvestTermEnum), obj);
        }
        /// <summary>
        /// 获取InvestTermEnum类型枚举的值，以整型展示形式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ValueToInteger(this InvestTermEnum obj)
        {
            return (int)obj;
        }
        /// <summary>
        /// 把字符串转换成InvestTermEnum类型枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static InvestTermEnum ValueToInvestTermEnum(this string value)
        {
            return (InvestTermEnum)Enum.Parse(typeof(InvestTermEnum), value);
        }
        /// <summary>
        /// 把数值转换成InvestTermEnum类型枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static InvestTermEnum ValueToInvestTermEnum(this int value)
        {
            return (InvestTermEnum)value;
        }
        #endregion

        #region InvestStateEnum
        public static string ValueToString(this InvestStateEnum obj)
        {
            return Enum.GetName(typeof(InvestStateEnum), obj);
        }
        public static InvestStateEnum ValueToInvestStateEnum(this string value)
        {
            return (InvestStateEnum)Enum.Parse(typeof(InvestStateEnum), value);
        }
        #endregion

        #region CardBindStateEnum
        public static string ValueToString(this CardBindStateEnum obj)
        {
            return Enum.GetName(typeof(CardBindStateEnum), obj);
        }
        public static CardBindStateEnum ValueToCardBindStateEnum(this string value)
        {
            return (CardBindStateEnum)Enum.Parse(typeof(CardBindStateEnum), value);
        }
        #endregion
    }
}
