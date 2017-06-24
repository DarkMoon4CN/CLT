using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CorpRegister
{
    public class ReCorpRegister
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { set; get; }
        /// <summary>
        /// 应答返回码
        /// </summary>
        public string RespCode { set; get; }
        /// <summary>
        /// 应答描述
        /// </summary>
        public string RespDesc { set; get; }

        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { set; get; }

        /// <summary>
        /// 用户号
        /// </summary>
        public string UsrId { set; get; }
        /// <summary>
        /// 用户号
        /// </summary>
        public string UsrName { set; get; }
        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string AuditStat { set; get; }
        /// <summary>
        /// 审核状态描述
        /// </summary>
        public string AuditDesc { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }
        /// <summary>
        /// 交易唯一标识
        /// </summary>
        public string TrxId { set; get; }

        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { set; get; }
        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string CardId { set; get; }

        /// <summary>
        /// 后台应答地址
        /// </summary>
        public string BgRetUrl { set; get; }
        /// <summary>
        /// 返参扩展域
        /// </summary>
        public string RespExt { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
}
