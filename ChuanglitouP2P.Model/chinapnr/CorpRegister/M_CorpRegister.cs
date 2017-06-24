using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.CorpRegister
{
    public class M_CorpRegister
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
        /// 用户号
        /// </summary>
        public string UsrId { set; get; }
        /// <summary>
        /// 真实名称
        /// </summary>
        public string UsrName { set; get; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string InstuCode { set; get; }
        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string BusiCode { set; get; }

        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxCode { set; get; }
        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }
        /// <summary>
        /// 编码集
        /// </summary>
        public string CharSet { set; get; }
        /// <summary>
        /// 担保类型
        /// </summary>
        public string GuarType { set; get; }

        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        public string BgRetUrl { set; get; }

        /// <summary>
        /// 入参扩展域
        /// </summary>
        public string ReqExt { set; get; }
        /// <summary>
        /// 企业用户备案金
        /// </summary>
        public string GuarCorpEarnestAmt { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }
    }
}
