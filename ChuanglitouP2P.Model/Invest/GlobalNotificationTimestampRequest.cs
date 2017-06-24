using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.Invest
{
    /// <summary>
    /// 请求最后变更的时间戳实体类
    /// </summary>
    public class GlobalNotificationTimestampRequest
    {
        /// <summary>
        /// 用户唯一标识码
        /// </summary>
        public int userId { get; set; }
    }

    /// <summary>
    /// 最后变更的时间戳实体类
    /// </summary>
    public class GlobalNotificationTimestamp
    {
        /// <summary>
        /// 代金券变更时间.APP端不把红包和加息券的最后变更时间拆分统计.统一使用  代金券变更时间
        /// </summary>
        public string Voucher { get; set; }
    }
}
