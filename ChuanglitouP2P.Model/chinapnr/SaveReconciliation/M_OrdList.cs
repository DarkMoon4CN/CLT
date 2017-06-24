using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.SaveReconciliation
{
    public class M_OrdList
    {
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { get; set; }
        
        /// <summary>
        /// 汇付交易状态
        /// </summary>
        public string TransStat { get; set; }

        /// <summary>
        /// 支付网关业务代号
        /// </summary>
        public string GateBusiId { get; set; }

        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { get; set; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string OpenAcctId { get; set; }

        /// <summary>
        /// 手续费金额
        /// </summary>
        public string FeeAmt { get; set; }

        /// <summary>
        /// 手续费扣款客户号
        /// </summary>
        public string FeeCustId { get; set; }

        /// <summary>
        /// 手续费扣款子账户号
        /// </summary>
        public string FeeAcctId { get; set; }
    }
}
