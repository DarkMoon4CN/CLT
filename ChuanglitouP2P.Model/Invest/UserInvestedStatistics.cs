using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.Invest
{

    /// <summary>
    /// 用户投资信息统计
    /// </summary>
    public class UserInvestedStatistics
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户投资总额
        /// </summary>
        public string InvestedAmount { get; set; }
    }
}
