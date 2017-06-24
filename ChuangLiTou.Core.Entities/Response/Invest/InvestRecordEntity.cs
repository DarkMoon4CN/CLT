using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Invest
{

    public class InvestRecordEntity
    {
        /// <summary>
        /// 手机号码
        /// </summary>        
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string phone { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? investMoney;

        /// <summary>
        /// 投资时间
        /// </summary>        
        [JsonProperty("investTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? investTime { get; set; }

    }
}
