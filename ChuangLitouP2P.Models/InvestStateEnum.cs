using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    /// <summary>
    /// 用户投资状态枚举
    /// </summary>
    public enum InvestStateEnum
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None,
        /// <summary>
        /// 未投资
        /// </summary>
        NoInvestment,
        /// <summary>
        /// 已投资
        /// </summary>
        AlreadyInvested
    }
}
