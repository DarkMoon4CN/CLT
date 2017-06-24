using ChuanglitouP2P.Model.chinapnr.ChinapnrAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ChuanglitouP2P.Model.chinapnr.UserBindCard
{

    /// <summary>
    /// 绑定用户银行卡
    /// </summary>
    public class M_UserBindCard
    {
        /// <summary>
        /// 版本
        /// </summary>
        [ChkValueSort(1)]
        public string Version { get; set; }

        /// <summary>
        /// 消息类型 UserBindCard
        /// </summary>
        [ChkValueSort(2)]
        public string CmdId { get; set; }

        /// <summary>
        /// 商户客户号
        /// </summary>
        [ChkValueSort(3)]
        public string MerCustId { get; set; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        [ChkValueSort(4)]
        public string UsrCustId { get; set; }


        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        [ChkValueSort(5)]
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 商户私有域
        /// </summary>
        [ChkValueSort(6)]
        public string MerPriv { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get;set; }
    }
}
