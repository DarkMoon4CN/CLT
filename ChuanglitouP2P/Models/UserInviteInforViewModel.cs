using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuanglitouP2P.Models
{
    /// <summary>
    /// 邀请投资记录视图模型类
    /// </summary>
    public class UserInviteInforViewModel
    {
        /// <summary>
        /// 邀请人ID
        /// </summary>
        public string InviterId { get; set; }
        /// <summary>
        /// 邀请人帐号
        /// </summary>
        public string InviterAccount { get; set; }

        /// <summary>
        /// 被邀请人ID
        /// </summary>
        public string BeInvitedId { get; set; }
        /// <summary>
        /// 被邀请人帐号
        /// </summary>
        public string BeInvitedAccount { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegistedDateTime { get; set; }
        /// <summary>
        /// 投资状态
        /// </summary>
        public string InvestState { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        public string InvestDateTime { get; set; }
        /// <summary>
        /// 此次投资金额
        /// </summary>
        public string InvestAmount { get; set; }
        /// <summary>
        /// 投资总金额
        /// </summary>
        public string InvestTotalAmount { get; set; }
        /// <summary>
        /// 投标订单号
        /// </summary>
        public string InvestOrderNo { get; set; }
        /// <summary>
        /// 投标信息
        /// </summary>
        public string InvestInformation { get; set; }
        /// <summary>
        /// 投标期限
        /// </summary>
        public string InvestTerm { get; set; }
        /// <summary>
        /// 是否已经绑定银行卡
        /// </summary>
        public string IsBindedCard { get; set; }
    }
}