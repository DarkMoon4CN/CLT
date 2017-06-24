using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.UserBindCard
{

    /// <summary>
    /// 绑定用户银行卡
    /// </summary>
    public class ReUserBindCard
    {
        /// <summary>
        /// 消息类型 UserBindCard
        /// </summary>
        public string CmdId { get; set; }

        /// <summary>
        /// 应答返回码
        /// </summary>
        public string RespCode { get; set; }

        /// <summary>
        /// 应答描述
        /// </summary>
        public string RespDesc { get; set; }

        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string OpenAcctId { get; set; }

        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { get; set; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { get; set; }

        /// <summary>
        /// 本平台交易唯一标识
        /// </summary>
        public string TrxId { get; set; }

        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }


    }
}
