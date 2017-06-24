using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.AddBidInfo
{
    public class R_AddBidInfo
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
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }

        /// <summary>
        /// 借款人ID
        /// </summary>
        public string BorrCustId { set; get; }

        /// <summary>
        /// 借款总金额
        /// </summary>
        public string BorrTotAmt { set; get; }

        /// <summary>
        /// 担保公司ID
        /// </summary>
        public string GuarCompId { set; get; }

        /// <summary>
        /// 担保金额
        /// </summary>
        public string GuarAmt { set; get; }

        /// <summary>
        /// 项目所在地
        /// </summary>
        public string ProArea { set; get; }

        /// <summary>
        /// 商户后台应答地址
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
