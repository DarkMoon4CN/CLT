using ChuanglitouP2P.Model.chinapnr.ChinapnrAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.UsrFreezeBg
{
    public class ReUsrFreezeBg
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        [ChkValueSort(1)]
        public string CmdId { set; get; }
        /// <summary>
        /// 应答返回码
        /// </summary>
        [ChkValueSort(2)]
        public string RespCode { set; get; }
        /// <summary>
        /// 应答描述
        /// </summary>
        public string RespDesc { set; get; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        [ChkValueSort(3)]
        public string MerCustId { set; get; }
        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }
        /// <summary>
        /// 子账户类型
        /// </summary>
        public string SubAcctType { set; get; }
        /// <summary>
        /// 子账户类型
        /// </summary>
        public string SubAcctId { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        [ChkValueSort(4)]
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        [ChkValueSort(5)]
        public string OrdDate { set; get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { set; get; }
        /// <summary>
        /// 页面返回URL
        /// </summary>
        [ChkValueSort(7)]
        public string RetUrl { set; get; }
        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        [ChkValueSort(8)]
        public string BgRetUrl { set; get; }
        /// <summary>
        /// 本平台交易唯一标识
        /// </summary>
        [ChkValueSort(6)]
        public string TrxId { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        [ChkValueSort(9)]
        public string MerPriv { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
}
