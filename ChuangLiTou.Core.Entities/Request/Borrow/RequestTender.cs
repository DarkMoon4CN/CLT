using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Request.Borrow
{
    /// <summary>
    /// Class RequestTender.
    /// </summary>
    public class RequestTender
    {
        /// <summary>
        ///用户ID
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 标ID
        /// </summary>
        public int targetId { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal investAmount { get; set; }

        /// <summary>
        /// 红包+代金券 id集合
        /// </summary>
        /// <value>The bonus ids.</value>
        public string bonusIds { get; set; }

        /// <summary>
        /// 加息券 id集合
        /// </summary>
        /// <value>add Rate Ids.</value>
        public string addRateIds { get; set; }

        /// <summary>
        /// 标的类型,普通标的赋值0,新手标按照数据赋值
        /// </summary>
        public int typeId { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        //public string invitedcode { set; get; }
    }
}
