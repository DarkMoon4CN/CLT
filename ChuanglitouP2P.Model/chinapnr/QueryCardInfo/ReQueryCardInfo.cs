using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.QueryCardInfo
{
    public class ReQueryCardInfo
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
        /// 用户客户号
        /// </summary>
        public string UsrCustId { set; get; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string CardId { set; get; }




        /// <summary>
        /// 用户银行卡信息列表
        /// </summary>
        public List<M_UsrCardInfolist> UsrCardInfolist { get; set; }


        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }



    }
}
