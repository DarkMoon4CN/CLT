using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using System.Text;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model.chinapnr.CashAudit;
using System.Data;
using ChuanglitouP2P.BLL;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 提现申请管理
    /// </summary>
    public class UserCashController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/UserCash
        [AdminVaildate()]
        public ActionResult Index(string username = "", int OrdIdState = -1, int ddlType = 3, int OrdIdState1 = -1, int Page = 1, int pageSize = 10)
        {
            Response.BufferOutput = true;
            int pageNumber = Page / 1;
            Expression<Func<V_UserCash_Bank, bool>> where = PredicateExtensionses.True<V_UserCash_Bank>();
            where = where.And(p => p.UserCashId > 0);

            if (ddlType != 3 && OrdIdState == -1)
            {
                OrdIdState = OrdIdState1;
            }

            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username));
            }
            if (OrdIdState >= 0)
            {
                where = where.And(p => p.OrdIdState == (OrdIdState));
            }
            IPagedList<V_UserCash_Bank> list = ef.V_UserCash_Bank.Where(where).OrderByDescending(p => p.UserCashId).ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.OrdIdState = OrdIdState;
            ViewBag.ddlType = ddlType;
            ViewBag.OrdIdState1 = OrdIdState1;

            return View(list);
        }


        public ActionResult CashProcessing(int UserCashId)
        {
            var model = ef.V_UserCash_Bank.Where(p => p.UserCashId == UserCashId).SingleOrDefault();
            IEnumerable<SelectListItem> ddlList = Utils.GetEnumToList(typeof(EnumOrdIdState)).Select(c => new SelectListItem { Value = c.key.ToString(), Text = c.value.ToString() });

            ViewBag.ddlList = ddlList;
            return View(model);
        }


        


        #region 向第三方汇付提交复核
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult PostCashProcessing(V_UserCash_Bank model)
        {
            Response.BufferOutput = true;
            StringBuilder strz = new StringBuilder();
            int state = 0;
            M_CashAudit mc = new M_CashAudit();
            string retUrl = Utils.GetRe_url("admin/UserCash/RePostCashProcessing");
            string bgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");
            bool res = BusinessLogicHelper.postCashHelper(model, ref state, ref mc, retUrl, bgRetUrl);
            if (!res && state == 0)
            {
                return Content(StringAlert.Alert("取现强制未通过!", "/admin/UserCash/CashProcessing?UserCashId=" + model.UserCashId));
            }
            if (res)
            {
                strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");
                strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + mc.Version + "\" />");
                strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + mc.CmdId + "\" />");
                strz.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + mc.MerCustId + "\" />");
                strz.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + mc.OrdId + "\" />");
                strz.Append("<input id=\"UsrCustId\" name=\"UsrCustId\" type=\"hidden\"  value=\"" + mc.UsrCustId + "\" />");
                strz.Append("<input id=\"TransAmt\" name=\"TransAmt\" type=\"hidden\"  value=\"" + mc.TransAmt + "\" />");
                strz.Append("<input id=\"AuditFlag\"  name=\"AuditFlag\" type=\"hidden\"  value=\"" + mc.AuditFlag + "\" />");
                strz.Append("<input id=\"RetUrl\" name=\"RetUrl\" type=\"hidden\"  value=\"" + mc.RetUrl + "\" />");
                strz.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + mc.BgRetUrl + "\" />");
                strz.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + mc.MerPriv + "\" />");
                strz.Append("<input id=\"ChkValue\" name=\"ChkValue\" type=\"hidden\"  value=\"" + mc.ChkValue + "\" />");
                strz.Append(" </form>");
                strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");
                LogInfo.WriteLog("提交参数表单:" + strz.ToString());
            }
            ViewBag.state = state;
            ViewBag.strz = strz;
            return View();
        }



        #endregion

        #region 复核 汇付通知页
        /// <summary>
        /// 复核 汇付通知页
        /// </summary>
        /// <returns></returns>
        public ActionResult RePostCashProcessing()
        {
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
            m.RetUrl = DNTRequest.GetString("RetUrl");
            m.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.ChkValue = DNTRequest.GetString("ChkValue");

            LogInfo.WriteLog("取现审核返回参数:" + FastJSON.toJOSN(m));
            BusinessLogicHelper.RePostCashHelper(m);
            ViewBag.info = Utils.GetReturnCode(Int32.Parse(m.RespCode));
            return View();
        }


        #endregion

        [HttpPost]
        [AdminVaildate()]
        public ActionResult CashProcessingMore(string str)
        {
            string json = "";
            if (string.IsNullOrEmpty(str))
            {
                json = @"{""ret"":0,""msg"":""参数错误""}";
            }
            string strCount = "";

            string bid = str;

            string[] s = bid.Split(new char[] { ',' });

            string sqllist = "";

            for (int i = 0; i < s.Length; i++)
            {
                sqllist = sqllist + "'" + s[i] + "',";

            }

            if (sqllist.Contains(","))
            {
                bid = Utils.ClearLastChar(sqllist);
            }

            int orstate = 1;
            sqllist = " select registerid,UserCashId,OrdId,OrdIdState,TransAmt,BankName,OpenBankId,OpenAcctId,UsrCustId,realname,Reason,Remarks,available_balance from V_UserCash_Bank ";
            sqllist += " where  UserCashId in (" + bid + ")";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sqllist);

            int succ = 0, lost = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                M_CashAudit mc = new M_CashAudit();
                mc.Version = "10";
                mc.CmdId = "CashAudit";
                mc.MerCustId = Utils.GetMerCustID();
                mc.OrdId = dt.Rows[i]["OrdId"].ToString();
                mc.UsrCustId = dt.Rows[i]["UsrCustId"].ToString();
                mc.TransAmt = dt.Rows[i]["TransAmt"].ToString();

                //判定用户在审核期间内把卡号 移除或取消绑定 或 绑定 状态等于0
                string bindCardSql = " SELECT * FROM hx_UsrBindCardC WHERE UsrCustId='{0}' AND OpenAcctId='{1}'  ";
                bindCardSql = string.Format(bindCardSql, dt.Rows[i]["UsrCustId"].ToString(), dt.Rows[i]["OpenAcctId"].ToString());
                DataTable bcDt = DbHelperSQL.GET_DataTable_List(bindCardSql);
                if (bcDt == null || bcDt.Rows.Count == 0)
                {
                    orstate = 4;
                }

                if (orstate == 4)
                {
                    mc.AuditFlag = "R";
                }
                else
                {
                    mc.AuditFlag = "S";
                }

                // mc.BgRetUrl = "";
                mc.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");

                mc.MerPriv = "chuanglitou";

                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(mc.Version);
                chkVal.Append(mc.CmdId);
                chkVal.Append(mc.MerCustId);
                chkVal.Append(mc.OrdId);
                chkVal.Append(mc.UsrCustId);
                chkVal.Append(mc.TransAmt);
                chkVal.Append(mc.AuditFlag);
                // chkVal.Append(mc.RetUrl);
                chkVal.Append(mc.BgRetUrl);
                chkVal.Append(mc.MerPriv);

                string chkv = chkVal.ToString();

                LogInfo.WriteLog("批量取现审核chkv字符:" + chkv);

                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int strer = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
                mc.ChkValue = sbChkValue.ToString();

                LogInfo.WriteLog("批量取现审核提交信息：" + FastJSON.toJOSN(mc));

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values.Add("Version", mc.Version);
                    values.Add("CmdId", mc.CmdId);
                    values.Add("MerCustId", mc.MerCustId);
                    values.Add("OrdId", mc.OrdId);
                    values.Add("UsrCustId", mc.UsrCustId);
                    values.Add("TransAmt", mc.TransAmt);
                    values.Add("AuditFlag", mc.AuditFlag);
                    values.Add("BgRetUrl", mc.BgRetUrl);
                    values.Add("MerPriv", mc.MerPriv);
                    values.Add("ChkValue", mc.ChkValue);

                    string url = Utils.GetChinapnrUrl();
                    //同步发送form表单请求
                    byte[] result = client.UploadValues(url, "POST", values);
                    var retStr = Encoding.UTF8.GetString(result);
                    //  Response.Write(retStr);

                    //   LogInfo.WriteLog("批量取现审同步form表单请求" + retStr);

                    ReCashAudit ReCa = new ReCashAudit();

                    var Re = (ReCashAudit)FastJSON.ToObject(retStr, ReCa);

                    LogInfo.WriteLog("批量取现审返回报文：" + FastJSON.toJOSN(Re));




                    StringBuilder builder = new StringBuilder();
                    builder.Append(Re.CmdId);
                    builder.Append(Re.RespCode);
                    builder.Append(Re.MerCustId);
                    builder.Append(Re.OrdId);
                    builder.Append(Re.UsrCustId);
                    builder.Append(Re.TransAmt);
                    builder.Append(Re.OpenAcctId);
                    builder.Append(Re.OpenBankId);
                    builder.Append(Re.AuditFlag);
                    builder.Append(HttpUtility.UrlDecode(Re.BgRetUrl));
                    builder.Append(Re.MerPriv);

                    var msg = builder.ToString();

                    LogInfo.WriteLog("批量取现审验签文明:" + msg);
                    //验签
                    string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                    int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, Re.ChkValue);




                    LogInfo.WriteLog("批量取现审验签 ret= " + ret.ToString());

                    if (ret == 0)
                    {
                        //更新数据库
                        string sql = "";

                        if (orstate == 1)
                        {
                            sql = "update hx_td_UserCash set Remarks='' where  UserCashId=" + dt.Rows[i]["UserCashId"].ToString();
                            DbHelperSQL.RunSql(sql);
                        }
                        else if (orstate == 3)
                        {

                            sql = "update hx_td_UserCash set Remarks='',OrdIdState=3 ,OperTime='" + DateTime.Now.ToString() + "' where  UserCashId=" + dt.Rows[i]["UserCashId"].ToString();
                            DbHelperSQL.RunSql(sql);
                        }
                        else if (orstate == 4)
                        {

                            //  sql = "update hx_td_UserCash set Remarks='" + Utils.CheckSQLHtml(model.Remarks) + "', Reason='" + Utils.CheckSQLHtml(model.Reason) + "',OrdIdState=4 ,OperTime='" + DateTime.Now.ToString() + "'  where  UserCashId=" + UserCashId.ToString();
                            // sql += ";update  hx_member_table set available_balance=available_balance+" + dt.Rows[0]["TransAmt"].ToString() + ",frozen_sum=frozen_sum-" + dt.Rows[0]["TransAmt"].ToString() + " where registerid=" + dt.Rows[0]["registerid"].ToString();
                            //  DbHelperSQL.RunSql(sql);

                            // return Content(StringAlert.Alert("取现未通过，金额已退还!", "/admin/UserCash/CashProcessing?UserCashId=" + model.UserCashId));
                        }


                        if (Re.RespCode == "000" || Re.RespCode == "999" || Re.RespCode == "406")
                        {
                            string cachename = Re.OrdId + "Cash" + Re.UsrCustId + Re.TransAmt.ToString();

                            if (Utils.GeTThirdCache(cachename) == 0)
                            {
                                Utils.SetThirdCache(cachename);

                                //提现成功后，得多事务处理账户金额，流水等
                                B_usercenter BUC = new B_usercenter();
                                M_Capital_account_water aw = new M_Capital_account_water();
                                if (BUC.Su_CashProcessing(Re, aw) > 0)
                                {
                                    strCount += Re.OrdId + " 取款审核成功<br /> ";
                                    LogInfo.WriteLog("后台取款审核成功:" + str);
                                    succ = succ + 1;
                                }
                                else
                                {
                                    lost = lost + 1;
                                    strCount += Re.OrdId + " 取款审核失败<br /> ";
                                    LogInfo.WriteLog("后台取款审核事误操作失败");
                                }
                            }
                        }
                        else
                        {
                            lost = lost + 1;
                            strCount += Re.OrdId + " 取款审核失败 (" + Re.RespCode + ")<br /> ";
                            LogInfo.WriteLog("后台提现失败" + Re.RespCode);
                        }
                    }
                    else
                    {
                        //  Response.Write(Re.UsrCustId + "取款审核验签失败  原因: " + Re.RespDesc + " <br>");

                    }
                }
            }
            json = @"{""ret"":1,""msg"":""<div style='margin: 9px;line-height:20px;'>批量复核操作成功 RR</div>"" }";
            string sfd = succ.ToString() + "笔成功, " + lost.ToString() + "失败<br />" + strCount + " ";
            json = json.Replace("RR", sfd);
            return Content(json, "text/json");
        }
    }
}