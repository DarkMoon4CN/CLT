using ChuanglitouP2P.BLL;
using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using System.Configuration;
using System.Data;
using Webdiyer.WebControls.Mvc;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using EntityFramework.Extensions;
using System.Web.Script.Serialization;
using ChuanglitouP2P.BLL.Api;

namespace WeiXin.Controllers
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class usercenterController : BaseController
    {
        //流米流量发送接口
        public string url = System.Configuration.ConfigurationManager.AppSettings["liumiurl"].ToString();//api接口地址前缀，注意地址中事了server
        public string appkey = System.Configuration.ConfigurationManager.AppSettings["liumiappkey"].ToString(); //请填写你的appkey
        public string appsecret = LiumiTools.MD5(System.Configuration.ConfigurationManager.AppSettings["liumiappsecret"].ToString());//请填写你的appsecret

        chuangtouEntities ef = new chuangtouEntities();


        /// <summary>
        /// 我的账户
        /// </summary>
        /// <returns></returns>


        public ActionResult Index()
        {
            Utils.SetSYSDateTimeFormat();
            int userid = CurrentUserId;
            B_usercenter o = new B_usercenter();

            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);

            //if (p ==null || p.UsrCustId == "")
            //{
            //    return Redirect("/opening_account/index/" + userid.ToString());
            //}

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


            hx_member_table HUsr = ef.hx_member_table.Where(p1 => p1.registerid == userid).FirstOrDefault();

            decimal jiangli = o.GetBonuses(userid);

            ViewBag.Dailyrevenue = o.getDailyrevenue(userid, DateTime.Now);  //今日赚取（元）
            ViewBag.zicai = o.GetCumulativeearned(userid); //+ jiangli;  //累计赚取（元）
            ViewBag.jiangli = jiangli;


            if (ActCount.GetRewardTime("WXRewardTimeXianJinQuan" + userid, 2, userid) == true)//抵扣券
            {
                ViewBag.XJQ = true;
            }
            else
            {
                ViewBag.XJQ = false;
            }
            if (ActCount.GetRewardTime("WXRewardTimeJiaXi" + userid, 3, userid) == true)//加息券
            {
                ViewBag.JXQ = true;
            }
            else
            {
                ViewBag.JXQ = false;
            }


            #region 新加注册送流量判断2016-10-14

            DateTime nowdate = DateTime.Now;
            DateTime startdate = new DateTime(2016, 10, 21, 9, 00, 00);
            DateTime enddate = new DateTime(2016, 10, 31, 23, 59, 59);
            if (nowdate > startdate && nowdate < enddate)
            {
                int ActivityFlowType = -1;//注册送流量0未送，1已送
                int IsGrant = -1;//投资送流量 0未送，1已送
                int zt = SelectMemberInfo(ref ActivityFlowType, ref IsGrant);
                if (zt == 1)//等于1 参加注册投资活动用户
                {
                    if (ActivityFlowType == 0)
                    {
                        ViewBag.ZCFlow = "false";
                    }
                    else
                    {
                        ViewBag.ZCFlow = "true";
                    }
                    if (IsGrant == 0)
                    {
                        ViewBag.TZFlow = "false";
                    }
                    else
                    {
                        ViewBag.TZFlow = "true";
                    }
                }
            }
            #endregion
            return View(HUsr);
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns> 
        public ActionResult UserInfo()
        {
            Utils.SetSYSDateTimeFormat();
            int userid = CurrentUserId;

            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();


            return View(HUsr);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns> 
        public JsonResult SaveUserInfo(hx_member_table mt)
        {
            Utils.SetSYSDateTimeFormat();
            int userid = CurrentUserId;
            mt = (hx_member_table)Utils.ValidateModelClass(mt);

            var result = ef.hx_member_table.Where(a => a.registerid == userid).Update(a => new hx_member_table { email = mt.email, homephone = mt.homephone, contactaddress = mt.contactaddress, zipcode = mt.zipcode, qq = mt.qq, msn = mt.msn });

            return Json(result > 0 ? 1 : 0);
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns> 
        public ActionResult Recharge()
        {
            int userid = CurrentUserId;
            List<hx_td_Bank> listBank = ef.hx_td_Bank.Where(h => h.Isquick == 1 && !string.IsNullOrEmpty(h.CardImage)).ToList();
            ViewBag.listBank = listBank;
            hx_UsrBindCardC htb = new hx_UsrBindCardC();
            hx_UsrBindCardC htbqm = new hx_UsrBindCardC();
            hx_td_Bank bank = new hx_td_Bank();
            hx_td_Bank qm = new hx_td_Bank();
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();
            #region 判定用户是不是有普通卡并且有正在提现的数据
            var bcc = ef.hx_UsrBindCardC.Where(c => c.UsrCustId == HUsr.UsrCustId && c.BindCardType == 0).ToList();
            string isEnable = "true";
            string openAcctIds = string.Empty;
            foreach (var item in bcc)
            {
                var existList = ef.V_UserCash_Bank.Where(c => c.OpenAcctId == item.OpenAcctId && c.OrdIdState == 0).ToList();
                if (existList != null && existList.Count != 0)
                {
                    isEnable = "false";
                    if (openAcctIds == string.Empty)
                    {
                        openAcctIds += Utils.ReplaceWithSpecialChar(existList.FirstOrDefault().OpenAcctId, 3, 4, '*');
                    }
                    else
                    {
                        openAcctIds += "," + Utils.ReplaceWithSpecialChar(existList.FirstOrDefault().OpenAcctId, 3, 4, '*');
                    }
                }
            }
            ViewBag.isEnable = isEnable;
            #endregion
            //判断用户是否开户
            if (string.IsNullOrEmpty(HUsr.UsrCustId))
            {
                string temstr = "/opening_account/Index/" + userid.ToString();
                return Redirect(temstr);
            }

            if (HUsr != null)
            {
                htbqm = ef.hx_UsrBindCardC.Where(p => p.BindCardType == 1 && p.UsrCustId == HUsr.UsrCustId).FirstOrDefault();
                if (htbqm != null)
                {
                    qm = ef.hx_td_Bank.Where(c => c.OpenBankId == htbqm.OpenBankId).FirstOrDefault();
                }
            }
            else
            {
            }
            ViewBag.qm = qm;
            ViewBag.SingleTransQuota = qm.SingleTransQuota;
            return View(HUsr);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        ///  
        public ActionResult Cash()
        {
            int userid = CurrentUserId;
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();

            //判断用户是否开户
            if (string.IsNullOrEmpty(HUsr.UsrCustId))
            {

                string temstr = "/opening_account/Index/" + userid.ToString();
                return Redirect(temstr);
            }


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


            List<V_UsrBindCardBank> listBank = ef.V_UsrBindCardBank.Where(c => c.registerid == userid && c.OpenBankId != null).ToList();
            listBank = BusinessLogicHelper.LeftOne(listBank);
            ViewBag.listBank = listBank;


            string OpenBankId = "", CardImage = "", UsrBindCardID = "", blankname = "", isgren = "";
            foreach (V_UsrBindCardBank item in listBank)
            {
                if (item.defCard == 1)
                {

                    OpenBankId = item.OpenBankId;
                    CardImage = item.CardImage;
                    UsrBindCardID = item.UsrBindCardID.ToString();

                    blankname = item.BankName + item.OpenAcctId.Substring(item.OpenAcctId.ToString().Length - 4);
                    isgren = item.isGren.ToString();
                }

            }

            ViewBag.OpenBankId = OpenBankId;
            ViewBag.CardImage = CardImage;
            ViewBag.UsrBindCardID = UsrBindCardID;
            ViewBag.blankname = blankname;
            ViewBag.isGren = isgren;

            return View(HUsr);
        }




        /// <summary>
        /// 账户总览
        /// </summary>
        /// <returns></returns> 
        public ActionResult ShowAccount()
        {
            int userid = CurrentUserId;
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();


            return View(HUsr);
        }

        #region 我的投资
        /// <summary>
        /// 我的投资
        /// </summary>
        /// <returns></returns>

        public ActionResult MyInvest(int pageIndex = 1)
        {

            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.V_hx_Bid_records_borrowing_target.Where(p => p.registerid == userid).Count();
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
            var list = ef.V_hx_Bid_records_borrowing_target.Where(p => p.registerid == userid).OrderByDescending(a => a.bid_records_id).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);

        }

        #endregion



        #region 还本付息记录
        /// <summary>
        ///  还本付息记录
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns> 
        public ActionResult interests(int bid, int pageIndex = 1)
        {
            int userid = CurrentUserId;

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.V_borrowing_Bid_records_income_statement_uc.Where(p => p.registerid == userid && p.bid_records_id == bid).Count();
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
            var list = ef.V_borrowing_Bid_records_income_statement_uc.Where(p => p.registerid == userid && p.bid_records_id == bid).OrderBy(a => a.targetid).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            ViewBag.bid = bid;
            return View(list);
        }
        #endregion


        #region 资金流水
        /// <summary>
        /// 资金流水
        /// </summary>
        /// <returns></returns> 
        public ActionResult CapitalFlow(int pageIndex = 1)
        {

            int userid = CurrentUserId;

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_Capital_account_water.Where(p => p.membertable_registerid == userid).Count();
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
            var list = ef.hx_Capital_account_water.Where(p => p.membertable_registerid == userid).OrderByDescending(a => a.account_water_id).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;

            return View(list);


        }

        #endregion

        /// <summary>
        /// 奖励流水
        /// </summary>
        /// <returns></returns>
        ///  
        public ActionResult RewardFlow(int pageIndex = 1)
        {
            int userid = CurrentUserId;

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(p => p.registerid == userid).Count();
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
            var list = ef.hx_UserAct.Where(p => p.registerid == userid).OrderByDescending(a => a.ActID).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;

            return View(list);

        }

        /// <summary>
        /// 安全中心
        /// </summary>
        /// <returns></returns> 
        public ActionResult Security()
        {
            int userid = CurrentUserId;
            hx_member_table HUsr = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();

            ViewBag.flag = ef.V_UsrBindCardBank.Any(p => p.registerid == userid);
            return View(HUsr);
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        /// <returns></returns> 
        public ActionResult Authentication()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns> 
        public ActionResult ChangePwd()
        {



            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult PostChangepassword()
        {
            int userid = CurrentUserId;

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
                    json = @" {""rs""    : ""n"", ""error""      :  ""原始密码输入错误!"" }";

                }

            }
            else
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""异常错误!""}";

            }
            return Content(json);
        }


        /// <summary>
        /// 交易密码
        /// </summary>
        /// <returns></returns> 
        public ActionResult ChangeTradePwd()
        {
            int userid = CurrentUserId;
            string url = Utils.GetChinapnrUrl();

            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);

            if (p.UsrCustId == null || p.UsrCustId == "")
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




        #region 网银充值
        /// <summary>
        /// 网银充值
        /// </summary>
        /// <returns></returns>
        ///  
        public ActionResult bankingRecharge()
        {


            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            StringBuilder str = new StringBuilder();
            int userid = CurrentUserId;
            string blankName = Utils.CheckSQLHtml(DNTRequest.GetString("cardid"));
            M_QPNetSave qp = new M_QPNetSave();
            ReQPNetSave rqp = new ReQPNetSave();
            M_Recharge_history rh = new M_Recharge_history();
            B_Recharge_history b = new B_Recharge_history();
            B_UsrBindCard bu = new B_UsrBindCard();
            M_UsrBindCard bm = new M_UsrBindCard();
            p = o.GetModel(userid);
            if (p.UsrCustId == null || p.UsrCustId == "")
            {
                return Redirect("/opening_account/index/" + userid.ToString());
            }


            string UsrCustId = p.UsrCustId; //这个是给用户充值  在充值前得保证商户余额足够
            decimal amt = RMB.GetDecimal(DNTRequest.GetDecimal("amt", 100.00M) * 1.00M, 2, true);
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
                // qp.RetUrl = "http://localhost:26263/usercenter/SuQPNetSave";

                //qp.BgRetUrl = Utils.GetRe_url("22Thirdparty/ReQPNetSave");
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
        /// 充值列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns> 
        public ActionResult RechargeRecord(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.V_Recharge_user_bank.Where(p => p.membertable_registerid == userid).Count();
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
            var list = ef.V_Recharge_user_bank.Where(p => p.membertable_registerid == userid).OrderByDescending(a => a.recharge_history_id).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }

        public ActionResult LLRechargeRecord(int pageIndex = 1)
        {
            int userid = CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.V_td_LLpay_bank.Where(p => p.UsrId == userid).Count();
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
            var list = ef.V_td_LLpay_bank.Where(p => p.UsrId == userid).OrderByDescending(a => a.Rechargeid).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }


        #region 登录汇付第三方
        /// <summary>
        /// 登录汇付第三方
        /// </summary>
        /// <returns></returns> 
        public ActionResult thirdpartylogin()
        {
            int userid = CurrentUserId;
            string url = Utils.GetChinapnrUrl();

            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);

            if (p.UsrCustId == null || p.UsrCustId == "")
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

        #region 注册投资送流量

        /// <summary>
        /// 判断是否是活动用户
        /// </summary>
        /// <param name="ActivityFlowType">注册发放状态</param>
        /// <param name="IsGrant">投资方法状态</param>
        /// <returns></returns>
        public int SelectMemberInfo(ref int ActivityFlowType, ref int IsGrant)
        {
            int userid = CurrentUserId;

            string sql = "select ActivityFlowID, RegisterID,ActivityFlowType,IsGrant,CreateTime from ActivityFlow where RegisterID=" + userid;

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                ActivityFlowType = (int)dt.Rows[0]["ActivityFlowType"];
                IsGrant = (int)dt.Rows[0]["IsGrant"];
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 注册流量发放
        /// </summary>
        /// <param name=""></param>
        public ActionResult registerReceive()
        {
            string responseJson = "";
            int userid = CurrentUserId;
            int ActivityFlowType = -1;
            int IsGrant = -1;
            DateTime nowdate = DateTime.Now;
            DateTime startdate = new DateTime(2016, 10, 21, 9, 00, 00);
            DateTime enddate = new DateTime(2016, 10, 31, 23, 59, 59);
            if (nowdate > startdate && nowdate < enddate)
            {
                int zt = SelectMemberInfo(ref ActivityFlowType, ref IsGrant);
                if (zt == 1)
                {
                    if (ActivityFlowType == 0)
                    {
                        B_member_table b = new B_member_table();
                        M_member_table p = new M_member_table();
                        p = b.GetModel(userid);
                        if (p.isrealname == 1)//如果实名认证
                        {
                            string resultInfo = "";
                            GetLiuMiTokeyInfo(ref resultInfo);
                            string token = Utils.GetCookie("CookToken");
                            if (token == "-1")
                            {
                                LogInfo.WriteLog("注册送流量活动：手机(" + p.mobile + ")获取token失败:" + resultInfo);
                                responseJson = @" {""rs"": ""n"", ""info"":  ""获取token失败:" + resultInfo + "}";
                            }
                            else
                            {
                                //流量包ID： 
                                //移动：YD10 / YD30 / YD70 / YD150 / YD500 / YD1024
                                //电信：DX5 / DX10 / DX30 / DX50 / DX100 / DX200 / DX500 / DX1024
                                //联通：LT20 / LT50 / LT100 / LT200 / LT500
                                string postpackage = "";
                                string appver = "Http";
                                string extno = "";//扩展号，可为空。一般为第三方系统内唯一ID
                                string fixtime = "";
                                string sign = "appkey" + appkey + "mobile" + p.mobile + "token" + token;
                                string json = "{\"appkey\":\"" + appkey + "\",\"mobile\":\"" + p.mobile + "\",\"token\":\"" + token + "\",\"sign\":\"" + LiumiTools.GetSHA1(sign) + "\"}";

                                string result = LiumiTools.PostJson(url + "phoneInfo", json);
                                JavaScriptSerializer sc = new JavaScriptSerializer();

                                Dictionary<String, object> dic = sc.Deserialize<Dictionary<String, object>>(result);
                                dic = sc.Deserialize<Dictionary<String, object>>(result);
                                string code = dic["code"].ToString();

                                if (code.Equals("000"))//如果下单成功
                                {
                                    //isp  1 为电信 2 为移动 3 为联通
                                    string isp = dic["isp"].ToString();
                                    if (isp == "1")
                                    {
                                        postpackage = "DX200";
                                    }
                                    else if (isp == "2")
                                    {
                                        postpackage = "YD150";
                                    }
                                    else if (isp == "3")
                                    {
                                        postpackage = "LT200";
                                    }

                                    sign = "appkey" + appkey + "appsecret" + appsecret + "appver" + appver + "mobile" + p.mobile + "postpackage" + postpackage + "token" + token;
                                    json = "{\"appkey\":\"" + appkey + "\",\"appsecret\":\"" + appsecret + "\",\"appver\":\"" + appver + "\",\"mobile\":\"" + p.mobile + "\",\"postpackage\":\"" + postpackage + "\",\"token\":\"" + token + "\",\"extno\":\"" + extno + "\",\"fixtime\":\"" + fixtime + "\",\"sign\":\"" + LiumiTools.GetSHA1(sign) + "\"}";

                                    result = LiumiTools.PostJson(url + "placeOrder", json);
                                    dic = sc.Deserialize<Dictionary<String, object>>(result);
                                    code = dic["code"].ToString();
                                    if (code.Equals("000"))//如果下单成功
                                    {
                                        Dictionary<string, object> data = (Dictionary<string, object>)dic["data"];//下单成功后获得的流水号

                                        DateTime dtime = DateTime.Now;
                                        string sqlStr = "insert into ActivityFlowWater (RegisterID,OrderNO,WaterType,CreateTime) values (" + userid + "," + data["orderNO"].ToString() + ",0,'" + dtime + "')";//添加注册流量充值流水
                                        DbHelperSQL.RunSql(sqlStr);
                                        sqlStr = "update ActivityFlow set ActivityFlowType=1 where RegisterID = " + userid;//更新流量充值表注册发放状态 
                                        DbHelperSQL.RunSql(sqlStr);
                                        responseJson = @" {""rs"": ""y"", ""info"":  ""获取流量成功,所送流量可能出现延迟，将会在24小时内发放到您的账号""}";
                                    }
                                    else
                                    {
                                        LogInfo.WriteLog("注册送流量活动：手机(" + p.mobile + ")下单失败:" + result);
                                        responseJson = @" {""rs"": ""n"", ""info"":  ""获取流量失败，请联系网站管理员""}";
                                    }
                                }
                                else
                                {
                                    LogInfo.WriteLog("注册送流量活动：手机(" + p.mobile + ")获取运营商信息失败:" + result);
                                    responseJson = @" {""rs"": ""n"", ""info"":  ""手机运营商查询失败，请联系网站管理员""}";
                                }
                            }

                        }
                        else
                        {
                            responseJson = @" {""rs"": ""n"", ""info"":  ""您尚未实名认证""}";
                        }
                    }
                    else
                    {
                        responseJson = @" {""rs"": ""n"", ""info"":  ""您已经领取过注册奖励""}";
                    }
                }
                else
                {
                    responseJson = @" {""rs"": ""n"", ""info"":  ""您不是活动用户""}";
                }
            }
            else
            {
                responseJson = @" {""rs"": ""n"", ""info"":  ""活动不在有效时间内""}";
            }
            return Content(responseJson);
        }

        /// <summary>
        /// 估计投标金额判断流量发放
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult getJinE()
        {
            string responseJson = "";
            DateTime nowdate = DateTime.Now;
            DateTime startdate = new DateTime(2016, 10, 21, 9, 00, 00);
            DateTime enddate = new DateTime(2016, 10, 31, 23, 59, 59);
            if (nowdate > startdate && nowdate < enddate)
            {
                string postpackage = "";
                int userid = CurrentUserId;

                int param = Convert.ToInt32(Utils.CheckSQLHtml(DNTRequest.GetString("param")));//领取按钮流量金额
                int ActivityFlowType = -1;
                int IsGrant = -1;
                int zt = SelectMemberInfo(ref ActivityFlowType, ref IsGrant);
                if (zt == 1)
                {
                    if (IsGrant == 0)
                    {
                        B_member_table b = new B_member_table();
                        M_member_table p = new M_member_table();
                        p = b.GetModel(userid);
                        if (p.isrealname == 1)
                        {
                            string sql = "select top 1 a.registerid,a.username,a.mobile,b.targetid,b.bid_records_id,b.investment_amount from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid  and a.registerid = " + userid + " and b.invest_state = 1 order by b.bid_records_id";//获取用户第一次投标记录信息 invest_state = 1 成功 2 失败 3 流标返还
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                            if (dt.Rows.Count > 0)
                            {
                                decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                                if (investAmt >= 1000)
                                {
                                    if (investAmt >= 1000 && investAmt < 3000)
                                    {
                                        if (param != 500)
                                        {
                                            responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                            return Content(responseJson);
                                        }

                                    }
                                    else if (investAmt >= 3000 && investAmt < 5000)
                                    {
                                        if (param != 1024)
                                        {
                                            responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                            return Content(responseJson);
                                        }

                                    }
                                    else if (investAmt >= 5000)
                                    {
                                        if (param != 2048)
                                        {
                                            responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                            return Content(responseJson);
                                        }

                                    }

                                    string isp = "0";
                                    string token = "";
                                    getYYSInfo(p.mobile, ref isp, ref token);
                                    if (isp == "1")
                                    {
                                        if (investAmt >= 1000 && investAmt < 3000)
                                        {
                                            responseJson = TZReceive(token, userid, p.mobile, "DX500");
                                        }
                                        else if (investAmt >= 3000 && investAmt < 5000)
                                        {
                                            responseJson = TZReceive(token, userid, p.mobile, "DX1024");
                                        }
                                        else if (investAmt >= 5000)
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                responseJson = TZReceive(token, userid, p.mobile, "DX1024");
                                            }
                                        }
                                    }
                                    else if (isp == "2")
                                    {
                                        if (investAmt >= 1000 && investAmt < 3000)
                                        {
                                            responseJson = TZReceive(token, userid, p.mobile, "YD500");
                                        }
                                        else if (investAmt >= 3000 && investAmt < 5000)
                                        {
                                            responseJson = TZReceive(token, userid, p.mobile, "YD1024");
                                        }
                                        else if (investAmt >= 5000)
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                responseJson = TZReceive(token, userid, p.mobile, "YD1024");
                                            }
                                        }
                                    }
                                    else if (isp == "3")
                                    {
                                        if (investAmt >= 1000 && investAmt < 3000)
                                        {
                                            responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                        }
                                        else if (investAmt >= 3000 && investAmt < 5000)
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                            }
                                        }
                                        else if (investAmt >= 5000)
                                        {
                                            for (int i = 0; i < 4; i++)
                                            {
                                                responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseJson = @" {""rs"": ""n"", ""info"":  ""获取号码归属地错误，请联系网站管理员""}";
                                    }
                                    //if (investAmt >= 3000 && investAmt < 5000)
                                    //{
                                    //    if (param != 500)
                                    //    {
                                    //        responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                    //        return Content(responseJson);
                                    //    }

                                    //}
                                    //else if (investAmt == 5000)
                                    //{
                                    //    if (param != 1024)
                                    //    {
                                    //        responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                    //        return Content(responseJson);
                                    //    }

                                    //}
                                    //else if (investAmt > 5000)
                                    //{
                                    //    if (param != 2048)
                                    //    {
                                    //        responseJson = @" {""rs"": ""n"", ""info"":  ""请根据您的投资选择相应奖励进行领取""}";
                                    //        return Content(responseJson);
                                    //    }

                                    //}

                                    //string isp = "0";
                                    //string token = "";
                                    //getYYSInfo(p.mobile, ref isp, ref token);
                                    //if (isp == "1")
                                    //{
                                    //    if (investAmt >= 3000 && investAmt < 5000)
                                    //    {
                                    //        responseJson = TZReceive(token, userid, p.mobile, "DX500");
                                    //    }
                                    //    else if (investAmt == 5000)
                                    //    {
                                    //        responseJson = TZReceive(token, userid, p.mobile, "DX1024");
                                    //    }
                                    //    else if (investAmt > 5000)
                                    //    {
                                    //        for (int i = 0; i < 2; i++)
                                    //        {
                                    //            responseJson = TZReceive(token, userid, p.mobile, "DX1024");
                                    //        }
                                    //    }
                                    //}
                                    //else if (isp == "2")
                                    //{
                                    //    if (investAmt >= 3000 && investAmt < 5000)
                                    //    {
                                    //        responseJson = TZReceive(token, userid, p.mobile, "YD500");
                                    //    }
                                    //    else if (investAmt == 5000)
                                    //    {
                                    //        responseJson = TZReceive(token, userid, p.mobile, "YD1024");
                                    //    }
                                    //    else if (investAmt > 5000)
                                    //    {
                                    //        for (int i = 0; i < 2; i++)
                                    //        {
                                    //            responseJson = TZReceive(token, userid, p.mobile, "YD1024");
                                    //        }
                                    //    }
                                    //}
                                    //else if (isp == "3")
                                    //{
                                    //    if (investAmt >= 3000 && investAmt < 5000)
                                    //    {
                                    //        responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                    //    }
                                    //    else if (investAmt == 5000)
                                    //    {
                                    //        for (int i = 0; i < 2; i++)
                                    //        {
                                    //            responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                    //        }
                                    //    }
                                    //    else if (investAmt > 5000)
                                    //    {
                                    //        for (int i = 0; i < 4; i++)
                                    //        {
                                    //            responseJson = TZReceive(token, userid, p.mobile, "LT500");
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    responseJson = @" {""rs"": ""n"", ""info"":  ""获取号码归属地错误，请联系网站管理员""}";
                                    //}
                                }
                                else
                                {
                                    responseJson = @" {""rs"": ""n"", ""info"":  ""投资金额小于3000元不参与本活动""}";
                                }
                            }
                            else
                            {
                                responseJson = @" {""rs"": ""n"", ""info"":  ""啊哦……投资后才能领取""}";//用户还未投资
                            }
                        }
                        else
                        {
                            responseJson = @" {""rs"": ""n"", ""info"":  ""您尚未实名认证""}";
                        }
                    }
                    else
                    {
                        responseJson = @" {""rs"": ""n"", ""info"":  ""您已经领取过投资奖励""}";
                    }
                }
                else
                {
                    responseJson = @" {""rs"": ""n"", ""info"":  ""您不是活动用户""}";
                }
            }
            else
            {
                responseJson = @" {""rs"": ""n"", ""info"":  ""活动不在有效时间内""}";
            }
            return Content(responseJson);
        }

        /// <summary>
        /// 投资流量发放
        /// </summary>
        /// <param name="token"></param>
        /// <param name="RegisterID"></param>
        /// <param name="mobile"></param>
        /// <param name="postpackage"></param>
        /// <returns></returns>
        public string TZReceive(string token, int RegisterID, string mobile, string postpackage)
        {
            string responseJson = "";

            string appver = "Http";
            string extno = "";//扩展号，可为空。一般为第三方系统内唯一ID
            string fixtime = "";

            JavaScriptSerializer sc = new JavaScriptSerializer();
            string sign = "appkey" + appkey + "appsecret" + appsecret + "appver" + appver + "mobile" + mobile + "postpackage" + postpackage + "token" + token;
            string json = "{\"appkey\":\"" + appkey + "\",\"appsecret\":\"" + appsecret + "\",\"appver\":\"" + appver + "\",\"mobile\":\"" + mobile + "\",\"postpackage\":\"" + postpackage + "\",\"token\":\"" + token + "\",\"extno\":\"" + extno + "\",\"fixtime\":\"" + fixtime + "\",\"sign\":\"" + LiumiTools.GetSHA1(sign) + "\"}";

            string result = LiumiTools.PostJson(url + "placeOrder", json);
            Dictionary<String, object> dic = sc.Deserialize<Dictionary<String, object>>(result);
            string code = dic["code"].ToString();
            if (code.Equals("000"))//如果流量充值成功
            {
                Dictionary<string, object> data = (Dictionary<string, object>)dic["data"];//流量充值成功后获得的流米流水号

                DateTime dtime = DateTime.Now;
                string sqlStr = "insert into ActivityFlowWater (RegisterID,OrderNO,WaterType,CreateTime) values (" + RegisterID + "," + data["orderNO"].ToString() + ",1,'" + dtime + "')";//添加投资流量充值流水
                DbHelperSQL.RunSql(sqlStr);
                sqlStr = "update ActivityFlow set IsGrant=1 where RegisterID = " + RegisterID;//更新流量充值表投资发放状态 
                DbHelperSQL.RunSql(sqlStr);
                responseJson = @" {""rs"": ""y"", ""info"":  ""获取流量成功,所送流量可能出现延迟，将会在24小时内发放到您的账号""}";
            }
            else
            {
                LogInfo.WriteLog("投资送流量活动：手机(" + mobile + ")下单失败:" + result);
                responseJson = @" {""rs"": ""n"", ""info"":  ""获取流量失败，请联系网站管理员""}";
            }
            return responseJson;
        }

        /// <summary>
        /// 获取运营商信息 isp  1为电信 2为移动 3为联通
        /// </summary>
        /// <param name="isp"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public void getYYSInfo(string mobile, ref string isp, ref string token)
        {
            string resultInfo = "";

            GetLiuMiTokeyInfo(ref resultInfo);
            string CookToken = Utils.GetCookie("CookToken");
            if (CookToken == "-1")
            {
                LogInfo.WriteLog("投资送流量活动：手机(" + mobile + ")获取token失败:" + resultInfo);
            }
            else
            {
                //流量包ID： 
                //移动：YD10 / YD30 / YD70 / YD150 / YD500 / YD1024
                //电信：DX5 / DX10 / DX30 / DX50 / DX100 / DX200 / DX500 / DX1024
                //联通：LT20 / LT50 / LT100 / LT200 / LT500
                token = CookToken;
                string sign = "appkey" + appkey + "mobile" + mobile + "token" + token;
                string json = "{\"appkey\":\"" + appkey + "\",\"mobile\":\"" + mobile + "\",\"token\":\"" + token + "\",\"sign\":\"" + LiumiTools.GetSHA1(sign) + "\"}";

                string result = LiumiTools.PostJson(url + "phoneInfo", json);
                JavaScriptSerializer sc = new JavaScriptSerializer();
                Dictionary<String, object> dic = sc.Deserialize<Dictionary<String, object>>(result);
                string code = dic["code"].ToString();

                if (code.Equals("000"))//如果下单成功
                {
                    //isp  1 为电信 2 为移动 3 为联通
                    isp = dic["isp"].ToString();
                }
                else
                {
                    LogInfo.WriteLog("投资送流量活动：手机(" + mobile + ")获取运营商信息失败:" + result);
                }
            }


        }

        /// <summary>
        /// 获取Cookie存储Tokey
        /// </summary>
        /// <param name="resultInfo"></param>
        public void GetLiuMiTokeyInfo(ref string resultInfo)
        {

            //保存token信息
            string CookToken = Utils.GetCookie("CookToken");
            if (string.IsNullOrWhiteSpace(CookToken))
            {

                string sign = "appkey" + appkey + "appsecret" + appsecret;
                string json = "{\"appkey\":\"" + appkey + "\",\"appsecret\":\"" + appsecret + "\",\"sign\":\"" + LiumiTools.GetSHA1(sign) + "\"}";
                //获取token
                string result = LiumiTools.PostJson(url + "getToken", json);

                JavaScriptSerializer sc = new JavaScriptSerializer();
                Dictionary<String, object> dic = sc.Deserialize<Dictionary<String, object>>(result);
                string lmcode = dic["code"].ToString();
                if (lmcode.Equals("000"))//如果获取token成功
                {
                    Dictionary<string, object> data = (Dictionary<string, object>)dic["data"];
                    string token = data["token"].ToString();

                    var cookies = new HttpCookie("CookToken");//保存至Cookie
                    cookies.Value = token;
                    cookies.Expires = DateTime.Now.AddHours(6);
                    Response.Cookies.Add(cookies);
                }
                else
                {
                    var cookies = new HttpCookie("CookToken");//保存至Cookie
                    cookies.Value = "-1";
                    cookies.Expires = DateTime.Now.AddHours(6);
                    Response.Cookies.Add(cookies);
                    resultInfo = result;
                }
            }
        }
        #endregion


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