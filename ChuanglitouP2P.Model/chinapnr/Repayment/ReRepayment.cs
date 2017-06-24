using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.Repayment
{
    public class ReRepayment
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
        /// 标的ID  可选  若是商户已有存管银行，则该字段必填
        /// </summary>
        public string ProId { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { set; get; }
        /// <summary>
        /// 出账客户号
        /// </summary>
        public string OutCustId { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SubOrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string SubOrdDate { set; get; }
        /// <summary>
        /// 出账子账户
        /// </summary>
        public string OutAcctId { set; get; }
        /// <summary>
        /// 交易金额     3.0接口废弃 
        /// </summary>
        /// 
        public string TransAmt { set; get; }
        /// <summary>
        /// 利息金额 3.0接口
        /// </summar
        public string InterestAmt { set; get; }

        /// <summary>
        /// 本金金额 3.0接口
        /// </summar
        public string PrincipalAmt { set; get; }
       
        /// <summary>
        /// 扣款手续费
        /// </summary>
        public string Fee { set; get; }
        /// <summary>
        /// 入账客户号
        /// </summary>
        public string InCustId { set; get; }
        /// <summary>
        /// 入账子账户
        /// </summary>
        public string InAcctId { set; get; }
        /// <summary>
        /// 续费收取对象标志I/O
        /// </summary>
        public string FeeObjFlag { set; get; }
        /// <summary>
        /// 垫资/代偿对象   如果是垫资还款必传商户或者担保企业 垫资/代偿对象
        /// </summary>
        public string DzObject { set; get; }
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
