//using ChuangLiTou.Core.BusinessLogic;
//using ChuangLiTou.Core.BusinessLogic.OldVersion;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Request.Recharge;
using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Model.chinapnr.NetSave;

namespace ChuangLiTouOpenApi.Areas.Recharge.Controllers
{
    /// <summary>
    /// 充值
    /// </summary>
    public class IndexController : BaseController
    {
        // GET: Recharge/Index
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 充值接口
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult RechargeSubmit(RequestParam<RequestRecharge> reqst)
        {
            LoggerHelper.Info(JsonHelper.Entity2Json(reqst));
            MemberLogic mLogic = new MemberLogic();
            StringBuilder str = new StringBuilder();
            string blankName = reqst.body.bankType;
            int userid = reqst.body.userId;
            var p = mLogic.SelectMemberByUserId(reqst.body.userId);
            if (p.UsrCustId.Length <= 0)
            {
                RequestParam<RequestValidate> vldParam = new RequestParam<RequestValidate>();
                RequestValidate rv = new RequestValidate();
                rv.userId = reqst.body.userId.ToString();
                vldParam.body = rv;
                return RedirectToAction("RequestRealName", "Index", new { area = "UserAuthentication", userId = reqst.body.userId.ToString() });//未实名，跳转至实名接口
            }

            #region 提现审核时，如果出现没有快捷卡时,禁止提现,防止用户绑定快捷卡
            string openAcctIds = string.Empty;
            var userCardList = mLogic.SelectUserBankList(userid);
            var quickList = new List<MemberBankEntity>();
            if (userCardList == null)
                userCardList = new List<MemberBankEntity>();
            if (userCardList != null)
                quickList = userCardList.Where(d => d.BindCardType == 1).ToList();

            if (quickList == null || quickList.Count == 0) //用户没有快捷卡时，有提现审核都拒绝
            {
                foreach (var item in userCardList)
                {
                    if (openAcctIds == string.Empty)
                    {
                        openAcctIds += "'" + item.OpenAcctId + "'";
                    }
                    else
                    {
                        openAcctIds += "," + "'" + item.OpenAcctId + "'";
                    }
                }
                //if (string.IsNullOrWhiteSpace(openAcctIds))
                //{
                //    LoggerHelper.Info("充值请求失败！未绑定银行卡,暂不能进行此操作!" + JsonHelper.Entity2Json(reqst));
                //    return Content("充值请求失败！未绑定银行卡,暂不能进行此操作!");
                //}
                try
                {
                    if (!string.IsNullOrWhiteSpace(openAcctIds))
                    {
                        bool isExist = mLogic.SelectVUserCashBank(openAcctIds, 0);
                        if (isExist)
                        {
                            LoggerHelper.Info("充值失败,提现审核中,暂不能进行其他操作" + JsonHelper.Entity2Json(reqst));
                            return Content("充值审核中,暂不能进行其他操作!");
                        }
                    }
                }
                catch
                {
                    LoggerHelper.Error("充值数据出现异常,提现审核中,暂不能进行其他操作。SelectVUserCashBank，openAcctIds=" + openAcctIds + "===>" + JsonHelper.Entity2Json(reqst));
                }
            }
            #endregion



            string UsrCustId = p.UsrCustId; //这个是给用户充值  在充值前得保证商户余额足够
            decimal amt = reqst.body.amountOfCharge;
            M_QPNetSave qp = new M_QPNetSave();
            M_Recharge_history rh = new M_Recharge_history();
            rh.membertable_registerid = reqst.body.userId;
            rh.recharge_amount = Math.Round(amt, 2);
            rh.recharge_time = DateTime.Now;
            rh.account_amount = amt;
            rh.order_No = Settings.Instance.OrderCode;
            rh.recharge_condition = 0; //1表示充值成功
            rh.recharge_bank = blankName; // 得接口返回;

            string CmdId = "NetSave";
            string MerCustId = Settings.Instance.MerCustId;

            var ckd = Settings.Instance.SiteDomain;
            RechargeHistoryLogic hLogic = new RechargeHistoryLogic();


            int Recid = hLogic.Add(rh);
            if (Recid > 0)
            {
                string MerPriv = EncryptHelper.Encrypt(reqst.body.userId + "_" + Recid, Settings.Instance.WebPass);

                qp.Version = "10";
                qp.CmdId = CmdId;
                qp.MerCustId = MerCustId;
                qp.UsrCustId = UsrCustId;
                qp.OrdId = rh.order_No;
                qp.OrdDate = rh.recharge_time.ToString("yyyyMMdd");
                qp.GateBusiId = "QP";  //快捷支付
                qp.OpenBankId = blankName;
                qp.DcFlag = "D";
                qp.TransAmt = amt.ToString("0.00");
                qp.RetUrl = Settings.Instance.GetCallbackUrl("/Recharge/Index/SuQPNetSave");
                qp.BgRetUrl = Settings.Instance.GetCallbackUrl("/Recharge/Index/ReQPNetSave");
                qp.MerPriv = MerPriv;


                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(qp.Version);
                chkVal.Append(qp.CmdId);
                chkVal.Append(qp.MerCustId);
                chkVal.Append(qp.UsrCustId);
                chkVal.Append(qp.OrdId);
                chkVal.Append(qp.OrdDate);
                chkVal.Append(qp.GateBusiId);
                chkVal.Append(qp.OpenBankId);
                chkVal.Append(qp.DcFlag);
                chkVal.Append(qp.TransAmt);
                chkVal.Append(qp.RetUrl);
                chkVal.Append(qp.BgRetUrl);
                chkVal.Append(qp.OpenAcctId);
                // chkVal.Append(qp.CertId);
                chkVal.Append(qp.MerPriv);
                string chkv = chkVal.ToString();
                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str1 = DllInterop.SignMsg(Settings.Instance.MerId, merKeyFile, chkv, len, sbChkValue);
                qp.ChkValue = sbChkValue.ToString();
                str.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Settings.Instance.ChinapnrUrl + "\" method=\"post\">");

                str.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + qp.Version + "\" />");

                str.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + qp.CmdId + "\" />");

                str.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + qp.MerCustId + "\" />");

                str.Append("<input id=\"UsrCustId\" name=\"UsrCustId\" type=\"hidden\"  value=\"" + qp.UsrCustId + "\" />");

                str.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + qp.OrdId + "\" />");

                str.Append("<input id=\"OrdDate\" name=\"OrdDate\" type=\"hidden\"  value=\"" + qp.OrdDate + "\" />");

                str.Append("<input id=\"GateBusiId\"  name=\"GateBusiId\" type=\"hidden\"  value=\"" + qp.GateBusiId + "\" />");

                str.Append("<input id=\"OpenBankId\"   name=\"OpenBankId\" type=\"hidden\"  value=\"" + qp.OpenBankId + "\" />");

                str.Append("<input id=\"DcFlag\" name=\"DcFlag\" type=\"hidden\"  value=\"" + qp.DcFlag + "\" />");

                str.Append("<input id=\"TransAmt\" name=\"TransAmt\" type=\"hidden\"  value=\"" + qp.TransAmt + "\" />");

                str.Append("<input id=\"RetUrl\" name=\"RetUrl\"  type=\"hidden\"  value=\"" + qp.RetUrl + "\" />");

                str.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + qp.BgRetUrl + "\" />");

                //  str.Append("<input id=\"CertId\" name=\"CertId\" type=\"hidden\"  value=\"" + qp.CertId + "\" />");

                str.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + qp.MerPriv + "\" />");

                str.Append("<input id=\"ChkValue\"  name=\"ChkValue\" type=\"hidden\"  value=\"" + qp.ChkValue + "\" />");

                str.Append(" </form>");

                str.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

                LoggerHelper.Info("快捷充值提交表单:" + str.ToString());
            }
            ViewBag.str = str.ToString();

            return View();

        }

        #region 快捷充值返回接口
        /// <summary>
        /// 快捷充值返回接口
        /// </summary>
        /// <returns></returns>
        public ActionResult SuQPNetSave()
        {
            ReQPNetSave m = new ReQPNetSave();
            lock (this)
            {
                Settings.Instance.SetSYSDateTimeFormat();

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
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(msg).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);
                // Response.Write("验签：" + ret.ToString());
                LoggerHelper.Info("快充接口前台验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc + "<br/>" + "快充接口前台充值返回报文:" + JsonHelper.Entity2Json(m));
                StringBuilder str = new StringBuilder();
                string sql = "";

                int bindcardtype = 0;
                if (ret == 0)
                {
                    if (m.RespCode == "000")
                    {
                        string MerPrivTemp = EncryptHelper.Decrypt(m.MerPriv, Settings.Instance.WebPass);
                        string[] arr = Settings.Instance.SplitString(MerPrivTemp, "_");  //第一位是用户id 二是 记录id
                        int userid = int.Parse(arr[0]);
                        int reid = int.Parse(arr[1]);
                        string cachename = m.OrdId + userid.ToString() + reid.ToString();

                        LoggerHelper.Info(" >>>>>>>>>>>>>>>>>>>>>>>" + cachename);
                        if (Settings.Instance.GeTThirdCache(cachename) == 0)
                        {
                            Settings.Instance.SetThirdCache(cachename);
                            sql = "select recharge_condition  from hx_Recharge_history where recharge_condition=0  and recharge_history_id=" + reid + " and order_No='" + m.OrdId + "'";
                            DataTable dtr = DbHelper.Query(sql).Tables[0];
                            if (dtr.Rows.Count > 0)
                            {
                                LoggerHelper.Info("快充接口前台充值数据没有写入情况下操作>>>>>>>>>>>>>>>>>>>>>>>");
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
                                LoggerHelper.Info("前台充值事务操作返回码小于=0 操作失败:" + bucrec.ToString());
                                if (m.GateBusiId == "QP")
                                {
                                    bindcardtype = 1;
                                    sql = "select UsrBindCardID from hx_UsrBindCardC where UsrCustId='" + m.UsrCustId + "' and OpenAcctId='" + m.CardId + "'";
                                    DataTable dt = DbHelper.Query(sql).Tables[0];



                                    if (dt.Rows.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        sql = "INSERT INTO hx_UsrBindCardC (UsrCustId,OpenAcctId,OpenBankId,defCard,BindCardType) VALUES ('" + m.UsrCustId + "','" + m.CardId + "','" + m.GateBankId + "',1,1)";
                                        DbHelper.ExecuteSql(sql);
                                        sql = "update hx_member_table set isbankcard=1 where registerid=" + userid.ToString();
                                        DbHelper.ExecuteSql(sql);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View(m);
        }
        #endregion

        #region 充值汇付后台主动通知
        /// <summary>
        /// 充值汇付后台主动通知
        /// </summary>
        /// <returns></returns>
        public ActionResult ReQPNetSave()
        {
            string str1 = "";
            lock (this)
            {
                Settings.Instance.SetSYSDateTimeFormat();

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
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(msg).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);
                // Response.Write("验签：" + ret.ToString());
                LoggerHelper.Info("快充接口后台验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc);

                LoggerHelper.Info("快充接口后台充值返回报文:" + JsonHelper.Entity2Json(m));
                StringBuilder str = new StringBuilder();
                string sql = "";

                if (ret == 0)
                {
                    if (m.RespCode == "000")
                    {
                        string MerPrivTemp = EncryptHelper.Decrypt(m.MerPriv, Settings.Instance.WebPass);
                        string[] arr = Settings.Instance.SplitString(MerPrivTemp, "_");  //第一位是用户id 二是 记录id
                        int userid = int.Parse(arr[0]);
                        int reid = int.Parse(arr[1]);
                        string cachename = m.OrdId + userid.ToString() + reid.ToString();

                        if (Settings.Instance.GeTThirdCache(cachename) == 0)
                        {
                            Settings.Instance.SetThirdCache(cachename);
                            sql = "select recharge_condition  from hx_Recharge_history where recharge_condition=0  and recharge_history_id=" + reid + " and order_No='" + m.OrdId + "'";
                            DataTable dtr = DbHelper.Query(sql).Tables[0];
                            if (dtr.Rows.Count > 0)
                            {
                                LoggerHelper.Info("快充接口后台充值数据没有写入情况下操作>>>>>>>>>>>>>>>>>>>>>>>");
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
                                LoggerHelper.Info("后台充值事务操作返回码小于=0 操作失败:" + bucrec.ToString());
                                if (m.GateBusiId == "QP")
                                {
                                    sql = "select UsrBindCardID from hx_UsrBindCardC where UsrCustId='" + m.UsrCustId + "' and OpenAcctId='" + m.CardId + "'";
                                    DataTable dt = DbHelper.Query(sql).Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        sql = "INSERT INTO hx_UsrBindCardC (UsrCustId,OpenAcctId,OpenBankId,defCard) VALUES ('" + m.UsrCustId + "','" + m.CardId + "','" + m.GateBankId + "',1)";
                                        DbHelper.Query(sql);
                                        sql = "update hx_member_table set  isbankcard=1 where registerid=" + userid.ToString();
                                        DbHelper.Query(sql);
                                    }
                                }
                            }

                        }
                        str1 = "RECV_ORD_ID_" + m.TrxId;
                    }
                }
            }
            return Content(str1);
        }
        #endregion
    }
}