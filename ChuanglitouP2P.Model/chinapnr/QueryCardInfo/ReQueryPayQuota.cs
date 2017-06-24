using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.chinapnr.QueryCardInfo
{
    public class ReQueryPayQuota
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
        /// 用户银行卡信息列表
        /// </summary>
        public List<M_PayQuotaDetails> PayQuotaDetails { get; set; }


        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
    /// <summary>
    /// 用户银行卡信息列表
    /// </summary>
    public class M_PayQuotaDetails
    {
        /// <summary>
        /// 银行编码
        /// </summary>
        public string OpenBankId { set; get; }
        /// <summary>
        /// 单笔限额
        /// </summary>
        public string SingleTransQuota { set; get; }
        /// <summary>
        /// 单日限额
        /// </summary>
        public string CardDailyTransQuota { set; get; }
    }
}
