using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
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
    public class loginController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();
        // GET: login
        public ActionResult Index()
        {
            ViewBag.jmpUrl = HttpContext.Request.QueryString["RedirectUrl"];
            ViewBag.rndstr = DateTime.Now.Ticks.ToString();
            return View();
        }



        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult signin()
        {
            string username = Utils.CheckSQLHtml(DNTRequest.GetString("username").Trim());

            string userpassword = DESEncrypt.Encrypt(Utils.CheckSQLHtml(DNTRequest.GetString("userpassword").Trim()), ConfigurationManager.AppSettings["webp"].ToString());

            string Validatecode = Utils.CheckSQLHtml(DNTRequest.GetString("Validatecode").Trim());

            int remember = DNTRequest.GetInt("remember", 0);
            string res = LoginIN(username, userpassword, Validatecode, remember);
            return Content(res, "text/json");
        }

        public string LoginIN(string username, string userpassword, string Validatecode, int remember, bool realMobileUser = false)
        {
            string ip = Utils.GetRealIP();
            string json = "";
            B_member_table o = new B_member_table();
            string strIdentify = "LoginValidateCode"; //随机字串存储键值，以便存储到Session中

            var ts = true;
            if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0)
            {
                ts = false;
            }


            if (realMobileUser || Session[strIdentify] != null)
            {
                if (realMobileUser && ts == false)
                {
                    if (Session[strIdentify].ToString() != Validatecode)
                    {
                        json = @" {""rs""    : ""n"", ""error""      :  ""验证码不对!""}";
                        return (json);
                    }
                }
                if (realMobileUser || Session[strIdentify].ToString() == Validatecode)
                {
                    int userid = o.CheckLogin(username, userpassword);

                    if (userid > 0)
                    {
                        M_login mlogin = new M_login();
                        mlogin.userid = userid;
                        mlogin.username = username;
                        mlogin.codeno = Utils.SetSessioncode();
                        Utils.AddLoginCache(username, mlogin);

                        string sql = "update hx_member_table set lastlogintime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastloginIP='" + Utils.GetRealIP() + "' where registerid=" + userid.ToString();

                        LogInfo.WriteLog("登录信息更新" + sql);
                        DbHelperSQL.ExecuteSql(sql);

                        #region 登录成功
                        try
                        {
                            hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                            usrmode.logintime = DateTime.Now;
                            usrmode.Loginusrname = username;
                            usrmode.loginusrpass = "********";
                            usrmode.registerid = userid;
                            usrmode.loginIP = ip;
                            usrmode.logincity = GetIpToCity.GetAddressByIp(ip);
                            usrmode.loginsource = 1;
                            usrmode.loginstate = 0;
                            ef.hx_td_usrlogininfo.Add(usrmode);
                            int ie = ef.SaveChanges();

                            //登录发送1580奖励（活动时间:2017.1.9-1.24）
                            using (ChuanglitouP2P.BLL.EF.ActFacade actFacade = new ChuanglitouP2P.BLL.EF.ActFacade())
                            {
                                actFacade.LoginSendDKQ(userid);
                            }
                        }
                        catch (Exception tx)
                        {

                            throw (tx);
                        }
                        #endregion
                        var jmpUrl = DNTRequest.GetString("jmpUrl");
                        if (!string.IsNullOrEmpty(jmpUrl))
                        {
                            return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "y", url = jmpUrl });
                        }

                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "y", url = "/" });


                    }
                    else if (userid == -100)
                    {

                        #region 登录失败
                        try
                        {
                            hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                            usrmode.logintime = DateTime.Now;
                            usrmode.Loginusrname = username;
                            usrmode.loginusrpass = "********";
                            usrmode.registerid = userid;
                            usrmode.loginIP = ip;
                            usrmode.logincity = GetIpToCity.GetAddressByIp(ip);
                            usrmode.loginsource = 1;
                            usrmode.loginstate = 2;
                            ef.hx_td_usrlogininfo.Add(usrmode);
                            int ie = ef.SaveChanges();
                        }
                        catch { }
                        #endregion


                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "n", error = "禁止登录!" });
                    }
                    else
                    {

                        #region 登录失败
                        try
                        {
                            hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                            usrmode.logintime = DateTime.Now;
                            usrmode.Loginusrname = username;
                            usrmode.loginusrpass = "********";
                            usrmode.registerid = userid;
                            usrmode.loginIP = ip;
                            usrmode.logincity = GetIpToCity.GetAddressByIp(ip);
                            usrmode.loginsource = 1;
                            usrmode.loginstate = 1;
                            ef.hx_td_usrlogininfo.Add(usrmode);
                            int ie = ef.SaveChanges();
                        }
                        catch { }
                        #endregion



                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "n", error = "用户名或密码错误!" });
                    }

                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "n", error = "验证码不对!" });

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { rs = "n", error = "验证码过期!" });
        }




        #endregion



        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["LoginValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }




        public ActionResult Validate()
        {

            if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) < 0)
            {
                return Content("y");
            }
            string str = "";
            string param = Utils.CheckSQLHtml(DNTRequest.GetString("param"));
            string strIdentify = "LoginValidateCode"; //随机字串存储键值，以便存储到Session中
            if (Session[strIdentify] != null)
            {
                if (param == Session[strIdentify].ToString())
                {
                    str = "y";
                }
                else
                {
                    str = "验证码不对!";
                }
            }
            else
            {
                str = "验证码已过期!";
            }
            return Content(str);
        }






        #region 找回密码
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Forgot()
        {

            return View();
        }


        #endregion



        #region 确认修改密码
        /// <summary>
        /// 确认修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ComForgot()
        {
            string tel1 = "";
            string rid1 = "";
            string pass = "";
            string vcode1 = "";
            string type1 = "";
            string rid = "";
            string tel = "";
            string vcode = "";
            string type2 = "";

            string json = "";

            if (Request["key"] != null)
            {
                rid1 = Utils.CheckSQLHtml(DNTRequest.GetString("key"));
                rid = DESEncrypt.Decrypt(rid1, ConfigurationManager.AppSettings["webp"].ToString());

            }

            if (Request["t"] != null)
            {
                tel1 = Utils.CheckSQLHtml(DNTRequest.GetString("t"));
                tel = DESEncrypt.Decrypt(tel1, ConfigurationManager.AppSettings["webp"].ToString());

            }

            if (Request["c"] != null)
            {
                vcode1 = Utils.CheckSQLHtml(DNTRequest.GetString("c"));
                vcode = DESEncrypt.Decrypt(vcode1, ConfigurationManager.AppSettings["webp"].ToString());
            }

            if (Request["p"] != "")
            {
                type1 = Utils.CheckSQLHtml(DNTRequest.GetString("p"));
                type2 = DESEncrypt.Decrypt(type1, ConfigurationManager.AppSettings["webp"].ToString());

            }
            if (type2 == "gettel")
            {

                string sql = "select top 1 smscontext,phone_number from hx_td_SMS_record where ( smstype=8 or  smstype=7)  and phone_number='" + tel + "' and vcode='" + vcode + "'  and  DATEDIFF(MINUTE,sendtime,getDate())<20  order by sms_record_id desc";
                DataTable dtc = DbHelperSQL.GET_DataTable_List(sql);
                var ts = true;
                if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0)
                {
                    ts = false;
                }
                if (dtc.Rows.Count > 0 || ts)
                {

                    sql = "select registerid from hx_member_table where registerid=" + rid + " and mobile='" + tel + "' ";
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {


                        /*
                         pass = DESEncrypt.Encrypt(Utils.CheckSQLHtml(DNTRequest.GetString("userpassword")), ConfigurationManager.AppSettings["webp"].ToString());


                          sql = "update hx_member_table set password='" + pass + "' where registerid=" + rid;

                          DbHelperSQL.RunSql(sql);
                          json = @" {""rs""    : ""y"",""info"":""密码修改成功"", ""url""      :  ""/""}";
                          */
                    }
                    else
                    {

                        Response.Write("<script language='javascript'>alert('数据异常!');javascript:window.location.href='/index.html';</script>");

                        Response.End();
                    }
                }
                else
                {

                    Response.Write("<script language='javascript'>alert('验证码无效，或已经过期，请重新获取!');javascript:window.location.href='/index.html';</script>");

                    Response.End();

                }
            }
            ViewBag.tel1 = tel1;
            ViewBag.rid1 = rid1;
            ViewBag.vcode1 = vcode1;
            ViewBag.type1 = type1;

            return View();
        }



        #endregion


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }



        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult smscode()
        {
            string json = "";
            string vvcode = "";
            B_usercenter bu = new B_usercenter();
            string mobile = Utils.CheckSQLHtml(Request["mobile"].ToString().Trim());
            int userid = DNTRequest.GetInt("userid", 0);
            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());
            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());
            string sql2 = "SELECT registerid,username,mobile from hx_member_table where  mobile='" + mobile + "'";
            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql2);
            if (dt1.Rows.Count > 0)
            {
                if (Request["vcodec"] != null)
                {
                    vvcode = Utils.CheckSQLHtml(Request["vcodec"].ToString().Trim());
                }

                string ipc = Utils.GetRealIP();

                if (vvcode.Length >= 4)
                {
                    string strIdentify = "LoginValidateCode"; //随机字串存储键值，以便存储到Session中
                    if (Session[strIdentify] != null)
                    {
                        if (Session[strIdentify].ToString() == vvcode)
                        {

                        }
                        else
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
                else if (vvcode.Length > 0 && vvcode.Length <= 3)
                {
                    json = @" {""rs"": ""n"", ""info"":  ""验证码位数不对!""}";
                    return Content(json);
                }
                else
                {
                    // json = @" {""rs"": ""n"", ""info"":  ""v""}";
                    // return Content(json);
                }

                if (Session["checkmobileq"] == null)
                {
                    Session["checkmobileq"] = DateTime.Now.ToString();
                }
                else
                {
                    DateTime dte = DateTime.Parse(Session["checkmobileq"].ToString());

                    long sec = Utils.DateDiff("Second", dte, DateTime.Now);

                    if (sec > 60)
                    {
                        Session["checkmobileq"] = null;
                    }
                    else
                    {
                        json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!请稍后再试""}";
                        return Content(json);
                    }
                }

                if (bu.checkipsess(Utils.GetRealIP(), smstype, smstype1) == false)
                {
                    json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!发送异常""}";
                    return Content(json);
                }

                if (bu.checkipnum(Utils.GetRealIP(), smstype, smstype1) >= 8)
                {
                    json = @" {""rs"": ""n"", ""info"":  ""短信发送太频繁!请与客服联系""}";
                    return Content(json);

                }
                else
                {
                    /*
                     hx_td_SMS_record记录短信，验证码类型应是1
                     * 
                     */
                    string contxt = Utils.GetMSMEmailContext(1, 0); // 获取注册成功邮件内容

                    M_td_SMS_record p = new M_td_SMS_record();
                    B_td_SMS_record o = new B_td_SMS_record();
                    string sql = "select sms_record_id,smscontext,phone_number,hits from hx_td_SMS_record where ( smstype=" + smstype + "  or  smstype=" + smstype1 + " ) and phone_number='" + mobile + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3 order by sms_record_id desc";

                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {
                        if (int.Parse(dt.Rows[0]["hits"].ToString()) < 3)
                        {
                            //以前存在，直接发送验证码
                            decimal dd = SendSMS.Send_SMS(dt.Rows[0]["phone_number"].ToString(), dt.Rows[0]["smscontext"].ToString());
                            if (dd != 0)
                            {
                                json = @" {""rs"": ""n"", ""info"":  ""短信发送异常，请与客报联系""}";
                                return Content(json);
                            }
                            //if (dd > 0)
                            // {
                            sql = "update hx_td_SMS_record set orderid=" + dd.ToString() + ",hits=hits+1  where sms_record_id=" + dt.Rows[0]["sms_record_id"].ToString();
                            DbHelperSQL.RunSql(sql);
                            //}
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

                        string smscontxt = Utils.GetMSMEmailContext(16, 1); // 获取注册成功邮件内容

                        StringBuilder sbsms = new StringBuilder(smscontxt);

                        sbsms = sbsms.Replace("#CODE#", vcode);
                        decimal dd = SendSMS.Send_SMS(mobile, sbsms.ToString());
                        if (dd != 0)
                        {
                            json = @" {""rs"": ""n"", ""info"":  ""短信发送异常，请与客报联系""}";
                            return Content(json);
                        }
                        p.phone_number = mobile;
                        p.sendtime = DateTime.Now;
                        p.senduserid = userid;
                        p.smstype = smstype;
                        p.smscontext = sbsms.ToString();
                        p.orderid = dd;
                        p.vcode = vcode;
                        p.ip = Utils.GetRealIP();
                        o.Add(p);
                    }
                    json = @" {""rs"": ""y"", ""info"":  ""短信发送成功! ""}";
                    return Content(json);
                }
            }
            return Content(json);
        }
        /// <summary>
        /// 修改密码验证手机真实姓
        /// </summary>
        /// <returns></returns>
        public ActionResult getmobile()
        {
            string json = "";

            string mobile = Utils.CheckSQLHtml(Request["mobile"].ToString().Trim());
            string ver_code = Utils.CheckSQLHtml(Request["ver_code"].ToString().Trim());

            string sql = "select top 1 smscontext,phone_number from hx_td_SMS_record where ( smstype=8 or  smstype=7)  and phone_number='" + mobile + "' and vcode='" + ver_code + "'  and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            var ts = true;
            if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0)
            {
                ts = false;
            }
            if (dt.Rows.Count > 0 || ts)
            {
                sql = "SELECT registerid,username,mobile from hx_member_table where   mobile='" + mobile + "'";
                DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);

                if (dt1.Rows.Count > 0)
                {
                    string keys = DESEncrypt.Encrypt(dt1.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string keys1 = DESEncrypt.Encrypt(dt1.Rows[0]["mobile"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string keys2 = DESEncrypt.Encrypt(ver_code, ConfigurationManager.AppSettings["webp"].ToString());

                    string type = DESEncrypt.Encrypt("gettel", ConfigurationManager.AppSettings["webp"].ToString());

                    string stf = Utils.GetRe_url("/Signin/ResetPass?key=" + keys + "&t=" + keys1 + "&c=" + keys2 + "&p=" + type);
                    json = @" {""rs"": ""y"", ""info"":  ""正在为您转向密码重置......"", ""url""      :  ""/index.html""}";

                    json = json.Replace("/index.html", stf);
                    return Content(json);

                }
                else
                {

                    json = @" {""rs"": ""n"", ""error"":  ""手机输入不对!""}";
                    return Content(json);
                }



            }
            else
            {
                json = @" {""rs"": ""n"", ""error"":  ""验证码错误!""}";

            }




            return Content(json);
        }




        public ActionResult Getwxmobile()
        {
            string json = "";

            string mobile = Utils.CheckSQLHtml(Request["mobile"].ToString().Trim());
            string ver_code = Utils.CheckSQLHtml(Request["vcodec"].ToString().Trim());
            string Validatecode = Utils.CheckSQLHtml(DNTRequest.GetString("Validatecode").Trim());

            string strIdentify = "LoginValidateCode"; //随机字串存储键值，以便存储到Session中
            string svalidate = Session[strIdentify].ToString();

            string sql = "select top 1 smscontext,phone_number from hx_td_SMS_record where ( smstype=8 or  smstype=7)  and phone_number='" + mobile + "' and vcode='" + ver_code + "'  and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            var ts = true;
            if (!(Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0))
            {
                ts = false;
            }

            if (ts == false && Session[strIdentify] != null && Session[strIdentify].ToString() != Validatecode)
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""验证码不对!""}";
                return Content(json, "text/json");
            }
            if (dt.Rows.Count > 0 && ts == false)
            {
                sql = "SELECT registerid,username,mobile from hx_member_table where   mobile='" + mobile + "'";
                DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);

                if (dt1.Rows.Count > 0)
                {
                    string keys = DESEncrypt.Encrypt(dt1.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string keys1 = DESEncrypt.Encrypt(dt1.Rows[0]["mobile"].ToString(), ConfigurationManager.AppSettings["webp"].ToString());

                    string keys2 = DESEncrypt.Encrypt(ver_code, ConfigurationManager.AppSettings["webp"].ToString());

                    string type = DESEncrypt.Encrypt("gettel", ConfigurationManager.AppSettings["webp"].ToString());

                    string stf = Utils.GetRe_url("/login/ComForgot?key=" + keys + "&t=" + keys1 + "&c=" + keys2 + "&p=" + type);
                    json = @" {""rs"": ""y"", ""info"":  ""正在为您转向密码重置......"", ""url""      :  ""/index.html""}";

                    json = json.Replace("/index.html", stf);
                    return Content(json);

                }
                else
                {

                    json = @" {""rs"": ""n"", ""error"":  ""手机输入不对!""}";
                    return Content(json);
                }



            }
            else
            {
                json = @" {""rs"": ""n"", ""error"":  ""手机验证码错误!""}";

            }





            return Content(json);
        }






        public ActionResult Changepass()
        {
            string json = "";
            string tel1 = Utils.CheckSQLHtml(DNTRequest.GetString("t"));
            string rid1 = Utils.CheckSQLHtml(DNTRequest.GetString("key"));
            string vcode1 = Utils.CheckSQLHtml(DNTRequest.GetString("c"));
            string pcode1 = Utils.CheckSQLHtml(DNTRequest.GetString("p"));


            string rid = "";

            if (rid1 != "")
            {
                rid = DESEncrypt.Decrypt(rid1, ConfigurationManager.AppSettings["webp"].ToString());
            }
            string tel = "";
            if (tel1 != "")
            {
                tel = DESEncrypt.Decrypt(tel1, ConfigurationManager.AppSettings["webp"].ToString());

            }

            string vcode = "";
            if (vcode1 != "")
            {
                vcode = DESEncrypt.Decrypt(vcode1, ConfigurationManager.AppSettings["webp"].ToString());
            }

            string pcode = "";
            if (pcode1 != "")
            {
                pcode = DESEncrypt.Decrypt(pcode1, ConfigurationManager.AppSettings["webp"].ToString());
            }

            string pass = "";
            string sql = "";
            if (pcode == "gettel")
            {
                sql = "select top 1 smscontext,phone_number from hx_td_SMS_record where ( smstype=8 or  smstype=7)  and phone_number='" + tel + "' and vcode='" + vcode + "'  and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";
                DataTable dtc = DbHelperSQL.GET_DataTable_List(sql);

                var ts = true;
                if (Settings.Instance.SiteDomain.IndexOf(PublicURL.NewPCUrl) >= 0)
                {
                    ts = false;
                }
                if (dtc.Rows.Count > 0 || ts)
                {

                    sql = "select registerid,mobile from hx_member_table where registerid=" + rid + " and mobile='" + tel + "' ";
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {

                        pass = DESEncrypt.Encrypt(Utils.CheckSQLHtml(DNTRequest.GetString("userpassword")), ConfigurationManager.AppSettings["webp"].ToString());


                        sql = "update hx_member_table set password='" + pass + "' where registerid=" + rid;

                        if (DbHelperSQL.ExecuteSql(sql) > 0)
                        {

                            DateTime dti = DateTime.Now;

                            M_td_System_message p = new M_td_System_message();
                            p.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                            p.Mstate = 0;
                            p.MTitle = "安全提示";
                            p.MContext = "您在" + dti.ToString("yyyy-MM-dd HH:mm:ss") + "使用手机进行了密码找回操作,如非本人操作请联系客服";
                            p.PubTime = dti;


                            B_usercenter.AddMessage(p);



                            M_td_SMS_record pm = new M_td_SMS_record();
                            B_td_SMS_record om = new B_td_SMS_record();

                            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.修改密码.ToString());

                            string smscontxt = Utils.GetMSMEmailContext(17, 1); // 获取注册成功邮件内容

                            StringBuilder sbsms = new StringBuilder(smscontxt);

                            string mobile = tel;
                            sbsms = sbsms.Replace("#DATATIME#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            pm.phone_number = mobile;
                            pm.sendtime = DateTime.Now;
                            pm.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                            pm.smstype = smstype;
                            pm.smscontext = sbsms.ToString();
                            // p.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());

                            pm.orderid = SendSMS.Send_Audio(mobile, smscontxt);
                            pm.vcode = "";

                            om.Add(pm);


                            json = @" {""rs"": ""y"", ""info"":  ""新密码设置成功!"",""url"":""/""}";
                            return Content(json);



                        }
                        else
                        {
                            json = @" {""rs"": ""n"", ""info"":  ""新密码设置失败!""}";
                            return Content(json);

                        }



                    }
                    else
                    {

                        json = @" {""rs"": ""n"", ""info"":  ""数据异常!""}";
                        return Content(json);
                    }
                }
                else
                {

                    json = @" {""rs"": ""n"", ""info"":  ""验证码无效，或已经过期，请重新获取!""}";
                    return Content(json);



                }
            }




            return Content(json);
        }
        public ActionResult RemoveAllCache()
        {

            CacheRemove.ClearAllCache();

            return Content("缓存清除成功");

        }
    }
}