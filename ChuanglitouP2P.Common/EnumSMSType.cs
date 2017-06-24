using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public enum EnumSMSType
    {
        [Description("语音短信验证码")]
        语音短信验证码 = 7,
        [Description("短信验证码")]
        短信验证码 = 8,
        [Description("一对一短信")]
        一对一短信 = 9,
        [Description("投资成功")]
        投资成功 = 10,
        [Description("投资回款")]
        投资回款 = 11,
        [Description("取现成功")]
        取现成功 = 12,
        [Description("修改密码")]
        修改密码 = 13,
        [Description("注册成功")]
        注册成功 = 14,
        [Description("活动奖励")]
        活动奖励 = 15


    }
}
