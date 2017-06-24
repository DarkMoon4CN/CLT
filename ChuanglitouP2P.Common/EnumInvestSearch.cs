using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 投资查询类型
    /// </summary>
    public enum EnumInvestSearch
    {
        [Description("年化收益")]
        Arp = 1,
        [Description("期限")]
        Repayment =2,
        [Description("融资金额")]
        Account =3,
        [Description("进度")]
        Schedule =4,
        [Description("状态")]
        Status =5

    }
}
