using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Bank
{
    public class BankEntity
    {
        /// <summary>
        /// Bankid
        /// </summary>        
        [JsonIgnore]
        public int Bankid { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>        
        [JsonProperty("bankName", NullValueHandling = NullValueHandling.Ignore)]
        public string BankName { get; set; }

        /// <summary>
        /// 银行简称
        /// </summary>        
        [JsonProperty("bankType", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenBankId { get; set; }

        /// <summary>
        /// CardImage
        /// </summary>
        [JsonIgnore]
        public string CardImage { get; set; }

        /// <summary>
        /// Isquick
        /// </summary>        
        [JsonIgnore]
        public int Isquick { get; set; }

        /// <summary>
        /// Isordinary
        /// </summary>        
        [JsonIgnore]
        public int Isordinary { get; set; }

        /// <summary>
        /// 每笔交易限制总额度
        /// </summary>
        [JsonProperty("AmountLimitPerTrade", NullValueHandling = NullValueHandling.Ignore)]
        public int AmountLimitPerTrade { get; set; }

        /// <summary>
        /// 每日交易限制总额度
        /// </summary>
        [JsonProperty("AmountLimitPerDay", NullValueHandling = NullValueHandling.Ignore)]
        public int AmountLimitPerDay { get; set; }

        /// <summary>
        /// 每月交易限制总额度
        /// </summary>
        [JsonProperty("AmountLimitPerMonth", NullValueHandling = NullValueHandling.Ignore)]
        public int AmountLimitPerMonth { get; set; }
    }
}
