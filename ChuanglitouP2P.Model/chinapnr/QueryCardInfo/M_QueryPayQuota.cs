using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.chinapnr.QueryCardInfo
{
    /// <summary>
    /// 快捷充值限额信息查询接口
    /// </summary>
    public class M_QueryPayQuota
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { set; get; }
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }
    }
}
