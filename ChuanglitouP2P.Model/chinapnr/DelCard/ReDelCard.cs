using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.DelCard
{
    public class ReDelCard
    {
        /// <summary>
        /// 消息类型
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
        /// 用户客户号
        /// </summary>
        public string UsrCustId { get; set; }
        /// <summary>
        /// 开户银行账号 (银行卡号)
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        ///签名
        /// </summary>
        public string ChkValue { get; set; }
    }
}
