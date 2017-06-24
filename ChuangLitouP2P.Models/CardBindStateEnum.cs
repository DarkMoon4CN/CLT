using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    /// <summary>
    /// 卡片绑定状态
    /// </summary>
    public enum CardBindStateEnum
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None,
        /// <summary>
        /// 未绑定
        /// </summary>
        Unbound,
        /// <summary>
        /// 已绑定
        /// </summary>
        Bounded
    }
}
