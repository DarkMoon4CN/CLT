using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WeiXin.Areas.activity.Controllers
{
    public class W20170106Controller : Controller
    {
        // GET: activity/W20170106
        public ActionResult Index()
        {
            int userid = ChuanglitouP2P.Common.Settings.Instance.CurrentUserId;
            string yqUrl = userid <= 0 ? "/login.html" : "/Invitation/List";
            string tzUrl = userid <= 0 ? "/login.html" : "/index.html";
            string invitedcode = "";
            if (userid > 0)
            {
                string sql = "select registerid,invitedcode from hx_member_table where registerid='" + userid + "' ";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                invitedcode += dt.Rows[0]["invitedcode"].ToString();
            }
            string shareUrl = (invitedcode == "" ? "http://www.chuanglitou.cn/" : "http://www.chuanglitou.cn/Invitation/" + invitedcode + ".html");// Utils.GetAppSetting("MReleaseURL") + invitedcode;

            TXShareHelper tx = new TXShareHelper();
            #region TXShareHelper 赋值逻辑
            tx.link = shareUrl;// Request.Url.AbsoluteUri.ToString().Trim();
            tx.CheckSignature(tx.link);
            tx.appid = Utils.GetAppSetting("WeiXinAppid");
            tx.title = "邀请好友来投资,双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！！";
            if (Utils.GetAppSetting("DeBug") == "1")
            {
                tx.imgUrl = Utils.GetAppSetting("MDeBugURL") + "Images/yhynhl.jpg";
            }
            else
            {
                tx.imgUrl = Utils.GetAppSetting("MReleaseURL") + "Images/yhynhl.jpg";
            }
            tx.desc = "邀请好友来投资,双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！！";
            tx.link = shareUrl;
            #endregion
            ViewBag.TXShareHelper = tx;
            ViewBag.YqUrl = yqUrl;
            ViewBag.TzUrl = tzUrl;



            string codesql = "select TOP 5 (SELECT  mobile from hx_member_table where registerid = Invpeopleid) mobile,count(invcode) invcode from hx_td_Userinvitation where invtime >= '2017/1/6 00:00:00' AND invtime <= '2017/3/31 23:59:59' GROUP BY Invpeopleid ORDER BY  invcode desc";//查询累计邀请人数排名

            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);

            if (dtcode.Rows.Count > 0)
            {
                ViewBag.RenShu = dtcode;
            }
            else
            {
                ViewBag.RenShu = null;
            }

            string codesql2 = "SELECT TOP 5 (SELECT  t.mobile from hx_member_table t where t.registerid=a.registerid) mobile , SUM(Amt)amt FROM hx_UserAct a where RewTypeID = 1 AND Createtime >= '2017/1/6 00:00:00' AND Createtime <= '2017/3/31 23:59:59' AND  registerid IN(SELECT distinct Invpeopleid from hx_td_Userinvitation where invtime >= '2017/1/6 00:00:00' AND invtime <= '2017/3/31 23:59:59') GROUP BY registerid ORDER BY amt desc";//查询累计邀请返现排名

            DataTable dtcode2 = DbHelperSQL.GET_DataTable_List(codesql2);

            if (dtcode2.Rows.Count > 0)
            {
                ViewBag.JinE = dtcode2;
            }
            else
            {
                ViewBag.JinE = null;
            }
            return View();
        }
    }
}