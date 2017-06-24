using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public enum EnumLLPayState
    {
        [Description("订单未支付")]
        订单未支付 = 0,
        [Description("付款成功")]
        付款成功 = 1,
        [Description("付款失败")]
        付款失败 = 2,
        [Description("等待审核")]
        等待审核 = 3,
        [Description("付款中")]
        付款中 = 5
    }
}
