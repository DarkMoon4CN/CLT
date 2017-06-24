using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Cash;
using ChuanglitouP2P.Model.chinapnr.InitiativeTender;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using ChuanglitouP2P.Model.chinapnr.QueryCardInfo;
using ChuanglitouP2P.Model.chinapnr.Repayment;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuanglitouP2P.Model.chinapnr.UserBindCard;
using ChuanglitouP2P.Model.chinapnr.UserLogin;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using ChuanglitouP2P.Model.chinapnr.UsrFreezeBg;
using ChuanglitouP2P.Model.chinapnr.UsrUnFreeze;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;

namespace ChuanglitouP2P.Common.chinapnr
{
    public class ChinapnrFacade
    {
        /// <summary>
        /// 根据用户客户号获取用户资金信息  ---待重构
        /// </summary>
        /// <param name="UsrCustId">用户汇付客户号</param>
        /// <returns>ReQueryBalanceBg 汇付帐户余额实体对象</returns>
        public static ReQueryBalanceBg QueryBalance(string usrCustId)
        {
            if (usrCustId != null && usrCustId.Length > 0)
            {
                string strLog = "";
                M_QueryBalanceBg m = new M_QueryBalanceBg();
                m.Version = "10";
                m.CmdId = "QueryBalanceBg";
                m.MerCustId = Utils.GetMerCustID();
                m.UsrCustId = usrCustId;
                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(m.Version);
                chkVal.Append(m.CmdId);
                chkVal.Append(m.MerCustId);
                chkVal.Append(m.UsrCustId);
                string chkv = chkVal.ToString();
                strLog = "后台余额同步：加签chkv字符:" + chkv;
                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
                strLog += "；加签字符:" + str.ToString();
                m.ChkValue = sbChkValue.ToString();
                strLog += "；提交信息：" + FastJSON.toJOSN(m);
                strLog += "；ChkValue:" + m.ChkValue;
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values.Add("Version", m.Version);
                    values.Add("CmdId", m.CmdId);
                    values.Add("MerCustId", m.MerCustId);
                    values.Add("UsrCustId", m.UsrCustId);
                    values.Add("ChkValue", m.ChkValue);
                    string url = Utils.GetChinapnrUrl();
                    //同步发送form表单请求
                    byte[] result = client.UploadValues(url, "POST", values);
                    var retStr = Encoding.UTF8.GetString(result);
                    // Response.Write(retStr);
                    LogInfo.WriteLog(retStr);
                    ReQueryBalanceBg reg = new ReQueryBalanceBg();
                    var retloan = (ReQueryBalanceBg)FastJSON.ToObject(retStr, reg);
                    StringBuilder builder = new StringBuilder();
                    builder.Append(retloan.CmdId);
                    builder.Append(retloan.RespCode);
                    //builder.Append(retloan.RespDesc);
                    builder.Append(retloan.MerCustId);
                    builder.Append(retloan.UsrCustId);
                    builder.Append(retloan.AvlBal);
                    builder.Append(retloan.AcctBal);
                    builder.Append(retloan.FrzBal);
                    //  builder.Append(retloan.ChkValue);
                    var msg = builder.ToString();
                    strLog += "；返回参数:" + msg;
                    //验签
                    string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                    int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);
                    strLog += "；验签ret:" + ret.ToString();
                    LogInfo.WriteLog(strLog);
                    if (ret == 0)
                    {
                        return retloan;
                    }
                }
            }
            return null;
        }

        //TODO 3.0接口还款
        public static ReRepayment Repayment3(M_Repayment mr)
        {
            mr.Version = "30";
            mr.CmdId = "Repayment";
            mr.MerCustId = Utils.GetMerCustID();
            mr.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Su_Repayment");

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(mr.Version);
            chkVal.Append(mr.CmdId);
            chkVal.Append(mr.MerCustId);
            chkVal.Append(mr.ProId);
            chkVal.Append(mr.OrdId);
            chkVal.Append(mr.OrdDate);
            chkVal.Append(mr.OutCustId);
            chkVal.Append(mr.SubOrdId);
            chkVal.Append(mr.SubOrdDate);
            chkVal.Append(mr.OutAcctId);
            //chkVal.Append(mr.TransAmt);
            chkVal.Append(mr.PrincipalAmt);
            chkVal.Append(mr.InterestAmt);
            chkVal.Append(mr.Fee);
            chkVal.Append(mr.InCustId);
            chkVal.Append(mr.InAcctId);
            chkVal.Append(mr.DivDetails);
            chkVal.Append(mr.FeeObjFlag);
            chkVal.Append(mr.DzObject);
            chkVal.Append(mr.BgRetUrl);
            chkVal.Append(mr.MerPriv);
            chkVal.Append(mr.ReqExt);
            string chkv = Utils.MD5(chkVal.ToString());
            LogInfo.WriteLog("还款：" + chkv);
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            LogInfo.WriteLog(str.ToString());
            mr.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("还款提交信息：" + FastJSON.toJOSN(mr));
            LogInfo.WriteLog("还款ChkValue:" + mr.ChkValue);
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", mr.Version);
                values.Add("CmdId", mr.CmdId);
                values.Add("MerCustId", mr.MerCustId);
                values.Add("ProId", mr.ProId);
                values.Add("OrdId", mr.OrdId);
                values.Add("OrdDate", mr.OrdDate);
                values.Add("OutCustId", mr.OutCustId);
                values.Add("SubOrdId", mr.SubOrdId);
                values.Add("SubOrdDate", mr.SubOrdDate);
                values.Add("OutAcctId", mr.OutAcctId);
                //values.Add("TransAmt", MR.TransAmt);
                values.Add("PrincipalAmt", mr.PrincipalAmt);
                values.Add("InterestAmt", mr.InterestAmt);

                values.Add("Fee", mr.Fee);
                values.Add("InCustId", mr.InCustId);
                values.Add("InAcctId", mr.InAcctId);
                values.Add("DivDetails", mr.DivDetails);
                values.Add("FeeObjFlag", mr.FeeObjFlag);
                values.Add("DzObject", mr.DzObject);
                values.Add("BgRetUrl", mr.BgRetUrl);
                values.Add("MerPriv", mr.MerPriv);
                values.Add("ReqExt", mr.ReqExt);
                values.Add("ChkValue", mr.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                var _result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(_result);

                LogInfo.WriteLog("还款同步form表单请求" + retStr);
                ReRepayment Rere = new ReRepayment();
                var re = (ReRepayment)FastJSON.ToObject(retStr, Rere);
                LogInfo.WriteLog("还款返回报文：" + FastJSON.toJOSN(re));
                StringBuilder builder = new StringBuilder();
                builder.Append(re.CmdId);
                builder.Append(re.RespCode);
                builder.Append(re.MerCustId);
                builder.Append(re.ProId);
                builder.Append(re.OrdId);
                builder.Append(re.OrdDate);
                builder.Append(re.OutCustId);
                builder.Append(re.SubOrdId);
                builder.Append(re.SubOrdDate);
                builder.Append(re.OutAcctId);
                builder.Append(re.PrincipalAmt);
                builder.Append(re.InterestAmt);
                //builder.Append(Re.TransAmt);
                builder.Append(re.Fee);
                builder.Append(re.InCustId);
                builder.Append(re.InAcctId);
                builder.Append(re.FeeObjFlag);
                builder.Append(re.DzObject);
                builder.Append(HttpUtility.UrlDecode(re.BgRetUrl));
                builder.Append(HttpUtility.UrlDecode(re.MerPriv));
                builder.Append(HttpUtility.UrlDecode(re.RespExt));
                // builder.Append(Re.ChkValue);
                var msg = Utils.MD5(builder.ToString());
                re.TransAmt = re.PrincipalAmt + re.InterestAmt;
                LogInfo.WriteLog("还款返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, re.ChkValue);
                LogInfo.WriteLog("验签 ret= " + ret.ToString());
                if (ret == 0)
                {
                    return re;
                }
            }
            return null;
        }

        #region 开户
        /// <summary>
        /// 构造用户开户对象及签名
        /// </summary>
        /// <param name="mobile">用户手机号</param>
        /// <param name="userName">用户名(当前同为手机号)</param>
        /// <returns></returns>
        public static M_UserRegister UserRegister(string mobile, string userName)
        {
            M_UserRegister m = new M_UserRegister();
            m.MerId = Utils.GetMerId();
            m.Version = "10";
            m.CmdId = "UserRegister";
            m.MerCustId = Utils.GetMerCustID();
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/Bg_Succ_Registered");
            m.RetUrl = Utils.GetRe_url("Register/Succ_Registered");
            m.UsrMp = mobile;
            m.UsrId = userName;

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.UsrId);
            chkVal.Append(m.UsrMp);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("注册开户请求报文:" + FastJSON.toJOSN(m));
            return m;
        }
        /// <summary>
        /// 处理用户注册回调,实体封装及验签。验签失败返回null
        /// </summary>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns>用户注册回调实体对象，验签失败返回null</returns>
        public static ReUserRegister UserRegisterCallBack(bool isFront = true)
        {
            ReUserRegister m = new ReUserRegister();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = DNTRequest.GetString("RespDesc");
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.UsrId = DNTRequest.GetString("UsrId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.RetUrl = DNTRequest.GetString("RetUrl");
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.IdType = DNTRequest.GetString("IdType");
            m.IdNo = DNTRequest.GetString("IdNo");
            m.UsrMp = DNTRequest.GetString("UsrMp");
            m.UsrEmail = DNTRequest.GetString("UsrEmail");
            m.UsrName = HttpUtility.UrlDecode(DNTRequest.GetString("UsrName"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            LogInfo.WriteLog("注册开户" + (isFront ? "前台" : "后台" + "返回报文:") + FastJSON.toJOSN(m));
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.MerPriv);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            LogInfo.WriteLog("；验签ret:" + ret.ToString());
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        #endregion

        #region 提现
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="userCustId"></param>
        /// <param name="cashAmt">提现金额</param>
        /// <param name="openAcctId">卡号</param>
        /// <param name="cashChl">GENERAL:一般提现  FAST:快速取现  IMMEDIATE:即时取现 </param>
        /// <param name="servFee">服务费</param>
        /// <param name="feeObjFlag">商家（M）/用户（U）付手费续</param>
        /// <returns></returns>
        public static M_Cash Cash(string userCustId, string cashAmt, string openAcctId, string cashChl, string servFee = "0.00", string feeObjFlag = "M")
        {
            M_Cash m = new M_Cash();
            m.Version = "20";
            m.CmdId = "Cash";
            m.MerCustId = Utils.GetMerCustID();
            m.OrdId = Utils.Createcode();
            m.UsrCustId = userCustId;
            m.TransAmt = cashAmt;

            /*普通取现不收会员手续费*/
            m.ServFee = servFee;
            m.ServFeeAcctId = Utils.GetMERDT();
            m.OpenAcctId = openAcctId;
            m.RetUrl = Utils.GetRe_url("Cash/PostGENERALAmt");
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/PostGENERALAmt");
            m.Remark = "";
            m.CharSet = "UTF-8";
            m.MerPriv = "chuanglitou";

            M_ReqExt mr = new M_ReqExt();
            mr.FeeObjFlag = feeObjFlag;
            mr.FeeAcctId = Utils.GetMERDT();
            mr.CashChl = cashChl;
            m.ReqExt = "[" + FastJSON.toJOSN(mr) + "]";
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.ServFee);
            chkVal.Append(m.ServFeeAcctId);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.Remark);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.ReqExt);
            string chkv = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("加签字符:" + ret.ToString() + "提现渠道" + mr.CashChl + "取现chkv字符:" + chkv);
            LogInfo.WriteLog("提现渠道" + mr.CashChl + "提现提交表单报文:" + FastJSON.toJOSN(m));
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        /// <summary>
        /// 计算提现手续费
        /// </summary>
        /// <param name="cashAmt">提现金额</param>
        /// <param name="cashChl">GENERAL:一般提现  FAST:快速取现  IMMEDIATE:即时取现</param>
        /// <param name="feeObjFlag">商家（M）/用户（U）付手费续</param>
        /// <returns></returns>
        public static decimal CalcCashFee(string cashAmt, string cashChl, string feeObjFlag = "M")
        {
            decimal res = 0;
            if (feeObjFlag == "M")
                res = 0;
            if (cashChl == "GENERAL")
                res = 2;
            if (cashChl == "FAST" || cashChl == "IMMEDIATE")
            {
                res = decimal.Parse(cashAmt) * 5 / 10000 + 2;
            }
            return res;
        }
        /// <summary>
        /// 提现回调
        /// </summary>
        /// <param name="ret">out 返回状态值</param>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns></returns>
        public static ReCash CashCallBack(out int ret, bool isFront = true)
        {
            ReCash m = new ReCash();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");
            m.ServFee = DNTRequest.GetString("ServFee");
            m.ServFeeAcctId = DNTRequest.GetString("ServFeeAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            m.RealTransAmt = DNTRequest.GetString("RealTransAmt");
            //验签
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.FeeAmt);
            chkVal.Append(m.FeeCustId);
            chkVal.Append(m.FeeAcctId);
            chkVal.Append(m.ServFee);
            chkVal.Append(m.ServFeeAcctId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.RespExt);
            string msg = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);

            LogInfo.WriteLog("取现接口" + (isFront ? "前台" : "后台") + "返回结果验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc);
            LogInfo.WriteLog("取现接口" + (isFront ? "前台" : "后台") + "返回报文:" + FastJSON.toJOSN(m));
            if (ret == 0)
            {
                return m;
            }
            return null;
        }

        #endregion

        #region  投资 
        /// <summary>
        /// 投资 ,调用此方法之前必须做冻结 (M_td_frozen) 操作
        /// </summary>
        /// <param name="p">必须参数 OrdId,invest_time,investment_amount,investment_amount,targetid</param>
        /// <param name="usrCustId">up.UsrCustId</param>
        /// <param name="borrUsrCustId">dt.Rows[0]["BorrUsrCustId"].ToString()</param>
        /// <param name="isinterest">0 代金券   1 加息劵</param>
        /// <param name="vocheramttemp">优惠金额</param>
        /// <param name="rewardsids">附带参数，用于代金券或加息劵编号</param>
        /// <param name="frozenidNo">冻结号</param>
        /// <param name="isWeb">是否是网页端生成</param>
        /// <returns></returns>
        public static M_InitiativeTender InitiativeTender(M_Bid_records p, string usrCustId, string borrUsrCustId, int isinterest, decimal vocheramttemp, string rewardsids, string frozenidNo, bool isWeb = true)
        {
            M_InitiativeTender m = new M_InitiativeTender();
            m.Version = "20";
            m.CmdId = "InitiativeTender";
            m.MerCustId = Utils.GetMerCustID();
            m.OrdId = p.OrdId.ToString();
            m.OrdDate = p.invest_time.ToString("yyyyMMdd");
            m.TransAmt = p.investment_amount.ToString("0.00");
            m.UsrCustId = usrCustId;
            m.MaxTenderRate = "0.20";
            TenderJosnPro mtp = new TenderJosnPro();
            mtp.BorrowerCustId = borrUsrCustId;
            //mtp.BorrowerAmt =decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()).ToString("0.00");
            mtp.BorrowerAmt = p.investment_amount.ToString("0.00");
            // mtp.BorrowerRate = decimal.Parse( dt.Rows[0]["loan_management_fee"].ToString()).ToString("0.00");
            mtp.BorrowerRate = "1.00"; //风控范围
            mtp.ProId = p.targetid.ToString();
            m.BorrowerDetails = "[" + FastJSON.toJOSN(mtp) + "]";

            #region 此处判断优惠类型
            if (isinterest == 0)  //代金券
            {
                TenderAccPro reqExt = new TenderAccPro();
                reqExt.AcctId = Utils.GetMERDT();
                reqExt.VocherAmt = vocheramttemp.ToString("0.00");
                if (rewardsids.Length > 0)
                {
                    m.ReqExt = "{" + "\"Vocher\":" + FastJSON.toJOSN(reqExt) + "}";
                }
            }
            #endregion
            m.IsFreeze = "Y";
            m.FreezeOrdId = frozenidNo;
            //Mt.RetUrl = "http://localhost:17745/investment_success_" + targetid.ToString() + ".html";
            m.RetUrl = Utils.GetRe_url((isWeb ? "" : "Home/") + "investment_success" + (isWeb ? "_" : "/") + p.targetid.ToString() + (isWeb ? ".html" : ""));
            //Mt.RetUrl = Utils.GetRe_url("Home/investment_success/" + targetid.ToString());
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/BG_investment_success");
            // Mt.BgRetUrl = Utils.GetRe_url("666Thirdparty/BG_investment_success");
            m.MerPriv = rewardsids;
            LogInfo.WriteLog("优惠券使用的id:" + rewardsids);

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.MaxTenderRate);
            chkVal.Append(m.BorrowerDetails);
            chkVal.Append(m.IsFreeze);
            chkVal.Append(m.FreezeOrdId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.ReqExt);
            string chkv = chkVal.ToString();
            LogInfo.WriteLog(chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("加签字符:" + ret.ToString() + "投标chkv字符:" + chkv);
            LogInfo.WriteLog("投标提交表单报文:" + FastJSON.toJOSN(m));
            if (ret == 0)
            {
                return m;
            }
            return null;
        }

        /// <summary>
        /// 投资回调
        /// </summary>
        /// <param name="ret">out 返回状态值</param>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns></returns>
        public static ReInitiativeTender InitiativeTenderCallBack(out int ret, bool isFront = true)
        {
            ReInitiativeTender p = new ReInitiativeTender();
            int id = DNTRequest.GetInt("id", 0);
            LogInfo.WriteLog("主动通知后台有响应成功!接收到的项目id=" + id);
            p.CmdId = DNTRequest.GetString("CmdId");
            p.RespCode = DNTRequest.GetString("RespCode");
            p.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            p.MerCustId = DNTRequest.GetString("MerCustId");
            p.OrdId = DNTRequest.GetString("OrdId");
            p.OrdDate = DNTRequest.GetString("OrdDate");
            p.TransAmt = DNTRequest.GetString("TransAmt");
            p.UsrCustId = DNTRequest.GetString("UsrCustId");
            p.TrxId = DNTRequest.GetString("TrxId");
            p.IsFreeze = DNTRequest.GetString("IsFreeze");
            p.FreezeOrdId = DNTRequest.GetString("FreezeOrdId");
            p.FreezeTrxId = DNTRequest.GetString("FreezeTrxId");
            p.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            p.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            string merp = DNTRequest.GetString("MerPriv");
            if (merp.Length > 0)
            {
                p.MerPriv = HttpUtility.UrlDecode(merp);
            }
            else
            {
                p.MerPriv = merp;
            }

            p.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            p.ChkValue = DNTRequest.GetString("ChkValue");
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(p.CmdId);
            chkVal.Append(p.RespCode);
            chkVal.Append(p.MerCustId);
            chkVal.Append(p.OrdId);
            chkVal.Append(p.OrdDate);
            chkVal.Append(p.TransAmt);
            chkVal.Append(p.UsrCustId);
            chkVal.Append(p.TrxId);
            chkVal.Append(p.IsFreeze);
            chkVal.Append(p.FreezeOrdId);
            chkVal.Append(p.FreezeTrxId);
            chkVal.Append(p.RetUrl);
            chkVal.Append(p.BgRetUrl);
            chkVal.Append(p.MerPriv);
            chkVal.Append(p.RespExt);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);
            LogInfo.WriteLog("投标接口" + (isFront ? "前台" : "后台") + "返回结果验签:ret=" + ret.ToString() + " RespCode:" + p.RespCode + p.RespDesc);
            LogInfo.WriteLog("投标接口" + (isFront ? "前台" : "后台") + "返回报文:" + FastJSON.toJOSN(p));
            if (ret == 0)
            {
                return p;
            }
            return null;
        }
        #endregion

        #region 充值
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="p">用户信息实体</param>
        /// <param name="rh">充值记录信息</param>
        /// <param name="blankName">充值银行 CMB CCB 等等</param>
        /// <param name="recid">充值记录信息ID</param>
        /// <param name="isQP">是否是快捷支付</param>
        /// <returns></returns>
        public static M_QPNetSave NetSave(M_member_table p, M_Recharge_history rh, string blankName = "CMB", int recid = 0, bool isQP = false)
        {
            M_QPNetSave m = new M_QPNetSave();
            string MerPriv = DESEncrypt.Encrypt(p.registerid.ToString() + "_" + recid.ToString(), ConfigurationManager.AppSettings["webp"].ToString());
            string cmdId = "NetSave";
            string merCustId = Utils.GetMerCustID();
            m.Version = "10";
            m.CmdId = cmdId;
            m.MerCustId = merCustId;
            m.UsrCustId = p.UsrCustId;
            m.OrdId = rh.order_No;
            m.OrdDate = rh.recharge_time.ToString("yyyyMMdd");

            if (isQP == false)
            {
                if (p.UsrCustId == Utils.GetDanbaoCustID())
                {
                    m.GateBusiId = "B2B"; //企业网银
                }
                else
                {
                    m.GateBusiId = "B2C";
                }
            }
            else
            {
                m.GateBusiId = "QP";  //快捷支付
            }
            m.OpenBankId = blankName;
            m.DcFlag = "D";
            m.TransAmt = rh.account_amount.ToString();
            m.RetUrl = Utils.GetRe_url("usercenter/SuQPNetSave");
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/ReQPNetSave");
            m.MerPriv = MerPriv;

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.GateBusiId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.DcFlag);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.MerPriv);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("加签字符:" + ret.ToString() + "投标chkv字符:" + chkv);
            LogInfo.WriteLog("投标提交表单报文:" + FastJSON.toJOSN(m));
            return m;
        }

        /// <summary>
        /// 处理用户充值回调,实体封装及验签。验签失败返回null
        /// </summary>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns>用户充值回调实体对象，验签失败返回null</returns>
        public static ReQPNetSave NetSaveCallBack(out int ret, bool isFront = true)
        {
            ReQPNetSave m = new ReQPNetSave();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.OrdDate = DNTRequest.GetString("OrdDate");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.GateBusiId = DNTRequest.GetString("GateBusiId");
            m.GateBankId = DNTRequest.GetString("GateBankId");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.CardId = DNTRequest.GetString("CardId");
            m.MerPriv = HttpUtility.UrlDecode(DNTRequest.GetString("MerPriv"));
            // m.MerPriv = DESEncrypt.Decrypt(DNTRequest.GetString("MerPriv"), ConfigurationManager.AppSettings["webp"].ToString());
            m.ChkValue = DNTRequest.GetString("ChkValue");

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            string msg = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);
            // Response.Write("验签：" + ret.ToString());
            LogInfo.WriteLog((m.GateBusiId == "QP" ? "快捷" : "网银") + "充值接口验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc);
            LogInfo.WriteLog((m.GateBusiId == "QP" ? "快捷" : "网银") + "充值接口" + (isFront ? "前台" : "后台") + "充值返回报文:" + FastJSON.toJOSN(m));
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        #endregion

        #region 平台向用户转账
        /// <summary>
        /// 平台向用户转账
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="UsrCustId">客户号</param>
        /// <returns></returns>
        public static ReTransfer platformToUserMoney(decimal money, string usrCustId)
        {
            M_Transfer m = new M_Transfer();
            m.Version = "10";
            m.CmdId = "Transfer";

            m.OrdId = Utils.Createcode();
            m.OutCustId = Utils.GetMerCustID();
            m.OutAcctId = "MDT000001";
            m.TransAmt = money.ToString("0.00");
            m.InCustId = Utils.CheckSQLHtml(usrCustId);
            m.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgToUserTransfer");

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
            LogInfo.WriteLog("加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            LogInfo.WriteLog("加签字符:" + str.ToString());

            m.ChkValue = sbChkValue.ToString();

            LogInfo.WriteLog("提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("ChkValue:" + m.ChkValue);

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

                LogInfo.WriteLog("自动扣款转账（商户用）返回报文" + retStr);


                ReTransfer reg = new ReTransfer();

                var retloan = (ReTransfer)FastJSON.ToObject(retStr, reg);

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

                LogInfo.WriteLog("返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                LogInfo.WriteLog("验签ret:" + ret.ToString());

                if (ret == 0)
                {
                    return retloan;
                }
            }
            return null;
        }

        /// <summary>
        /// 平台向用户转账 回调
        /// </summary>
        /// <param name="ret">out 返回状态值</param>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns></returns>
        public static ReTransfer TransferCallBack(out int ret, bool isFront = true)
        {
            ReTransfer m = new ReTransfer();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.OrdId = DNTRequest.GetString("OrdId");
            m.OutCustId = DNTRequest.GetString("OutCustId");
            m.OutAcctId = DNTRequest.GetString("OutAcctId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.InCustId = DNTRequest.GetString("InCustId");
            m.InAcctId = DNTRequest.GetString("InAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));

            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.ChkValue = DNTRequest.GetString("ChkValue");

            string chkv = CFormHelper.GetChkValue<ReTransfer>(m);
            LogInfo.WriteLog(chkv);

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            LogInfo.WriteLog("平台向用户转账" + (isFront ? "前台" : "后台" + "返回报文:") + FastJSON.toJOSN(m));
            LogInfo.WriteLog("平台向用户活动转账后---1台验签：" + ret.ToString());
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        #endregion

        #region 绑卡
        /// <summary>
        /// 绑卡
        /// </summary>
        /// <param name="usrCustId">用户客户号</param>
        /// <returns></returns>
        public static M_UserBindCard UserBindCard(string usrCustId)
        {
            M_UserBindCard m = new M_UserBindCard();
            m.Version = "10";
            m.CmdId = "UserBindCard";
            m.MerCustId = Utils.GetMerCustID();
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/Bgthirdpartybindbank");
            m.MerPriv = Utils.Base64Encoder("chuanglitou");
            m.UsrCustId = usrCustId;

            string chkv = CFormHelper.GetChkValue<M_UserBindCard>(m);
            LogInfo.WriteLog(chkv);

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("加签字符:" + ret.ToString() + "绑卡chkv字符:" + chkv);
            LogInfo.WriteLog("绑卡提交表单报文:" + FastJSON.toJOSN(m));
            if (ret == 0)
            {
                return m;
            }
            return null;
        }


        /// <summary>
        /// 绑卡回调 
        /// </summary>
        /// <param name="ret">out 返回状态值</param>
        /// <param name="isFront">是否是前台form形式调用</param>
        /// <returns></returns>
        public static ReUserBindCard UserBindCardCallBack(out int ret, bool isFront = true)
        {
            ReUserBindCard m = new ReUserBindCard();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = HttpUtility.UrlDecode(DNTRequest.GetString("MerPriv"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            string chkv = CFormHelper.GetChkValue<ReUserBindCard>(m);
            LogInfo.WriteLog(chkv);
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            LogInfo.WriteLog("绑卡" + (isFront ? "前台" : "后台" + "返回报文:") + FastJSON.toJOSN(m));
            LogInfo.WriteLog("绑卡---1台验签：" + ret.ToString());
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        #endregion

        #region 卡信息查询
        /// <summary>
        /// 卡信息查询
        /// </summary>
        /// <param name="usrCustId">用户客户号</param>
        /// <returns></returns>
        public static M_QueryCardInfo QueryCardInfo(string usrCustId)
        {
            if (string.IsNullOrWhiteSpace(usrCustId)) { return null; }
            M_QueryCardInfo m = new M_QueryCardInfo();
            m.Version = "10";
            m.CmdId = "QueryCardInfo";
            m.MerCustId = Utils.GetMerCustID();
            m.UsrCustId = usrCustId;
            string chkv = CFormHelper.GetChkValue<M_QueryCardInfo>(m);
            LogInfo.WriteLog(chkv);
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            if (ret == 0)
            {
                return m;
            }
            return null;
        }
        #endregion

        #region 登录汇付
        /// <summary>
        /// 汇付登录  
        /// </summary>
        /// <param name="usrCustId">用户客户号</param>
        /// <returns></returns>
        public static M_UserLogin UserLogin(string usrCustId)
        {
            M_UserLogin m = new M_UserLogin();
            m.Version = "10";
            m.CmdId = "UserLogin";
            m.MerCustId = Utils.GetMerCustID();
            m.UsrCustId = usrCustId;
            return m;
        }
        #endregion

        #region 用户资金解冻
        /// <summary>
        /// 用户资金解冻
        /// </summary>
        /// <param name="ordDate">冻结单</param>
        /// <param name="freezeTrxId">冻结号</param>
        /// <returns></returns>
        public static M_UsrUnFreeze UsrUnFreeze(string ordDate, string freezeTrxId)
        {
            M_UsrUnFreeze m = new M_UsrUnFreeze();
            m.Version = "10";
            m.CmdId = "UsrUnFreeze";
            m.MerCustId = Utils.GetMerCustID();
            m.OrdId = Utils.Createcode();
            m.OrdDate = ordDate;
            m.TrxId = freezeTrxId;
            m.RetUrl = "";
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/BG_UsrUnFreeze");
            m.MerPriv = Utils.Base64Encoder("chuanglitou");
            string chkv = CFormHelper.GetChkValue<M_UsrUnFreeze>(m);
            LogInfo.WriteLog("解冻加签chkv字符:" + chkv);
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            int ret = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("解冻提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("解冻ChkValue:" + m.ChkValue);
            if (ret == 0)
            {
                return m;
            }
            return null;
        }

        public static ReUsrFreezeBg UsrUnFreezeCallBack(out int ret)
        {
            ReUsrFreezeBg m = new ReUsrFreezeBg();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.OrdDate = DNTRequest.GetString("OrdDate");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = HttpUtility.UrlDecode(DNTRequest.GetString("MerPriv"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            //m.UsrCustId = DNTRequest.GetString("UsrCustId");

            string chkv = CFormHelper.GetChkValue<ReUsrFreezeBg>(m);
            LogInfo.WriteLog("解冻加签chkv字符:" + chkv);
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            LogInfo.WriteLog("后台解冻加签信息---1台验签：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("解冻加签chkv字符---1台验签：" + ret.ToString());

            if (ret == 0)
            {
                return m;
            }
            return null;
        }

        #endregion
    }
}
