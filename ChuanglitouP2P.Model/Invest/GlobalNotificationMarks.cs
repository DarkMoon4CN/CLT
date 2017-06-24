using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.Invest
{
    /// <summary>
    /// 全局数据变更标记请求实体类
    /// </summary>
    public class GlobalNotificationMarksRequest
    {
        /// <summary>
        /// 用户唯一标识码
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 客户端执行请求数据是否有变化的参考时间
        /// </summary>
        public string referenceDateTime { get; set; }
    }

    /// <summary>
    /// 全局数据变更标记状态结果实体类
    /// </summary>
    public class GlobalNotificationMarks
    {
        /// <summary>
        /// 数据统计时间戳
        /// </summary>
        public string StatisticsDateTime { get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff"); } }
        /// <summary>
        /// 代金券数据是否有变更
        /// </summary>
        public bool BonusHasChanged { get; set; }
    }
}
