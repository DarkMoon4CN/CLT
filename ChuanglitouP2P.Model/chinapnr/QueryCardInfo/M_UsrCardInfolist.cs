using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryCardInfo
{
    public  class M_UsrCardInfolist
    {
        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { set; get; }

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }
        /// <summary>
        /// 真实名称
        /// </summary>
        public string UsrName { set; get; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertId { set; get; }
        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string BankId { set; get; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string CardId { set; get; }

        /// <summary>
        /// 银行卡是否实名
        /// </summary>
        public string RealFlag { set; get; }


        /// <summary>
        /// 时间
        /// </summary>
        public string UpdDateTime { set; get; }

       /// <summary>
        /// 银行省份
       /// </summary> 
        public string ProvId { set; get; }

        /// <summary>
        /// 银行地区
        /// </summary>
        public string AreaId { set; get; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public string IsDefault { set; get; }


        /// <summary>
        /// 是否快捷卡 Y
        /// </summary>
        public string ExpressFlag { set; get; }



    }
}
