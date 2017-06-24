using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using ChuangLitouP2P.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WeiXin.Controllers
{
    public class registerController : Controller
    {
        CheckEXISTS ck = new CheckEXISTS();

        // GET: register
        public ActionResult Index(string invitedcode = "")
        {
            string cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channel"));
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue);
                Utils.SetInvCookie("Invitation", keyValue, -1);
            }

            invitedcode = Utils.CheckSQLHtml(invitedcode);
            if (!string.IsNullOrWhiteSpace(invitedcode))
            {
                string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + invitedcode + "' ";

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    HttpCookie cok = new HttpCookie("Invitation");
                    cok.Values.Add("InvCode", DESEncrypt.Encrypt(invitedcode, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(cok);
                }
            }
            TXShareHelper tx = new TXShareHelper();

            #region TXShareHelper 赋值逻辑
            tx.link = Request.Url.AbsoluteUri.ToString().Trim();
            tx.CheckSignature(tx.link);
            tx.appid = Utils.GetAppSetting("WeiXinAppid");
            tx.title = "来创利投金服，新手理财送好礼，现金红包送不停";
            if (Utils.GetAppSetting("DeBug") == "1")
            {
                tx.imgUrl = Utils.GetAppSetting("MDeBugURL") + "Images/xrzcshl.jpg";
            }
            else
            {
                tx.imgUrl = Utils.GetAppSetting("MReleaseURL") + "Images/xrzcshl.jpg";
            }
            tx.desc = "创利投金服，新手理财送好礼，现金红包送不停";
            #endregion

            ViewBag.TXShareHelper = tx;
            return View();
        }

        /// <summary>
        /// 触发式验证手机号是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult checkmobile()
        {
            string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("param"));
            string str = ck.checkmobile(mobile);
            return Content(str);
        }

        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }



        #region 单击获取短信按钮 
        /// <summary>
        /// 单击获取短信按钮
        /// </summary>
        /// <returns></returns>
        public ActionResult Regsmscode()
        {
            string json = "";
            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());
            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());
            string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            string vvcode = Utils.CheckSQLHtml(DNTRequest.GetString("vcode"));
            string ipc = Utils.GetRealIP();

            if (vvcode.Length >= 4)
            {
                string strIdentify = "ValidateCode"; //随机字串存储键值，以便存储到Session中
                if (Session[strIdentify] != null)
                {
                    if (Session[strIdentify].ToString() != vvcode)
                    {
                        json = @" {""rs"": ""n"", ""info"":  ""验证码不对!""}";
                        return Content(json);
                    }
                }
                else
                {
                    json = @" {""rs"": ""n"", ""info"":  ""验证码已过期!""}";
                    return Content(json);
                }
            }
            else
            {
                json = @" {""rs"": ""n"", ""info"":  ""v""}";
                return Content(json);
            }

            if (ck.checkmobile(mobile) != "y")
            {
                json = @" {""rs"": ""n"", ""info"":  ""手机号已经被注册!""}";
                return Content(json);
            }

            //短信防刷

            if (Session["checkmobile"] == null)
            {
                Session["checkmobile"] = DateTime.Now.ToString();
            }
            else
            {
                DateTime dte = DateTime.Parse(Session["checkmobile"].ToString());

                long sec = Utils.DateDiff("Second", dte, DateTime.Now);

                if (sec > 60)
                {
                    Session["checkmobile"] = null;
                }
                else
                {
                    json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!请稍后再试""}";
                    return Content(json);
                }

            }

            if (ck.checkipsess(ipc, smstype, smstype1) == false)
            {
                json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!发送异常""}";
                return Content(json);
            }

            //限制ip访问发送次数验证码最多发送4次

            if (ck.checkipnum(Utils.GetRealIP(), smstype, smstype1) >= 8)
            {
                json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!请与客服联系""}";
                return Content(json);
            }
            else
            {
                string contxt = Utils.GetMSMEmailContext(8, 1); // 获取注册验证码

                M_td_SMS_record p = new M_td_SMS_record();
                B_td_SMS_record o = new B_td_SMS_record();
                string sql = "select sms_record_id,smscontext,phone_number,hits from hx_td_SMS_record where ( smstype=" + smstype + "  or  smstype=" + smstype1 + " ) and phone_number='" + mobile + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    //以前存在，直接发送验证码

                    if (int.Parse(dt.Rows[0]["hits"].ToString()) < 8)
                    {
                        decimal dd = SendSMS.Send_SMS(dt.Rows[0]["phone_number"].ToString(), dt.Rows[0]["smscontext"].ToString());

                        sql = "update hx_td_SMS_record set orderid=" + dd.ToString() + ",hits=hits+1 where sms_record_id=" + dt.Rows[0]["sms_record_id"].ToString();

                        DbHelperSQL.RunSql(sql);
                    }
                    else
                    {
                        json = @" {""rs"": ""n"", ""info"":  ""短信发送异常，请与客报联系""}";
                        return Content(json);

                    }

                }
                else
                {
                    //不存在生成新的验证码
                    string vcode = Utils.RndNum(6);

                    string smscontxt = Utils.GetMSMEmailContext(8, 1); // 获取注册成功邮件内容

                    StringBuilder sbsms = new StringBuilder(smscontxt);

                    sbsms = sbsms.Replace("#CODE#", vcode);
                    p.phone_number = mobile;
                    p.sendtime = DateTime.Now;
                    p.senduserid = 0;
                    p.smstype = smstype;
                    p.smscontext = sbsms.ToString();
                    p.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                    p.vcode = vcode;
                    p.ip = Utils.GetRealIP();
                    o.Add(p);

                }
                json = @" {""rs"": ""y"", ""info"":  ""短信发送成功! ""}";
            }
            return Content(json);
        }
        #endregion



        /// <summary>
        /// 短信验证码触发式验证
        /// </summary>
        /// <returns></returns>
        public ActionResult vcodec()
        {
            string json = "";
            string mobilec = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            string param = Utils.CheckSQLHtml(DNTRequest.GetString("param"));
            if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0)
            {
                json = ck.GetVcodeWX(param, mobilec);
            }
            else { json = "y"; }
            return Content(json);

        }


        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {

            string Validatecode = Utils.CheckSQLHtml(DNTRequest.GetString("vcodec").Trim());
            string userpassword1 = Utils.CheckSQLHtml(DNTRequest.GetString("userpassword").Trim());
            string mobile1 = Utils.CheckSQLHtml(DNTRequest.GetString("mobile").Trim());
            string username1 = mobile1;
            string res = Register(Validatecode, userpassword1, mobile1, username1);
            return Content(res);
        }

        public string Register(string Validatecode, string userpassword1, string mobile1, string username1, bool realMobileUser = false)
        {
            Utils.SetSYSDateTimeFormat();
            string json = "";
            //注册奖历
            M_Activity_schedule ma = new M_Activity_schedule();
            B_Activity_schedule ba = new B_Activity_schedule();

            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();

            B_bonus_account bb = new B_bonus_account();
            M_bonus_account mb = new M_bonus_account();

            M_bonus_account_water mbaw = new M_bonus_account_water();
            B_bonus_account_water bbaw = new B_bonus_account_water();
            string email1 = "";
            string strvcode = realMobileUser ? "y" : ck.GetVcode(Validatecode, mobile1);
            //string strvcode = "y";
            if ((Settings.Instance.SiteDomain.IndexOf(PublicURL.NewWXUrl) >= 0))
            {
                strvcode = ck.GetVcodeWX(Validatecode, mobile1);
            }
            string tid = Utils.GetCookie("Cooperation", "tid");
            int ch = 0;

            string invcode = Utils.GetInvCode();

            #region 渠道
            //渠道的cookie
            var cc_keyValue = Utils.GetInvCookie("channel");
            string cInvitedcode = string.Empty;
            if (string.IsNullOrEmpty(invcode) && cc_keyValue.Count != 0)
            {
                cInvitedcode = cc_keyValue["Invitedcode"];
            }
            #endregion

            if (Utils.GetCookie("Cooperation", "ch") != "")
            {
                try
                {

                    ch = int.Parse(Utils.GetCookie("Cooperation", "ch"));
                }
                catch
                {
                    ch = 0;
                }
            }

            if (ch == 0)
            {
                if (invcode != "")
                {
                    ch = 3;
                }
                else
                {
                    ch = 2;
                }

            }

            if (realMobileUser || strvcode != null)
            {
                if (realMobileUser || strvcode == "y")
                {
                    string checkMob = ck.checkmobile(mobile1, 0);

                    if (checkMob != "y")
                    {
                        json = @" {""rs""    : ""n"", ""error""      :  ""手机号已被注册!""}";
                        return (json);
                    }

                    p.username = username1;
                    p.password = DESEncrypt.Encrypt(userpassword1, ConfigurationManager.AppSettings["webp"].ToString());
                    p.mobile = mobile1;
                    p.email = email1;
                    p.usertypes = 0;
                    p.invitedcode = Calculator.Getinvitedcode();
                    p.ismobile = 1;
                    p.Channelsource = ch;
                    p.Tid = tid;

                    p.channel_invitedcode = cInvitedcode;
                    LogInfo.WriteLog("用户注册内容:" + FastJSON.toJOSN(p) + "     IP:" + Utils.GetRealIP());
                    int uid = o.Add(p);//注册成功返回会员ID
                    if (uid > 0)
                    {
                        //记录用户信息到活动流量发放表里  2016.10.21-31日
                        //string CookCode = Utils.GetCookie("CookCode");
                        //if (!string.IsNullOrWhiteSpace(CookCode))
                        //{
                        //    if (CookCode== "liumi")
                        //    {
                        //        AddReceiveInfo(uid);
                        //    }

                        //}

                        if (tid != "")
                        {
                            //    Utils.GetCoopAPI(tid, uid.ToString(), 1);
                        }
                        string keys = DESEncrypt.Encrypt(uid.ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                        string keys1 = DESEncrypt.Encrypt(username1, ConfigurationManager.AppSettings["webp"].ToString());

                        M_td_SMS_record pm = new M_td_SMS_record();
                        B_td_SMS_record om = new B_td_SMS_record();
                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.注册成功.ToString());
                        string smscontxt = Utils.GetMSMEmailContext(18, 1); // 获取注册成功邮件内容
                        StringBuilder sbsms = new StringBuilder(smscontxt);
                        string mobile = mobile1;
                        sbsms = sbsms.Replace("#USERANEM#", p.username.ToString());
                        pm.phone_number = mobile;
                        pm.sendtime = DateTime.Now;
                        pm.senduserid = uid;
                        pm.smstype = smstype;
                        pm.smscontext = sbsms.ToString();
                        pm.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                        pm.vcode = "";
                        om.Add(pm);

                        M_login mlogin = new M_login();
                        mlogin.userid = uid;
                        mlogin.username = p.username;
                        mlogin.codeno = Utils.SetSessioncode();
                        int remember = 0;


                        FormsAuthentication.SignOut();

                        Utils.AddLoginCache(p.username, mlogin);

                        string sql = "update hx_member_table set lastlogintime='" + DateTime.Now + "',lastloginIP='" + Utils.GetRealIP() + "' where registerid=" + uid.ToString();
                        DbHelperSQL.ExecuteSql(sql);

                        #region MyRegion   //记录邀请关系
                        LogInfo.WriteLog("注册:邀请码:" + invcode);
                        if (invcode != "")
                        {
                            string codesql = "SELECT invcode from  hx_td_Userinvitation where  invpersonid=" + uid.ToString();//查询本人是否已经被邀请注册过  invcode='" + invcode + "' and
                            LogInfo.WriteLog("codesql2:" + codesql);
                            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
                            if (dtcode.Rows.Count == 0)
                            {
                                M_td_Userinvitation myao = new M_td_Userinvitation();
                                B_td_Userinvitation dyao = new B_td_Userinvitation();
                                int yaouids = Utils.GetCodeUid();
                                myao.invcode = invcode;
                                myao.invtime = DateTime.Now;
                                myao.invpersonid = uid;
                                myao.Invpeopleid = yaouids;
                                myao.InvitesStates = 0;
                                myao.Invitereward = 0;
                                int myaoint = dyao.Add(myao);
                            }
                        }
                        #endregion MyRegion   //记录邀请关系

                        json = @" {""rs""    : ""y"", ""url""      :  ""/index.html"",""uid"":" + uid + "}";
                        string temstr = "/opening_account/Index/" + uid.ToString();
                        json = json.Replace("/index.html", temstr);
                        return (json);

                    }
                    else
                    {
                        json = @" {""rs""    : ""n"", ""error""      :  ""注册失败!""}";
                        return (json);

                    }
                }
                else
                {
                    json = @" {""rs""    : ""n"", ""error""      :  ""验证码不对!""}";
                    return (json);
                }
            }
            else
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""验证码不存在或过期!""}";


            }
            return (json);
        }

        /// <summary>
        /// 添加注册、投资流量信息
        /// </summary>
        public void AddReceiveInfo(int uid)
        {
            string sqlStr = "select ActivityFlowID, RegisterID,ActivityFlowType,IsGrant,CreateTime from ActivityFlow where RegisterID=" + uid;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sqlStr);
            if (dt.Rows.Count == 0)
            {
                DateTime dtime = DateTime.Now;
                sqlStr = "insert into ActivityFlow (RegisterID,ActivityFlowType,IsGrant,CreateTime) values (" + uid + ",0,0,'" + dtime + "')";//
                int a = DbHelperSQL.ExecuteSql(sqlStr);
            }
        }



        #region 注册成功汇付返回通知
        /// <summary>
        /// 注册成功汇付返回通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Succ_Registered()
        {
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
            LogInfo.WriteLog("注册开户返回报文:" + FastJSON.toJOSN(m));
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
            if (ret == 0)
            {
                if (m.RespCode == "000")
                { //为避免重复调用,增加缓存校验
                    string cachename = "UserRegister" + m.TrxId;
                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        M_bonus_account_water mbaw = new M_bonus_account_water();
                        B_bonus_account_water bbaw = new B_bonus_account_water();
                        B_usercenter b = new B_usercenter();
                        it = b.Succ_Reg(m); //汇付开户成功后更新用户信息
                        if (it > 0)
                        {
                            B_member_table ob = new B_member_table();
                            M_member_table pm = new M_member_table();
                            pm = ob.GetModel(Utils.GetUserSplit(m.UsrId));
                            ViewBag.userid = pm.registerid;
                            //新人注册奖励
                            ActFacade act = new ActFacade();
                            act.SendBonusAfterRegister(pm.registerid, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.wap);
                        }
                        else
                        {
                            /*第三方成功，本地服务器操作失败*/
                        }
                    }
                }
            }
            else
            {
                /*验签不成功*/
            }
            return View(m);
        }
        #endregion
    }
}