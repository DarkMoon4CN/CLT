using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20160819Controller : Controller
    {
        // GET: Topic/T20160819
        public ActionResult Index()
        {
            string rndstr = Utils.RndNumChar(10).ToString();
            var code = Utils.CheckSQLHtml(DNTRequest.GetString("code"));

            string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + code + "' ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                // Session["invitedcode"] = code;
                var ckes = DateTime.Now.AddDays(1);
                HttpCookie cok = new HttpCookie("Invitation");

                //if (cok != null)
                //{
                //    TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                //    cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在                           
                //    Response.AppendCookie(cok);
                //}

                cok.Domain = PublicURL.NewUrl;
                cok.Values.Add("InvCode", DESEncrypt.Encrypt(code, ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Expires = ckes;
                Response.AppendCookie(cok);
            }
            ViewBag.rndstr = rndstr;
            return View();
        }
    }
}