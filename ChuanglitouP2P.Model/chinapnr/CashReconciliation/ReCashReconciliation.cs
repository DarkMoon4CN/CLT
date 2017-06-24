using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CashReconciliation
{
    public class ReCashReconciliation
    {

        /// <summary>
        ///消息类型
        /// </summary>
        public string CmdId { get; set; }

        /// <summary>
        /// 应答返回码
        /// </summary>
        public string RespCode { get; set; }

        /// <summary>
        /// 应答描述
        /// </summary>
        public string RespDesc { get; set; }
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
        /// 记录总条数
        /// </summary>
        public string TotalItems { get; set; }

        /// <summary>
        /// 手续费收取方
        /// </summary>
        public string FeeObj { get; set; }

        /// <summary>
        /// 取现对账结果串
        /// </summary>
        public string CashReconciliationDatoList { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }





    }
}
