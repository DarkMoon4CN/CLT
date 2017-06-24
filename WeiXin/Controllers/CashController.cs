using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.chinapnr;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Cash;
using ChuangLitouP2P.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace WeiXin.Controllers
{
    public class CashController : BaseController
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Cash
        public ActionResult Index()
        {
            return View();
        }


        #region 提现记录
        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>

        public ActionResult CashRecord(int pageIndex = 1)
        {
            int userid = CurrentUserId;

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_td_UserCash.Where(p => p.registerid == userid && string.IsNullOrEmpty(p.OpenBankId) == false).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_td_UserCash.Where(p => p.registerid == userid && string.IsNullOrEmpty(p.OpenBankId) == false).OrderByDescending(a => a.UserCashId).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);

        }

        public ActionResult CashLLRecord(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_td_LL_cash.Where(p => p.Usrid == userid).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_td_LL_cash.Where(p => p.Usrid == userid).OrderByDescending(a => a.LLcashid).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }

        #endregion

        /// <summary>
        /// 抵扣券未使用页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CashCoupon(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            //新加抵扣券提醒
            var cookie = new HttpCookie("WXRewardTimeXianJinQuan" + userid);//保存至Cookie
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.Date.AddYears(1);
            Response.Cookies.Add(cookie);

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 0).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 0).OrderByDescending(c => c.UserAct).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }

        /// <summary>
        /// 抵扣券已使用
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult CashUsed(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 1).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 1).OrderByDescending(c => c.UserAct).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }

        /// <summary>
        /// 抵扣券已过期
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult CashOverdue(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 2).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(c => c.registerid == userid && c.RewTypeID == 2 && c.UseState == 2).OrderByDescending(c => c.UserAct).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }


        #region 提现提交处理页
        /// <summary>
        /// 提现提交处理方法---快速提现
        /// </summary>
        /// <returns></returns> 
        public ActionResult CashGENERAL()
        {
            int userid = CurrentUserId;
            string url = Utils.GetChinapnrUrl();
            decimal Amt = decimal.Parse(DNTRequest.GetString("amt"));
            int UsrBindCardID = DNTRequest.GetInt("UsrBindCardID", 0);
            int UsrCashType = DNTRequest.GetInt("UsrCashType", 1);
            /*
            string vcode = DNTRequest.GetString("vcode");
            string strIdentify = "CashValidateCode"; //随机字串存储键值，以便存储到Session中
            if (Session[strIdentify] != null)
            {
                if (Session[strIdentify].ToString() != vcode)
                {
                    return Content(StringAlert.Alert("验证码不对!"), "text/html");
                }
            }
            else
            {
                return Content(StringAlert.Alert("验证码已过期!"), "text/html");
            }*/
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
            string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.GENERAL);
            //string cashType = "FAST";
            string FeeObjFlag = "M";
            if (UsrCashType == 2)
            {
                cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.IMMEDIATE);
                //FeeObjFlag = "U";
            }
            //bug 修复，应该为快速提现。此前为普通提现 by fangjianmin
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
        #endregion


        #region 提现前端返回页
        /// <summary>
        ///提现前端返回页
        /// </summary>
        /// <returns></returns>
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
            //提现成功提示语（是即时提现并且额度小于20万时，更改提示语）
            ViewBag.cashChl = "";
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
                        int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.TransAmt, m.UsrCustId);
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
                            if (mre.CashChl == cashChl && decimal.Parse(m.TransAmt) <= 200000)
                            {
                                ViewBag.cashChl = "yes";  //是即时提现，前端提示文字不同
                                string retUrl = Utils.GetImage_url("admin/UserCash/RePostCashProcessing");
                                string bgRetUrl = Utils.GetImage_url("admin/Thirdparty/BgCashProcessing");
                                BusinessLogicHelper.AutoCheckCash(m.UsrCustId, retUrl, bgRetUrl);
                            }
                        }
                    }
                }
            }
            return View(m);
        }
        #endregion
    }
}