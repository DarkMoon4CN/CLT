using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Member
{
    public class MemberBankEntity
    {
        /// <summary>
		/// 编号
        /// </summary>
        [JsonProperty("usrBindCardId", NullValueHandling = NullValueHandling.Ignore)]
        public int? UsrBindCardID { get; set; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        [JsonProperty("usrCustId", NullValueHandling = NullValueHandling.Ignore)]
        public string UsrCustId { get; set; }

        /// <summary>
        /// 开户银行号
        /// </summary>
        [JsonProperty("openAcctId", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenAcctId { get; set; }

        /// <summary>
        /// 开户银行代码
        /// </summary>
        [JsonProperty("openBankId", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenBankId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [JsonProperty("realName", NullValueHandling = NullValueHandling.Ignore)]
        public string RealName { get; set; }

        /// <summary>
        /// LOGO
        /// </summary>
        [JsonProperty("cardImage", NullValueHandling = NullValueHandling.Ignore)]
        public string CardImage { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [JsonProperty("bankName", NullValueHandling = NullValueHandling.Ignore)]
        public string BankName { get; set; }

        /// <summary>
        /// 0 默认   1设置为默认卡
        /// </summary>
        [JsonProperty("defCard", NullValueHandling = NullValueHandling.Ignore)]
        public int? defCard { get; set; }

        /// <summary>
        /// BindCardType
        /// </summary>
        [JsonProperty("bindCardType", NullValueHandling = NullValueHandling.Ignore)]
        public int? BindCardType { get; set; }

        /// <summary>
        /// 获取此绑定的银行卡是否支持即时体现业务
        /// </summary>
        public bool CanImmediateWithdrawal { get; set; }
    }
}