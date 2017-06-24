using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryCardInfo
{
    public class M_QueryCardInfo
    {

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { set; get; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { set; get; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string CardId { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
}
