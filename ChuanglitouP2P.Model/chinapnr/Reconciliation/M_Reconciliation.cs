using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.Reconciliation
{
    public class M_Reconciliation
    {

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { get; set; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public string PageNum { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public string PageSize { get; set; }

        /// <summary>
        /// 交易查询类型
        /// </summary>
        public string QueryTransType { get; set; }


        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }
    }
}
