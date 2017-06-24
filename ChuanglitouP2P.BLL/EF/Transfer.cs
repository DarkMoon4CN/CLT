using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 自动扣款转账（商户用）
    /// 商户向用户转帐 
    /// </summary>
    public class Transfer
    {

        #region 自动扣款转账（商户用）接口 商户向用户转帐
        /// <summary>
        /// 自动扣款转账（商户用）接口 商户向用户转帐 
        /// </summary>
        /// <param name="UsrCustId">客户号</param>
        /// <param name="TranAMT">转账金额</param>
        /// <param name="ReturnUrl">汇付返回主动通知页面如:admin/Thirdparty/BgToUserTransfer</param>
        /// <returns></returns>
        public ReTransfer ToUserTransfer(string UsrCustId, decimal TranAMT, string OrdId, string UserAct, string ReturnUrl)
        {
            M_Transfer m = new M_Transfer();
            ReTransfer retloan = new ReTransfer();
            m.Version = "10";
            m.CmdId = "Transfer";
            m.OrdId = OrdId;
            m.OutCustId = Utils.GetMerCustID();
            m.OutAcctId = "MDT000001";
            m.TransAmt = TranAMT.ToString("0.00");
            m.InCustId = UsrCustId;
            m.BgRetUrl = Utils.GetRe_url(ReturnUrl);
            m.MerPriv = UserAct;
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OutCustId);
            chkVal.Append(m.OutAcctId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.InCustId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();
            string log= "商户向用户转账加签chkv字符:" + chkv;

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            log+= "<br>加签字符:" + str.ToString();
            m.ChkValue = sbChkValue.ToString();
            log += "<br>提交信息：" + FastJSON.toJOSN(m);
            log += "<br>ChkValue:" + m.ChkValue;
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("OrdId", m.OrdId);
                values.Add("OutCustId", m.OutCustId);
                values.Add("OutAcctId", m.OutAcctId);
                values.Add("TransAmt", m.TransAmt);
                values.Add("InCustId", m.InCustId);
                values.Add("InAcctId", m.InAcctId);
                values.Add("RetUrl", m.RetUrl);
                values.Add("BgRetUrl", m.BgRetUrl);
                values.Add("MerPriv", m.MerPriv);
                values.Add("ChkValue", m.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);

                log += "<br>自动扣款转账（商户用）返回报文:" + retStr;
                ReTransfer reg = new ReTransfer();
                retloan = (ReTransfer)FastJSON.ToObject(retStr, reg);
                StringBuilder builder = new StringBuilder();
                builder.Append(retloan.CmdId);
                builder.Append(retloan.RespCode);
                builder.Append(retloan.OrdId);
                builder.Append(retloan.OutCustId);
                builder.Append(retloan.OutAcctId);
                builder.Append(retloan.TransAmt);
                builder.Append(retloan.InCustId);
                builder.Append(retloan.InAcctId);
                builder.Append(retloan.RetUrl);
                builder.Append(retloan.BgRetUrl);
                builder.Append(retloan.MerPriv);
                var msg = builder.ToString();

                log += "<br>商户向用户转账返回参数:" + msg;
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);
                log += "<br>商户向用户转账验签ret:" + ret.ToString();
                if (ret == 0)
                {
                    if (retloan.RespCode == "000")
                    {

                    }
                    else
                    {
                        retloan = null;
                    }
                }


            }
            LogInfo.WriteLog(log);
            return retloan;
        } 
        #endregion



    }
}
