using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.NetSave
{
    public class M_NetSave
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///消息类型
        /// </summary>
        public string CmdId { get; set; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }
        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { set; get; }
        /// <summary>
        /// 支付网关业务代号
        /// </summary>
        public string GateBusiId { set; get; }
        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { set; get; }
        /// <summary>
        /// 借贷记标记
        /// </summary>
        public string DcFlag { set; get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { set; get; }
        /// <summary>
        /// 页面返回URL
        /// </summary>
        public string RetUrl { set; get; }
        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        public string BgRetUrl { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }



    }
}
