using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.ChinaPnr
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 汇付商户号
        /// </summary>
        public string MerId { get; set; }
        /// <summary>
        /// 版本 10
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string CmdId { get; set; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }
        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RetUrl { get; set; }
    }
}
