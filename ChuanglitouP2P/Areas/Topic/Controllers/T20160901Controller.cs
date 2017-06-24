using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20160901Controller : Controller
    {
        // GET: Topic/T20160901
        public ActionResult Index()
        {
            FillDrawPersons();
            return View();
        }

        /// <summary>
        /// 中奖榜单初始化
        /// </summary>
        private void FillDrawPersons()
        {
            string msg = "";
            int state = CheckActivityTime(ref msg);
            if (state != 0)
            {
                ViewBag.ltrCanUseTimes = "0";
                ViewBag.ltrLuckCount = "0";
                Response.Write("<script>alert('" + msg + "');</script>");
                return;
            }

            int luckCount = 0;
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckMan> lucks = bllLuckDraw.GetLastLuckMan(out luckCount);
            lucks.ForEach(c =>
            {
                c.UserName = c.UserName.Substring(0, 1) + "*******" + c.UserName.Substring(c.UserName.Length - 1, 1);
                c.AwardName = c.AwardName.Replace("9月抽奖送", ""); //c.AwardName.Length > 6 ? c.AwardName.Substring(c.AwardName.Length - 6, 6) : c.AwardName;
            });
            //if (lucks == null || lucks.Count < 20)
            //{
            //    Dictionary<int, string> dicAwards = new Dictionary<int, string>();
            //    dicAwards.Add(1, "50元现金");
            //    dicAwards.Add(2, "50元代金券");
            //    dicAwards.Add(3, "20元代金券");
            //    dicAwards.Add(4, "10元代金券");
            //    dicAwards.Add(5, "1%加息券");
            //    dicAwards.Add(6, "2%加息券");
            //    Random random = new Random();
            //    int lucksCount = lucks.Count;
            //    for (int i = 1; i <= 20 - lucksCount; i++)
            //    {
            //        //Random random = new Random(unchecked((int)DateTime.Now.Ticks) / (20 - i) * i);
            //        lucks.Add(new M_LuckMan { AwardName = dicAwards[random.Next(1, 7)], UserName = random.Next(1, 10) + "*******" + random.Next(1, 10) });
            //        luckCount++;
            //    }
            //}
            StringBuilder builder = new StringBuilder();
            foreach (M_LuckMan luck in lucks)
            {
                builder.Append(" <li>");
                builder.AppendFormat("<p><span>{0}</span>抽中了<strong>{1}</strong></p>", luck.UserName, luck.AwardName);
                builder.Append("</li>");
            }
            ViewBag.ltrLuckMan = builder.ToString();
            ViewBag.ltrLuckCount = luckCount.ToString();
            ViewBag.ltrCanUseTimes = GetCanUseTimes().ToString();
        }

        /// <summary>
        /// 检查是否在活动时间内
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private int CheckActivityTime(ref string msg)
        {
            DateTime startTime = Convert.ToDateTime("2016-09-03");
            DateTime endTime = Convert.ToDateTime("2016-10-01");
            if (DateTime.Now < startTime)
            {
                msg = "活动未开始，敬请关注！";
                return -1;
            }
            if (DateTime.Now >= endTime)
            {
                msg = "活动已结束，请关注其他活动！";
                return -2;
            }
            return 0;
        }

        /// <summary>
        /// 获取可用的抽奖次数
        /// </summary>
        /// <returns></returns>
        public int GetCanUseTimes()
        {
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + Utils.GetUserIDCookieslocahost().ToString());
            if (M_uid == null)
            {
                return 0;
            }
            //获取登录用户编号
            int userID = Utils.checkloginsession();
            //可抽奖次数
            int CanUseTimes = 1;
            B_member_table bllMember = new B_member_table();
            //获取会员信息
            PartialMemberModel member = bllMember.GetPartialModel(userID);
            //验证用户是否为今天新注册并认证的用户
            if (member.registration_time >= DateTime.Now.Date && member.registration_time < DateTime.Now.AddDays(1).Date && member.isrealname == 1)
            {
                CanUseTimes += 1;
            }

            //获取用户的抽奖记录（当天的）
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            int recordsCount = bllLuckDraw.GetRecordsCount(userID, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
            CanUseTimes = CanUseTimes >= recordsCount ? CanUseTimes - recordsCount : 0;
            return CanUseTimes;
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
        /// <summary>
        /// 抽奖方法
        /// </summary>
        /// <returns></returns>
        public ActionResult LuckDrawAward()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            AjaxResponseData arData = new AjaxResponseData();
            try
            {
                string msg = "";
                int state = CheckActivityTime(ref msg);
                if (state != 0)
                {
                    arData = new AjaxResponseData { code = "4", data = msg };
                    return Content(jss.Serialize(arData));
                }
                M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + Utils.GetUserIDCookieslocahost().ToString());
                if (M_uid == null)
                {
                    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                    return Content(jss.Serialize(arData));
                }
                if (M_uid.codeno != Utils.getSessioncode())
                {
                    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                    return Content(jss.Serialize(arData));
                }
                //获取登录用户编号
                int userID = M_uid.userid;
                //可抽奖次数
                int CanUseTimes = 1;
                B_member_table bllMember = new B_member_table();
                //获取会员信息
                PartialMemberModel member = bllMember.GetPartialModel(userID);
                //验证用户是否为今天新注册的用户
                if (member.registration_time >= DateTime.Now.Date && member.registration_time < DateTime.Now.AddDays(1).Date && member.isrealname == 1)
                {
                    CanUseTimes += 1;
                }
                B_LuckDraw bllLuckDraw = new B_LuckDraw();
                //获取用户的抽奖记录数量（当天的）
                int recordsCount = bllLuckDraw.GetRecordsCount(userID, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                CanUseTimes -= recordsCount;

                if (CanUseTimes <= 0)
                {
                    arData = new AjaxResponseData { code = "2", data = "抽奖机会已用完，请明天再来" };
                    return Content(jss.Serialize(arData));
                }
                #region 规则及数据
                // 现金50元   50  0.17 %(60次抽满后，每天限中2次，概率填充入50元代金券)  
                //1 -17
                //50元代金券  2450    23.33 %
                //18-2350
                //20元代金卷  2100    20.00 %
                //2351-4350
                //10元代金卷  2100    20.00 %
                //4351-6350
                //1 % 加息券   1750    16.67 %
                //6351-8017
                //2 % 加息券   1400    13.33 %
                //8018-9350
                //谢谢参与    682.5   6.50 %
                //9351-10000

                //47    9月注册送500元奖励
                //48    9月注册送200元奖励
                //49    9月注册送50元奖励
                //50    9月注册送20元奖励
                //51    [9月注册送10元奖励]
                //52    [9月注册送8元奖励]

                //1   3 % 加息券
                //2   1 % 加息券
                //3   2 % 加息券
                //4   加息券
                #endregion
                //现金50元已抽中的个数
                int cash50Count = bllLuckDraw.GetCash50RecordsCount(-2, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                //B_Activity_schedule bllASchedule = new B_Activity_schedule();
                ActFacade act = new ActFacade();
                //B_Activity bllActivity = new B_Activity();
                //admin.users.AddBonusForUser adfu = new admin.users.AddBonusForUser();
                //奖品编号
                int awardID = 0;
                //奖品标题
                string title = "";
                //奖品类型
                int awardType = -1;
                //轮盘区块编号
                int awardBlockID = 0;
                Random random = new Random();
                //随机数
                int randomRate = random.Next(1, 10001);
                //randomRate = 9350;
                //switch (randomRate)
                //{
                if (randomRate <= 8)
                {
                    //每天限中2次，概率填充入50元代金券
                    if (cash50Count < 2)
                    {
                        awardID = ConfigHelper.GetConfigInt("AwardCash50");
                        awardType = 0;
                        title = "现金50元";
                        awardBlockID = 3;
                    }
                    else
                    {
                        awardID = ConfigHelper.GetConfigInt("AwardBonus50");
                        awardType = 1;
                        awardBlockID = 7;
                    }
                }
                //现金50元
                else if (randomRate <= 17)
                {
                    //每天限中2次，概率填充入50元代金券
                    if (cash50Count < 2)
                    {
                        awardID = ConfigHelper.GetConfigInt("AwardCash50");
                        awardType = 0;
                        title = "现金50元";
                        awardBlockID = 5;
                    }
                    else
                    {
                        awardID = ConfigHelper.GetConfigInt("AwardBonus50");
                        awardType = 1;
                        awardBlockID = 7;
                    }
                }
                //50元代金券
                else if (randomRate <= 2350)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardBonus50"); awardType = 1; awardBlockID = 7;
                }
                //20元代金卷
                else if (randomRate <= 4350)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; awardBlockID = 4;
                }
                //10元代金卷
                else if (randomRate <= 6350)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardBonus10"); awardType = 1; awardBlockID = 6;
                }
                //1 % 加息券
                else if (randomRate <= 8017)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardRate1"); awardType = 2; awardBlockID = 0;
                }
                //2 % 加息券
                else if (randomRate <= 9350)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardRate2"); awardType = 2; awardBlockID = 2;
                }
                //谢谢参与
                else if (randomRate <= 10000)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardThankYou"); awardType = 3; title = "谢谢参与"; awardBlockID = 1;
                }
                //谢谢参与
                if (awardType == -1)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardThankYou"); awardType = 3; title = "谢谢参与"; awardBlockID = 1;
                }
                //}
                //发放代金券
                if (awardType == 1 || awardType == 2)
                {
                    var sourceData = act.GetActivityModel(awardID); //bllASchedule.GetModel(awardID);
                    title = sourceData.ActName; //sourceData.activity_schedule_name.Replace("活动", "抽奖");
                    if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.ActName, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                    {
                        DrawBonus(sourceData, userID);
                        arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                        return Content(jss.Serialize(arData));
                    }
                    else
                    {
                        arData = new AjaxResponseData { code = "3", data = "碰到点小问题，刷新一下试试^v^" };
                        return Content(jss.Serialize(arData));
                    }
                }
                ////发放加息券
                //if (awardType == 2)
                //{
                //    var sourceData = bllActivity.GetModel(awardID);
                //    title = "9月抽奖" + sourceData.ActivityName;
                //    if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.ActivityName, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                //    {
                //        thisObj.SubmitRate(awardID, 1, userID, DateTime.Now, sourceData.EndDate, title);
                //        arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                //        return jss.Serialize(arData);
                //    }
                //}
                //增加现金和“谢谢参与”的抽奖记录
                if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = title, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                {
                    arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                    return Content(jss.Serialize(arData));
                }
                //没有增加任何抽奖记录时的异常状态
                arData = new AjaxResponseData { code = "3", data = "碰到点小问题，刷新一下试试" };
                return Content(jss.Serialize(arData));
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("9月抽奖活动异常日志:" + "msg：" + ex.Message + "   StackTrace" + ex.StackTrace);

                //没有增加任何抽奖记录时的异常状态
                arData = new AjaxResponseData { code = "3", data = ex.Message };
                return Content(jss.Serialize(arData));
            }
        }
        /// <summary>
        /// 发放奖励
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="registerID"></param>
        private void DrawBonus(hx_ActivityTable activity, int registerID)
        {
            chuangtouEntities ef = new chuangtouEntities();

            B_bonus_account bb = new B_bonus_account();
            M_bonus_account mb = new M_bonus_account();

            M_bonus_account_water mbaw = new M_bonus_account_water();
            B_bonus_account_water bbaw = new B_bonus_account_water();

            DateTime dte = DateTime.Now;//当前时间截点
            //hx_ActivityTable act = ef.hx_ActivityTable.Where(c => c.ActID == activity.ActID).FirstOrDefault();//添加奖励类型
            if (activity.ActEndtime >= dte && activity.ActStarttime <= dte)
            {
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(activity.ActRule);
                hx_UserAct hua = new hx_UserAct();
                hua.ActTypeId = activity.ActTypeId;
                hua.registerid = registerID;
                hua.RewTypeID = activity.RewTypeID;
                hua.ActID = activity.ActID;
                hua.Amt = mcp.cash;
                hua.Uselower = mcp.Msplitarr == null || mcp.Msplitarr.Count <= 0 ? 0 : mcp.Msplitarr[0].startAmt;
                hua.Usehight = mcp.Msplitarr == null || mcp.Msplitarr.Count <= 0 ? 0 : mcp.Msplitarr[0].endAmt;
                hua.AmtEndtime = activity.ActEndtime;
                hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                hua.UseState = 0;  //现金未转账
                hua.AmtProid = 0; //未使用默认为0
                hua.ISSmsOne = 0;
                hua.IsSmsThree = 0;
                hua.isSmsFifteen = 0;
                hua.IsSmsSeven = 0;
                hua.isSmsSixteen = 0;
                hua.Createtime = DateTime.Now;

                ef.hx_UserAct.Add(hua);
                int i = ef.SaveChanges();

                mb.activity_schedule_id = activity.ActID;
                mb.membertable_registerid = registerID;
                mb.activity_schedule_name = activity.ActName;
                mb.amount_of_reward = decimal.Parse(hua.Amt.ToString());

                mb.use_lower_limit = Convert.ToDecimal(hua.Uselower);
                mb.reward = 0;
                mb.start_date = Convert.ToDateTime(hua.Createtime);
                mb.end_date = Convert.ToDateTime(hua.AmtEndtime);
                mb.entry_time = dte;
                int bbid = bb.Add(mb);
                if (bbid > 0) //奖励记录成功后插入明细记录
                {
                    mbaw.bonus_account_id = bbid;
                    mbaw.membertable_registerid = registerID;
                    mbaw.income = decimal.Parse(hua.Amt.ToString());
                    mbaw.expenditure = 0.00M;
                    mbaw.time_of_occurrence = DateTime.Now;
                    mbaw.award_description = activity.ActName;
                    mbaw.water_type = 0;

                    bbaw.Add(mbaw);
                }

            }
        }

        //public void SubmitBonus(int ActivityId, int k, int uid, DateTime starTime, DateTime endTime, string title)
        //{
        //    M_Activity_schedule ma = new M_Activity_schedule();
        //    B_Activity_schedule ba = new B_Activity_schedule();

        //    B_bonus_account bb = new B_bonus_account();
        //    M_bonus_account mb = new M_bonus_account();

        //    M_bonus_account_water mbaw = new M_bonus_account_water();
        //    B_bonus_account_water bbaw = new B_bonus_account_water();

        //    DateTime dte = DateTime.Now;//当前时间截点
        //    ma = ba.GetModel(ActivityId);//添加奖励类型
        //    if (ma.end_date >= dte && ma.start_date <= dte)
        //    {
        //        for (int i = 0; i < k; i++) // i为添加奖励数
        //        {
        //            mb.activity_schedule_id = ma.activity_schedule_id;
        //            mb.membertable_registerid = uid;
        //            mb.activity_schedule_name = title;
        //            mb.amount_of_reward = ma.amount_of_reward;

        //            mb.use_lower_limit = ma.use_lower_limit;
        //            mb.reward = ma.reward;
        //            mb.start_date = starTime;
        //            mb.end_date = endTime;
        //            mb.entry_time = dte;
        //            int bbid = bb.Add(mb);
        //            if (bbid > 0) //奖励记录成功后插入明细记录
        //            {
        //                mbaw.bonus_account_id = bbid;
        //                mbaw.membertable_registerid = mb.membertable_registerid;
        //                mbaw.income = mb.amount_of_reward;
        //                mbaw.expenditure = 0.00M;
        //                mbaw.time_of_occurrence = mb.entry_time;
        //                // mbaw.
        //                mbaw.award_description = title;
        //                mbaw.water_type = 0;
        //                bbaw.Add(mbaw);
        //            }
        //        }
        //    }
        //}
        //public void SubmitCash(int ActTypeId, int Registerid, int RewTypeID, int ActID, decimal actamt, DateTime ActEndtime)
        //{
        //    try
        //    {
        //        chuangtouEntities ef = new chuangtouEntities();
        //        hx_UserAct hua = new hx_UserAct();

        //        hua.ActTypeId = ActTypeId;
        //        hua.registerid = Registerid;
        //        hua.RewTypeID = RewTypeID;
        //        hua.ActID = ActID;
        //        hua.Amt = actamt;
        //        hua.Uselower = 0.00M;
        //        hua.Usehight = 0.00M;
        //        hua.UseState = 5;  //现金未转账
        //        hua.AmtEndtime = ActEndtime;
        //        hua.OrderID = decimal.Parse(Utils.Createcode());

        //        hua.AmtUses = 1; //没指定情况下默认为单独使用

        //        hua.AmtProid = 0; //未使用默认为0
        //        hua.ISSmsOne = 0;
        //        hua.IsSmsThree = 0;
        //        hua.isSmsFifteen = 0;
        //        hua.IsSmsSeven = 0;
        //        hua.isSmsSixteen = 0;

        //        hua.Createtime = DateTime.Now;
        //        ef.hx_UserAct.Add(hua);
        //        int i = ef.SaveChanges();
        //        if (i > 0)
        //        {
        //            LogInfo.WriteLog("九月转盘抽奖活动，奖励发放成功");
        //        }
        //        else
        //        {
        //            LogInfo.WriteLog("九月转盘抽奖活动，奖励发放失败");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogInfo.WriteLog("九月转盘抽奖活动，程序异常，StackTrace消息：" + ex.StackTrace + ",Message：" + ex.Message);
        //    }
        //}

        //public void SubmitRate(int rateId, int k, int uid, DateTime starTime, DateTime endTime, string title)
        //{

        //    B_ActivityLogs activityLogsLogic = new B_ActivityLogs();

        //    B_Activity bllActivity = new B_Activity();
        //    var sourceData = bllActivity.GetModel(rateId);

        //    #region 循环活动发放的数量
        //    for (int i = 0; i < Convert.ToInt32(k); i++)
        //    {

        //        ActivityLogs al = new ActivityLogs();
        //        al.ActivityName = tixtle;
        //        al.ActType = 3;
        //        al.ActivityId = rateId;
        //        al.UseBeginOn = starTime.ToString();
        //        al.UseEndOn = endTime.ToString();
        //        al.AddRate = sourceData.AddRate;// decimal.Parse("3.0");
        //        al.UserId = uid;
        //        al.UseStatus = 0;
        //        al.UsedRecordId = 0;
        //        al.UsedTargetId = 0;
        //        al.RecordId = 0;
        //        al.TargetId = 0;
        //        al.CreateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        activityLogsLogic.Add(al);
        //    }
        //    #endregion
        //}
    }
}