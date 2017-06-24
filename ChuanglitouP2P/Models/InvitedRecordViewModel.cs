using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuanglitouP2P.Models
{
    /// <summary>
    /// 邀请列表视图模型类
    /// </summary>
    public class InvitedRecordViewModel
    {
        /// <summary>
        /// 投资记录编号
        /// </summary>
        public string InvestRecordId { get; set; }
        /// <summary>
        /// 邀请人帐户
        /// </summary>
        public string InviterAccount { get; set; }
        /// <summary>
        /// 邀请人真实姓名
        /// </summary>
        public string InviterRealName { get; set; }
        /// <summary>
        /// 被邀请用户帐号
        /// </summary>
        public string BeInvitedAccount { get; set; }
        /// <summary>
        /// 被邀请人真实姓名
        /// </summary>
        public string BeInvitedRealName { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        public string InvestDatetime { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public string InvestAmount { get; set; }
        /// <summary>
        /// 投资订单号
        /// </summary>
        public string InvestOrderNo { get; set; }

        /// <summary>
        /// 邀请人ID
        /// </summary>
        public string YaoUid { get; set; }

        /// <summary>
        /// 被邀请人ID
        /// </summary>
        public string InvUid { get; set; }
    }
}