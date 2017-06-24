using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.FssTrans
{
    public class M_FssTrans
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
        /// 订单号
        /// </summary>
        public string OrdId { set; get; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { set; get; }

        /// <summary>
        /// 页面返回url
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
        /// 入参扩展域
        /// </summary>
        public string ReqExt { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkVale { set; get; }
    }
}
