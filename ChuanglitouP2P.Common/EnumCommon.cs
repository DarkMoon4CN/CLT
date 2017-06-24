using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    public class EnumCommon
    {
        public class E_hx_ActivityTable
        {
            /// <summary>
            /// 面向平台
            /// </summary>
            public struct E_ActTargetPlatform
            {
                /// <summary>
                /// 全部
                /// </summary>
                public static string all = "1111";
                /// <summary>
                /// 网站
                /// </summary>
                public static string web = "1000";
                /// <summary>
                /// 手机站
                /// </summary>
                public static string wap = "0100";
                /// <summary>
                /// 安卓客户端
                /// </summary>
                public static string android = "0010";
                /// <summary>
                /// 苹果客户端
                /// </summary>
                public static string ios = "0001";
            }
        }
        public class E_hx_td_UserCash
        {
            /// <summary>
            /// 提现方式
            /// </summary>
            public enum EnumCashChl
            {
                /// <summary>
                /// 普通提现
                /// </summary>
                GENERAL = 0,
                /// <summary>
                /// 快速提现
                /// </summary>
                FAST = 1,
                /// <summary>
                /// 即时提现
                /// </summary>
                IMMEDIATE = 2
            }
            public static string GetCashChl(string cashChl)
            {
                switch (cashChl)
                {
                    case "GENERAL": return "普通提现";
                    case "FAST": return "快速提现";
                    case "IMMEDIATE": return "即时提现";
                    default: return "普通提现";
                }
            }
        }
    }
}
