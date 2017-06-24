using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public enum EnumTypesFinance
    {
        [Description("充值")]
        充值 = 3,
        [Description("借款人还款")]
        借款人还款 = 9,
        [Description("提现卡不存在")]
        提现卡不存在 = 26,
        [Description("提现审核中")]
        提现审核中 = 27,
        [Description("提现失败")]
        提现审核未通过 = 28,
        [Description("提现成功")]
        提现成功 = 29,
        [Description("项目投资")]
        项目投资 = 37,
        [Description("还款")]
        还款 = 38,
        [Description("服务费")]
        服务费 = 39,
        [Description("手续费")]
        手续费 = 40,
        [Description("借款转入")]
        借款转入 = 41,
        [Description("连连充值")]
        连连充值 = 4,
        [Description("连连取现")]
        连连取现 = 5,
        [Description("移动端充值")]
        移动端充值 = 6,
        [Description("PC端充值")]
        PC端充值 = 8,

        [Description("邀请奖励")]
        邀请奖励 = 42,
        [Description("现金奖励")]
        现金奖励 = 43,
        [Description("手续费退回")]
        手续费退回 = 44,
        [Description("平台向用户划账")]
        平台向用户划账 = 50,
        [Description("用户向平台划账")]
        用户向平台划账 = 60

        //  [Description("奖励金投资")]
        //  奖励金投资 = 66,



        //  [Description("与托管帐户同步可用余额")]
        //  与托管帐户同步可用余额 = 67

    }
}
