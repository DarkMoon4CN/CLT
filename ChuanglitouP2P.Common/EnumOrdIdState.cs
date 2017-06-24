using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public enum EnumOrdIdState
    {
          
        [Description("待审核 ")]
        待审核 = 0,
        [Description("待付款")]
        待付款 = 1,
        [Description("已付款")]
        已付款 = 3,
        [Description("未通过")]
        未通过 = 4,
        [Description("强制未通过")]
        强制未通过 = 5

    }
}
