using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace WeiXin.Controllers
{
    /// <summary>
    /// 邀请相关
    /// </summary>
    public class InvitationController : BaseController
    {

        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 邀请奖励
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {


            return View();
        }

        /// <summary>
        /// 邀请列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int pageIndex = 1)
        {
            int userid = ChuanglitouP2P.Common.Settings.Instance.CurrentUserId;
            string invitedcode = string.Empty;
            if (userid > 0)
            {
                string sql = "select registerid,invitedcode from hx_member_table where registerid='" + userid + "' ";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                invitedcode += "register/index?invitedcode=" + dt.Rows[0]["invitedcode"].ToString();
            }
            string shareUrl = Utils.GetAppSetting("MReleaseURL") + invitedcode;

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
            tx.desc = "来创利投金服，新手理财送好礼，现金红包送不停";
            tx.link = shareUrl;
           
            #endregion
            // var list = ef.V_YaoQinList.Where(p => p.membertable_registerid == userid).OrderByDescending(p => p.Createtime).ToPagedList(pageIndex ?? 1, pgaesize);
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.V_YaoQinList.Where(p => p.membertable_registerid == userid).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.V_YaoQinList.Where(p => p.membertable_registerid == userid).OrderByDescending(a => a.Createtime).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            ViewBag.TXShareHelper = tx;
            return View(list);
        }
    }
}