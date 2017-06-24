using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.AutoCreditAssign
{
    public class M_AutoCreditAssign
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { set; get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { set; get; }
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
        /// 债权转让明细 
        /// </summary>
        public string BidDetails { set; get; }
        /// <summary>
        /// 被转让的投标订单号
        /// </summary>
        public string BidOrdId { set; get; }
        /// <summary>
        /// 被转让的投标订单日期
        /// </summary>
        public string BidOrdDate { set; get; }
        /// <summary>
        /// 转让金额
        /// </summary>
        public string BidCreditAmt { set; get; }
        /// <summary>
        /// 借款人客户号
        /// </summary>
        public string BorrowerCustId { set; get; }
        /// <summary>
        /// 明细转让金额
        /// </summary>
        public string BorrowerCreditAmt { set; get; }
        /// <summary>
        /// 已还款金额
        /// </summary>
        public string PrinAmt { set; get; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }
        /// <summary>
        /// 扣款手续费
        /// </summary>
        public string Fee { set; get; }
        /// <summary>
        /// 分账账号串
        /// </summary>
        public string DivDetails { set; get; }
        /// <summary>
        /// 分账账户号
        /// </summary>
        public string DivAcctId { set; get; }
        /// <summary>
        /// 分账金额
        /// </summary>
        public string DivAmt { set; get; }
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
        /// 入参扩展域
        /// </summary>
        public string ReqExt { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }

    }
}
