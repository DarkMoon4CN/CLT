using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.Invest
{
    /// <summary>
    /// 邀请明细
    /// </summary>
    public class InvitationDetail
    {
        /// <summary>
        /// 被邀请的人投资总额
        /// </summary>
        public string InvitationTotalAmount { get; set; }

        /// <summary>
        /// 已邀请并注册的总人数
        /// </summary>
        public string  InvitedPeopleCount { get; set; }

        /// <summary>
        /// 已邀请并成功投资的总人数
        /// </summary>
        public string InvitedInvestedPeopleCount { get; set; }

        ///// <summary>
        ///// 已邀请用户投资情况统计集合
        ///// </summary>
        //public List<UserInvestedStatistics> InvitedDetailInformation { get; set; }
    }
}
