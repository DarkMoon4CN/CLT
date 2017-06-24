using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.P2Peye
{
    /// <summary>
    /// 投资实体 xiezh 
    /// </summary>
    public class InvestmentEntity
    {
       
        /// <summary>
        /// 标编号	标的唯一编号(不为空,很重要)
        /// </summary>
        public string id { get; set; }
        /// <summary>
        ///标的链接	URL链接.
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// 用户所在地	投标人所在城市.
        /// </summary>
        public string useraddress { get; set; }
        /// <summary>
        /// 用户名	投标人的用户名称,登录账号,可辨识区分,可支持加密数据.
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户编号	投标人的用户编号/ID.
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 投标方式	例如:手动、自动.
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 投标金额	投标金额实际生效部分(保留两位小数).
        /// </summary>
        public double money { get; set; }
        /// <summary>
        /// 有效金额	投标金额实际生效部分(保留两位小数),请过滤掉投资金额小于10块的记录.
        /// </summary>
        public double account { get; set; }
        /// <summary>
        /// 投标状态	例如:成功、部分成功、失败.
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 投标时间	格式如:2014-03-13 16:44:26.
        /// </summary>
        public string add_time { get; set; }
    }
}
