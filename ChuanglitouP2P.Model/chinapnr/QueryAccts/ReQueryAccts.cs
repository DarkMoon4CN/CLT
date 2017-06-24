using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryAccts
{
    public class ReQueryAccts
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
        /// 账务结果串
        /// </summary>
     //   public string AcctDetails { get; set; }

        public M_Details AcctDetails { get; set; }

      

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }

    }
}
