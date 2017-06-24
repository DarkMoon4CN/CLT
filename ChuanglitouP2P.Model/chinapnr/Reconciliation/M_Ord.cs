using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.Reconciliation
{
    public class M_Ord
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { get; set; }

        /// <summary>
        /// 高户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 投资人客户号
        /// </summary>
        public string InvestCustId { get; set; }

        /// <summary>
        /// 借款人客户号
        /// </summary>
        public string BorrCustId { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { get; set; }

        /// <summary>
        /// 汇付交易状态
        /// </summary>
        public string TransStat { get; set; }

        /// <summary>
        /// 汇付交易日期
        /// </summary>
        public string PnrDate { get; set; }
        /// <summary>
        /// 汇付交易流水
        /// </summary>
        public string PnrSeqId { get; set; }





    }
}
