using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.CashAudit;
using ChuangLitouP2P.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL
{
    public static class BusinessLogicHelper
    {
        /// <summary>
        /// 提现自动审核
        /// </summary>
        /// <param name="UsrCustId"></param>
        /// <param name="retUrl"></param>
        /// <param name="bgRetUrl"></param>
        public static void AutoCheckCash(string UsrCustId, string retUrl, string bgRetUrl)
        {
            chuangtouEntities ef = new chuangtouEntities();
            var user = ef.hx_member_table.Where(c => c.UsrCustId == UsrCustId).FirstOrDefault();
            if (user == null)
            {
                LogInfo.WriteLog("用户提现，自动审核异常：用户不存在！UsrCustID为：" + UsrCustId);
                return;
            }
            var userCash = ef.hx_td_UserCash.Where(c => c.registerid == user.registerid && (c.OrdIdState == 0 || c.OrdIdState == 1)).OrderByDescending(c => c.UserCashId).FirstOrDefault();
            if (userCash == null)
            {
                LogInfo.WriteLog("用户提现，自动审核异常：用户提现记录不存在！registerid为：" + user.registerid);
                return;
            }
            var model = ef.V_UserCash_Bank.Where(p => p.UserCashId == userCash.UserCashId).SingleOrDefault();
            if (model == null)
            {
                LogInfo.WriteLog("用户提现，自动审核异常：用户提现记录不存在！UserCashId为：" + userCash.UserCashId);
                return;
            }

            V_UserCash_Bank vub = new V_UserCash_Bank()
            {
                available_balance = model.available_balance,
                OrdIdState = 1,//model.OrdIdState,//审核通过
                UsrCustId = model.UsrCustId,
                BankName = model.BankName,
                FeeAmt = model.FeeAmt,
                FeeObjFlag = model.FeeObjFlag,
                mobile = model.mobile,
                OpenAcctId = model.OpenAcctId,
                OpenBankId = model.OpenBankId,
                OperTime = model.OperTime,
                OrdId = model.OrdId,
                OrdIdTime = model.OrdIdTime,
                realname = model.realname,
                Reason = model.Reason,
                registerid = model.registerid,
                Remarks = model.Remarks,
                TransAmt = model.TransAmt,
                TransState = model.TransState,
                UserCashId = model.UserCashId,
                useridentity = model.useridentity,
                username = model.username,
                usertypes = model.usertypes
            };

            StringBuilder strz = new StringBuilder();
            int state = 0;
            M_CashAudit mc = new M_CashAudit();
            //string retUrl = Utils.GetRe_url("admin/UserCash/RePostCashProcessing");
            //string bgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");
            bool postRes = postCashHelper(vub, ref state, ref mc, retUrl, bgRetUrl);
            if (!postRes && state == 0)
            {
                //return Content(StringAlert.Alert("取现强制未通过!", "/admin/UserCash/CashProcessing?UserCashId=" + model.UserCashId));
            }
            else if (postRes)
            {
                string url = Utils.GetChinapnrUrl();
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Version={0}", mc.Version);
                strBuilder.AppendFormat("&CmdId={0}", mc.CmdId);
                strBuilder.AppendFormat("&MerCustId={0}", mc.MerCustId);
                strBuilder.AppendFormat("&OrdId={0}", mc.OrdId);
                strBuilder.AppendFormat("&UsrCustId={0}", mc.UsrCustId);
                strBuilder.AppendFormat("&TransAmt={0}", mc.TransAmt);
                strBuilder.AppendFormat("&AuditFlag={0}", mc.AuditFlag);
                strBuilder.AppendFormat("&RetUrl={0}", mc.RetUrl);
                strBuilder.AppendFormat("&BgRetUrl={0}", mc.BgRetUrl);
                strBuilder.AppendFormat("&MerPriv={0}", mc.MerPriv);
                strBuilder.AppendFormat("&ChkValue={0}", mc.ChkValue);

                string html = Http.Post(url, strBuilder.ToString());

                Dictionary<string, string> resdic = new Dictionary<string, string>();
                string reg = "<input name=.*/>";
                Regex regex = new Regex(reg);
                var macs = regex.Matches(html);
                foreach (Match mac in macs)
                {
                    string[] input = mac.Value.Replace("'", "\"").Split('\"');
                    resdic.Add(input[1], input[7]);
                }
                ReCashAudit m = new ReCashAudit()
                {
                    AuditFlag = resdic["AuditFlag"],
                    BgRetUrl = resdic["BgRetUrl"],
                    MerCustId = resdic["MerCustId"],
                    ChkValue = resdic["ChkValue"],
                    CmdId = resdic["CmdId"],
                    FeeAcctId = resdic["FeeAcctId"],
                    FeeAmt = resdic["FeeAmt"],
                    FeeCustId = resdic["FeeCustId"],
                    MerPriv = resdic["MerPriv"],
                    OpenAcctId = resdic["OpenAcctId"],
                    OpenBankId = resdic["OpenBankId"],
                    OrdId = resdic["OrdId"],
                    RespCode = resdic["RespCode"],
                    RespDesc = resdic["RespDesc"],
                    RetUrl = resdic["RetUrl"],
                    TransAmt = resdic["TransAmt"],
                    UsrCustId = resdic["UsrCustId"]
                };
                LogInfo.WriteLog("自动审核审核数据回调：" + JsonConvert.SerializeObject(m));
                RePostCashHelper(m);
            }
        }
        /// <summary>
        /// 后台审核逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="state"></param>
        /// <param name="mc"></param>
        /// <param name="RetUrl"></param>
        /// <param name="BgRetUrl"></param>
        /// <returns></returns>
        public static bool postCashHelper(V_UserCash_Bank model, ref int state, ref M_CashAudit mc, string RetUrl, string BgRetUrl)
        {
            int UserCashId = model.UserCashId;
            int orstate = int.Parse(model.OrdIdState.ToString());
            string sql = " select registerid,OrdId,OrdIdState,TransAmt,BankName,OpenBankId,OpenAcctId,UsrCustId,realname,Reason,Remarks,available_balance from V_UserCash_Bank where UserCashId=" + UserCashId.ToString();

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            bool isExistCard = true;
            state = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (orstate == 5)//强制审核不通过,用于处理异常情况
                {
                    sql = "update hx_td_UserCash set Remarks='" + Utils.CheckSQLHtml(model.Remarks) + "', Reason='" + Utils.CheckSQLHtml(model.Reason) + "',OrdIdState=4 ,OperTime='" + DateTime.Now.ToString() + "'  where  UserCashId=" + UserCashId.ToString();
                    DbHelperSQL.RunSql(sql);
                    return false; //Content(StringAlert.Alert("取现强制未通过!", "/admin/UserCash/CashProcessing?UserCashId=" + model.UserCashId));
                }

                //判定用户在审核期间内把卡号 移除或取消绑定 或 绑定 状态等于0
                string bindCardSql = " SELECT * FROM hx_UsrBindCardC WHERE UsrCustId='{0}' AND OpenAcctId='{1}'  ";
                bindCardSql = string.Format(bindCardSql, dt.Rows[0]["UsrCustId"].ToString(), model.OpenAcctId);
                DataTable bcDt = DbHelperSQL.GET_DataTable_List(bindCardSql);
                if (bcDt == null || bcDt.Rows.Count == 0)
                {
                    orstate = 4;
                    isExistCard = false;
                }
                if (orstate == 1 || orstate == 4)
                {
                    //审核接口
                    state = 1;
                    mc = new M_CashAudit();
                    mc.Version = "10";
                    mc.CmdId = "CashAudit";
                    mc.MerCustId = Utils.GetMerCustID();
                    mc.OrdId = dt.Rows[0]["OrdId"].ToString();
                    mc.UsrCustId = dt.Rows[0]["UsrCustId"].ToString();
                    mc.TransAmt = dt.Rows[0]["TransAmt"].ToString();
                    if (orstate == 4)
                    {
                        mc.AuditFlag = "R";
                    }
                    else
                    {
                        mc.AuditFlag = "S";
                    }
                    //  mc.RetUrl =  "http://localhost:17745/admin/UserCash/RePostCashProcessing";
                    // mc.BgRetUrl = Utils.GetRe_url("111admin/Thirdparty/BgCashProcessing");
                    mc.RetUrl = RetUrl;//Utils.GetRe_url("admin/UserCash/RePostCashProcessing");
                    mc.BgRetUrl = BgRetUrl;// Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");
                    mc.MerPriv = "chuanglitou";
                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(mc.Version);
                    chkVal.Append(mc.CmdId);
                    chkVal.Append(mc.MerCustId);
                    chkVal.Append(mc.OrdId);
                    chkVal.Append(mc.UsrCustId);
                    chkVal.Append(mc.TransAmt);
                    chkVal.Append(mc.AuditFlag);
                    chkVal.Append(mc.RetUrl);
                    chkVal.Append(mc.BgRetUrl);
                    chkVal.Append(mc.MerPriv);

                    string chkv = chkVal.ToString();
                    LogInfo.WriteLog("取现审核chkv字符:" + chkv);
                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
                    mc.ChkValue = sbChkValue.ToString();
                    if (str == 0)
                    {
                        //更新数据库
                        string sql1 = "";
                        if (orstate == 1)
                        {
                            sql = "update hx_td_UserCash set Remarks='" + Utils.CheckSQLHtml(model.Remarks) + "' where  UserCashId=" + UserCashId.ToString();
                            DbHelperSQL.RunSql(sql);
                        }
                        else if (orstate == 3)
                        {
                            sql = "update hx_td_UserCash set Remarks='" + Utils.CheckSQLHtml(model.Remarks) + "',OrdIdState=3 ,OperTime='" + DateTime.Now.ToString() + "' where  UserCashId=" + UserCashId.ToString();
                            DbHelperSQL.RunSql(sql);
                        }
                        else if (orstate == 4)
                        {
                            if (!isExistCard)
                            {
                                List<string> strList = new List<string>();
                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into hx_Capital_account_water(");
                                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                strSql.Append(" values (");
                                strSql.Append("" + Convert.ToInt32(dt.Rows[0]["registerid"].ToString()) + "," + Convert.ToDecimal("0") + "," + Convert.ToDecimal("0") + ",'" + DateTime.Now.ToString() + "'," + (Convert.ToDecimal(dt.Rows[0]["available_balance"])) + "," + (int)EnumTypesFinance.提现卡不存在 + ",'" + DateTime.Now.ToString() + "','0','提现未通过')");
                                strList.Add(strSql.ToString());
                                var i = DbHelperSQL.ExecuteSqlTran(strList);
                                strSql.Clear();
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    // return Content(StringAlert.Alert("验签失败!", "/admin/UserCash/CashProcessing?UserCashId=" + model.UserCashId));
                }
            }
            return false;
        }
        /// <summary>
        /// 汇付回调逻辑
        /// </summary>
        /// <param name="m"></param>
        public static void RePostCashHelper(ReCashAudit m)
        {
            string str = "";
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

            LogInfo.WriteLog("前台取现复核验签返回参数:" + ret.ToString());

            #region 复核验签成功
            if (ret == 0)
            {
                //在406取现失败后,再次复核申请时会变成415
                if (m.RespCode == "000" || m.RespCode == "999" || m.RespCode == "406")
                {
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
        }
        /// <summary>
        /// 去除重复银行卡
        /// </summary>
        /// <param name="listBank"></param>
        /// <returns></returns>
        public static List<V_UsrBindCardBank> LeftOne(List<V_UsrBindCardBank> listBank)
        {
            var vubcTmp = (from item in listBank
                           group item by item.OpenAcctId into g
                           select new
                           {
                               openAcctID = g.Key,
                               cardCount = g.Count()
                           }).Where(c => c.cardCount > 1).ToList();
            if (vubcTmp != null && vubcTmp.Count > 0)
            {
                foreach (var v in vubcTmp)
                {
                    listBank.Remove(listBank.Where(c => c.OpenAcctId == v.openAcctID).First());
                }
            };

            return listBank;
        }
    }
}
