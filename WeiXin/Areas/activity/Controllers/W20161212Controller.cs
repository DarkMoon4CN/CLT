using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeiXin.Controllers;

namespace WeiXin.Areas.activity.Controllers
{
    public class W20161212Controller : Controller
    {
        public ActionResult Index()
        {
            ViewBag.StartTime = TActivity_Luck.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.EndTime = TActivity_Luck.endTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (CheckAppLogin())
            {
                ViewBag.ApploginState = 1;
            }
            else
            {
                ViewBag.ApploginState = 0;
            }
            FillDrawPersons();
            return View();
        }

        //private void CheckAppLogin()
        //{
        //    string app = DNTRequest.GetString("app");
        //    if (app.ToLower() == "clt")
        //    {
        //        string p = DNTRequest.GetString("p");
        //        string userid = DNTRequest.GetString("userid");
        //        string key = DNTRequest.GetString("key");
        //        TActivity_Luck.loginIn login = new loginController().LoginIN;
        //        bool res = TActivity_Luck.AppLoginSite(userid, p, key, login);
        //    }
        //}

        private bool CheckAppLogin()
        {
            bool res = false;
            string app = DNTRequest.GetString("app");
            if (app.ToLower() == "clt")
            {
                string p = DNTRequest.GetString("p");
                string userid = DNTRequest.GetString("userid");
                if (userid != "0")
                {
                    string key = DNTRequest.GetString("key");
                    TActivity_Luck.loginIn login = new loginController().LoginIN;
                    res = TActivity_Luck.AppLoginSite(userid, p, key, login);
                }
            }
            return res;
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
            string app = DNTRequest.GetString("app");
            string msg = "";
            int state = TActivity_Luck.CheckActivityTime(startTime, endTime, ref msg);
            if (state != 0)
            {
                ViewBag.ltrCanUseTimes = "0";
                ViewBag.ltrLuckCount = "0";
                return;
            }

            int luckCount = 0;
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckMan> lucks = bllLuckDraw.GetLuckDrawRecordList(30, "双12抽奖", out luckCount);
            lucks.ForEach(c =>
            {
                c.Mobile = c.Mobile.Substring(0, 3) + "****" + c.Mobile.Substring(c.Mobile.Length - 4, 4);
                c.AwardName = c.AwardName.Replace("双12抽奖送", "");
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
            int userID = 0;
            if (app.ToLower() == "clt")
            {
                string uid = DNTRequest.GetString("userid");
                userID = int.Parse(string.IsNullOrWhiteSpace(uid) ? "0" : uid);
            }
            else
            {
                userID = Settings.Instance.CurrentUserId;//获取登录用户编号
            }

            if (userID <= 0)
            {
                ViewBag.ltrCanUseTimes = 0;
            }
            else
            {
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
            string app = DNTRequest.GetString("app");
            int userID = 0;
            if (app.ToLower() == "clt")
            {
                string uid = DNTRequest.GetString("userid");
                userID = int.Parse(string.IsNullOrWhiteSpace(uid) ? "0" : uid);
            }
            else
            {
                userID = Settings.Instance.CurrentUserId;//获取登录用户编号
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            AjaxResponseData arData = new AjaxResponseData();
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