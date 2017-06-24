using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.AddBidInfo;
using ChuanglitouP2P.Model.chinapnr.CashAudit;
using ChuanglitouP2P.Model.chinapnr.CorpRegister;
using ChuanglitouP2P.Model.chinapnr.Loans;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using ChuanglitouP2P.Model.chinapnr.Repayment;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuanglitouP2P.Model.chinapnr.UsrAcctPay;
using ChuangLitouP2P.Models;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ThirdpartyController : Controller
    {
        // GET: Admin/Thirdparty
        public ActionResult Index()
        {
            return View();
        }

        #region 开通担保公司汇付后台主动通知接口
        /// <summary>
        /// 开通担保公司汇付后台主动通知接口
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult BgCorpRegister()
        {
            string str = "";

            ReCorpRegister m = new ReCorpRegister();

            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.UsrId = DNTRequest.GetString("UsrId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.AuditStat = DNTRequest.GetString("AuditStat");
            m.AuditDesc = DNTRequest.GetString("AuditDesc");
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.CardId = DNTRequest.GetString("CardId");
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            m.RespExt = DNTRequest.GetString("RespExt");
            LogInfo.WriteLog("开户的返回参数:" + FastJSON.toJOSN(m));
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrId);
            //chkVal.Append(m.UsrName);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.AuditStat);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.CardId);
            chkVal.Append(m.BgRetUrl);
            // chkVal.Append(m.RespExt);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            //LogInfo.WriteLog("企业开户验签:" + ret.ToString());
            //if (ret == 0)
            // {
            if (m.RespCode == "000")
            {
                if (m.AuditStat == "Y")
                {
                    string sql = "update  hx_bonding_company  set UsrCustId='" + m.UsrCustId + "',AuditStat='" + m.AuditStat + "',TrxId='" + m.TrxId + "',OpenBandkId='" + m.OpenBankId + "',CardId='" + m.CardId + "' where  companyid=" + m.MerPriv;
                    LogInfo.WriteLog("企业开户sql:" + sql);
                    DbHelperSQL.RunSql(sql);
                    str = "RECV_ORD_ID_" + m.TrxId;
                }


            }

            //}
            return Content(str);
        }
        #endregion


        #region 录入标的汇付主动通知
        /// <summary>
        /// 录入标的汇付主动通知
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult BgReviw_case()
        {

            string str = "";
            R_AddBidInfo m = new R_AddBidInfo();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.ProId = DNTRequest.GetString("ProId");
            m.BorrCustId = DNTRequest.GetString("BorrCustId");
            m.BorrTotAmt = DNTRequest.GetString("BorrTotAmt");
            m.GuarCompId = DNTRequest.GetString("GuarCompId");
            m.GuarAmt = DNTRequest.GetString("GuarAmt");
            m.ProArea = DNTRequest.GetString("ProArea");
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.RespExt = DNTRequest.GetString("RespExt");
            m.ChkValue = DNTRequest.GetString("ChkValue");


            LogInfo.WriteLog("录入标的返回参数:" + FastJSON.toJOSN(m));

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.ProId);
            chkVal.Append(m.BorrCustId);
            chkVal.Append(m.BorrTotAmt);

            if (m.MerPriv == "1")
            {
                chkVal.Append(m.GuarCompId);
                chkVal.Append(m.GuarAmt);
            }

            chkVal.Append(m.ProArea);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.RespExt);

            string msg = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);


            LogInfo.WriteLog("录入标的验签返回参数:" + ret.ToString());

            if (ret == 0)
            {

                if (m.RespCode == "000")
                {
                    // Response.Write("RECV_ORD_ID_");

                    str = "RECV_ORD_ID_" + m.ProId;
                }
                else
                {
                    string sql = "update  hx_borrowing_target set  tender_state=1 where targetid=" + m.ProId;
                    DbHelperSQL.RunSql(sql);
                }
            }
            else
            {
                string sql = "update  hx_borrowing_target set  tender_state=1 where targetid=" + m.ProId;
                DbHelperSQL.RunSql(sql);
            }
            return Content(str);
        }

        #endregion

        #region 取现复枋汇付主动通知
        /// <summary>
        ///取现复枋汇付主动通知
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult BgCashProcessing()
        {
            //  Response.BufferOutput = true;
            // System.Threading.Thread.Sleep(1000);



            string str = "";
            ReCashAudit m = new ReCashAudit();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.AuditFlag = DNTRequest.GetString("AuditFlag");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = HttpUtility.UrlDecode(DNTRequest.GetString("MerPriv"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            LogInfo.WriteLog("后台取现审核返回参数:" + FastJSON.toJOSN(m));
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.AuditFlag);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            string msg = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);
            LogInfo.WriteLog("后台验签返回参数:" + ret.ToString());



            #region 复核验签结果
            if (ret == 0)
            {
                if (m.RespCode == "000" || m.RespCode == "999" || m.RespCode == "406")
                {
                    System.Threading.Thread.Sleep(500);
                    string cachename = m.OrdId + "Cash" + m.UsrCustId + m.TransAmt.ToString();
                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        //提现成功后，得多事务处理账户金额，流水等
                        B_usercenter BUC = new B_usercenter();
                        M_Capital_account_water aw = new M_Capital_account_water();
                        if (BUC.Su_CashProcessing(m, aw) > 0)
                        {
                            str = "RECV_ORD_ID_" + m.OrdId;
                            LogInfo.WriteLog("后台取款审核成功" + str);
                        }
                        else
                        {
                            LogInfo.WriteLog("后台取款审核事误操作失败");
                        }
                    }
                    str = "RECV_ORD_ID_" + m.OrdId;
                }
                else
                {
                    LogInfo.WriteLog("后台提现失败");
                }
            }
            else
            {
                LogInfo.WriteLog("后台验签失败");

            }
            #endregion

            return Content(str);



        }
        #endregion

        #region 企业充值汇付主动通知
        /// <summary>
        /// 企业充值汇付主动通知
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult Re_Enterpriserecharge()
        {
            string str = "";
            ReNetSave m = new ReNetSave();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = DNTRequest.GetString("RespDesc");
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.OrdDate = DNTRequest.GetString("OrdDate");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.RetUrl = DNTRequest.GetString("RetUrl");
            m.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.GateBusiId = DNTRequest.GetString("GateBusiId");
            m.GateBankId = DNTRequest.GetString("GateBankId");
            m.ChkValue = DNTRequest.GetString("ChkValue");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");

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

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);


            LogInfo.WriteLog("验签：" + ret.ToString());

            #region 企业充值验签
            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    string cachename = m.OrdId + "Enterpriserecharge" + m.UsrCustId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);

                        string sql = "update hx_Recharge_history set recharge_condition=1,recharge_bank='" + m.GateBankId + "' where  order_No='" + m.OrdId + "'";

                        DbHelperSQL.RunSql(sql);
                    }
                    str = "RECV_ORD_ID_" + m.TrxId;
                }
                else
                {
                    // Response.Write("充值失败");
                }
            }
            else
            {
                //失败

                // Response.Write("验签失败");

            }


            #endregion


            return Content(str);
        }
        #endregion

        #region 用户向平台划账汇付主动通知
        /// <summary>
        /// 用户向平台划账汇付主动通知
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult BgUsrPay()
        {
            string str = "";
            ReUsrAcctPay m = new ReUsrAcctPay();


            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.OrdId = DNTRequest.GetString("OrdId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.InAcctId = DNTRequest.GetString("InAcctId");
            m.InAcctType = DNTRequest.GetString("InAcctType");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.ChkValue = DNTRequest.GetString("ChkValue");



            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.InAcctId);
            chkVal.Append(m.InAcctType);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();
            string logstr = "RECV_ORD_ID_" + m.OrdId;
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);

            LogInfo.WriteLog("用户向平台商户号转账返回信息：" + FastJSON.toJOSN(m));


            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    string cachename = m.OrdId + "BgUsrPay" + m.UsrCustId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);



                        string sql = "update hx_member_table  set available_balance=available_balance-" + m.TransAmt + " where UsrCustId='" + m.UsrCustId + "'";
                        DbHelperSQL.RunSql(sql);
                        #region 流水信息
                        sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + m.UsrCustId + "'";
                        System.Data.DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                        if (dt.Rows.Count > 0)
                        {
                            B_usercenter ors = new B_usercenter();
                            decimal di = ors.GetUsridAvailable_balance(int.Parse(dt.Rows[0]["registerid"].ToString()));
                            // di = di + decimal.Parse(hua.Amt.ToString());
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into hx_Capital_account_water(");
                            strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                            strSql.Append(" values (");
                            strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + ",0," + decimal.Parse(m.TransAmt) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + (di - decimal.Parse(m.TransAmt)) + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.用户向平台划账.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "用户向平台划账" + "')");
                            DbHelperSQL.RunSql(strSql.ToString());
                            strSql.Clear();
                        }
                        #endregion
                        //Response.Write(logstr + "转账成功!");


                        LogInfo.WriteLog(logstr + "用户向平台商户号转账成功：" + sql);
                    }
                }


            }
            else
            {
                LogInfo.WriteLog(m.UsrCustId + "用户向平台商户号转账后台失败：");

            }





            return Content(logstr);
        }
        #endregion


        #region 放款汇付主动通知接口
        /// <summary>
        /// 放款汇付主动通知接口
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult BgLoans()
        {

            string outstr = "";

            ReLoans m = new ReLoans();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = DNTRequest.GetString("RespDesc");
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.OrdDate = DNTRequest.GetString("OrdDate");
            m.OutCustId = DNTRequest.GetString("OutCustId");
            m.OutAcctId = DNTRequest.GetString("OutAcctId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.Fee = DNTRequest.GetString("Fee");
            m.InCustId = DNTRequest.GetString("InCustId");
            m.InAcctId = DNTRequest.GetString("InAcctId");
            m.SubOrdId = DNTRequest.GetString("SubOrdId");
            m.SubOrdDate = DNTRequest.GetString("SubOrdDate");
            m.FeeObjFlag = DNTRequest.GetString("FeeObjFlag");
            m.IsDefault = DNTRequest.GetString("IsDefault");
            m.IsUnFreeze = DNTRequest.GetString("IsUnFreeze");
            m.UnFreezeOrdId = DNTRequest.GetString("UnFreezeOrdId");
            m.FreezeTrxId = DNTRequest.GetString("FreezeTrxId");
            m.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.RespExt = DNTRequest.GetString("RespExt");
            m.ChkValue = DNTRequest.GetString("ChkValue");

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.OutCustId);
            chkVal.Append(m.OutAcctId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.Fee);
            chkVal.Append(m.InCustId);
            chkVal.Append(m.InAcctId);
            chkVal.Append(m.SubOrdId);
            chkVal.Append(m.SubOrdDate);
            chkVal.Append(m.FeeObjFlag);
            chkVal.Append(m.IsDefault);
            chkVal.Append(m.IsUnFreeze);
            chkVal.Append(m.UnFreezeOrdId);
            chkVal.Append(m.FreezeTrxId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.RespExt);

            string msg = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);


            LogInfo.WriteLog("验签：" + ret.ToString());

            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    // M_borrowing_target m = new M_borrowing_target();
                    B_borrowing_target o = new B_borrowing_target();

                    B_member_table o1 = new B_member_table();
                    M_member_table p1 = new M_member_table();
                    M_member_table p2 = new M_member_table();
                    B_usercenter BUC = new B_usercenter();


                    string cachename = m.OrdId + "Loans" + m.FreezeTrxId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);

                        M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象


                        //标的id,借款人id,投资人id,投资记录bid,抵扣券金额
                        string strings = m.MerPriv;
                        string[] sArray = strings.Split('_');
                        if (sArray.Count() == 5)
                        {

                            p1 = o1.GetModel(int.Parse(sArray[1])); //借款人用户对象

                            LogInfo.WriteLog("sArray[1].ToString():" + sArray[1].ToString());
                            baw.membertable_registerid = p1.registerid;
                            baw.income = decimal.Parse(m.TransAmt);
                            baw.expenditure = 0.00M;

                            baw.time_of_occurrence = DateTime.Now;

                            // decimal ff = p1.available_balance + decimal.Parse(retloan.TransAmt);
                            baw.account_balance = p1.available_balance;  //面要得么帐户余额

                            LogInfo.WriteLog("借款人余额:" + p1.available_balance);

                            baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款转入.ToString());
                            baw.createtime = DateTime.Now;
                            baw.keyid = 0;
                            baw.remarks = m.OrdId + "," + m.OrdDate;


                            M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                            p2 = o1.GetModel(int.Parse(sArray[2].ToString())); //投款人用户对象

                            iaw.membertable_registerid = p2.registerid;
                            iaw.income = 0.00M;
                            iaw.expenditure = decimal.Parse(m.TransAmt);
                            iaw.time_of_occurrence = DateTime.Now;
                            // decimal df=p1.available_balance - decimal.Parse(retloan.TransAmt);
                            iaw.account_balance = p2.available_balance;  //面要得么帐户余额
                            LogInfo.WriteLog("投资人余额:" + p2.ToString());

                            iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.项目投资.ToString());
                            iaw.createtime = DateTime.Now;
                            iaw.keyid = 0;
                            iaw.remarks = m.OrdId + "," + m.OrdDate;

                            LogInfo.WriteLog("放款bid_records_id:" + sArray[3].ToString());


                            if (BUC.Loan_Successfully(m, baw, iaw, sArray[3].ToString(), decimal.Parse(sArray[4].ToString())) > 0)
                            {
                                //成功

                                outstr = "RECV_ORD_ID_" + m.OrdId;
                                LogInfo.WriteLog("后台主动通知操作成功:" + outstr);
                                return Content(outstr);
                                //  Response.Redirect("/usercenter/index.html");

                            }






                        }



                    }



                }
            }





            return Content("");
        }
        #endregion



        #region 还款汇付主动通知
        /// <summary>
        /// 还款汇付主动通知
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult Su_Repayment()
        {
            Utils.SetSYSDateTimeFormat();

            chuangtouEntities ef = new chuangtouEntities();
            string str = "";
            ReRepayment Re = new ReRepayment();
            Re.CmdId = DNTRequest.GetString("CmdId");
            Re.RespCode = DNTRequest.GetString("RespCode");
            Re.MerCustId = DNTRequest.GetString("MerCustId");
            Re.ProId = DNTRequest.GetString("ProId");
            Re.OrdId = DNTRequest.GetString("OrdId");
            Re.OrdDate = DNTRequest.GetString("OrdDate");
            Re.OutCustId = DNTRequest.GetString("OutCustId");
            Re.SubOrdId = DNTRequest.GetString("SubOrdId");
            Re.SubOrdDate = DNTRequest.GetString("SubOrdDate");
            Re.OutAcctId = DNTRequest.GetString("OutAcctId");
            Re.TransAmt = DNTRequest.GetString("TransAmt");
            Re.PrincipalAmt = DNTRequest.GetString("PrincipalAmt");
            Re.InterestAmt = DNTRequest.GetString("InterestAmt");
            Re.Fee = DNTRequest.GetString("Fee");
            Re.InCustId = DNTRequest.GetString("InCustId");
            Re.InAcctId = DNTRequest.GetString("InAcctId");
            Re.FeeObjFlag = DNTRequest.GetString("FeeObjFlag");
            Re.DzObject = DNTRequest.GetString("DzObject");
            Re.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            Re.MerPriv = DNTRequest.GetString("MerPriv");
            Re.RespExt = DNTRequest.GetString("RespExt");
            LogInfo.WriteLog("后台还款返回报文：" + FastJSON.toJOSN(Re));

            string version = "20";
            if (string.IsNullOrEmpty(Re.TransAmt) || Re.TransAmt == "0.00")
            {
                version = "30";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(Re.CmdId);
            builder.Append(Re.RespCode);
            builder.Append(Re.MerCustId);
            if (version == "30")
            {
                builder.Append(Re.ProId);
            }
            builder.Append(Re.OrdId);
            builder.Append(Re.OrdDate);
            builder.Append(Re.OutCustId);
            builder.Append(Re.SubOrdId);
            builder.Append(Re.SubOrdDate);
            builder.Append(Re.OutAcctId);
            if (version == "30")
            {
                builder.Append(Re.PrincipalAmt);
                builder.Append(Re.InterestAmt);
            }
            else
            {
                builder.Append(Re.TransAmt);
            }
            builder.Append(Re.Fee);
            builder.Append(Re.InCustId);
            builder.Append(Re.InAcctId);
            builder.Append(Re.FeeObjFlag);
            if (version == "30")
            {
                builder.Append(Re.DzObject);
            }
            builder.Append(HttpUtility.UrlDecode(Re.BgRetUrl));
            builder.Append(Re.MerPriv);
            builder.Append(HttpUtility.UrlDecode(Re.RespExt));
            // builder.Append(Re.ChkValue);
            var msg = builder.ToString();
            if (version == "30")
            {
                msg = Utils.MD5(msg);
            }
            string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, Re.ChkValue);
            LogInfo.WriteLog("后台验签 ret= " + ret.ToString());
            if (ret == 0)
            {
                if (Re.RespCode == "000" || Re.TransAmt == "0.00")
                {
                    string cachename = "Repayment" + Re.OrdId;
                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);

                        B_member_table o = new B_member_table();
                        M_member_table p = new M_member_table();
                        M_member_table p2 = new M_member_table();
                        M_Capital_account_water baw = new M_Capital_account_water();
                        V_borrowing_Bid_records_income_statement item = new V_borrowing_Bid_records_income_statement();
                        int income_statement_id = int.Parse(Re.MerPriv);
                        item = ef.V_borrowing_Bid_records_income_statement.AsNoTracking().Where(ps => ps.income_statement_id == income_statement_id).FirstOrDefault();
                        p = o.GetModel(int.Parse(item.borrower_registerid.ToString())); //借款人用户对象

                        baw.membertable_registerid = p.registerid;
                        baw.income = 0.00M;
                        baw.expenditure = decimal.Parse(Re.TransAmt);
                        baw.time_of_occurrence = DateTime.Now;
                        baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                        baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                        baw.createtime = DateTime.Now;
                        baw.keyid = 0;
                        baw.remarks = Re.OrdId + "," + Re.OrdDate;

                        M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                        p2 = o.GetModel(int.Parse(item.investor_registerid.ToString())); //投资人用户对象

                        iaw.membertable_registerid = p2.registerid;
                        iaw.income = decimal.Parse(Re.TransAmt);
                        iaw.expenditure = 0.00M;
                        iaw.time_of_occurrence = DateTime.Now;
                        iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                        iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                        iaw.createtime = DateTime.Now;
                        iaw.keyid = 0;
                        iaw.remarks = Re.OrdId + "," + Re.OrdDate;


                        int targetid = int.Parse(item.targetid.ToString());
                        //判断是否为最后一期还款，并进行相应状态处理
                        hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();
                        var lastcur = plan.current_period;
                        //判断是否是最后一期，如果是对本金处理。得直接用最后一期还款金额减去本金，这样就解决多出本金问题
                        bool lastrepamt = false;

                        if (item.current_investment_period.ToString() == lastcur.ToString())
                        {
                            lastrepamt = true;
                        }



                        //需要更新投资记录表已还款金额

                        B_usercenter BUC = new B_usercenter();

                        decimal PrincipalInterest = decimal.Parse(item.Principal.ToString()) + decimal.Parse(item.interestpayment.ToString());  //本金加利息

                        decimal PInterest = decimal.Parse(item.interestpayment.ToString());  //利息

                        Re.MerPriv = DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"); //传入还款日期
                        int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);

                        if (bucd > 0)
                        {
                            // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】



                            #region 短信通知
                            //短信通知

                            string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                            StringBuilder sbsms = new StringBuilder(contxt);

                            sbsms = sbsms.Replace("#USERANEM#", item.username.ToString());

                            sbsms = sbsms.Replace("#PID#", item.targetid.ToString());

                            sbsms = sbsms.Replace("#ORDER#", item.current_investment_period.ToString());

                            sbsms = sbsms.Replace("#MONEY#", item.repayment_amount.ToString());


                            string mobile = item.mobile.ToString();

                            M_td_SMS_record psms = new M_td_SMS_record();
                            B_td_SMS_record osms = new B_td_SMS_record();
                            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                            psms.phone_number = mobile;
                            psms.sendtime = DateTime.Now;
                            psms.senduserid = int.Parse(item.investor_registerid.ToString());
                            psms.smstype = smstype;
                            psms.smscontext = sbsms.ToString();
                            psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                            psms.vcode = "";

                            osms.Add(psms);
                            #endregion


                            #region 系统消息
                            //系统消息


                            DateTime dti = DateTime.Now;

                            M_td_System_message pm = new M_td_System_message();
                            pm.MReg = int.Parse(item.investor_registerid.ToString());
                            pm.Mstate = 0;
                            pm.MTitle = "投资回款";
                            //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                            pm.MContext = sbsms.ToString();
                            pm.PubTime = dti;
                            pm.Mtype = 2;
                            B_usercenter.AddMessage(pm);


                            #endregion

                            #region 更新标的状态为还还清
                            ///更新标的状态为还还清

                            if (item.current_investment_period.ToString() == lastcur.ToString())
                            {
                                //更新标的状态为还还清

                                string sql = "update hx_borrowing_target set tender_state=5 where targetid=" + item.targetid.ToString() + "";
                                DbHelperSQL.ExecuteSql(sql);

                                sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + item.bid_records_id.ToString();
                                DbHelperSQL.ExecuteSql(sql);

                            }
                            #endregion


                            //Response.Write("还款验签成功！");
                        }
                        else
                        {
                            //Response.Write("还款更新失败！" + bucd.ToString());
                        }

                    }

                    str = "RECV_ORD_ID_" + Re.OrdId;
                    LogInfo.WriteLog(str + "后台还款成功");

                }
                else
                {
                    string cc = Utils.GetReturnCode(Int32.Parse(Re.RespCode));


                    if (cc.Contains("还款金额超过还款总额") || cc.Contains("重复的还款请求"))
                    {
                        B_member_table o = new B_member_table();
                        M_member_table p = new M_member_table();
                        M_member_table p2 = new M_member_table();
                        M_Capital_account_water baw = new M_Capital_account_water();
                        V_borrowing_Bid_records_income_statement item = new V_borrowing_Bid_records_income_statement();
                        int income_statement_id = int.Parse(Re.MerPriv);
                        item = ef.V_borrowing_Bid_records_income_statement.AsNoTracking().Where(ps => ps.income_statement_id == income_statement_id).FirstOrDefault();
                        p = o.GetModel(int.Parse(item.borrower_registerid.ToString())); //借款人用户对象

                        baw.membertable_registerid = p.registerid;
                        baw.income = 0.00M;
                        baw.expenditure = decimal.Parse(Re.TransAmt);
                        baw.time_of_occurrence = DateTime.Now;
                        baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                        baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                        baw.createtime = DateTime.Now;
                        baw.keyid = 0;
                        baw.remarks = Re.OrdId + "," + Re.OrdDate;

                        M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                        p2 = o.GetModel(int.Parse(item.investor_registerid.ToString())); //投资人用户对象

                        iaw.membertable_registerid = p2.registerid;
                        iaw.income = decimal.Parse(Re.TransAmt);
                        iaw.expenditure = 0.00M;
                        iaw.time_of_occurrence = DateTime.Now;
                        iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                        iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                        iaw.createtime = DateTime.Now;
                        iaw.keyid = 0;
                        iaw.remarks = Re.OrdId + "," + Re.OrdDate;


                        int targetid = int.Parse(item.targetid.ToString());
                        //判断是否为最后一期还款，并进行相应状态处理
                        hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();
                        var lastcur = plan.current_period;
                        //判断是否是最后一期，如果是对本金处理。得直接用最后一期还款金额减去本金，这样就解决多出本金问题
                        bool lastrepamt = false;

                        if (item.current_investment_period.ToString() == lastcur.ToString())
                        {
                            lastrepamt = true;
                        }



                        //需要更新投资记录表已还款金额

                        B_usercenter BUC = new B_usercenter();

                        decimal PrincipalInterest = decimal.Parse(item.Principal.ToString()) + decimal.Parse(item.interestpayment.ToString());  //本金加利息

                        decimal PInterest = decimal.Parse(item.interestpayment.ToString());  //利息

                        Re.MerPriv = DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"); //传入还款日期
                        int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);

                        if (bucd > 0)
                        {
                            // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】



                            #region 短信通知
                            //短信通知

                            string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                            StringBuilder sbsms = new StringBuilder(contxt);

                            sbsms = sbsms.Replace("#USERANEM#", item.username.ToString());

                            sbsms = sbsms.Replace("#PID#", item.targetid.ToString());

                            sbsms = sbsms.Replace("#ORDER#", item.current_investment_period.ToString());

                            sbsms = sbsms.Replace("#MONEY#", item.repayment_amount.ToString());


                            string mobile = item.mobile.ToString();

                            M_td_SMS_record psms = new M_td_SMS_record();
                            B_td_SMS_record osms = new B_td_SMS_record();
                            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                            psms.phone_number = mobile;
                            psms.sendtime = DateTime.Now;
                            psms.senduserid = int.Parse(item.investor_registerid.ToString());
                            psms.smstype = smstype;
                            psms.smscontext = sbsms.ToString();
                            psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                            psms.vcode = "";

                            osms.Add(psms);
                            #endregion


                            #region 系统消息
                            //系统消息


                            DateTime dti = DateTime.Now;

                            M_td_System_message pm = new M_td_System_message();
                            pm.MReg = int.Parse(item.investor_registerid.ToString());
                            pm.Mstate = 0;
                            pm.MTitle = "投资回款";
                            //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                            pm.MContext = sbsms.ToString();
                            pm.PubTime = dti;
                            pm.Mtype = 2;
                            B_usercenter.AddMessage(pm);


                            #endregion

                            #region 更新标的状态为还还清
                            ///更新标的状态为还还清

                            if (item.current_investment_period.ToString() == lastcur.ToString())
                            {
                                //更新标的状态为还还清

                                string sql = "update hx_borrowing_target set tender_state=5 where targetid=" + item.targetid.ToString() + "";
                                DbHelperSQL.ExecuteSql(sql);

                                sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + item.bid_records_id.ToString();
                                DbHelperSQL.ExecuteSql(sql);

                            }
                            #endregion


                            //Response.Write("还款验签成功！");
                        }
                        else
                        {
                            //Response.Write("还款更新失败！" + bucd.ToString());
                        }


                    }


                    //Response.Write("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));
                    /*
                    string cc = Utils.GetReturnCode(Int32.Parse(Re.RespCode));


                    if (cc.Contains("还款金额超过还款总额") || cc.Contains("重复的还款请求"))
                    {

                        B_member_table o = new B_member_table();
                        M_member_table p = new M_member_table();
                        M_member_table p2 = new M_member_table();
                        M_Capital_account_water baw = new M_Capital_account_water();
                        V_borrowing_Bid_records_income_statement item = new V_borrowing_Bid_records_income_statement();
                        int income_statement_id = int.Parse(Re.MerPriv);
                        item = ef.V_borrowing_Bid_records_income_statement.AsNoTracking().Where(ps => ps.income_statement_id == income_statement_id).FirstOrDefault();
                        p = o.GetModel(int.Parse(item.borrower_registerid.ToString())); //借款人用户对象


                        int ic = ef.hx_income_statement.Where(c => c.bid_records_id == item.bid_records_id && c.targetid == item.targetid && c.orderid== Re.OrdId).Update(c => new hx_income_statement { payment_status = 1, repayment_period = DateTime.Now });

                        if (ic > 0)
                        {
                            int userid = int.Parse(item.borrower_registerid.ToString());
                            UserInfoData ud = new UserInfoData();
                            ReQueryBalanceBg retloan = ud.Querybalance(userid);

                            if (retloan.RespCode == "000")
                            {
                                // sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + " ,frozen_sum=" + decimal.Parse(retloan.FrzBal) + " where  registerid=" + userid.ToString() + "";

                                //sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + "  where  registerid=" + userid.ToString() + "";

                                //DbHelperSQL.RunSql(sql);
                                B_usercenter bu = new B_usercenter();
                                bu.DataSync(retloan, userid.ToString());




                            }
                        }
                        */

                }





            }




            return Content(str);
        }

        #endregion




        #region 平台向用户转账
        /// <summary>
        /// 平台向用户转账
        /// </summary>
        /// <returns></returns>
        public ActionResult BgToUserTransfer()
        {

            ReTransfer p = new ReTransfer();

            p.CmdId = DNTRequest.GetString("CmdId");
            p.RespCode = DNTRequest.GetString("RespCode");
            p.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            p.OrdId = DNTRequest.GetString("OrdId");
            p.OutCustId = DNTRequest.GetString("OutCustId");
            p.OutAcctId = DNTRequest.GetString("OutAcctId");
            p.TransAmt = DNTRequest.GetString("TransAmt");
            p.InCustId = DNTRequest.GetString("InCustId");
            p.InAcctId = DNTRequest.GetString("InAcctId");

            p.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));

            p.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            p.MerPriv = DNTRequest.GetString("MerPriv");

            p.ChkValue = DNTRequest.GetString("ChkValue");



            StringBuilder builder = new StringBuilder();
            builder.Append(p.CmdId);
            builder.Append(p.RespCode);
            builder.Append(p.OrdId);
            builder.Append(p.OutCustId);
            builder.Append(p.OutAcctId);
            builder.Append(p.TransAmt);
            builder.Append(p.InCustId);
            builder.Append(p.InAcctId);
            builder.Append(HttpUtility.UrlDecode(p.RetUrl));
            builder.Append(HttpUtility.UrlDecode(p.BgRetUrl));
            builder.Append(p.MerPriv);



            string chkv = builder.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);

            LogInfo.WriteLog("平台向用户活动转账后---1台验签：" + ret.ToString());

            LogInfo.WriteLog("平台向用户活动转账后---1台主动投标返回报文:" + FastJSON.toJOSN(p));

            string sql = "";
            string srt = "";

            if (ret == 0)
            {
                if (p.RespCode == "000")
                {
                    /*
                   sql = "update hx_CashAwards  set  OrdIdstate=3  where OrdIdstate=1 and OrdId=" + p.OrdId + " and  proid =" + p.MerPriv;
                   DbHelperSQL.RunSql(sql);*/

                    sql = "update hx_member_table  set account_total_assets=account_total_assets+" + p.TransAmt + " ,available_balance=available_balance+" + p.TransAmt + " where UsrCustId='" + p.InCustId + "'";
                    DbHelperSQL.RunSql(sql);


                    srt = "RECV_ORD_ID_" + p.OrdId;


                    LogInfo.WriteLog("平台向用户活动转账后成功：" + srt + "   " + sql);

                }
                else
                {

                }

            }





            return Content(srt);
        }

        #endregion

    }
}