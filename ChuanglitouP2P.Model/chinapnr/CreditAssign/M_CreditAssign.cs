using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CreditAssign
{
    public class M_CreditAssign
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
        /// 扣款手续费
        /// </summary>
        public string Fee { set; get; }
        /// <summary>
        /// 分账账号串
        /// </summary>
        public string DivDetails { set; get; }

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
        /// <summary>
        /// 挂牌债权ID ,可选,本次挂牌转让的债权编号。编号由商户定义，不可以重复。
        /// </summary>
        public string LcId { set; get; }
        /// <summary>
        /// 挂牌债权总金额,可选,本次挂牌转让的债权总金额，传挂牌债权ID时，挂牌债权总金额为必须
        /// </summary>
        public string TotalLcAmt { set; get; }
        /// <summary>
        /// 页面类型,可选 此参数只是适用于移动端，PC端无需关注此参数。三种移动端类型页面
        ///PageType为空：即自适应风格页面
        ///PageType=1：app应用风格页面（无标题）
        ///PageType=2：app应用风格页面（有标题）
        /// </summary>
        public string PageType { set; get; }
    }
}
