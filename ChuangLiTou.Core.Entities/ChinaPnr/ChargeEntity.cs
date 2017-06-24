using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.ChinaPnr
{
    /// <summary>
    /// 
    /// </summary>
    public class ChargeEntity : BaseRequest
    {



        /// <summary>
        /// 由商户的系统生成，必须保证唯一，请使用纯数字
        /// </summary>
        public string OrdId { get; set; }
        /// <summary>
        /// 格式为 YYYYMMDD，例如：20130307
        /// </summary>
        public string OrdDate { get; set; }
        /// <summary>
        /// 由汇付生成，用户的唯一性标识
        /// </summary>
        public string UsrCustId { get; set; }
        /// <summary>
        ///支付网关业务代号 网关的细分业务类型
        /// 
        ///B2C--B2C 网银支付
        ///B2B--B2B 网银支付
        ///FPAY--快捷支付
        ///WH--代扣
        ///网关的具体说明请见“附件二：网关支付类型“
        /// </summary>
        public string GateBusiId { get; set; }
        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { get; set; }
        /// <summary>
        /// 借贷记标记
        /// </summary>
        public string DcFlag { get; set; }
        /// <summary>
        /// 泛指交易金额，如充值、支付、取现、冻结和解冻金额（金额格式必须是###.00）比如 2.00，2.01
        /// </summary>
        public string TransAmt { get; set; }
    }
}
