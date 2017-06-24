using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Member
{
    public class MemberEntity
    {


        #region Model
        /// <summary>
        /// 注册id
        /// </summary>
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int? registerid { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string username { set; get; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonIgnore]
        public string password { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        [JsonProperty("userMobile", NullValueHandling = NullValueHandling.Ignore)]
        public string mobile { set; get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string email { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>

        [JsonProperty("realName", NullValueHandling = NullValueHandling.Ignore)]
        public string realname { set; get; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        [JsonProperty("customNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string UsrCustId { set; get; }



        /// <summary>
        /// 用户头像地址
        /// </summary>
        [JsonProperty("userPhotoUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string userPhotoUrl { set; get; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [JsonProperty("idCode", NullValueHandling = NullValueHandling.Ignore)]
        public string iD_number { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string transactionpassword { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? istransactionpassword { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ismobile { set; get; }

        /// <summary>
        ///  是否实名认证 0 未认证   1已认证
        /// </summary>
        [JsonProperty("isRealName", NullValueHandling = NullValueHandling.Ignore)]
        public int? isrealname { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? isbankcard { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? isemail { set; get; }

        /// <summary>
        /// 0 正常  1禁止登录
        /// </summary>
        /// 
        [JsonProperty("userState", NullValueHandling = NullValueHandling.Ignore)]
        public int? userstate { set; get; }

        /// <summary>
        /// 帐户总资产
        /// </summary>
        [JsonProperty("totalAssets", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? account_total_assets { set; get; }
        /// <summary>
        /// 帐户总收益
        /// </summary>
        [JsonProperty("totalGains", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? totalGains { set; get; }
        /// <summary>
        /// 利息收益
        /// </summary>
        [JsonProperty("interestIncome", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? interestIncome { set; get; }

        /// <summary>
        /// 活动奖励
        /// </summary>
        [JsonProperty("activityReward", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? activityReward { set; get; }

        /// <summary>
        /// 加息券收益
        /// </summary>
        [JsonProperty("couponIncome", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? couponIncome { set; get; }

        /// <summary>
        /// 债券收益
        /// </summary>
        [JsonProperty("claimsIncome", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? claimsIncome { set; get; }


        /// <summary>
        /// 已赚的利息
        /// </summary>
        [JsonProperty("receivedIncome", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? receivedIncome { get; set; }
        /// <summary>
        /// 未赚的利息
        /// </summary>
        [JsonProperty("receivingIncome", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? receivingIncome { get; set; }

        /// <summary>
        /// 待收本金
        /// </summary>
        [JsonProperty("totalAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? totalAmount { get; set; }

        /// <summary>
        /// 奖励金额
        /// </summary>
        [JsonProperty("bonusAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bonusAmount { get; set; }



        /// <summary>
        /// 总投资记录
        /// </summary>
        [JsonProperty("totalInverstAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? totalInverstAmount { get; set; }



        /// <summary>
        /// 可用余额
        /// </summary>
        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_balance { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? collect_total_amount { set; get; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        [JsonProperty("frozen", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? frozen_sum { set; get; }

        /// <summary>
        /// 0 默认未开通   1 已开通
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? open_tonto_account { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tonto_account_user { set; get; }

        /// <summary>
        /// 0 投资  1借款用户  2借款企业
        /// </summary>
        [JsonProperty("userType", NullValueHandling = NullValueHandling.Ignore)]
        public int? usertypes { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? registration_time { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lastLoginTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lastlogintime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string lastloginIP { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? customserviceid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string homephone { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contactaddress { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zipcode { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string qq { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string msn { set; get; }



        /// <summary>
        /// 邀请码
        /// </summary>
        [JsonProperty("inviteCode", NullValueHandling = NullValueHandling.Ignore)]
        public string invitedcode { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? useridentity { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CopName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Channelsource { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WXAppId { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserPhotoVirtualPath { get; set; }

        #endregion Model
    }


}
