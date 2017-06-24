using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.wdzg
{
    /// <summary>
    /// 投资信息实体 
    /// </summary>
    public class InvestRecordEntity
    {
     
        /// <summary>
        /// 投资ID
        /// </summary>
        public string tender_id { get; set; }
        /// <summary>
        /// 投资人名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 有效投资金额 : 单位：元
        /// </summary>
        public decimal account { get; set; }
        /// <summary>
        /// 标ID
        /// </summary>
        public string borrow_id { get; set; }
        /// <summary>
        /// 投资时间 : 时间戳格式
        /// </summary>
        public string tender_time { get; set; }
        /// <summary>
        /// 状态	status	*	0 待审核 1 待回款 2 成功回款 3 坏账 4债权转让 5投标失败
        /// </summary>
        public string status { get; set; }


    }
}
