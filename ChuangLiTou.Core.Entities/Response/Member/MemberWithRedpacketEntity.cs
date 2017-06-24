using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Member
{
    /// <summary>
    /// 会员基本信息的扩展，增加优惠券数量和红包金额字段
    /// </summary>
    public class MemberWithRedpacketEntity: MemberEntity
    {
        /// <summary>
        /// 优惠券数量
        /// </summary>
        [JsonProperty("voucherCount", NullValueHandling = NullValueHandling.Ignore)]
        public int VoucherCount { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        [JsonProperty("redpacketMoney", NullValueHandling = NullValueHandling.Ignore)]
        public decimal RedpacketMoney { get; set; }
    }
}
