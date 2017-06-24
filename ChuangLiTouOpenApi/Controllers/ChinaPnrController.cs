using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChinaPnrController : ApiController
    {
        [System.Web.Http.HttpPost]
        public ResultInfo<UserEntity> GetChkValue(UserEntity m)
        {

#pragma warning disable 1587
            ///签名规则
            /// Version
            /// CmdId
            /// MerCustId
            /// BgRetUrl
            /// RetUrl
            /// UsrId
            /// UsrName
            /// IdType 
            /// IdNo
            /// UsrMp
            /// UsrEmail
            /// MerPriv
#pragma warning restore 1587
            m.MerId = Settings.Instance.MerId;
            StringBuilder chkVal = new StringBuilder();

            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.UsrId);
            chkVal.Append(System.Web.HttpUtility.UrlEncode(m.UsrName, System.Text.Encoding.UTF8));
            chkVal.Append(m.IdType);
            chkVal.Append(m.IdNo);
            chkVal.Append(m.UsrMp);
            chkVal.Append(m.UsrEmail);



            string chkv = chkVal.ToString(); 

            //私钥文件
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            ResultInfo<UserEntity> r = new ResultInfo<UserEntity>("1");
            m.UsrName = HttpUtility.UrlEncode(m.UsrName);
            r.body = m;

            return r;
        }

    }
}