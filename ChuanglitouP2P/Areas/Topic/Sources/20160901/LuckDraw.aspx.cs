using ChuanglitouP2P.Bll;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChuanglitouP2P.topic._20160901
{
    /// <summary>
    /// 异步调用返回数据模板
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
    public partial class LuckDraw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDrawPersons();
            }
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
                ltrCanUseTimes.Text = "0";
                ltrLuckCount.Text = "0";
                Response.Write("<script>alert('" + msg + "');</script>");
                return;
            }

            int luckCount = 0;
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckMan> lucks = bllLuckDraw.GetLastLuckMan(out luckCount);
            lucks.ForEach(c =>
            {
                c.UserName = c.UserName.Substring(0, 1) + "*******" + c.UserName.Substring(c.UserName.Length - 1, 1);
                c.AwardName = c.AwardName.Length > 5 ? c.AwardName.Substring(c.AwardName.Length - 5, 5) : c.AwardName;
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
            ltrLuckMan.Text = builder.ToString();
            ltrLuckCount.Text = luckCount.ToString();
            ltrCanUseTimes.Text = GetCanUseTimes().ToString();
        }
        /// <summary>
        /// 获取可用的抽奖次数
        /// </summary>
        /// <returns></returns>
        public int GetCanUseTimes()
        {
            M_login M_uid = (M_login)DataCache.GetCache(Utils.GetUserIDCookieslocahost().ToString());
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
        /// 抽奖方法
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string LuckDrawAward()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            AjaxResponseData arData = new AjaxResponseData();
            try
            {
                string msg = "";
                int state = new LuckDraw().CheckActivityTime(ref msg);
                if (state != 0)
                {
                    arData = new AjaxResponseData { code = "4", data = msg };
                    return jss.Serialize(arData);
                }
                M_login M_uid = (M_login)DataCache.GetCache(Utils.GetUserIDCookieslocahost().ToString());
                if (M_uid == null)
                {
                    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                    return jss.Serialize(arData);
                }
                if (M_uid.codeno != Utils.getSessioncode())
                {
                    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                    return jss.Serialize(arData);
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
                    return jss.Serialize(arData);
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
                B_Activity_schedule bllASchedule = new B_Activity_schedule();
                B_Activity bllActivity = new B_Activity();
                admin.users.AddBonusForUser adfu = new admin.users.AddBonusForUser();
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
                if (awardType == 1)
                {
                    var sourceData = bllASchedule.GetModel(awardID);
                    title = sourceData.activity_schedule_name.Replace("活动", "抽奖");
                    if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.activity_schedule_name, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                    {
                        adfu.SubmitBonus(awardID, 1, userID, DateTime.Now, sourceData.end_date, title);
                        arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                        return jss.Serialize(arData);
                    }
                }
                //发放加息券
                if (awardType == 2)
                {
                    var sourceData = bllActivity.GetModel(awardID);
                    title = "9月抽奖" + sourceData.ActivityName;
                    if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.ActivityName, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                    {
                        adfu.SubmitRate(awardID, 1, userID, DateTime.Now, sourceData.EndDate, title);
                        arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                        return jss.Serialize(arData);
                    }
                }
                //增加现金和“谢谢参与”的抽奖记录
                if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = title, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
                {
                    arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
                    return jss.Serialize(arData);
                }
                //没有增加任何抽奖记录时的异常状态
                arData = new AjaxResponseData { code = "3", data = "碰到点小问题，刷新一下试试" };
                return jss.Serialize(arData);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("9月抽奖活动异常日志:" + "msg：" + ex.Message + "   StackTrace" + ex.StackTrace);

                //没有增加任何抽奖记录时的异常状态
                arData = new AjaxResponseData { code = "3", data = ex.Message };
                return jss.Serialize(arData);
            }
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
        //[WebMethod]
        //public static string Test50Cash()
        //{
        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    AjaxResponseData arData = new AjaxResponseData();
        //    try
        //    {
        //        string msg = "";
        //        int state = new LuckDraw().CheckActivityTime(ref msg);
        //        if (state != 0)
        //        {
        //            arData = new AjaxResponseData { code = "4", data = msg };
        //            return jss.Serialize(arData);
        //        }
        //        M_login M_uid = (M_login)DataCache.GetCache(Utils.GetUserIDCookieslocahost().ToString());
        //        if (M_uid == null)
        //        {
        //            arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
        //            return jss.Serialize(arData);
        //        }
        //        if (M_uid.codeno != Utils.getSessioncode())
        //        {
        //            arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
        //            return jss.Serialize(arData);
        //        }
        //        //获取登录用户编号
        //        int userID = M_uid.userid;
        //        //可抽奖次数
        //        int CanUseTimes = 1;
        //        B_member_table bllMember = new B_member_table();
        //        //获取会员信息
        //        PartialMemberModel member = bllMember.GetPartialModel(userID);
        //        //验证用户是否为今天新注册的用户
        //        if (member.registration_time >= DateTime.Now.Date && member.registration_time < DateTime.Now.AddDays(1).Date && member.isrealname == 1)
        //        {
        //            CanUseTimes += 1;
        //        }
        //        B_LuckDraw bllLuckDraw = new B_LuckDraw();
        //        //获取用户的抽奖记录数量（当天的）
        //        int recordsCount = bllLuckDraw.GetRecordsCount(userID, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
        //        CanUseTimes -= recordsCount;

        //        if (CanUseTimes <= 0)
        //        {
        //            arData = new AjaxResponseData { code = "2", data = "抽奖机会已用完，请明天再来" };
        //            return jss.Serialize(arData);
        //        }
        //        #region 规则及数据
        //        // 现金50元   50  0.17 %(60次抽满后，每天限中2次，概率填充入50元代金券)  
        //        //1 -17
        //        //50元代金券  2450    23.33 %
        //        //18-2350
        //        //20元代金卷  2100    20.00 %
        //        //2351-4350
        //        //10元代金卷  2100    20.00 %
        //        //4351-6350
        //        //1 % 加息券   1750    16.67 %
        //        //6351-8017
        //        //2 % 加息券   1400    13.33 %
        //        //8018-9350
        //        //谢谢参与    682.5   6.50 %
        //        //9351-10000

        //        //47    9月注册送500元奖励
        //        //48    9月注册送200元奖励
        //        //49    9月注册送50元奖励
        //        //50    9月注册送20元奖励
        //        //51    [9月注册送10元奖励]
        //        //52    [9月注册送8元奖励]

        //        //1   3 % 加息券
        //        //2   1 % 加息券
        //        //3   2 % 加息券
        //        //4   加息券
        //        #endregion
        //        //现金50元已抽中的个数
        //        int cash50Count = bllLuckDraw.GetCash50RecordsCount(-2, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
        //        B_Activity_schedule bllASchedule = new B_Activity_schedule();
        //        B_Activity bllActivity = new B_Activity();
        //        admin.users.AddBonusForUser adfu = new admin.users.AddBonusForUser();
        //        //奖品编号
        //        int awardID = 0;
        //        //奖品标题
        //        string title = "";
        //        //奖品类型
        //        int awardType = -1;
        //        //轮盘区块编号
        //        int awardBlockID = 0;
        //        Random random = new Random();
        //        //随机数
        //        int randomRate = random.Next(1, 10001);
        //        randomRate = 10;
        //        //switch (randomRate)
        //        //{
        //        if (randomRate <= 8)
        //        {
        //            //每天限中2次，概率填充入50元代金券
        //            if (cash50Count < 2)
        //            {
        //                awardID = ConfigHelper.GetConfigInt("AwardCash50");
        //                awardType = 0;
        //                title = "现金50元";
        //                awardBlockID = 3;
        //            }
        //            else
        //            {
        //                awardID = ConfigHelper.GetConfigInt("AwardBonus50");
        //                awardType = 1;
        //                awardBlockID = 7;
        //            }
        //        }
        //        //现金50元
        //        else if (randomRate <= 17)
        //        {
        //            //每天限中2次，概率填充入50元代金券
        //            if (cash50Count < 2)
        //            {
        //                awardID = ConfigHelper.GetConfigInt("AwardCash50");
        //                awardType = 0;
        //                title = "现金50元";
        //                awardBlockID = 5;
        //            }
        //            else
        //            {
        //                awardID = ConfigHelper.GetConfigInt("AwardBonus50");
        //                awardType = 1;
        //                awardBlockID = 7;
        //            }
        //        }
        //        //50元代金券
        //        else if (randomRate <= 2350)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardBonus50"); awardType = 1; awardBlockID = 7;
        //        }
        //        //20元代金卷
        //        else if (randomRate <= 4350)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; awardBlockID = 4;
        //        }
        //        //10元代金卷
        //        else if (randomRate <= 6350)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardBonus10"); awardType = 1; awardBlockID = 6;
        //        }
        //        //1 % 加息券
        //        else if (randomRate <= 8017)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardRate1"); awardType = 2; awardBlockID = 0;
        //        }
        //        //2 % 加息券
        //        else if (randomRate <= 9350)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardRate2"); awardType = 2; awardBlockID = 2;
        //        }
        //        //谢谢参与
        //        else if (randomRate <= 10000)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardThankYou"); awardType = 3; title = "谢谢参与"; awardBlockID = 1;
        //        }
        //        //谢谢参与
        //        if (awardType == -1)
        //        {
        //            awardID = ConfigHelper.GetConfigInt("AwardThankYou"); awardType = 3; title = "谢谢参与"; awardBlockID = 1;
        //        }
        //        //}
        //        //发放代金券
        //        if (awardType == 1)
        //        {
        //            var sourceData = bllASchedule.GetModel(awardID);
        //            title = sourceData.activity_schedule_name.Replace("活动", "抽奖");
        //            if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.activity_schedule_name, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
        //            {
        //                adfu.SubmitBonus(awardID, 1, userID, DateTime.Now, sourceData.end_date, title);
        //                arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
        //                return jss.Serialize(arData);
        //            }
        //        }
        //        //发放加息券
        //        if (awardType == 2)
        //        {
        //            var sourceData = bllActivity.GetModel(awardID);
        //            title = "9月抽奖" + sourceData.ActivityName;
        //            if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = sourceData.ActivityName, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
        //            {
        //                adfu.SubmitRate(awardID, 1, userID, DateTime.Now, sourceData.EndDate, title);
        //                arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
        //                return jss.Serialize(arData);
        //            }
        //        }
        //        //增加现金和“谢谢参与”的抽奖记录
        //        if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = title, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID }))
        //        {
        //            arData = new AjaxResponseData { code = "0", data = awardBlockID.ToString() };
        //            return jss.Serialize(arData);
        //        }
        //        //没有增加任何抽奖记录时的异常状态
        //        arData = new AjaxResponseData { code = "3", data = "碰到点小问题，刷新一下试试" };
        //        return jss.Serialize(arData);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogInfo.WriteLog("9月抽奖活动异常日志:" + "msg：" + ex.Message + "   StackTrace" + ex.StackTrace);

        //        //没有增加任何抽奖记录时的异常状态
        //        arData = new AjaxResponseData { code = "3", data = ex.Message };
        //        return jss.Serialize(arData);
        //    }
        //}
    }
}