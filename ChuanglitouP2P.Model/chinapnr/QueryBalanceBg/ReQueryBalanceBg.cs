using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryBalanceBg
{
    public class ReQueryBalanceBg
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
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public string AvlBal { set; get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public string AcctBal { set; get; }

        /// <summary>
        /// 冻结余额
        /// </summary>
        public string FrzBal { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
}
