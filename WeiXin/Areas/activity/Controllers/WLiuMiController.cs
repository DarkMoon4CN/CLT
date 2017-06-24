using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeiXin.Controllers;

namespace WeiXin.Areas.activity.Controllers
{
    public class WLiuMiController : Controller
    {
      
        // GET: activity/WLiuMi
        public ActionResult Index(string code = "")
        {
            code = Utils.CheckSQLHtml(code);

            string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + code + "' ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                // Session["invitedcode"] = code;
                var ckes = DateTime.Now.AddDays(1);
                HttpCookie cok = new HttpCookie("Invitation");
                #region modified by fangjianmin 该段代码可能导致客户端浏览器清理cookie
                //if (cok != null)
                //{
                //    TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                //    cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在                           
                //    Response.AppendCookie(cok);
                //}
                #endregion
                cok.Domain = PublicURL.NewUrl;
                cok.Values.Add("InvCode", DESEncrypt.Encrypt(code, ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Expires = ckes;
                Response.AppendCookie(cok);
            }

            return View();
        }


        public ActionResult Receive()
        {
            //保存Code信息

            string CookCode = Utils.GetCookie("CookCode");
            if (string.IsNullOrWhiteSpace(CookCode))
            {
                var cookie = new HttpCookie("CookCode");//保存至Cookie
                cookie.Value = "liumi";
                cookie.Expires = DateTime.Now.Date.AddDays(1);
                Response.Cookies.Add(cookie);

            }

            return View();
        }

        public ActionResult IsLogon()
        {
            string responseJson = "";
            string LoginAuth = Utils.GetCookie("LoginAuth");
            if (!string.IsNullOrWhiteSpace(LoginAuth))
            {
                responseJson = @" {""rs"": ""y"", ""info"":  ""登录成功""}";
            }
            else
            {
                responseJson = @" {""rs"": ""n"", ""info"":  ""您尚未登录""}";
            }
            return Content(responseJson);
        }
    }
}