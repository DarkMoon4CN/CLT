using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.AutoCreditAssign
{
    public class ReAutoCreditAssign
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
        /// 转让人客户号
        /// </summary>
        public string SellCustId { set; get; }
        /// <summary>
        /// 转让金额
        /// </summary>
        public string CreditAmt { set; get; }
        /// <summary>
        /// 承接金额
        /// </summary>
        public string CreditDealAmt { set; get; }
        /// <summary>
        /// 扣款手续费
        /// </summary>
        public string Fee { set; get; }
        /// <summary>
        /// 承接人客户号
        /// </summary>
        public string BuyCustId { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { set; get; }
        /// <summary>
        /// 页面返回URL
        /// </summary>
        public string RetUrl { set; get; }
        /// <summary>
        ///商户后台应答地址 
        /// </summary>
        public string BgRetUrl { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }
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
