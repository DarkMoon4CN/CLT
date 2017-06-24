using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuanglitouP2P.Model.chinapnr.UsrAcctPay;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class RechargeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 充值列表
        /// </summary>
        /// <param name="orderNO"></param>
        /// <param name="paystate"></param>
        /// <param name="txtStart"></param>
        /// <param name="txtEnd"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Index(string orderNO = "", int paystate = -1, string txtStart = "", string txtEnd = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;

            //var datas = from a in ef.V_Recharge_user_bank orderby a.recharge_history_id descending select new { a.recharge_history_id, a.recharge_amount, a.recharge_time, a.account_amount, a.order_No, a.recharge_condition, a.recharge_bank, a.username, a.BankName, a.realname };

            Expression<Func<V_Recharge_user_bank, bool>> where = PredicateExtensionses.True<V_Recharge_user_bank>();
            where = where.And(p => p.recharge_history_id > 0);
            if (!string.IsNullOrEmpty(orderNO))
            {
                //datas = datas.Where(p => p.order_No.Contains(orderNO));
                where = where.And(p => p.order_No.Contains(orderNO));
            }
            if (paystate >= 0)
            { 
                where = where.And(p => p.recharge_condition == paystate);
            }
            if (!string.IsNullOrEmpty(txtStart))
            {
                 DateTime dtStart = Convert.ToDateTime(txtStart);
                where = where.And(p => DbFunctions.DiffDays(p.recharge_time, dtStart) <= 0);
              
            }
            
            if (!string.IsNullOrEmpty(txtEnd))
            {
                DateTime dtEnd = Convert.ToDateTime(txtEnd);
                where = where.And(p => DbFunctions.DiffDays(p.recharge_time, dtEnd) >= 0);              
            }

            IPagedList<V_Recharge_user_bank> list = ef.V_Recharge_user_bank.Where(where).OrderByDescending(p => p.recharge_history_id).ToPagedList(pageNumber, pageSize);

            ViewBag.orderNO = orderNO;
            ViewBag.paystate = paystate;
            ViewBag.txtStart = txtStart;
            ViewBag.txtEnd = txtEnd;

            return View(list);
        }

        #region 平台账户充值

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Recharge()
        {
            return View();
        }
        

        /// <summary>
        /// 充值，2
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_Money()
        {
           // float money = DNTRequest.GetFormFloat("money", 0);
           

            M_Recharge_history rh = new M_Recharge_history();
            B_Recharge_history b = new B_Recharge_history();


            B_UsrBindCard bu = new B_UsrBindCard();
            M_UsrBindCard bm = new M_UsrBindCard();

            decimal amt = DNTRequest.GetDecimal("money", 10000.00M);


            if (amt <= 0)
            {
                return Content(StringAlert.Alert("充值金额必须大于零!"), "text/html");
            }

            rh.membertable_registerid = 0;
            rh.recharge_amount = amt;
            rh.recharge_time = DateTime.Now;
            rh.account_amount = amt;
            rh.order_No = Utils.Createcode(); ;
            rh.recharge_condition = 0; //1表示充值成功
            rh.recharge_bank = ""; // 得接口返回;


            string CmdId = "NetSave";
            string MerCustId = Utils.GetMerCustID();
            string GateBusiId = "B2C";
            string UsrCustId = Utils.GetMerCustID(); //给商户充值
            string MerPriv = Utils.Base64Encoder("chuanglitou");
            //string MerPriv = "chuanglitou";
            string ChkValue = "";
            int Recid = b.Add(rh);
            M_NetSave mn = new M_NetSave();

            mn.Version = "10";
            mn.CmdId = CmdId;
            mn.MerCustId = MerCustId;
            mn.UsrCustId = UsrCustId;
            mn.OrdId = rh.order_No;
            mn.OrdDate = rh.recharge_time.ToString("yyyyMMdd");
            //  mn.GateBusiId = GateBusiId;
            //  mn.OpenBankId = OpenBankId;
            //  mn.DcFlag = DcFlag;
            mn.TransAmt = rh.recharge_amount.ToString("0.00");
            mn.RetUrl = Utils.GetRe_url("admin/Recharge/Su_Enterpriserecharge");

           // mn.RetUrl = "http://localhost:17745/admin/Recharge/Su_Enterpriserecharge";
            mn.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Re_Enterpriserecharge");
            mn.MerPriv = MerPriv;


            StringBuilder str = new StringBuilder();

            str.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

            str.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + mn.Version + "\" />");

            str.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + mn.CmdId + "\" />");

            str.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + mn.MerCustId + "\" />");

            str.Append("<input id=\"UsrCustId\" name=\"UsrCustId\" type=\"hidden\"  value=\"" + mn.UsrCustId + "\" />");

            str.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + mn.OrdId + "\" />");

            str.Append("<input id=\"OrdDate\" name=\"OrdDate\" type=\"hidden\"  value=\"" + mn.OrdDate + "\" />");

            str.Append("<input id=\"TransAmt\" name=\"TransAmt\" type=\"hidden\"  value=\"" + mn.TransAmt + "\" />");

            str.Append("<input id=\"RetUrl\" name=\"RetUrl\"  type=\"hidden\"  value=\"" + mn.RetUrl + "\" />");

            str.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + mn.BgRetUrl + "\" />");

            str.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + mn.MerPriv + "\" />");


            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(mn.Version);
            chkVal.Append(mn.CmdId);
            chkVal.Append(mn.MerCustId);
            chkVal.Append(mn.UsrCustId);
            chkVal.Append(mn.OrdId);
            chkVal.Append(mn.OrdDate);
            chkVal.Append(mn.TransAmt);
            chkVal.Append(mn.RetUrl);
            chkVal.Append(mn.BgRetUrl);
            chkVal.Append(mn.MerPriv);



            string chkv = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str1 = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            // Response.Write((str1.ToString()));

            // ChkValue = sbChkValue.ToString();

            mn.ChkValue = sbChkValue.ToString();

            //
            str.Append("<input id=\"ChkValue\"  name=\"ChkValue\" type=\"hidden\"  value=\"" + mn.ChkValue + "\" />");

            str.Append(" </form>");

            str.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

            LogInfo.WriteLog("企业充值提交表单:" + str.ToString());

            ViewBag.str = str.ToString();

            return View();
        }

        #endregion


        #region 企业充值汇付返回通知页
        /// <summary>
        /// 企业充值汇付返回通知页
        /// </summary>
        /// <returns></returns>
        public ActionResult Su_Enterpriserecharge()
        {

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

                        //string sql = "update hx_Recharge_history set recharge_condition=1,recharge_bank='" + m.GateBankId + "' where  order_No='" + m.OrdId + "'";

                        //DbHelperSQL.RunSql(sql);
                    }
                  }
                else
                {
                   
                }
            }
            else
            {
                //失败

                // Response.Write("验签失败");

            }

            #endregion




            return View(m);
        } 
        #endregion




        #region 用户向平台划账

        [AdminVaildate()]
        public ActionResult UserToPlatform()
        {
            ViewBag.InCustId = Utils.GetMerCustID();
            return View();
        }
                        
        public ActionResult UserToPlatformMoney(string UsrCustId="", decimal money=0, string InCustId="")
        {
            if (string.IsNullOrEmpty(UsrCustId))
            {
                return Content(StringAlert.Alert("客户号不可以为空!"), "text/html");
            }
            if (string.IsNullOrEmpty(InCustId))
            {
                return Content(StringAlert.Alert("入账客户号不可以为空!"), "text/html");
            }
            if (money<=0)
            {
                return Content(StringAlert.Alert("转账金额错误!"), "text/html");
            }
            M_UsrAcctPay m = new M_UsrAcctPay();
            m.Version = "10";
            m.CmdId = "UsrAcctPay";
            m.OrdId = Utils.Createcode();
            // m.UsrCustId = "6000060000898051";
            m.UsrCustId = Utils.CheckSQLHtml(UsrCustId);

            m.MerCustId = Utils.GetMerCustID();
            m.InAcctId = "MDT000001";
            m.InAcctType = "MERDT";
            m.RetUrl = Utils.GetRe_url("admin/Recharge/UsrPay");
            m.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgUsrPay");

            //m.TransAmt = "7749.60";
            m.TransAmt = money.ToString("0.00");     
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.InAcctId);
            chkVal.Append(m.InAcctType);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            string chkv = chkVal.ToString();
            LogInfo.WriteLog("加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
            LogInfo.WriteLog("向平台转帐加签字符:" + str.ToString());
            m.ChkValue = sbChkValue.ToString();
            LogInfo.WriteLog("向平台转帐提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("ChkValue:" + m.ChkValue);

            StringBuilder strz = new StringBuilder();
            strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");
            strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + m.Version + "\" />");
            strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + m.CmdId + "\" />");
            strz.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + m.OrdId + "\" />");
            strz.Append("<input id=\"UsrCustId\" name=\"UsrCustId\" type=\"hidden\"  value=\"" + m.UsrCustId + "\" />");
            strz.Append("<input id=\"MerCustId\" name=\"MerCustId\" type=\"hidden\"  value=\"" + m.MerCustId + "\" />");
            strz.Append("<input id=\"TransAmt\"  name=\"TransAmt\" type=\"hidden\"  value=\"" + m.TransAmt + "\" />");
            strz.Append("<input id=\"InAcctId\"   name=\"InAcctId\" type=\"hidden\"  value=\"" + m.InAcctId + "\" />");
            strz.Append("<input id=\"InAcctType\" name=\"InAcctType\" type=\"hidden\"  value=" + m.InAcctType + " />");
            strz.Append("<input id=\"RetUrl\" name=\"RetUrl\" type=\"hidden\"  value=\"" + m.RetUrl + "\" />");
            strz.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + m.BgRetUrl + "\" />");
            strz.Append("<input id=\"ChkValue\" name=\"ChkValue\" type=\"hidden\"  value=\"" + m.ChkValue + "\" />");
            strz.Append(" </form>");
            strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");
            LogInfo.WriteLog(strz.ToString());
            ViewBag.str = strz.ToString();
            return View();
        }
        #endregion


        #region 用户向平台划账汇付通知页
        /// <summary>
        /// 用户向平台划账汇付通知页
        /// </summary>
        /// <returns></returns>
        public ActionResult UsrPay()
        {
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
                    }
                }            

            }           
            return View(m);
        } 
        #endregion


        #region 平台向用户划账

        [AdminVaildate()]
        public ActionResult PlatformToUser()
        {
            return View();
        }

        
        public ActionResult PlatformToUserMoney(string UsrCustId="",decimal money=0)
        {
            string keypassword = ConfigurationManager.AppSettings["RechargePassWord"].ToString();
            string password = DNTRequest.GetFormString("pssword");

            if (string.IsNullOrEmpty(UsrCustId))
            {
                return Content(StringAlert.Alert("客户号不可以为空!"), "text/html");
            }
            if (money <= 0)
            {
                return Content(StringAlert.Alert("转账金额错误!"), "text/html");
            }
            if (String.IsNullOrEmpty(password))
            {
                return Content(StringAlert.Alert("密码不能为空！"),"text/html");
            }
            if (keypassword != password)
            {
                return Content(StringAlert.Alert("充值密码不正确"), "text/html");
            }

            M_Transfer m = new M_Transfer();
            m.Version = "10";
            m.CmdId = "Transfer";
            m.OrdId = Utils.Createcode();
            m.OutCustId = Utils.GetMerCustID();
            m.OutAcctId = "MDT000001";
            m.TransAmt = money.ToString("0.00");
            m.InCustId = Utils.CheckSQLHtml(UsrCustId);
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
            string _resultStr = "";
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
                    if (retloan.RespCode == "000")
                    {
                        //_resultStr = retloan.RespCode.ToString() + "  <br>  " + HttpUtility.UrlDecode(retloan.RespDesc) + "<br>转账成功";
                        #region 流水信息
                        string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retloan.InCustId + "'";
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
                            strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + "," + decimal.Parse(retloan.TransAmt) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + (di+ decimal.Parse(retloan.TransAmt)) + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.平台向用户划账.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "平台向用户划账" + "')");
                            DbHelperSQL.RunSql(strSql.ToString());
                            strSql.Clear();
                        }
                        #endregion
                        _resultStr = "<br>转账成功";
                    }
                    else
                    {
                        _resultStr = HttpUtility.UrlDecode(retloan.RespDesc);
                    }
                }
            }
            ViewBag.str = _resultStr;
            return View();
        }


        #endregion

        #region 连连充值列表

        [AdminVaildate()]
        public ActionResult LLPayList(string orderNO = "", int paystate = -1, string txtStart = "", string txtEnd = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;

            Expression<Func<V_LLPay_Re, bool>> where = PredicateExtensionses.True<V_LLPay_Re>();
            where = where.And(p => p.Rechargeid > 0);

            if (!string.IsNullOrEmpty(orderNO))
            {
                where = where.And(p => p.no_order.Contains(orderNO));
            }
            if (paystate >= 0)
            {
                where = where.And(p => p.ReState == paystate);
            }
            if (!string.IsNullOrEmpty(txtStart))
            {
                DateTime dtStart = Convert.ToDateTime(txtStart);
                where = where.And(p => Convert.ToDateTime(p.ordertime) >= dtStart);
               // where = where.And(p => DbFunctions.DiffDays(p.ordertime, dtStart) <= 0);
            }

            if (!string.IsNullOrEmpty(txtEnd))
            {
                DateTime dtEnd = Convert.ToDateTime(txtEnd);
                where = where.And(p => Convert.ToDateTime(p.ordertime) <= dtEnd);
            }


            IPagedList<V_LLPay_Re> list = ef.V_LLPay_Re.Where(where).OrderByDescending(p => p.Rechargeid).ToPagedList(pageNumber, pageSize);

            ViewBag.orderNO = orderNO;
            ViewBag.paystate = paystate;
            ViewBag.txtStart = txtStart;
            ViewBag.txtEnd = txtEnd;

            return View(list);
        }

        #endregion
    }
}