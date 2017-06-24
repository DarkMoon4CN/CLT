using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public enum EnumMType
    {

        [Description("系统消息")]
        系统消息 = 0,
        [Description("投标完成")]
        投标完成 = 1,
        [Description("项目满标")]
        项目满标 = 2,
        [Description("投标放款")]
        投标放款 = 3,
        [Description("项目回款")]
        项目回款 = 4

    }
}
