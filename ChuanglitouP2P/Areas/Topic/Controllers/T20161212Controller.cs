using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20161212Controller : Controller
    {
        // GET: Topic/T20161122
        public ActionResult Index()
        {
            ViewBag.StartTime = TActivity_Luck.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.EndTime = TActivity_Luck.endTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            FillDrawPersons();
            return View();
        }

        #region[活动时间]
        DateTime startTime = TActivity_Luck.startTime;
        DateTime endTime = TActivity_Luck.endTime;
        int amount = 1000;  //规则金额
        #endregion
        /// <summary>
        /// 中奖榜单初始化
        /// </summary>
        private void FillDrawPersons()
        {
            string msg = "";
            int state = TActivity_Luck.CheckActivityTime(startTime, endTime, ref msg);
            if (state != 0)
            {
                ViewBag.ltrCanUseTimes = "0";
                ViewBag.ltrLuckCount = "0";
                //Response.Write("<script>alert('" + msg + "');</script>");
                return;
            }

            int luckCount = 0;
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckMan> lucks = bllLuckDraw.GetLuckDrawRecordList(30, "双12抽奖", out luckCount);
            lucks.ForEach(c =>
            {
                c.Mobile = c.Mobile.Substring(0, 3) + "****" + c.Mobile.Substring(c.Mobile.Length - 4, 4);
                //c.UserName = c.UserName.Substring(0, 1) + "*******" + c.UserName.Substring(c.UserName.Length - 1, 1);
                c.AwardName = c.AwardName.Replace("双12抽奖送", ""); //c.AwardName.Length > 6 ? c.AwardName.Substring(c.AwardName.Length - 6, 6) : c.AwardName;
            });
            StringBuilder builder = new StringBuilder();
            builder.Append("<ul id='xstCont'>");
            foreach (M_LuckMan luck in lucks)
            {
                builder.Append(" <li>");
                builder.AppendFormat("<span>恭喜{0}用户</span><span>获得{1}</span>", luck.Mobile, luck.AwardName);
                builder.Append("</li>");
            }
            builder.Append("</ul>");
            ViewBag.ltrLuckMan = builder.ToString();
            ViewBag.ltrLuckCount = luckCount.ToString();

            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + Utils.GetUserIDCookieslocahost().ToString());
            if (M_uid == null)
            {
                ViewBag.ltrCanUseTimes = 0;
            }
            else
            {
                int userID = M_uid.userid;//PC获取登录用户编号
                if (!TActivity_Luck.CheckChannel(userID))
                {
                    string channelType = "";
                    if (TActivity_Luck.CheckIsChannel(userID, ref channelType))
                    {
                        if (channelType == "cps1")
                        {
                            Response.Write("<script>alert('抱歉，您是渠道用户，不可以参加本次活动，再投一笔即可抽奖！');</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('抱歉，您是渠道用户，不可以参加本次活动！');</script>");
                        }
                    }
                }
                ViewBag.ltrCanUseTimes = TActivity_Luck.GetCanUseTimes(userID, startTime, endTime, amount).ToString();
            }
        }


        /// <summary>
        /// 抽奖方法
        /// </summary>
        /// <returns></returns>
        public ActionResult LuckDrawAward()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            AjaxResponseData arData = new AjaxResponseData();
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + Utils.GetUserIDCookieslocahost().ToString());
            if (M_uid == null)
            {
                arData = new AjaxResponseData { code = "1", data = "您还没有登录" };
                return Content(jss.Serialize(arData));
            }
            int userID = Utils.checkloginsession();//获取登录用户编号
            string returnStr = TActivity_Luck.LuckDrawAward(userID, startTime, endTime, amount);
            string[] spl = returnStr.Split(';');
            arData = new AjaxResponseData { code = spl[0], data = spl[1] };
            return Content(jss.Serialize(arData));
        }

        /// <summary>
        /// 返回数据模板
        /// </summary>
        public class AjaxResponseData
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string data { get; set; }
        }
    }
}