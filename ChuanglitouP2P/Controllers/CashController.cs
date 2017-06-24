using ChuanglitouP2P.Areas.Admin.Controllers;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.chinapnr;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Cash;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using ChuangLitouP2P.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class CashController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="timeday"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        // GET: Cash
        public ActionResult Index(int? pageIndex, string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {
            ViewBag.rndstr = Utils.RndNumChar(10).ToString();

            int OrdIdState = DNTRequest.GetInt("OrdIdState", 0);

            ViewBag.OrdIdState = OrdIdState;
            int userid = Utils.checkloginsession();
            int Counts = 0;
            int lostCount = 0;
            int succCount = 0;
            decimal Totals = 0.00M;
            decimal lostTotal = 0.00M;
            decimal succTotal = 0.00M;

            B_member_table b = new B_member_table();
            M_member_table pu = new M_member_table();
            pu = b.GetModel(userid);

            //判断用户是否开户
            if (string.IsNullOrEmpty(pu.UsrCustId))
            {

                string temstr = "/opening_account/Index/" + userid.ToString();
                return Redirect(temstr);
            }


            new BindCardController().checkbank(pu.UsrCustId);

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
                        //  string sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + " ,frozen_sum=" + decimal.Parse(retloan.FrzBal) + " where  registerid=" + userid.ToString() + "";

                        //string sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + "  where  registerid=" + userid.ToString() + "";

                        //DbHelperSQL.RunSql(sql);
                        B_usercenter bu = new B_usercenter();
                        bu.DataSync(retloan, userid.ToString());

                        Session["retloan1"] = "updateUserbalance";
                    }
                }

            }


            List<V_UsrBindCardBank> vubc = ef.V_UsrBindCardBank.Where(c => c.registerid == userid && c.OpenBankId != null).ToList();



            //判断用户是否绑定一行卡
            //if (vubc.Count <= 0)
            //{
            //    string temstr = "/BindCard/Index";
            //    return Redirect(temstr);
            //}


            var ListByOwner = ef.hx_td_UserCash.Where(l => l.registerid == userid && l.OpenBankId != null).GroupBy(l => l.registerid)
            .Select(lg => new
            {
                Owner = lg.Key,
                Counts = lg.Count(),
                lostCount = lg.Where(lo => lo.OrdIdState == 0).Count(),
                succCount = lg.Where(su => su.OrdIdState == 3).Count(),
                Totals = lg.Where(w => w.OrdIdState == 3).Sum(w => w.TransAmt),
                lostTotal = lg.Where(w => w.OrdIdState == 0).Sum(w => w.TransAmt),
                succTotal = lg.Where(w => w.OrdIdState == 3).Sum(w => w.TransAmt),
            });

            foreach (var itc in ListByOwner)
            {
                if (itc.Counts > 0)
                {
                    //Counts = itc.Counts;
                    // Totals = (decimal)itc.Totals;
                    int.TryParse(itc.Counts.ToString(), out Counts);
                    decimal.TryParse(itc.Totals.ToString(), out Totals);
                    int.TryParse(itc.lostCount.ToString(), out lostCount);
                    int.TryParse(itc.succCount.ToString(), out succCount);
                    decimal.TryParse(itc.lostTotal.ToString(), out lostTotal);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotal);
                }
            }
            Expression<Func<hx_td_UserCash, bool>> where = PredicateExtensionses.True<hx_td_UserCash>();
            where = where.And(p => p.UserCashId > 0);
            where = where.And(p => p.registerid == userid);

            where = where.And(p => p.OpenBankId != null);

            if (ViewBag.OrdIdState == 3)
            {
                where = where.And(p => p.OrdIdState == OrdIdState);

            }


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
                where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(dt2) <= 0);
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
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(dt2) <= 0);
                        break;
                    case 30:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-30);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(dt2) <= 0);
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(sdatetime) >= 0);
                        where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(dt2) <= 0);
                        break;
                    case 90:
                        sdatetime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-90);
                        dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        break;

                    default:
                        break;
                }
            }
            var list = ef.hx_td_UserCash.Where(where).OrderByDescending(p => p.UserCashId).ToPagedList(pageIndex ?? 1, pgaesize);



            ViewBag.startdatetime = startdatetime;
            ViewBag.enddatetime = enddatetime;
            ViewBag.timeday = timeday;
            ViewBag.pageIndex = pageIndex;
            ViewBag.Counts = Counts;
            ViewBag.Totals = Totals;
            ViewBag.lostCount = lostCount;

            ViewBag.lostTotal = lostTotal;


            ViewBag.succTotal = succTotal;
            ViewBag.succCount = succCount;

            ViewBag.users = pu;
            vubc = BusinessLogicHelper.LeftOne(vubc);
            ViewBag.UsrBindCard = vubc.OrderByDescending(c => c.defCard).ToList();

            ViewBag.Isquick = vubc.Where(c => c.Isquick == 1).Count() > 0;

            ViewBag.GENERAL = DateTime.Now.AddDays(1).ToString("MM月dd日");
            ViewBag.QM = DateTime.Now.ToString("MM月dd日");
            if (Request.IsAjaxRequest())
                return PartialView("_Cashlist", list);
            return View(list);
        }



        public JsonResult IsOpenAccount()
        {
            int userid = Utils.checkloginsession();
            string temstr = string.Empty;
            string rs = "n";//默认没开通
            B_member_table b = new B_member_table();
            M_member_table pu = new M_member_table();
            pu = b.GetModel(userid);

            //判断用户是否开户
            if (pu == null)
            {
                rs = "y";
                temstr = "/login.thml";
            }
            else if (!string.IsNullOrEmpty(pu.UsrCustId))
            {
                rs = "y";
            }
            else
            {
                rs = "n";
                temstr = "/opening_account/Index/" + userid.ToString();
            }

            var result = new { rs = rs, info = temstr };
            return Json(result);

        }


        /// <summary>
        /// 普通提现
        /// </summary>
        /// <returns></returns>
        public ActionResult CashGENERAL()
        {
            int userid = Utils.checkloginsession();
            string url = Utils.GetChinapnrUrl();
            decimal Amt = decimal.Parse(DNTRequest.GetString("TransAmt"));
            int UsrBindCardID = DNTRequest.GetInt("UsrBindCardID", 0);
            //string vcode = DNTRequest.GetString("vcode");
            string strIdentify = "CashValidateCode"; //随机字串存储键值，以便存储到Session中
            LogInfo.WriteLog("普通提现取现:userid=" + userid + ";url=" + url + ";Amt=" + Amt + ";UsrBindCardID=" + UsrBindCardID + ";strIdentify=" + strIdentify);
            //if (Session[strIdentify] != null)
            //{
            //    if (Session[strIdentify].ToString() != vcode)
            //    {
            //        return Content(StringAlert.Alert("验证码不对!"), "text/html");
            //    }
            //}
            //else
            //{
            //    return Content(StringAlert.Alert("验证码已过期!"), "text/html");
            //}

            ViewBag.errCode = "0";
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
            if (p.available_balance < Amt)
            {
                ViewBag.errCode = "-1";
                return View(new M_Cash());
            }
            hx_UsrBindCardC ubc = ef.hx_UsrBindCardC.Where(g => g.UsrBindCardID == UsrBindCardID).FirstOrDefault();
            if (ubc == null)
            {
                ViewBag.errCode = "-2";
                return View(new M_Cash());
            }
            ViewBag.url = url;
            decimal servf = 0.00M;
            string FeeObjFlag = "M";
            string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.GENERAL);
            M_Cash mc = ChinapnrFacade.Cash(p.UsrCustId, Amt.ToString("0.00"), ubc.OpenAcctId, cashChl, servf.ToString("0.00"), FeeObjFlag);
            if (mc != null)
            {
                M_td_UserCash mu = new M_td_UserCash();
                B_td_UserCash mo = new B_td_UserCash();
                mu.registerid = p.registerid;
                mu.UsrCustId = p.UsrCustId;
                mu.TransAmt = decimal.Parse(mc.TransAmt);
                mu.FeeAmt = servf;
                mu.OrdId = mc.OrdId;
                mu.OrdIdTime = DateTime.Now;
                mu.OrdIdState = 0;
                mu.FeeObjFlag = FeeObjFlag;
                mu.CashChl = cashChl;
                mo.Add(mu);
            }
            return View(mc);
        }

        /// <summary>
        /// 验证本页的验证码
        /// </summary>
        /// <param name="vcode"></param>
        public ContentResult Vcodecheck(string vcode)
        {
            string str = "";
            string strIdentify = "ValidateCode"; //随机字串存储键值，以便存储到Session中
            if (Session[strIdentify] != null)
            {
                if (Session[strIdentify].ToString() != vcode)
                {
                    str = "验证码不对!";
                }
            }
            else
            {
                str = "验证码已过期!";
            }
            return Content(StringAlert.Alert(str), "text/html");
        }


        /// <summary>
        /// 快速提现
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CashQm()
        {
            int userid = Utils.checkloginsession();
            string url = Utils.GetChinapnrUrl();
            decimal Amt = decimal.Parse(DNTRequest.GetString("TransAmt"));
            int UsrBindCardID = DNTRequest.GetInt("UsrBindCardID", 0);
            //string vcode = DNTRequest.GetString("vcode");
            string strIdentify = "CashValidateCode"; //随机字串存储键值，以便存储到Session中
            LogInfo.WriteLog("快速提现取现:userid=" + userid + ";url=" + url + ";Amt=" + Amt + ";UsrBindCardID=" + UsrBindCardID + ";strIdentify=" + strIdentify);

            decimal ServFee = 0.00M;
            //if (Session[strIdentify] != null)
            //{
            //    if (Session[strIdentify].ToString() != vcode)
            //    {
            //        return Content(StringAlert.Alert("验证码不对!"), "text/html");
            //    }
            //}
            //else
            //{
            //    return Content(StringAlert.Alert("验证码已过期!"), "text/html");
            //}
            decimal servf = ServFee;
            ViewBag.errCode = "0";
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
            if (p.available_balance < Amt)
            {
                ViewBag.errCode = "-1";
                return View(new M_Cash());
            }
            if (Amt <= ServFee)
            {
                return Content(StringAlert.Alert("取现金额小于或等于手续费,不能提现!"), "text/html");
            }
            else if (p.available_balance < Amt + ServFee)
            {
                Amt = Amt - ServFee;
            }
            hx_UsrBindCardC ubc = ef.hx_UsrBindCardC.Where(g => g.UsrBindCardID == UsrBindCardID).FirstOrDefault();
            if (ubc == null)
            {
                ViewBag.errCode = "-2";
                return View(new M_Cash());
            }
            ViewBag.url = url;
            string FeeObjFlag = "M";
            string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.FAST);
            M_Cash mc = ChinapnrFacade.Cash(p.UsrCustId, Amt.ToString("0.00"), ubc.OpenAcctId, cashChl, ServFee.ToString("0.00"), FeeObjFlag);
            if (mc != null)
            {
                M_td_UserCash mu = new M_td_UserCash();
                B_td_UserCash mo = new B_td_UserCash();
                mu.registerid = p.registerid;
                mu.UsrCustId = p.UsrCustId;
                mu.TransAmt = decimal.Parse(mc.TransAmt);
                mu.FeeAmt = servf;
                mu.OrdId = mc.OrdId;
                mu.OrdIdTime = DateTime.Now;
                mu.OrdIdState = 0;
                mu.FeeObjFlag = FeeObjFlag;
                mu.CashChl = cashChl;
                mo.Add(mu);
            }
            return View(mc);
        }


        public ActionResult CashValidateCode()
        {
            // 产生图形验证码
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["CashValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        public ActionResult PostGENERALAmt()
        {
            //显示于页面给汇付
            string str = string.Empty;
            //日志变量
            string log = string.Empty;
            //响应通知加签的状态码
            int ret = -1;
            //业务的sql语句变量
            string sql = string.Empty;
            ReCash m = ChinapnrFacade.CashCallBack(out ret);
            if (m != null)
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
                        int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.RealTransAmt, m.UsrCustId, m.FeeAmt, mre.FeeObjFlag, mre.CashChl);
                        if (CashOp > 0)
                        {

                            sql = "select registerid,username,mobile,UsrCustId,available_balance from hx_member_table where UsrCustId='" + m.UsrCustId + "'";
                            LogInfo.WriteLog("审请取现成功短信sql:" + sql);
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
                                pm.MTitle = "提现";
                                pm.MContext = sbsms.ToString();
                                pm.PubTime = dti;
                                pm.Mtype = 3;
                                B_usercenter.AddMessage(pm);
                            }
                            string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.IMMEDIATE);

                            if (mre.CashChl == cashChl && decimal.Parse(m.TransAmt) <= 200000M)
                            {
                                string retUrl = Utils.GetRe_url("admin/UserCash/RePostCashProcessing");
                                string bgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCashProcessing");
                                BusinessLogicHelper.AutoCheckCash(m.UsrCustId, retUrl, bgRetUrl);
                            }
                        }
                    }
                }
            }
            return View(m);
        }



        public ActionResult NewCash(int? pageIndex, string startdatetime, string enddatetime, int timeday = 0, int pgaesize = 5)
        {
            return Index(pageIndex, startdatetime, enddatetime, timeday, pgaesize);
        }
        /// <summary>
        /// 即时提现
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CashIMMEDIATE()
        {
            int userid = Utils.checkloginsession();
            string url = Utils.GetChinapnrUrl();
            decimal Amt = decimal.Parse(DNTRequest.GetString("TransAmt"));
            int UsrBindCardID = DNTRequest.GetInt("UsrBindCardID", 0);
            //string vcode = DNTRequest.GetString("vcode");
            string strIdentify = "CashValidateCode"; //随机字串存储键值，以便存储到Session中
            LogInfo.WriteLog("即时取现:userid=" + userid + ";url=" + url + ";Amt=" + Amt + ";UsrBindCardID=" + UsrBindCardID + ";strIdentify=" + strIdentify);

            decimal ServFee = 0.00M;
            //if (Session[strIdentify] != null)
            //{
            //    if (Session[strIdentify].ToString() != vcode)
            //    {
            //        return Content(StringAlert.Alert("验证码不对!"), "text/html");
            //    }
            //}
            //else
            //{
            //    return Content(StringAlert.Alert("验证码已过期!"), "text/html");
            //}
            decimal servf = ServFee;

            ViewBag.errCode = "0";
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
            if (p.available_balance < Amt)
            {
                ViewBag.errCode = "-1";
                return View(new M_Cash());
            }
            if (Amt <= ServFee)
            {
                return Content(StringAlert.Alert("取现金额小于或等于手续费,不能提现!"), "text/html");
            }
            else if (p.available_balance < Amt + ServFee)
            {
                Amt = Amt - ServFee;
            }
            hx_UsrBindCardC ubc = ef.hx_UsrBindCardC.Where(g => g.UsrBindCardID == UsrBindCardID).FirstOrDefault();
            if (ubc == null)
            {
                ViewBag.errCode = "-2";
                return View(new M_Cash());
            }
            ViewBag.url = url;
            string FeeObjFlag = "M";//"U";

            string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.IMMEDIATE);
            M_Cash mc = ChinapnrFacade.Cash(p.UsrCustId, Amt.ToString("0.00"), ubc.OpenAcctId, cashChl, ServFee.ToString("0.00"), FeeObjFlag);
            if (mc != null)
            {
                M_td_UserCash mu = new M_td_UserCash();
                B_td_UserCash mo = new B_td_UserCash();
                mu.registerid = p.registerid;
                mu.UsrCustId = p.UsrCustId;
                mu.TransAmt = decimal.Parse(mc.TransAmt);
                mu.FeeAmt = ChinapnrFacade.CalcCashFee(Amt.ToString("0.00"), cashChl, FeeObjFlag) + servf;
                mu.OrdId = mc.OrdId;
                mu.OrdIdTime = DateTime.Now;
                mu.OrdIdState = 0;
                mu.FeeObjFlag = FeeObjFlag;
                mu.CashChl = cashChl;
                mo.Add(mu);
            }
            return View("CashQm", mc);
        }

        public ActionResult CalcFeeAmt(int cashType, string transAmt)
        {
            decimal tAmt = 0;
            decimal.TryParse(transAmt, out tAmt);
            ServiceFeesLogic logic = new ServiceFeesLogic();
            M_WithdrawalCash mwc = logic.GetWithdrawalCashFees(cashType, tAmt);
            if (mwc == null)
            {
                return Content("0.00");
            }
            return Content(mwc.ServiceFees.ToString("0.00"));
        }
    }
}