using ChuanglitouP2P.Areas.Admin.Controllers;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Cash;
using ChuanglitouP2P.Model.chinapnr.InitiativeTender;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuanglitouP2P.Model.chinapnr.UserBindCard;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using ChuanglitouP2P.Model.chinapnr.UsrFreezeBg;
using ChuangLitouP2P.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class ThirdpartyController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        B_usercenter o = new B_usercenter();
        // GET: Thirdparty
        public ActionResult Index()
        {
            return View();
        }

        private static object lockobj = new object();
        public ActionResult Bg_Succ_Registered()
        {
            string str = "";
            int it = -10000;
            string username = "";
            string useremail = "";
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
            useremail = DNTRequest.GetString("UsrEmail");
            m.UsrEmail = useremail;
            username = HttpUtility.UrlDecode(DNTRequest.GetString("UsrName"));
            m.UsrName = username;
            m.ChkValue = DNTRequest.GetString("ChkValue");
            string log = "注册开户返回报文(异步):" + FastJSON.toJOSN(m);
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
            log += "；ret:" + ret;
            if (ret == 0)
            {
                B_member_table ob = new B_member_table();
                M_member_table pm = new M_member_table();
                if (m.RespCode == "000")
                {
                    lock (lockobj)//增加同步锁，防止同时调用
                    {
                        //为避免重复调用,增加缓存校验
                        string cachename = "UserRegister" + m.TrxId;
                        if (Utils.GeTThirdCache(cachename) == 0)
                        {
                            Utils.SetThirdCache(cachename);
                            M_bonus_account_water mbaw = new M_bonus_account_water();
                            B_bonus_account_water bbaw = new B_bonus_account_water();
                            B_usercenter b = new B_usercenter();
                            it = b.Succ_Reg(m);
                            log += "；汇付开户成功后进行用户数据更新返回:" + it;
                            if (it > 0)
                            {
                                pm = ob.GetModel(Utils.GetUserSplit(m.UsrId));
                                //ViewBag.registerid = pm.registerid;
                                //if (false)//有bug暂时注释掉 重复发往奖励
                                //{//新人注册奖励
                                ActFacade act = new ActFacade();
                                log += "；注册ID:" + pm.registerid;
                                act.SendBonusAfterRegister(pm.registerid, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.web);
                                // }
                            }
                            else
                            {
                                /*第三方成功，本地服务器操作失败*/
                            }
                        }

                    }
                    str = "RECV_ORD_ID_" + m.TrxId;
                }
            }
            else
            {
                /*验签不成功*/
                log += "验签不成功";
            }
            LogInfo.WriteLog(log);
            return Content(str);
        }


        #region 充值汇付后台主动通知
        /// <summary>
        /// 充值汇付后台主动通知
        /// </summary>
        /// <returns></returns>
        public ActionResult ReQPNetSave()
        {
            string str1 = "";

            Utils.SetSYSDateTimeFormat();

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
            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);
            // Response.Write("验签：" + ret.ToString());
            LogInfo.WriteLog("快充接口后台验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc);

            LogInfo.WriteLog("快充接口后台充值返回报文:" + FastJSON.toJOSN(m));
            StringBuilder str = new StringBuilder();
            string sql = "";

            if (ret == 0)
            {
                if (m.RespCode == "000")
                {



                    string MerPrivTemp = DESEncrypt.Decrypt(m.MerPriv, ConfigurationManager.AppSettings["webp"].ToString());
                    string[] arr = Utils.SplitString(MerPrivTemp, "_");  //第一位是用户id 二是 记录id
                    int userid = int.Parse(arr[0]);
                    int reid = int.Parse(arr[1]);
                    string cachename = m.OrdId + userid.ToString() + reid.ToString();

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        sql = "select recharge_condition  from hx_Recharge_history where recharge_condition=0  and recharge_history_id=" + reid + " and order_No='" + m.OrdId + "'";
                        DataTable dtr = DbHelperSQL.GET_DataTable_List(sql);
                        if (dtr.Rows.Count > 0)
                        {
                            LogInfo.WriteLog("快充接口后台充值数据没有写入情况下操作>>>>>>>>>>>>>>>>>>>>>>>");
                            M_Recharge_history rh = new M_Recharge_history();
                            M_Capital_account_water aw = new M_Capital_account_water();
                            B_member_table o = new B_member_table();
                            M_member_table p = new M_member_table();
                            p = o.GetModel(userid);
                            rh.membertable_registerid = userid;
                            rh.recharge_amount = decimal.Parse(m.TransAmt);
                            rh.recharge_time = DateTime.Now;
                            rh.account_amount = decimal.Parse(m.TransAmt);
                            rh.order_No = m.OrdId;
                            rh.recharge_condition = 1;  //1表示充值成功
                            rh.recharge_bank = m.GateBankId; // 得接口返回;
                            rh.recharge_history_id = reid;  //本值提交里存的充值id
                            aw.membertable_registerid = userid;
                            aw.income = decimal.Parse(m.TransAmt);
                            aw.expenditure = 0.00M;
                            aw.time_of_occurrence = rh.recharge_time;
                            aw.account_balance = p.available_balance + aw.income;  //要得么帐户余额
                            aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.充值.ToString());
                            aw.createtime = DateTime.Now;
                            aw.keyid = 0;
                            aw.remarks = m.OrdId;
                            B_usercenter BUC = new B_usercenter();
                            int bucrec = BUC.rechargeTran(rh, aw);
                            #region MyRegion  系统消息
                            DateTime dti = DateTime.Now;
                            M_td_System_message pm = new M_td_System_message();
                            pm.MReg = p.registerid;
                            pm.Mstate = 0;
                            pm.MTitle = "充值成功";
                            pm.MContext = "尊敬的用户" + p.username + "：您好！恭喜您充值成功，充值金额是：" + rh.recharge_amount.ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                            pm.PubTime = dti;
                            pm.Mtype = 4;
                            B_usercenter.AddMessage(pm);
                            #endregion

                            LogInfo.WriteLog("后台充值事务操作返回码小于=0 操作失败:" + bucrec.ToString());
                            if (m.GateBusiId == "QP")
                            {
                                sql = "select UsrBindCardID from hx_UsrBindCardC where UsrCustId='" + m.UsrCustId + "' and OpenAcctId='" + m.CardId + "'";
                                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                if (dt.Rows.Count > 0)
                                {
                                    sql = "update  hx_UsrBindCardC  set  BindCardType =1 where UsrCustId='" + m.UsrCustId + "' and OpenAcctId='" + m.CardId + "' ";
                                    DbHelperSQL.RunSql(sql);
                                }
                                else
                                {
                                    sql = "INSERT INTO hx_UsrBindCardC (UsrCustId,OpenAcctId,OpenBankId,defCard,BindCardType) VALUES ('" + m.UsrCustId + "','" + m.CardId + "','" + m.GateBankId + "',1,1)";
                                    DbHelperSQL.RunSql(sql);
                                    sql = "update    hx_member_table set  isbankcard=1 where registerid=" + userid.ToString();
                                    DbHelperSQL.RunSql(sql);
                                }
                            }
                        }
                    }
                    str1 = "RECV_ORD_ID_" + m.TrxId;
                }
            }
            return Content(str1);
        }
        #endregion

        #region 普通提现汇付后台主动通知
        /// <summary>
        /// 普通提现汇付后台主动通知
        /// </summary>
        /// <returns></returns>
        public ActionResult PostGENERALAmt()
        {
            string str1 = "";
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
            LogInfo.WriteLog("后台取现返回报文:" + FastJSON.toJOSN(m));

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

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);

            // LogInfo.WriteLog("验签返回参数:" + ret.ToString());
            StringBuilder str = new StringBuilder();
            if (ret == 0)
            {
                //提现成功后，得多事务处理账户金额，流水及冻结金额等
                if (m.RespCode == "000")
                {
                    string cachename = m.OrdId + "Cash" + m.UsrCustId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        B_usercenter BUC = new B_usercenter();
                        M_ReqExt mre = JsonConvert.DeserializeObject<M_ReqExt>(m.RespExt.Replace("[", "").Replace("]", ""));
                        int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.TransAmt, m.UsrCustId, m.FeeAmt, mre.FeeObjFlag, mre.CashChl);
                        //int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.TransAmt, m.UsrCustId);
                        if (CashOp > 0)
                        {
                            string sql = "select registerid,username,mobile,UsrCustId,available_balance from hx_member_table where UsrCustId='" + m.UsrCustId + "'";
                            LogInfo.WriteLog("后台审请取现成功短信sql:" + sql);
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                            if (dt.Rows.Count > 0)
                            {

                                //短信通知
                                //尊敬的#USERANEM#,您已成功提现#MONEY#元,账户余额#MONEY1#.请注意查收!【创利投】
                                string contxt = Utils.GetMSMEmailContext(12, 1); // 获取注册成功邮件内容

                                StringBuilder sbsms = new StringBuilder(contxt);

                                sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                sbsms = sbsms.Replace("#MONEY#", m.TransAmt);

                                decimal amt = decimal.Parse(dt.Rows[0]["available_balance"].ToString()) - decimal.Parse(m.TransAmt);

                                sbsms = sbsms.Replace("#MONEY1#", amt.ToString());

                                string mobile = dt.Rows[0]["mobile"].ToString();

                                M_td_SMS_record psms = new M_td_SMS_record();
                                B_td_SMS_record osms = new B_td_SMS_record();
                                int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.取现成功.ToString());
                                psms.phone_number = mobile;
                                psms.sendtime = DateTime.Now;
                                psms.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                psms.smstype = smstype;
                                psms.smscontext = sbsms.ToString();
                                psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                psms.vcode = "";
                                osms.Add(psms);
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "提现成功";
                                pm.MContext = sbsms.ToString();
                                pm.PubTime = dti;
                                pm.Mtype = 3;
                                B_usercenter.AddMessage(pm);


                                if (mre.CashChl == "IMMEDIATE" && decimal.Parse(m.TransAmt) <= 200000M)
                                {
                                    string retUrl = Utils.GetRe_url("admin/UserCash/RePostCashProcessing");
                                    string bgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");
                                    BusinessLogicHelper.AutoCheckCash(m.UsrCustId, retUrl, bgRetUrl);
                                }
                            }
                        }

                    }


                    str1 = "RECV_ORD_ID_" + m.OrdId;

                }
            }
            return Content(str1);
        }
        #endregion

        #region 投资成功,汇付后台主动通知
        /// <summary>
        /// 投资成功,汇付后台主动通知
        /// </summary>
        /// <returns></returns>
        public ActionResult BG_investment_success()
        {
            int id = 0;
            string srt = "";
            ReInitiativeTender p = new ReInitiativeTender();
            id = DNTRequest.GetInt("id", 0);
            string log = "";
            log += "主动通知后台有响应成功!接收到的项目id=" + id;
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
            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);
            log += "<br>投标后台主动投标返回报文:" + FastJSON.toJOSN(p);
            string sql = "";

            int invcount = 0; //记录用户是否是首次投资
            #region 验签
            if (ret == 0)
            {
                if (p.RespCode == "000" || p.RespCode == "322" || p.RespCode == "534" || p.RespCode == "360" || p.RespCode == "099")
                {
                    string cachename = p.OrdId + "InvestWeb" + p.UsrCustId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        if (p.FreezeTrxId != "")
                        {
                            sql = "select ordstate  from hx_Bid_records  where ordstate =0 and  OrdId='" + p.OrdId + "'";
                            DataTable dts = DbHelperSQL.GET_DataTable_List(sql);

                            if (dts.Rows.Count > 0)
                            {
                                //同步处理用户金额
                                B_usercenter BUC = new B_usercenter();
                                int d = BUC.ReInvest_success(p.UsrCustId, p.FreezeOrdId, p.TransAmt, p.FreezeTrxId, p.OrdId, p.MerPriv);
                                log += "<br>后台投标:id" + id.ToString() + "返回唯一冻结标识:" + p.FreezeTrxId + "事务执行结果:" + d.ToString();
                                if (d > 0)
                                {
                                    sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";
                                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                    if (dt.Rows.Count > 0)
                                    {
                                        decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                                        string OrdId = p.OrdId;
                                        int registerid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        ViewBag.userid = registerid;
                                        string targetid = dt.Rows[0]["targetid"].ToString();

                                        #region 待提取为公共方法
                                        #region MyRegion  系统消息
                                        DateTime dti = DateTime.Now;
                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = registerid;
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资成功";
                                        pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + investAmt + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.PubTime = dti;
                                        pm.Mtype = 1;
                                        B_usercenter.AddMessage(pm);
                                        #endregion

                                        #region MyRegion//短信通知
                                        string contxt = Utils.GetMSMEmailContext(15, 1); // 获取注册成功邮件内容
                                        StringBuilder sbsms = new StringBuilder(contxt);
                                        sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());
                                        sbsms = sbsms.Replace("#PID#", targetid);
                                        sbsms = sbsms.Replace("#MONEY#", investAmt.ToString());
                                        string mobile = dt.Rows[0]["mobile"].ToString();
                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资成功.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = registerid;
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                        psms.vcode = "";
                                        osms.Add(psms);
                                        #endregion

                                        #region 远程调用生成合同？？？ 稍后替换为本地方法调用  微信端可远程调用
                                        string postString = "action=MUserPDF&data=" + targetid.ToString() + "&uc=" + registerid.ToString() + "&OrdId=" + OrdId;
                                        string sr = Utils.PostWebRequest(Utils.GetRemote_url("pdf/index"), postString, Encoding.UTF8);
                                        #endregion

                                        #region 渠道合作 第一投标调用接口？？？
                                        B_member_table bmt = new B_member_table();
                                        M_member_table mmt = new M_member_table();
                                        mmt = bmt.GetModel(registerid);
                                        if (mmt.Tid != null && mmt.Channelsource == 1)
                                        {
                                            if (B_usercenter.GetInvestCountByUserid(mmt.registerid) == 1)
                                            {
                                                string ret3 = Utils.GetCoopAPI(mmt.Tid, investAmt.ToString("0.00"), 2);
                                                log += "<br>前台渠道合作第一次返回结果:" + ret3 + "  用户id:" + mmt.registerid + " 订单id " + OrdId;
                                            }
                                        }
                                        #endregion
                                        #endregion 待提取为公共方法

                                        //发放奖励
                                        ActFacade act = new ActFacade();
                                        act.SendBonusAfterInvest(dt, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.web);

                                        #region 下线 投资六月专享标一元抢Iphone 2016年9月11日 9点 至2016年9月30日
                                        if (false)
                                        {
                                            DateTime nowdate = DateTime.Now;
                                            DateTime startdate = new DateTime(2016, 09, 11, 9, 00, 00);
                                            DateTime enddate = new DateTime(2016, 09, 30, 23, 59, 59);

                                            if (nowdate > startdate && nowdate < enddate)
                                            {
                                                log += "<br>PC端【一元抢Iphone】";
                                                log += "<br>标的期限：" + dt.Rows[0]["life_of_loan"].ToString();
                                                if (dt.Rows[0]["unit_day"].ToString() == "1" && dt.Rows[0]["life_of_loan"].ToString() == "6")//是否六月标
                                                {

                                                    log += "<br>用户注册时间：" + mmt.Registration_time.ToString();
                                                    if (mmt.Registration_time > new DateTime(2016, 09, 11, 0, 0, 0))
                                                    {


                                                        B_GrabIphone gi = new B_GrabIphone();

                                                        bool isCount = gi.Exists(mmt.registerid);//查询是否存在该用户
                                                        log += "<br> 用户ID：" + mmt.registerid + ";查询是否存在该用户:" + isCount;
                                                        if (isCount != true)
                                                        {
                                                            M_GrabIphone model = new M_GrabIphone();
                                                            model.RegrsterID = mmt.registerid;
                                                            model.Color = "";
                                                            model.Addtime = nowdate;
                                                            model.LuckDrawState = 0;
                                                            model.WinningState = 0;
                                                            model.WinningTime = nowdate;
                                                            model.TargetID = int.Parse(dt.Rows[0]["targetid"].ToString());
                                                            model.BidRecordsID = int.Parse(dt.Rows[0]["bid_records_id"].ToString());
                                                            model.InvestmentAmount = dt.Rows[0]["investment_amount"].ToString();
                                                            gi.Add(model); //增加一条数据
                                                            int ljcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrabIphone"].ToString());//获取启动抽奖人数
                                                            List<M_GrabIphone> giList = gi.GetModelList(ljcount, "LuckDrawState=0", "ID");//获取当前阶段投资人数
                                                            if (giList != null)
                                                            {
                                                                log += "<br>当前阶段投资人数:" + giList.Count;

                                                                if (giList.Count >= ljcount)
                                                                {
                                                                    bool bo = gi.UpdateLuckDrawState();//批量更新抽奖状态
                                                                    log += "<br> 批量更新抽奖状态:" + bo;
                                                                    if (bo == true)
                                                                    {
                                                                        int count = giList.Count;
                                                                        int index = new Random().Next(count);
                                                                        M_GrabIphone randowitem = giList[index];

                                                                        if (randowitem != null)
                                                                        {
                                                                            log += "<br> 获奖用户ID:" + randowitem.RegrsterID;
                                                                            bool co = gi.Update("", 1, DateTime.Now, randowitem.RegrsterID);//更新中奖用户状态
                                                                            log += "<br> 更新中奖用户状态:" + co;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            srt = "RECV_ORD_ID_" + p.OrdId;
                        }
                    }/*缓存检查结束位置*/
                }
            }
            LogInfo.WriteLog(log);
            #endregion
            return Content(srt);
        }
        #endregion

        #region 活动企业向用户转账
        /// <summary>
        /// 活动企业向用户转账
        /// </summary>
        /// <returns></returns>
        public ActionResult ToUserTransfer()
        {

            //  Thread.Sleep(6000);
            string str = "";
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

            string log = "后台平台向用户活动转账后";
            log += "<br>1台验签：" + ret.ToString();

            log += "<br>1台主动投标返回报文:" + FastJSON.toJOSN(p);

            LogInfo.WriteLog(log);
            if (ret == 0)
            {
                if (p.RespCode == "000")
                {

                    #region 验签缓存处理
                    string cachename = p.OrdId + "ToUserTransfer" + p.InCustId;

                    if (Utils.GeTThirdCache(cachename) == 0)
                    {

                        Utils.SetThirdCache(cachename);
                        B_usercenter BUC = new B_usercenter();


                        int bid_records_id = DbHelperSQL.Execint(" select  amtproid from hx_UserAct where  OrderID='" + p.OrdId + "' and UseState=5");

                        int ic = BUC.UpateActToUserTransfer(p, bid_records_id);
                        if (ic > 0)
                        {

                            string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + p.InCustId + "'";
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                            if (dt.Rows.Count > 0)
                            {
                                int ActID = int.Parse(p.MerPriv);
                                hx_ActivityTable hat = ef.hx_ActivityTable.Where(c => c.ActID == ActID).FirstOrDefault();



                                #region 流水信息
                                /*
                                B_usercenter ors = new B_usercenter();
                                decimal di = ors.GetUsridAvailable_balance(int.Parse(dt.Rows[0]["registerid"].ToString()));
                                // di = di + decimal.Parse(hua.Amt.ToString());
                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into hx_Capital_account_water(");
                                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                strSql.Append(" values (");
                                strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + "," + decimal.Parse(p.TransAmt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.现金奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "现金奖励" + "')");

                                DbHelperSQL.RunSql(strSql.ToString());

                                strSql.Clear();
                                */
                                #endregion

                                #region 奖励流水
                                M_bonus_account_water mbaw = new M_bonus_account_water();
                                B_bonus_account_water bbaw = new B_bonus_account_water();
                                DateTime dte = DateTime.Now;
                                mbaw.bonus_account_id = int.Parse(hat.ActID.ToString());
                                mbaw.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                mbaw.income = decimal.Parse(p.TransAmt);
                                mbaw.expenditure = 0.00M;
                                mbaw.time_of_occurrence = DateTime.Now;

                                mbaw.award_description = hat.ActName + "奖励已汇入个人账户";
                                mbaw.water_type = 0;
                                bbaw.Add(mbaw);

                                #endregion


                                #region MyRegion  系统消息
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = hat.ActName;
                                pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目，现金奖励 " + p.TransAmt + "元。如有问题可咨询创利投的客服！";
                                pm.PubTime = dti;
                                pm.Mtype = 2;
                                B_usercenter.AddMessage(pm);
                                #endregion

                            }
                        }


                    }
                    #endregion

                    str = "RECV_ORD_ID_" + p.OrdId.ToString();

                }
                else
                {

                }

            }

            return Content(str);
        }
        #endregion



        #region 汇付绑卡回调
        /// <summary>
        /// 汇付绑卡回调
        /// </summary>
        /// <returns></returns>
        public ActionResult Bgthirdpartybindbank()
        {

            string srt = "";
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

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            string log = "";
            log += "签名返回结果:" + ret.ToString();

            log += "<br>绑卡报文:" + FastJSON.toJOSN(m);
            LogInfo.WriteLog(log);
            if (ret == 0)
            {

                if (m.RespCode == "000")
                {

                    LogInfo.WriteLog("进入处理:UsrCustId" + m.UsrCustId);
                    M_UsrBindCard p = new M_UsrBindCard();
                    B_UsrBindCard o = new B_UsrBindCard();
                    // p.registerid = 0;
                    p.UsrCustId = m.UsrCustId;
                    p.OpenAcctId = m.OpenAcctId;
                    p.OpenBankId = m.OpenBankId;

                    if (o.Exists(p.UsrCustId))
                    {

                        p.defCard = 0;
                    }
                    else
                    {
                        p.defCard = 1;
                    }

                    LogInfo.WriteLog("卡状态：" + p.defCard.ToString());

                    bool df = o.Add(p);
                    LogInfo.WriteLog("绑卡卡数据写入：" + df);

                    string sql = "update hx_member_table set isbankcard=1 where UsrCustId='" + m.UsrCustId + "'";

                    DbHelperSQL.ExecuteSql(sql);




                    srt = "RECV_ORD_ID_" + m.TrxId;



                    LogInfo.WriteLog("卡绑定操作签名成功" + srt);
                }
                else
                {
                    LogInfo.WriteLog("卡绑定操作签名失败");
                }

            }
            else
            {
                LogInfo.WriteLog("签名失败");
            }
            return Content(srt);

        }
        #endregion

        #region 汇付用户资金解冻回调
        /// <summary>
        /// 汇付用户资金解冻回调
        /// </summary>
        /// <returns></returns>
        public ActionResult BG_UsrUnFreeze()
        {

            string srt = "";
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

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            string log = "";
            log += "签名返回结果:" + ret.ToString();
            log += "<br>解冻回调报文:" + FastJSON.toJOSN(m);
            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    //ToDO
                    LogInfo.WriteLog("进入处理:资金解冻未实现" + m.UsrCustId);
                    srt = "RECV_ORD_ID";
                }
            }
            else
            {
                log += "<br>签名失败";
            }
            LogInfo.WriteLog(log);
            return Content(srt);

        }
        #endregion

    }
}