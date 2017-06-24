using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.Cash
{
    public class M_ReqExt
    {
        /// <summary>
        /// 手续费收取对象
        /// </summary>
        public string FeeObjFlag { set; get; }
        /// <summary>
        /// 手续费收取子帐户
        /// </summary>
        public string FeeAcctId { set; get; }
        /// <summary>
        /// 取现渠道
        /// </summary>
        public string CashChl { set; get; }

    }
}
