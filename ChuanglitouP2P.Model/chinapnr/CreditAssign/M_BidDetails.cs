using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CreditAssign
{
    public class M_BidDetails
    {
        /// <summary>
        /// 被转让的投标订单号
        /// </summary>
        public string BidOrdId { set; get; }
        /// <summary>
        /// 被转让的投标订单日期
        /// </summary>
        public string BidOrdDate { set; get; }
        /// <summary>
        /// 转让金额
        /// </summary>
        public string BidCreditAmt { set; get; }
        /// <summary>
        /// 借款人客户号
        /// </summary>
        public string BorrowerCustId { set; get; }
        /// <summary>
        /// 明细转让金额
        /// </summary>
        public string BorrowerCreditAmt { set; get; }
        /// <summary>
        /// 已还款金额
        /// </summary>
        public string PrinAmt { set; get; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }




    }
}
