using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CashReconciliation
{
    public class M_OrdId
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { get; set; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { get; set; }

        /// <summary>
        /// 开户银行号
        /// </summary>
        public string CardId { get; set; }

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

        /// <summary>
        /// 手续费金额
        /// </summary>
        public string FeeAmt { get; set; }

        /// <summary>
        /// 商户收取服务费金额
        /// </summary>
        public string ServFee { get; set; }
        /// <summary>
        /// 商户子帐户号
        /// </summary>
        public string ServFeeAcctId { get; set; }




    }
}
