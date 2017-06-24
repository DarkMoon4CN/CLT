using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Borrow
{
    /// <summary>
    /// 项目标请求参数
    /// </summary>
    public class RequestBorrow : RequestPage
    {
        /// <summary>
        /// 状态  -1  录入 0 审核中 1 初审通过  2 复审通过(开标上线)    3 满标 (还款中)     4放款 (还款中)   5 已还清   6初审未通过   7 复审未通过  8 流标
        /// </summary>
        public int status { get; set; }
    }


}
