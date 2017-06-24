using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.ChinaPnr
{
    public class UserEntity : BaseRequest
    {
        /// <summary>
        /// 真实名称
        /// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrName { get; set; }
        ///// <summary>
        ///// 证件类型 00身份证
        ///// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IdType { get; set; }
        ///// <summary>
        ///// 用户证件号码
        ///// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IdNo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrMp { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrEmail { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RespCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RespDesc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrCustId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TrxId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MerPriv { get; set; }
    }
}
