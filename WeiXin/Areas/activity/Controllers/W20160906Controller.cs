using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WeiXin.Areas.activity.Controllers
{
    public class W20160906Controller : Controller
    {
        // GET: activity/W20160906
        public ActionResult Index(string code = "")
        {
            string cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channel"));
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue);
                Utils.SetInvCookie("Invitation", keyValue, -1);
            }
            else
            {
                var invitedcode = Utils.CheckSQLHtml(code);
                if (!string.IsNullOrWhiteSpace(invitedcode))
                {
                    string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + invitedcode + "' ";
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {
                        HttpCookie cok = new HttpCookie("Invitation");
                        cok.Domain = PublicURL.NewUrl;
                        cok.Values.Add("InvCode", DESEncrypt.Encrypt(invitedcode, ConfigurationManager.AppSettings["webp"].ToString()));
                        cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                        cok.Expires = DateTime.Now.AddDays(1);
                        Response.AppendCookie(cok);
                    }
                }
            }
            ViewBag.BorrowingTargetData = GetBorrowingTarget();
            return View();
        }

        private string GetBorrowingTarget()
        {
            B_borrowing_target bllBorrowingTarget = new B_borrowing_target();
            M_borrowTargetZhuolu target = bllBorrowingTarget.GetModelByParams(2);
            if (target == null || target.targetid == 0)
            {
                target = bllBorrowingTarget.GetModelByParams(-1);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"zc_n_03\">");
            builder.Append("<div class=\"zc_n_03_main\">");
            builder.AppendFormat("<h2>{0}</h2>", target.project_type_name);
            builder.AppendFormat("<p style=\"position: absolute;left: 41%;top: 19%;font-size: 0.6rem;color: #a6a6a6;\">历史平均年化利率</p>");
            builder.Append("<p class=\"zc_n_03_main_p1\">"+ target.annual_interest_rate.ToString("0.0") + "<small>%</small></p>");
            //if (target.life_of_loan >=3 && target.life_of_loan <6 &&target.unit_day==1)
            //{
            //    builder.Append("<a><sub>+1%返现</sub></a>");
            //}
            //else if(target.life_of_loan >= 6 && target.unit_day == 1)
            //{
            //    builder.Append("<a><sub>+2%返现</sub></a>");
            //}
            //else
            //{
            //    builder.Append("");
            //}
            
            builder.Append("<p class=\"zc_n_03_main_p2\">期限<span>" + (target.unit_day == 1 ? target.life_of_loan + "个月" : target.life_of_loan + "天") + "</span> 起投金额&nbsp&nbsp100元</p>");
            builder.Append("</div>");
            builder.AppendFormat("<a href=\"{0}\" class=\"zc_n_03_btn\"></a>", "/home/ProjectDetail?targetid=" + target.targetid + "");
            builder.Append("</div>");
            return builder.ToString();
        }
    }
}