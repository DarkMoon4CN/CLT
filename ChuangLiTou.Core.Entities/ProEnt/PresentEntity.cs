using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.ProEnt
{
    /// <summary>
    /// Class PresentEntity.
    /// </summary>
    public class PresentEntity
    {/// <summary>
        /// UserCashId
        /// </summary>        
        [JsonProperty("UserCashId", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserCashId { get; set; }

        /// <summary>
        /// registerid
        /// </summary>        
        [JsonProperty("registerid", NullValueHandling = NullValueHandling.Ignore)]
        public int? registerid { get; set; }

        /// <summary>
        /// 由汇付生成的唯一标识
        /// </summary>        
        [JsonProperty("UsrCustId", NullValueHandling = NullValueHandling.Ignore)]
        public string UsrCustId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>        
        [JsonProperty("presentAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TransAmt { get; set; }

        /// <summary>
        /// FeeAmt
        /// </summary>        
        [JsonProperty("FeeAmt", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? FeeAmt { get; set; }

        /// <summary>
        /// OrdId
        /// </summary>        
        [JsonProperty("OrdId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrdId { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>        
        [JsonProperty("appliedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? OrdIdTime { get; set; }

        /// <summary>
        /// 0 待审核 1待付款  3 已付款 4未通过(必须填写原因)
        /// </summary>        
        [JsonProperty("ordIdState", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrdIdState { get; set; }

        /// <summary>
        /// OperTime
        /// </summary>        
        [JsonProperty("OperTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? OperTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>        
        [JsonProperty("Reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>        
        [JsonProperty("Remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }

        /// <summary>
        /// 提现状态 999 处理中(默认)      400 取现失败    000成功
        /// </summary>        
        [JsonProperty("presentStatu", NullValueHandling = NullValueHandling.Ignore)]
        public int? TransState { get; set; }

        /// <summary>
        /// OpenAcctId
        /// </summary>        
        [JsonProperty("OpenAcctId", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenAcctId { get; set; }

        /// <summary>
        /// OpenBankId
        /// </summary>        
        [JsonProperty("OpenBankId", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenBankId { get; set; }

        /// <summary>
        /// FeeObjFlag
        /// </summary>        
        [JsonProperty("FeeObjFlag", NullValueHandling = NullValueHandling.Ignore)]
        public string FeeObjFlag { get; set; }

    }
}
