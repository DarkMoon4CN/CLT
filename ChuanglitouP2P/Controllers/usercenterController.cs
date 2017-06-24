using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLitouP2P.Models;
using EntityFramework.Extensions;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL;
using System.Text;
using ChuanglitouP2P.DBUtility;
using System.Configuration;
using System.Data;
using Webdiyer.WebControls.Mvc;
using System.Linq.Expressions;
using ChuanglitouP2P.Common.Extensionses;
using System.IO;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Areas.Admin.Controllers;
using System.Web.UI;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;

namespace ChuanglitouP2P.Controllers
{
    public class usercenterController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: usercenter
        public ActionResult Index()
        {
            
            int userid = Utils.checkloginsession();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();

            //最近回款
            // List<V_borrowing_Bid_records_income_statement> income = ef.V_borrowing_Bid_records_income_statement.Where(p=>p.investor_registerid==userid && p.payment_status>0).Take(5).ToList();
            List<V_borrowing_Bid_records_income_statement_uc> income = ef.V_borrowing_Bid_records_income_statement_uc.Where(p => p.investor_registerid == userid).OrderByDescending(p => p.bid_records_id).Take(5).ToList();


            List<V_hx_Bid_records_borrowing_target> vbht = ef.V_hx_Bid_records_borrowing_target.Where(a => a.investor_registerid == userid && a.tender_state >= 2 && a.tender_state <= 5 && a.ordstate == 1).OrderByDescending(a=>a.bid_records_id).Take(5).ToList();

            //投资计录

            ViewBag.income = income;
            ViewBag.vbht = vbht;

            //加息券
            List<hx_UserAct> uat = ef.hx_UserAct.Where(h => h.RewTypeID == 3 && h.UseState==0 && h.registerid == userid).ToList();

            ViewBag.jiaxinum = uat.Count();
            GetCacheNesList nes = new GetCacheNesList();

            ViewBag.gonggao = nes.GetNews(21, 4, 0, 1); // 公告


            Utils.UpdateUserAct();


            if (userid > 0)
            {
                if (Session["retloan1"] != null)
                {

                }
                else
                {
                    UserInfoData ud = new UserInfoData();
                    ReQueryBalanceBg retloan = ud.Querybalance(userid);

                    if (retloan.RespCode == "000")
                    {
                       
                        B_usercenter bu = new B_usercenter();
                        bu.DataSync(retloan, userid.ToString());

                        Session["retloan1"] = "updateUserbalance";
                    }
                }

            }
            if (HUsr == null)
            {
                HUsr = new hx_member_table();
            }

            return View(HUsr);

        }

        public ActionResult Index1()
        {

            int userid = Utils.checkloginsession();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();

            //最近回款
            List<V_borrowing_Bid_records_income_statement> income = ef.V_borrowing_Bid_records_income_statement.Where(p => p.investor_registerid == userid && p.payment_status > 0).ToList();
            List<V_hx_Bid_records_borrowing_target> vbht = ef.V_hx_Bid_records_borrowing_target.Where(a => a.investor_registerid == userid && a.tender_state >= 2 && a.tender_state <= 5 && a.ordstate == 1).OrderByDescending(a => a.bid_records_id).Take(10).ToList();

            //投资计录

            ViewBag.income = income;
            ViewBag.vbht = vbht;
            return View(HUsr);

        }


        public ActionResult Thirdparty_login()
        {
            int userid = Utils.checkloginsession();
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
            ViewBag.Version = "10";
            ViewBag.CmdId = "UserLogin";
            ViewBag.MerCustId = Utils.GetMerCustID();
            ViewBag.UsrCustId = p.UsrCustId;
            ViewBag.url = Utils.GetChinapnrUrl();
            return View();
        }


        public ActionResult Userinfo()
        {
            int userid = Utils.checkloginsession();

            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();


            return View(HUsr);
        }




        /// <summary>
        /// 更新常用地址
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateAddress()
        {
            string str = "";
            int userid = Utils.checkloginsessiontop();
            string addr = Utils.CheckSQLHtml(DNTRequest.GetString("addr"));

            if (userid > 0)
            {
                int i = ef.hx_member_table.Where(p => p.registerid == userid).Update(p => new hx_member_table { contactaddress = addr });
                if (i > 0)
                {
                    str = @"{""rs""    : ""y"", ""info""      :  ""地址保存成功!""}";
                }
                else
                {
                    str = @"{""rs""    : ""y"", ""info""      :  ""地址保存失败!""}";
                }
            }
            else
            {
                str = @"{""rs""    : ""n"", ""info""      :  ""请登录后再修改!"",""url"":""/Signin/Index""}";
            }

            return Content(str, "text/json");
        }

        /// <summary>
        /// 更新QQ
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateQQ()
        {
            string str = "";
            int userid = Utils.checkloginsessiontop();
            string qq = Utils.CheckSQLHtml(DNTRequest.GetString("qq"));
            if (userid > 0)
            {
                int i = ef.hx_member_table.Where(p => p.registerid == userid).Update(p => new hx_member_table { qq = qq });
          
                if (i > 0)
                {
                    str = @"{""rs""    : ""y"", ""info""      :  ""QQ保存成功!""}";
                }
                else
                {
                    str = @"{""rs""    : ""n"", ""info""      :  ""QQ保存失败!""}";
                }
            }
            else
            {
                str = @"{""rs""    : ""n"", ""info""      :  ""请登录后再修改!"",""url"":""/Signin/Index""}";
            }
            return Content(str, "text/json");
        }



        #region 充值列表
        /// <summary>
        /// 充值列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public ActionResult recharge(int? pageIndex, string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {
            int userid = Utils.checkloginsession();
            int Counts = 0;
            int lostCount = 0;
            int succCount = 0;
            decimal Totals = 0.00M;
            decimal lostTotal = 0.00M;
            decimal succTotal = 0.00M;

            List<hx_td_Bank> listBank = ef.hx_td_Bank.Where(h => h.Isordinary == 1).ToList();
            List<hx_td_Bank> listQuickBank = ef.hx_td_Bank.Where(h => h.Isquick == 1).ToList();

            var ListByOwner = ef.hx_Recharge_history.Where(l => l.membertable_registerid == userid && l.recharge_condition == 1).GroupBy(l => l.membertable_registerid)
                .Select(lg => new
                {
                    Owner = lg.Key,
                    Counts = lg.Count(),
                    lostCount = lg.Where(lo => lo.recharge_condition == 0).Count(),
                    succCount = lg.Where(su => su.recharge_condition == 1).Count(),
                    Totals = lg.Sum(w => w.recharge_amount),
                    lostTotal = lg.Where(w => w.recharge_condition == 0).Sum(w => w.recharge_amount),
                    succTotal = lg.Where(w => w.recharge_condition == 1).Sum(w => w.recharge_amount),
                });
            foreach (var itc in ListByOwner)
            {
                if (itc.Counts > 0)
                {
                    Counts = itc.Counts;
                    Totals = (decimal)itc.Totals;
                    int.TryParse(itc.lostCount.ToString(), out lostCount);
                    int.TryParse(itc.succCount.ToString(), out succCount);
                    decimal.TryParse(itc.lostTotal.ToString(), out lostTotal);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotal);
                }
            }

            Expression<Func<V_Recharge_user_bank, bool>> where = PredicateExtensionses.True<V_Recharge_user_bank>();
            where = where.And(p => p.recharge_history_id > 0);
            where = where.And(p => p.membertable_registerid == userid);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);
            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.recharge_time).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.recharge_time).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                switch (timeday)
                {
                    case 0:

                        break;
                    case 1:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(dt2) <= 0);
                        break;
                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.recharge_time).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        break;

                    default:
                        break;
                }
            }

            hx_UsrBindCardC htb = new hx_UsrBindCardC();
            hx_UsrBindCardC htbqm = new hx_UsrBindCardC();
            hx_td_Bank bank = new hx_td_Bank();
            hx_td_Bank qm = new hx_td_Bank();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();
            if (HUsr != null)
            {
                BindCardController bccon = new BindCardController();
                bccon.checkbank(HUsr.UsrCustId);//
                ViewBag.amt = HUsr.available_balance;
                htb = ef.hx_UsrBindCardC.Where(p => p.defCard == 1 && p.UsrCustId == HUsr.UsrCustId).FirstOrDefault();
                htbqm = ef.hx_UsrBindCardC.Where(p => p.BindCardType == 1 && p.UsrCustId == HUsr.UsrCustId).FirstOrDefault();
                if (htb!=null)
                {
                    bank = ef.hx_td_Bank.Where(c => c.OpenBankId == htb.OpenBankId).FirstOrDefault();
                }
              
                if(htbqm!=null)
                {
                    qm = ef.hx_td_Bank.Where(c => c.OpenBankId == htbqm.OpenBankId).FirstOrDefault();
                }
            }
            else
            {
                ViewBag.amt = 0.00;
            }

            ViewBag.htb = htb;
            ViewBag.bank = bank;
            ViewBag.qm = qm;
            ViewBag.SingleTransQuota = qm.SingleTransQuota;

            var list = ef.V_Recharge_user_bank.Where(where).OrderByDescending(p => p.recharge_history_id).ToPagedList(pageIndex ?? 1, pgaesize);
            ViewBag.startdatetime = startdatetime;
            ViewBag.enddatetime = enddatetime;
            ViewBag.timeday = timeday;
            ViewBag.pageIndex = pageIndex;

            //判定用户是不是有普通卡并且有正在提现的数据
            var bcc=ef.hx_UsrBindCardC.Where(p => p.UsrCustId == HUsr.UsrCustId && p.BindCardType==0).ToList();
            string isEnable = "true";
            string openAcctIds = string.Empty;
            foreach (var item in bcc)
            {
                var existList= ef.V_UserCash_Bank.Where(p=>p.OpenAcctId== item.OpenAcctId && p.OrdIdState==0).ToList();
                if (existList != null && existList.Count != 0)
                {
                    isEnable = "false";
                    if (openAcctIds==string.Empty)
                    {
                        openAcctIds += Utils.ReplaceWithSpecialChar(existList.FirstOrDefault().OpenAcctId, 3, 4, '*'); 
                    }
                    else
                    {
                        openAcctIds += ","+ Utils.ReplaceWithSpecialChar(existList.FirstOrDefault().OpenAcctId, 3, 4, '*');
                    }
                }
            }
            ViewBag.isEnable = isEnable;
            ViewBag.OpenAcctIds = openAcctIds;
            ViewBag.Counts = Counts;
            ViewBag.Totals = Totals;
            ViewBag.lostCount = lostCount;
            ViewBag.succCount = succCount;
            ViewBag.lostTotal = lostTotal;
            ViewBag.succTotal = succTotal;
            ViewBag.listBank = listBank;
            ViewBag.listQuickBank = listQuickBank;
            
            if (Request.IsAjaxRequest())
                return PartialView("_UCRechareList", list);
            return View(list);

        }

        public ActionResult RechargeTips()
        {
            List<hx_td_Bank> listbank = ef.hx_td_Bank.Where(x=>x.Isquick==1).ToList();

            ViewBag.listBank = listbank;
            return View();
        }

        #endregion


        #region 网银充值
        /// <summary>
        /// 网银充值
        /// </summary>
        /// <returns></returns>
        public ActionResult bankingRecharge()
        {
            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            StringBuilder str = new StringBuilder();
            int userid = Utils.checkloginsession();
            string blankName = Utils.CheckSQLHtml(DNTRequest.GetString("blankName1"));
            M_QPNetSave qp = new M_QPNetSave();
            ReQPNetSave rqp = new ReQPNetSave();
            M_Recharge_history rh = new M_Recharge_history();
            B_Recharge_history b = new B_Recharge_history();
            B_UsrBindCard bu = new B_UsrBindCard();
            M_UsrBindCard bm = new M_UsrBindCard();
            p = o.GetModel(userid);
            if (p.UsrCustId ==null || p.UsrCustId == "")
            {
                return Redirect("/opening_account/index/" + userid.ToString());
            }

            string UsrCustId = p.UsrCustId; //这个是给用户充值  在充值前得保证商户余额足够
            decimal amt = RMB.GetDecimal(DNTRequest.GetDecimal("Reprice", 100.00M) * 1.00M, 2, true);
            rh.membertable_registerid = userid;
            rh.recharge_amount = Math.Round(amt, 2);
            rh.recharge_time = DateTime.Now;
            rh.account_amount = amt;
            rh.order_No = Utils.Createcode(); ;
            rh.recharge_condition = 0; //1表示充值成功
            rh.recharge_bank = blankName; // 得接口返回;
            string CmdId = "NetSave";
            string MerCustId = Utils.GetMerCustID();

            int Recid = b.Add(rh);
            if (Recid > 0)
            {
                string MerPriv = DESEncrypt.Encrypt(userid.ToString() + "_" + Recid.ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                qp.Version = "10";
                qp.CmdId = CmdId;
                qp.MerCustId = MerCustId;
                qp.UsrCustId = UsrCustId;
                qp.OrdId = rh.order_No;
                qp.OrdDate = rh.recharge_time.ToString("yyyyMMdd");
                if(UsrCustId== Utils.GetDanbaoCustID()) {
                    qp.GateBusiId = "B2B"; //企业网银
                }
                else
                {
                    qp.GateBusiId = "B2C";
                }
                qp.OpenBankId = blankName;
                qp.DcFlag = "D";
                qp.TransAmt = amt.ToString();
                qp.RetUrl = Utils.GetRe_url("usercenter/SuQPNetSave");
                qp.BgRetUrl = Utils.GetRe_url("Thirdparty/ReQPNetSave");
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
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str1 = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
                qp.ChkValue = sbChkValue.ToString();
                str.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

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

                LogInfo.WriteLog("网银充值提交表单:" + str.ToString());
            }


            ViewBag.str = str.ToString();
            return View();
        } 
        #endregion

        #region 提交充值
        /// <summary>
        /// 提交充值
        /// </summary>
        /// <returns></returns>
        public ActionResult postrecharge()
        {
            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            StringBuilder str = new StringBuilder();
            int userid = Utils.checkloginsession();
            string blankName = Utils.CheckSQLHtml(DNTRequest.GetString("blankName2"));
            M_QPNetSave qp = new M_QPNetSave();
            ReQPNetSave rqp = new ReQPNetSave();
            M_Recharge_history rh = new M_Recharge_history();
            B_Recharge_history b = new B_Recharge_history();
            B_UsrBindCard bu = new B_UsrBindCard();
            M_UsrBindCard bm = new M_UsrBindCard();
            p = o.GetModel(userid);
            if (p.UsrCustId ==null || p.UsrCustId == "")
            {
                return Redirect("/opening_account/index/" + userid.ToString());
            }

            string UsrCustId = p.UsrCustId; //这个是给用户充值  在充值前得保证商户余额足够
            decimal amt = RMB.GetDecimal(DNTRequest.GetDecimal("Reprice", 100.00M) * 1.00M, 2, true);

            rh.membertable_registerid = userid;
            rh.recharge_amount = Math.Round(amt, 2);
            rh.recharge_time = DateTime.Now;
            rh.account_amount = amt;
            rh.order_No = Utils.Createcode(); ;
            rh.recharge_condition = 0; //1表示充值成功
            rh.recharge_bank = blankName; // 得接口返回;

            string CmdId = "NetSave";
            string MerCustId = Utils.GetMerCustID();

            int Recid = b.Add(rh);
            if (Recid > 0)
            {
                string MerPriv = DESEncrypt.Encrypt(userid.ToString() + "_" + Recid.ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                qp.Version = "10";
                qp.CmdId = CmdId;
                qp.MerCustId = MerCustId;
                qp.UsrCustId = UsrCustId;
                qp.OrdId = rh.order_No;
                qp.OrdDate = rh.recharge_time.ToString("yyyyMMdd");
                qp.GateBusiId = "QP";  //快捷支付
                qp.OpenBankId = blankName;
                qp.DcFlag = "D";
                qp.TransAmt = amt.ToString();
                qp.RetUrl = Utils.GetRe_url("usercenter/SuQPNetSave");
                qp.BgRetUrl = Utils.GetRe_url("Thirdparty/ReQPNetSave");
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
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str1 = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);
                qp.ChkValue = sbChkValue.ToString();
                str.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

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

                LogInfo.WriteLog("快捷充值提交表单:" + str.ToString());
            }


            ViewBag.str = str.ToString();

            return View();


        }

        #endregion



        #region 快捷充值返回接口
        /// <summary>
        /// 快捷充值返回接口
        /// </summary>
        /// <returns></returns>
        public ActionResult SuQPNetSave()
        {

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
            LogInfo.WriteLog("快充接口前台验签:ret=" + ret.ToString() + " RespCode:" + m.RespCode + m.RespDesc);

            LogInfo.WriteLog("快充接口前台充值返回报文:" + FastJSON.toJOSN(m));
            StringBuilder str = new StringBuilder();
            string sql = "";

            int bindcardtype = 0;
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
                            LogInfo.WriteLog("快充接口前台充值数据没有写入情况下操作>>>>>>>>>>>>>>>>>>>>>>>");
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


                            LogInfo.WriteLog("前台充值事务操作返回码小于=0 操作失败:" + bucrec.ToString());
                            if (m.GateBusiId == "QP")
                            {
                                bindcardtype = 1;
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

                }

            }
            return View(m);
        } 
        #endregion
  
        /// <summary>
        /// 不用了 无用的
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        public ActionResult AjaxRechare(int? pageIndex, int proid = 1)
        {
            Expression<Func<V_Recharge_user_bank, bool>> where = PredicateExtensionses.True<V_Recharge_user_bank>();
            where = where.And(p => p.recharge_history_id > 0);

            PagedList<V_Recharge_user_bank> list = ef.V_Recharge_user_bank.Where(where).OrderByDescending(p => p.recharge_history_id).ToPagedList(pageIndex ?? 1, 2);
            //}
             if (Request.IsAjaxRequest())
               return PartialView("UCRechareList", list);
            return View(list);   
        }





        #region 资金明细
        /// <summary>
        ///资金明细
        /// </summary>
        /// <returns></returns>
        public ActionResult Financial()
        {
            int userid = Utils.checkloginsession();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();



          
            return View(HUsr);
        }
        #endregion


        #region 投资记录
        /// <summary>
        /// 投资记录
        /// </summary>
        /// <returns></returns>
        public ActionResult touzi(int? pageIndex, string startdatetime, string enddatetime, int tdays = 0, int pgaesize = 5)
        {
            int userid = Utils.checkloginsession();

            Expression<Func<V_hx_Bid_records_borrowing_target, bool>> where = PredicateExtensionses.True<V_hx_Bid_records_borrowing_target>();
            where = where.And(p => p.bid_records_id > 0);
            where = where.And(p => p.investor_registerid == userid);
           // where = where.And(p => p.tender_state == 1);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                switch (tdays)
                {
                    case 0:

                        break;
                    case 1:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        break;
                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        break;

                    default:
                        break;
                }
            }

            var list = ef.V_hx_Bid_records_borrowing_target.Where(where).OrderByDescending(p => p.bid_records_id).ToPagedList(pageIndex ?? 1, pgaesize);
            ViewBag.startdatetime = startdatetime;
            ViewBag.enddatetime = enddatetime;
            ViewBag.tdays = tdays;
            ViewBag.pageIndex = pageIndex;


            if (Request.IsAjaxRequest())
                return PartialView("_touziList", list);
            return View(list);

        }
        #endregion

        #region 投资记录进行中的
        /// <summary>
        /// 投资记录
        /// </summary>
        /// <returns></returns>
        public ActionResult touzi1(int? pageIndex,  int pgaesize = 5)
        {
            int userid = Utils.checkloginsession();

            Expression<Func<V_hx_Bid_records_borrowing_target, bool>> where = PredicateExtensionses.True<V_hx_Bid_records_borrowing_target>();
            where = where.And(p => p.bid_records_id > 0);
            where = where.And(p => p.investor_registerid == userid);

            where = where.And(p => p.tender_state >=2 );

            where = where.And(p => p.tender_state <5 );



            var list = ef.V_hx_Bid_records_borrowing_target.Where(where).OrderByDescending(p => p.bid_records_id).ToPagedList(pageIndex ?? 1, pgaesize);
           
            ViewBag.pageIndex = pageIndex;


            if (Request.IsAjaxRequest())
            {
                return PartialView("_touzilist1", list);
            }

            ViewBag.list = list;
            return View(list);

        }
        #endregion



        #region 回款计划
        /// <summary>
        /// 回款计划
        /// </summary>
        /// <returns></returns>
        public ActionResult huikuan(int? pageIndex, string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {

            int userid = Utils.checkloginsession();

            Expression<Func<V_borrowing_Bid_records_income_statement_uc, bool>> where = PredicateExtensionses.True<V_borrowing_Bid_records_income_statement_uc>();
            where = where.And(p => p.bid_records_id > 0);
            where = where.And(p => p.investor_registerid == userid);
            
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);
            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                DateTime vdt = DateTime.Now;
                switch (timeday)
                {
                    case 0:   //全部

                        /*
                        sdatetime = DateTime.Parse(vdt.Year.ToString() + "-" + vdt.Month.ToString() + "-01 00:00:00");

                        DateTime dt4 = Utils.LastDayOfMonth(vdt);

                        dt2 = DateTime.Parse(dt4.Year.ToString() + "-" + dt4.Month.ToString() + "-"+ dt4.Day.ToString()+ " 23:59:59");
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);*/
                        break;
                        
                    case 1: //当天
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        // sdatetime = Utils.FirstDayOfPreviousMonth(DateTime.Now);
                        // dt2 = Utils.LastDayOfPrdviousMonth(DateTime.Now);
                        // sdatetime = DateTime.Parse(vdt.Year.ToString("yyyy") + "-" + vdt.Month.ToString("MM") + "-01 00:00:00");
                        // dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(dt2) <= 0);
                        break;
                    case 2:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.interest_payment_date).CompareTo(dt2) <= 0);
                        break;
                    //case 90:
                    //    sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                    //    dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                    //    break;

                    default:
                        break;
                }
            }


            var list = ef.V_borrowing_Bid_records_income_statement_uc.Where(where).OrderByDescending(p=>p.bid_records_id).ThenBy(p=>p.value_date).ToPagedList(pageIndex ?? 1, pgaesize);
            ViewBag.startdatetime = startdatetime;
            ViewBag.enddatetime = enddatetime;
            ViewBag.timeday = timeday;
            ViewBag.pageIndex = pageIndex;
            B_usercenter o = new B_usercenter();
            var principal= o.GetPrincipal(userid);
            var collectTotal=o.Getcollecttotalamountinterest(userid);
            //2016-09-14 本金+代收利息总额
            ViewBag.ds = Math.Round((principal + collectTotal), 2).ToString();

            //ViewBag.ds = Math.Round(o.Getbeixi(userid), 2).ToString();

            ViewBag.count= Math.Round(o.GetInvesTotal(userid), 2).ToString();

            var vbrisuc= ef.V_borrowing_Bid_records_income_statement_uc
                       .Where(p => p.investor_registerid == userid && p.payment_status == 1)
                       .OrderByDescending(p => p.repayment_period).FirstOrDefault();
            string fd=string.Empty;//= o.GetValue_date(userid);
            if (vbrisuc != null)
            {
                fd = DateTime.Parse(vbrisuc.repayment_period.ToString()).ToString("yyyy-MM-dd");
                ViewBag.str = fd;
            }else
            {
               ViewBag.str = "";
            }

            //if (fd!="")
            //{
            //    ViewBag.str = DateTime.Parse(o.GetValue_date(userid)).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    ViewBag.str = "";
            //}




            if (Request.IsAjaxRequest())
                return PartialView("_huikuanList", list);
            return View(list);


           
        } 
        #endregion



        public ActionResult message(int? pageIndex, int pgaesize = 10)
        {
            int userid = Utils.checkloginsession();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();

            int mtype = DNTRequest.GetInt("mtype", 0);

            PagedList<hx_td_System_message> list;
             
            if(mtype==0)
            {
                list = ef.hx_td_System_message.Where(p => p.MReg == userid ).OrderByDescending(p => p.Messageid).ToPagedList(pageIndex ?? 1, pgaesize);
            } 
            else
            {
                list = ef.hx_td_System_message.Where(p => p.MReg == userid && p.Mtype == mtype).OrderByDescending(p => p.Messageid).ToPagedList(pageIndex ?? 1, pgaesize);
            }
                
          


            if (Request.IsAjaxRequest())
                return PartialView("_messagelist", list);
            return View(list);

           

        }





        public ActionResult Delmessage(int idC)
        {
            int userid = Utils.checkloginsession();

            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            if (idC < -1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else if(idC==-1)
            {
                ef.hx_td_System_message.Where(p => p.MReg == userid).Delete();
                json = "{\"ret\":1,\"msg\":\"操作成功\"}";
            }
            else
            {
                ef.hx_td_System_message.Where(p => p.MReg == userid  && p.Messageid==idC).Delete();

                json = "{\"ret\":1,\"msg\":\"操作成功\"}";
               
            }

            return Content(json, "text/json");
        }



        #region 邀请奖励
        /// <summary>
        /// 邀请奖励
        /// </summary>
        /// <returns></returns>
        public ActionResult yaoqing(int? pageIndex, int pgaesize = 5)
        {
            int userid = Utils.checkloginsession();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();


            var uid = userid;
            var src = Utils.GetRe_url("Invitation/" + HUsr.invitedcode + ".html");
            var url = Server.UrlEncode(src);
            var desc = Server.UrlEncode(string.Format("创利投喊你领钱啦!新手注册送280元抵扣券，不用靠死工资，快快跟我理财吧！！{0}", src));
            var title = Server.UrlEncode("创利投_安全阳光的P2B网络理财平台_P2P理财_P2P网贷");
            var img = Server.UrlEncode(Utils.GetRe_url("images/logo.jpg"));
            var site = Server.UrlEncode("创利投");
            var qzone = string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);
            var weibo = string.Format("http://v.t.sina.com.cn/share/share.php?appkey=&&source=&content={0}&url={1}&title={2}&pic={3}&site={4}", desc, url, desc, img, site);

            var QQ = string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?to=pengyou&url={0}&desc={1}&title={2}&pics={3}&site={4} title = '分享到朋友社区'", url, desc, desc, img, site);

            QQ = string.Format("http://connect.qq.com/widget/shareqq/index.html?url={0}&title={1}&summary={2}&site={3}&pics={4}",url,desc,desc,site,img);

            var doubai=string.Format("http://www.douban.com/recommend/?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);

            var QQweibo = string.Format("http://v.t.qq.com/share/share.php?title={0}&url={1}&appkey={2}&site={3}&pic={4}", desc, url, "125522", site, img);

            var newWxUrl = string.Empty;
            #region 微信链接逻辑 //2016-09-19
            if (Utils.GetAppSetting("DeBug") == "1")
            {
                newWxUrl = Utils.GetAppSetting("MDeBugURL") + "register/index?invitedcode=" + HUsr.invitedcode;
            }
            else
            {
                newWxUrl = Utils.GetAppSetting("MReleaseURL") + "register/index?invitedcode=" + HUsr.invitedcode;
            }
            #endregion
            ViewBag.NewWxUrl = newWxUrl;

            ViewBag.QQweibo = QQweibo;

            ViewBag.doubai = doubai;

            ViewBag.QQ = QQ;

            ViewBag.qzone = qzone;
            ViewBag.weibo = weibo;

            ViewBag.codes = Utils.GetRe_url("Invitation/" + HUsr.invitedcode + ".html");
            ViewBag.HUsr = HUsr;
            
            var list = ef.V_YaoQinList.Where(p=>p.membertable_registerid==userid).OrderByDescending(p => p.Createtime).ToPagedList(pageIndex ?? 1, pgaesize);
            



            if (Request.IsAjaxRequest())
               return PartialView("_yaoqinglist", list);
            return View(list);


           
        }
        #endregion




        #region 读取PDF文件
        /// <summary>
        /// 读取PDF文件
        /// </summary>
        /// <param name="fName">文件名称(可以从其他地方传进来)</param>
        /// <returns></returns>
        public FileStreamResult readPDF(string fName = "111.pdf")
        {
            string dirp = @"/PDF/";
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            string pdfSrc = string.Empty;
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    string x = System.IO.Path.GetDirectoryName(fi.FullName);
                    string s = System.IO.Path.GetExtension(fi.FullName);
                    if (fi.Name == fName)
                    {
                        pdfSrc = dirp + "\\" + fi.Name;//pdf路径
                        ViewBag.title = fi.Name;//网页标题
                    }
                }
            }
            FileStream fs = new FileStream(pdfSrc, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }
        #endregion




        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Changepassword()
        {
            int userid = Utils.checkloginsession();

            return View();
        } 
        #endregion
        

        #region 提交密码
        /// <summary>
        /// 提交密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostChangepassword()
        {
            int userid = Utils.checkloginsession();

            string olduserpassword = DESEncrypt.Encrypt(Utils.CheckSQL(Request["olduserpassword"].ToString().Trim()), ConfigurationManager.AppSettings["webp"].ToString());

            string newuserpassword = DESEncrypt.Encrypt(Utils.CheckSQL(Request["newuserpassword"].ToString().Trim()), ConfigurationManager.AppSettings["webp"].ToString());

            string passw = "";
            string json = "";

            string sql = "select password from hx_member_table where registerid=" + userid.ToString();
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                passw = dt.Rows[0]["password"].ToString();

                if (passw == olduserpassword)
                {
                    sql = "update hx_member_table set password = '" + newuserpassword + "' where  registerid=" + userid.ToString();

                    if (DbHelperSQL.ExecuteSql(sql) > 0)
                    {
                        json = @" {""rs""    : ""y"", ""error""      :  ""新密码修改成功!""}";


                    }
                    else
                    {
                        json = @" {""rs""    : ""n"", ""error""      :  ""新密码修改失败!""}";

                    }

                }
                else
                {
                    json = @" {""rs""    : ""n"", ""error""      :  ""原始密码输入错误!""}";

                }

            }
            else
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""异常错误!""}";

            }
            return Content(json);
        }
        #endregion




        #region 登录汇付第三方
        /// <summary>
        /// 登录汇付第三方
        /// </summary>
        /// <returns></returns>
        public ActionResult thirdpartylogin()
        {
            int userid = Utils.checkloginsession();
            string url = Utils.GetChinapnrUrl();

            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
         
            if (p.UsrCustId ==null || p.UsrCustId == "")
            {
                return Redirect("/opening_account/index/" + userid.ToString());
            }

            ViewBag.Version = "10";
            ViewBag.CmdId = "UserLogin";
            ViewBag.MerCustId = Utils.GetMerCustID();
            ViewBag.UsrCustId = p.UsrCustId;
            ViewBag.url = url;
            return View();
        }


        #endregion




        #region 邮箱验证
        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <returns></returns>
        public ActionResult EmailVerify()
        {

            string json = "";
            int userid = Utils.checkloginsession();
            string email = Utils.CheckSQLHtml(Request["email"].ToString().Trim());
            string sql = "select email from hx_member_table where email='" + email + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                json = @" {""rs"": ""n"", ""info"":  ""邮箱已经被注册!""}";
              
            }
            else
            {
                //int uidc = "";
                // uid = int.Parse(DESEncrypt.Decrypt(userstr, ConfigurationManager.AppSettings["webp"].ToString()));

                sql = "SELECT registerid,username,mobile from hx_member_table where    registerid=" + userid.ToString();
                DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);

                if (dt1.Rows.Count > 0)
                {
                    string keys = DESEncrypt.Encrypt(dt1.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string keys1 = DESEncrypt.Encrypt(dt1.Rows[0]["username"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string emaild= DESEncrypt.Encrypt(email, ConfigurationManager.AppSettings["webp"].ToString());

                    string contxt = Utils.GetMSMEmailContext(2, 0); // 获取注册成功邮件内容

                    StringBuilder sb = new StringBuilder(contxt);

                    sb = sb.Replace("#UserName#", Utils.GetUserNameCookies());

                    string urlc = Utils.GetRe_url("/usercenter/EVf?key=" + keys + "&u=" + keys1+"&e="+ emaild);

                    sb = sb.Replace("#LINK#", urlc);

                    if (SendMail.SendMailTitle(email, "创利投", "创利投邮件认证", sb.ToString()))
                    {
                        json = @" {""rs"": ""y"", ""info"":  ""邮件发送成功!请登录邮件激活邮箱""}";

                    }
                    else
                    {
                        json = @" {""rs"": ""n"", ""info"":  ""邮件发送失败!""}";
                       
                    }

                }
                else
                {
                    json = @" {""rs"": ""n"", ""info"":  ""数据异常!""}";                   
                }

            }

            return Content(json, "text/json");
        }


        #endregion





        public ActionResult EVf()
        {

           
            string userstr = DNTRequest.GetString("key");

            userstr = DESEncrypt.Decrypt(userstr, ConfigurationManager.AppSettings["webp"].ToString());

            /// Response.Write("userstr:" + userstr + "<br>");

            string username = DNTRequest.GetString("u");

            username = DESEncrypt.Decrypt(username, ConfigurationManager.AppSettings["webp"].ToString());


            string email= DNTRequest.GetString("e");

            email = DESEncrypt.Decrypt(email, ConfigurationManager.AppSettings["webp"].ToString());

            //  Response.Write("username:" + username + "<br>");

            string sql = "SELECT registerid,username,mobile,isemail from hx_member_table where  registerid='" + userstr + "' and  username='" + username + "'";
            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);

            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["isemail"].ToString() == "0")
                {

                    sql = "update hx_member_table set isemail=1,email='"+ email + "'   where registerid='" + userstr + "' and  username='" + username + "'";

                    DbHelperSQL.RunSql(sql);

                    Response.Write("<script language='javascript'>alert('您的邮箱认证成功!');javascript:window.location.href='/';</script>");
                    Response.End();

                }
                else
                {
                    Response.Write("<script language='javascript'>alert('您的邮箱已认证!');javascript:window.location.href='/';</script>");
                    Response.End();
                }

            }
            else
            {
                Response.Write("<script language='javascript'>alert('异常数据!');javascript:window.location.href='/';</script>");
                Response.End();

            }




            return View();
        }


        public ActionResult IndexToExcel(int? pageIndex, string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {


            /*
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT registerid as ID,username as '用户名',realname as '姓名',mobile as '手机',case when ISNULL(open_tonto_account,0)=0 then '否' else '是' end as 托管 ");
            sql.Append(",case ISNULL(useridentity,0) when 6 then '钻石' when 1 then 'VIP' when 2 then '黄金' when 3 then '虚假' when 4 then '渠道' when 5 then '白金' else '普通' end as '等级' ");
            sql.Append(",account_total_assets as '总资产',available_balance as '可用',frozen_sum as '冻结',convert(varchar(10),registration_time,120) as '注册时间' ");
            sql.Append(",lastlogintime as '最后登录时间',Tid as '来源',ISNULL(CommNum,0) as '沟通次数' ");
            sql.Append("FROM hx_member_table ");
            sql.Append("Where registerid>0 ");
            #region 查询条件
            if (!string.IsNullOrEmpty(username))
            {
                sql.AppendFormat(" AND username like '%{0}%' ", username);
            }
            if (!string.IsNullOrEmpty(realname))
            {
                sql.AppendFormat(" AND realname like '%{0}%' ", realname);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql.AppendFormat(" AND mobile like '%{0}%' ", mobile);
            }
            if (useridentity >= 0)
            {
                sql.AppendFormat(" AND useridentity ='{0}' ", useridentity);
            }
            if (Channelsource >= 0)
            {
                sql.AppendFormat(" AND Channelsource ='{0}' ", Channelsource);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                sql.AppendFormat(" AND conver(varchar(10),registration_time,120)>='{0}' ", time1);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                sql.AppendFormat(" AND conver(varchar(10),registration_time,120)>='{0}' ", time2);
            }
            #endregion
            sql.Append(" order by  registerid desc; ");
            */

            int userid = Utils.checkloginsession();

            Expression<Func<V_borrowing_Bid_records_income_statement_uc, bool>> where = PredicateExtensionses.True<V_borrowing_Bid_records_income_statement_uc>();
            where = where.And(p => p.bid_records_id > 0);
            where = where.And(p => p.investor_registerid == userid);
            // where = where.And(p => p.tender_state == 1);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                switch (timeday)
                {
                    case 0:

                        break;
                    case 1:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        break;
                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        break;

                    default:
                        break;
                }
            }

            List<V_borrowing_Bid_records_income_statement_uc> list = ef.V_borrowing_Bid_records_income_statement_uc.Where(where).OrderByDescending(p => p.bid_records_id).ToList();



            // DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());
            DataTable dt = ListToDataTable(list);
            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }



        #region 邮箱验证
        /// <summary>
        /// list转datatable
        /// </summary>
        /// <param name="list"></param>      
        /// <returns></returns>
        public DataTable ListToDataTable(List<V_borrowing_Bid_records_income_statement_uc> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("回款项目");           
            dt.Columns.Add("投资金额");
            dt.Columns.Add("预计本息");
            dt.Columns.Add("预计回款时间");
            dt.Columns.Add("回款时间");
            dt.Columns.Add("状态");
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr["回款项目"] = item.borrowing_title;              
                dr["投资金额"] = item.investment_amount;
                dr["预计本息"] = item.repayment_amount;
                dr["预计回款时间"] = item.interest_payment_date == null ? "--" : item.interest_payment_date.ToString();
                dr["回款时间"] = item.repayment_period == null ? "--" : item.repayment_period.ToString();                
                dr["状态"] = item.repayment_period == null ? "未回款" : "已回款";  
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion



        #region 资金流水
        /// <summary>
        /// 资金流水
        /// </summary>
        /// <returns></returns>
       // [OutputCache(Duration = 120, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Detailsfunds(int? pageIndex,int? regpageindex,  string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {
            int userid = Utils.checkloginsession();

            UsrDetialsFunds mode = new UsrDetialsFunds();
            // 
            Expression<Func<hx_Capital_account_water, bool>> where = PredicateExtensionses.True<hx_Capital_account_water>();
            where = where.And(p => p.account_water_id > 0);
            where = where.And(p => p.membertable_registerid == userid);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                switch (timeday)
                {
                    case 0:

                        break;
                    
                    case 1:   //当天
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;
                    case -3:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-3);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;
                    case 3:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-3);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;
                    case 7:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-7);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;

                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
                        break;

                    default:
                        break;
                }
            }


            Expression<Func<hx_UserAct, bool>> where1 = PredicateExtensionses.True<hx_UserAct>();
            where1 = where1.And(p => p.ActID > 0);
            where1 = where1.And(p => p.registerid == userid);
            //DateTime sdatetime = new DateTime();
           // DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
            }
            else
            {
                DateTime dt2;
                switch (timeday)
                {
                    case 0:

                        break;
                    case 1:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;
                    case -3:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-3);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;
                    case 3:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-3);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;
                    case 7:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-7);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;
                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                        where1 = where1.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
                        break;

                    default:
                        break;
                }
            }


            mode.Capital = ef.hx_Capital_account_water.Where(where).OrderByDescending(p => p.account_water_id).ToPagedList(pageIndex ?? 1, pgaesize);
            var count = ef.hx_UserAct.Where(where1).OrderByDescending(p => p.Createtime).ToList().Count;
            mode.V_ACT = ef.hx_UserAct.Where(where1).OrderByDescending(p => p.Createtime).ToPagedList(regpageindex ?? 1, pgaesize);

            ViewBag.startdatetime = startdatetime;
            ViewBag.enddatetime = enddatetime;
            ViewBag.timeday = timeday;
            ViewBag.pageIndex = pageIndex;           

            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "ACT")  //充值列表
                {
                    return PartialView("_ACT", mode.V_ACT);
                }
                else if (target == "Capital")  //资金流水
                {
                    return PartialView("_Detailsfunds", mode.Capital);
                }
            }


            return View(mode);
        } 

        #endregion


    }
}