using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryTransStat
{
    public class ReQueryTransStat
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { get; set; }
        /// <summary>
        /// 应答返回码
        /// </summary>
        public string RespCode { get; set; }

        /// <summary>
        /// 应签描述
        /// </summary>
        public string RespDesc { get; set; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdId { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrdDate { get; set; }
        /// <summary>
        /// 交易查询类型
        /// </summary>
        public string QueryTransType { get; set; }

        /// <summary>
        /// 汇付交易状态
        /// </summary>
        public string TransStat { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }

    }
}
