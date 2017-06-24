namespace ChuangLiTou.Core.Entities.ChinaPnr
{
    /// <summary>
    /// Class SubmitTenderEntity.
    /// </summary>
    public class SubmitTenderEntity : BaseRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { set; get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { set; get; }
        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }
        /// <summary>
        /// 最大投资手续费率
        /// </summary>
        public string MaxTenderRate { set; get; }
        /// <summary>
        /// 借款人信息
        /// </summary>
        public string BorrowerDetails { set; get; }
        /// <summary>
        /// 借款人客户号
        /// </summary>
        public string BorrowerCustId { set; get; }
        /// <summary>
        /// 借款金额
        /// </summary>
        public string BorrowerAmt { set; get; }
        /// <summary>
        /// 借款手续费率
        /// </summary>
        public string BorrowerRate { set; get; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }
        /// <summary>
        /// 是否冻结
        /// </summary>
        public string IsFreeze { set; get; }
        /// <summary>
        /// 冻结订单号
        /// </summary>
        public string FreezeOrdId { set; get; }
        /// <summary>
        /// 页面返回URL
        /// </summary>
        public string RetUrl { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }
        /// <summary>
        /// 入参扩展域
        /// </summary>
        public string ReqExt { set; get; }
        /// <summary>
        /// 代金券出账子账户
        /// </summary>
        public string AcctId { set; get; }
        /// <summary>
        /// 代金券金额
        /// </summary>
        public string VocherAmt { set; get; }
    }
}
