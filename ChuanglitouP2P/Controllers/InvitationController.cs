using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using System.Data;
using ChuanglitouP2P.DBUtility;
using System.Configuration;

namespace ChuanglitouP2P.Controllers
{
    public class InvitationController : Controller
    {
        // GET: Invitation
        public ActionResult Index(string id)
        {
            //string InvCode = DNTRequest.GetString("id");


            string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + id + "' ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                HttpCookie cok = new HttpCookie("Invitation");
                cok.Values.Add("InvCode", DESEncrypt.Encrypt(id, ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cok);
               // Redirect("/");
            }

            ViewBag.invitedcode = id;

            //ViewBag.InvCode = id;

            //return Redirect("/");
            return View();
        }
    }
}