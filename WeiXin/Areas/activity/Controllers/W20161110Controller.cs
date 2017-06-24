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
    public class W20161110Controller : Controller
    {
        // GET: activity/W20161110
        public ActionResult Index()
        {
            int userid = ChuanglitouP2P.Common.Settings.Instance.CurrentUserId;
            string yqUrl = userid <= 0 ? "/login.html" : "/Invitation/List";
            string tzUrl = userid <= 0 ? "/login.html" : "/index.html";
            string invitedcode = string.Empty;
            if (userid > 0)
            {
                string sql = "select registerid,invitedcode from hx_member_table where registerid='" + userid + "' ";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                invitedcode += "register/index?invitedcode="+dt.Rows[0]["invitedcode"].ToString();
            }
            string shareUrl = Utils.GetAppSetting("MReleaseURL")+ invitedcode;

            TXShareHelper tx = new TXShareHelper();
            #region TXShareHelper 赋值逻辑
            tx.link = Request.Url.AbsoluteUri.ToString().Trim();
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
            return View();
        }
    }
}