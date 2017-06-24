using ChuanglitouP2P.Model.chinapnr.ChinapnrAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.UsrUnFreeze
{
    public class M_UsrUnFreeze
    {

        /// <summary>
        /// 版本号
        /// </summary>
        [ChkValueSort(1)]
        public string Version { set; get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        [ChkValueSort(2)]
        public string CmdId { set; get; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        [ChkValueSort(3)]
        public string MerCustId { set; get; }
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
        /// 本平台交易唯一标识
        /// </summary>
        [ChkValueSort(6)]
        public string TrxId { set; get; }
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
