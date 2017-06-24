using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.UserRegister
{
    /// <summary>
    /// 用户开户
    /// </summary>
    public class M_UserRegister
    {


        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }


        /// <summary>
        /// 商户号
        /// </summary>
        public string MerId { get; set; }


        /// <summary>
        /// 消息类型 UserRegister
        /// </summary>
        public string CmdId { get; set; }

        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { get; set; }

        /// <summary>
        /// 商户后台应签地址
        /// </summary>
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 页面返回Url
        /// </summary>
        public string RetUrl { get; set; }

        /// <summary>
        /// 用户号
        /// </summary>
        public string UsrId { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string UsrName { get; set; }


        /// <summary>
        /// 证个把类型 00-身份证
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

        //用户eamil
        public string UsrEmail { get; set; }

        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 编码集
        /// </summary>
        public string CharSet { get; set; }

        
        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { get; set; }




    }

}
