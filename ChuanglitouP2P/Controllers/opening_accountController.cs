using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class opening_accountController : Controller
    {

        #region 用户注册，第三方开户
        /// <summary>
        /// 用户注册，第三方开户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: opening_account
        public ActionResult Index(int id)
        {
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            int uid = Utils.checkloginsession();
            p = b.GetModel(uid);
            string UsrId1 = p.username;
            string url = Utils.GetChinapnrUrl();

            M_UserRegister m = new M_UserRegister();
            m.MerId = Utils.GetMerId();
            m.Version = "10";
            m.CmdId = "UserRegister";
            m.MerCustId = Utils.GetMerCustID();
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/Bg_Succ_Registered");
           // m.BgRetUrl = Utils.GetRe_url("22Thirdparty/Bg_Succ_Registered");
           // m.RetUrl = "http://localhost:17745/Register/Succ_Registered";
            m.RetUrl = Utils.GetRe_url("Register/Succ_Registered");
            m.UsrMp = p.mobile;
            // m.UsrEmail = p.email;
            m.UsrId = UsrId1;
            LogInfo.WriteLog("注册开户请求报文:" + FastJSON.toJOSN(m));
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.UsrId);
            chkVal.Append(m.UsrMp);

            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);

            m.ChkValue = sbChkValue.ToString();





            ViewBag.url = url;
            ViewBag.uid = uid;
            return View(m);
        } 
        #endregion
    }
}