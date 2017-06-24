using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class RegisterController : Controller
    {
        private chuangtouEntities ef = new chuangtouEntities();
        CheckEXISTS ck = new CheckEXISTS();
        // GET: Register
        public ActionResult Index()
        {
            string cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channel"));
            if (string.IsNullOrEmpty(cInvitedcode))
            {//此前放出了一批错误连接channnel
                cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channnel"));
            }
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue);
                Utils.SetInvCookie("Invitation", keyValue, -1);
                CheckFubaba();
            }
            else
            {
                var invitedcode = Utils.CheckSQLHtml("invitedcode");
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
            }




            Utils.SetSYSDateTimeFormat();
            ViewBag.day = DateTime.Now;

            ViewBag.rndstr = Utils.RndNumChar(10).ToString();
            return View();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Forget()
        {

            return View();
        }


        /// <summary>
        /// 注册
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
            //注册奖励
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
            string tid = "";//富爸爸参数uid //Utils.GetCookie("Cooperation", "tid");//渠道来源ID
            string invcode = Utils.GetInvCode();

            // 用户邀请用户关系存在的时候，不走 新渠道机制（channel）
            #region 渠道
            //渠道的cookie
            var cc_keyValue = Utils.GetInvCookie("channel");
            string cInvitedcode = string.Empty;
            if (string.IsNullOrEmpty(invcode) && cc_keyValue.Count != 0)
            {
                cInvitedcode = cc_keyValue["Invitedcode"];
                var channelInvitedCode = ef.hx_Channel.Where(c => c.ChannelName == "fubaba").Select(c => c.Invitedcode).FirstOrDefault();
                if (cInvitedcode.Trim() == channelInvitedCode)
                {
                    tid = cc_keyValue["uid"];
                }
            }
            #endregion 
            int ch = 0;
            if (Utils.GetCookie("Cooperation", "ch") != "")
            {
                try
                {
                    ch = int.Parse(Utils.GetCookie("Cooperation", "ch"));//渠道来源
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
                    ch = 1;
                }

            }

            if (strvcode != null)
            {
                if (strvcode == "y")
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
                    int uid = o.Add(p);
                    if (uid > 0)
                    {
                        CheckXiCai(p.mobile, cInvitedcode);
                        //if (tid != "")
                        //{
                        //    Utils.GetCoopAPI(tid, uid.ToString(), 1);
                        //}
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
                        if (Utils.LoginWriteSession(mlogin, remember) > 0)
                        {
                            string sql = "update hx_member_table set lastlogintime='" + DateTime.Now + "',lastloginIP='" + Utils.GetRealIP() + "' where registerid=" + uid.ToString();
                            LogInfo.WriteLog(sql);
                            DbHelperSQL.ExecuteSql(sql);
                        }


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

                        json = @" {""rs""    : ""y"", ""url""      :  ""/""}";
                        string temstr = "/opening_account/Index/" + uid.ToString();
                        json = json.Replace("/", temstr);
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
        /// 富爸爸渠道信息记录
        /// </summary>
        private void CheckFubaba()
        {
            string cInvitedcode = DNTRequest.GetString("channel");

            var channelInvitedCode = ef.hx_Channel.Where(c => c.ChannelName == "fubaba").Select(c => c.Invitedcode).FirstOrDefault();
            if (cInvitedcode.Trim() != channelInvitedCode)
                return;
            string uid = "";
            if (Utils.GetAppSetting("DeBug") == "1")
                uid = "1107";
            else
                uid = DNTRequest.GetString("uid");
            if (string.IsNullOrWhiteSpace(uid))
                uid = "1316";

            var keyValue = new Dictionary<string, string>();
            keyValue.Add("uid", uid);
            keyValue.Add("Invitedcode", channelInvitedCode);
            Utils.SetInvCookie("channel", keyValue, 30);
        }
        /// <summary>
        /// 希财渠道信息记录
        /// </summary>
        private void CheckXiCai(string phone, string cInvitedcode)
        {
            //string cInvitedcode = DNTRequest.GetString("channel");

            var channelInvitedCode = ef.hx_Channel.Where(c => c.ChannelName == "xicai").Select(c => c.Invitedcode).FirstOrDefault();
            if (cInvitedcode.Trim() != channelInvitedCode)
                return;
            var user = ef.hx_member_table.Where(c => c.mobile == phone && c.channel_invitedcode == cInvitedcode).First();
            new XiCaiHelper("").InserXiCaiUser(user);
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


        #region 语音通知短信接口
        /// <summary>
        /// 语音通知短信接口
        /// </summary>
        /// <returns></returns>
        public ActionResult vsmscode()
        {
            string json = "";
            string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            string vvcode = Utils.CheckSQLHtml(DNTRequest.GetString("vcode"));
            string ipc = Utils.GetRealIP();
            M_td_SMS_record p = new M_td_SMS_record();
            B_td_SMS_record o = new B_td_SMS_record();
            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());
            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());
            string sql = "select sms_record_id,smscontext,phone_number,vcode,sendtime,hits from hx_td_SMS_record where (smstype=" + smstype + " or smstype=" + smstype1 + "   ) and phone_number='" + mobile + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";
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
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    //以前存在，直接发送验证码

                    // decimal dd = SendSMS.Send_SMS(dt.Rows[0]["phone_number"].ToString(), dt.Rows[0]["smscontext"].ToString());
                    if (int.Parse(dt.Rows[0]["hits"].ToString()) < 8)
                    {
                        //string vstr = "您的验证码是 " + Utils.strJoin(dt.Rows[0]["vcode"].ToString());

                        string vstr = dt.Rows[0]["vcode"].ToString();

                        decimal dd = SendSMS.Send_Audio(dt.Rows[0]["phone_number"].ToString(), vstr);

                        if (dd > 0)
                        {
                            sql = "update hx_td_SMS_record set orderid=" + dd.ToString() + ",hits=hits+1 where sms_record_id=" + dt.Rows[0]["sms_record_id"].ToString();

                            DbHelperSQL.RunSql(sql);
                        }


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
                    //string vstr = "您的验证码是 " + Utils.strJoin(vcode);
                    string vstr = vcode;

                    string smscontxt = Utils.GetMSMEmailContext(8, 1); // 获取注册成功邮件内容

                    StringBuilder sbsms = new StringBuilder(smscontxt);

                    sbsms = sbsms.Replace("#CODE#", vcode);
                    p.phone_number = mobile;
                    p.sendtime = DateTime.Now;
                    p.senduserid = 0;
                    p.smstype = smstype;
                    p.smscontext = sbsms.ToString();
                    // p.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());

                    p.orderid = SendSMS.Send_Audio(mobile, vstr);
                    p.vcode = vcode;
                    o.Add(p);

                }
                json = @" {""rs"": ""y"", ""info"":  ""语音短信发送成功! 请留意接听电话 ""}";

            }
            return Content(json);
        }
        #endregion

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
            json = ck.GetVcode(param, mobilec);
            return Content(json);

        }

        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        #region 注册成功汇付返回通知  
        //汇付模拟发起的请求，没有携带用户的cookie，造成邀请丢失，邀请关系逻辑迁移至注册成功
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
            string log = "注册开户返回报文:" + FastJSON.toJOSN(m);
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
                            ViewBag.registerid = pm.registerid;

                            //新人注册奖励
                            ActFacade act = new ActFacade();
                            log += "；注册ID:" + pm.registerid;
                            act.SendBonusAfterRegister(pm.registerid, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.web);
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
                log += "验签不成功";
                /*验签不成功*/
            }
            LogInfo.WriteLog(log);
            return View(m);
        }
        #endregion


    }
}