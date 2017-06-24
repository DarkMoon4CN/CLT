using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 费用支付方式枚举
    /// </summary>
    public enum FeesPayerEnum
    {
        /// <summary>
        /// 平台支付
        /// </summary>
        Platform,
        /// <summary>
        /// 收款人支付
        /// </summary>
        Remittee,
        /// <summary>
        /// 汇款人支付
        /// </summary>
        Lenders
    }
    public class M_WithdrawalCash
    {
        /// <summary>
        /// 提现类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public Decimal Amount { get; set; }
        /// <summary>
        /// 提现服务费
        /// </summary>
        public Decimal ServiceFees { get; set; }
        /// <summary>
        /// 提现限额
        /// </summary>
        public Decimal Limit { get; set; }
        /// <summary>
        /// 预期到账时间
        /// </summary>
        public DateTime ArrivalDate { get; set; }
        /// <summary>
        /// 费用支付方
        /// </summary>
        public FeesPayerEnum Payer { get; set; }
    }
}
