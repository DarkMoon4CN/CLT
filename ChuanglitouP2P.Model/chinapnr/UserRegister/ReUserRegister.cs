using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.UserRegister
{
   public class ReUserRegister
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
       /// 应答描述
       /// </summary>
        public string RespDesc { get; set; }

       /// <summary>
       /// 商户客户号
       /// </summary>
        public string MerCustId { get; set; }

       /// <summary>
       /// 用户号
       /// </summary>
        public string UsrId { get; set; }

       /// <summary>
       /// 用户客户号
       /// </summary>
        public string UsrCustId { get; set; }

       /// <summary>
       /// 商户后台应答地址
       /// </summary>
        public string BgRetUrl { get; set; }

       /// <summary>
       /// 本平台交易唯一标识
       /// </summary>
        public string TrxId { get; set; }

       /// <summary>
       /// 页面返回URl
       /// </summary>
        public string RetUrl { get; set; }

       /// <summary>
       /// 商户私有域
       /// </summary>
        public string MerPriv { get; set; }
       /// <summary>
       /// 证件类型
       /// </summary>
        public string IdType { get; set; }

       /// <summary>
       /// 证件号码
       /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string UsrMp { get; set; }

       /// <summary>
       /// 用户Email
       /// </summary>
        public string UsrEmail { get; set; }
       /// <summary>
       /// 真实名称
       /// </summary>
        public string UsrName { get; set; }
       /// <summary>
       /// 签名
       /// </summary>
        public string ChkValue { get; set; }



    }
}
