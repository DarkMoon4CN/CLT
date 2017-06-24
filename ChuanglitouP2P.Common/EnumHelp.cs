using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    public class EnumHelp
    {
        public class LuckDrawEnum
        {
            /// <summary>
            /// 奖品类型枚举值
            /// </summary>
            public enum E_AwardType
            {
                /// <summary>
                /// 现金（未发）
                /// </summary>
                type0 = 0,
                /// <summary>
                /// 代金券
                /// </summary>
                type1 = 1,
                /// <summary>
                /// 加息券
                /// </summary>
                type2 = 2,
                /// <summary>
                /// 谢谢参与
                /// </summary>
                type3 = 3,
                /// <summary>
                /// 现金（已发）
                /// </summary>
                type4 = 4
            }
        }
    }
}
