using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ChuanglitouP2P.BLL
{
    public static class TActivity_Luck
    {
        static chuangtouEntities ef = new chuangtouEntities();
        #region [公用方法]
        /// <summary>
        /// 获取当次投资获得的抽奖次数
        /// </summary>
        /// <returns></returns>
        public static int GetOneShotCount(int registerID, string ordID, int amount)
        {
            if (!(startTime < DateTime.Now && DateTime.Now < endTime))
            {
                return 0;
            }
            //活动规则金额
            //可抽奖次数(规则：抽奖次数按照一次投资金额进行折算，如一次投资1万元，获得10次抽奖机会)
            int CanUseTimes = 0;
            decimal oid = Convert.ToDecimal(ordID);
            if (registerID == -1)
            {
                var bidRecord = ef.hx_Bid_records.Where(c => c.OrdId == oid).FirstOrDefault();
                registerID = bidRecord.investor_registerid ?? 0;
            }
            //查询本人在活动开始之前的投资次数
            string channelInvestCountSql = "SELECT count(*) from hx_Bid_records where ordstate = 1 and investor_registerid=" + registerID;
            int channelInvestCount = Convert.ToInt32(DbHelperSQL.GetSingle(channelInvestCountSql));
            //检测本人是否为渠道用户
            string channelType = "";
            bool isChannel = CheckIsChannel(registerID, ref channelType);
            //检测是否可以参加活动
            bool canInvest = CheckChannel(registerID);
            if (!canInvest) return 0;
            hx_Bid_records record = ef.hx_Bid_records.Where(c => c.ordstate == 1 && c.investor_registerid == registerID && c.OrdId == oid).FirstOrDefault();

            if (record != null)
            {
                //如果是渠道用户，并且活动开始前未进行过投资，则将活动期间的投资记录中，最早的一次投资记录删掉，不参与可抽奖次数的计算
                if (canInvest && isChannel && channelType == "cps1" && channelInvestCount <= 1) { return 0; }
                CanUseTimes = (Convert.ToInt32(((record.investment_amount ?? 0) - (record.BonusAmt ?? 0)).ToString("0")) / amount);
            }
            return CanUseTimes;
        }

        public delegate string loginIn(string username, string userpassword, string Validatecode, int remember, bool realMobileUser = false);
        public static bool AppLoginSite(string userID, string p, string key, loginIn login)
        {
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(p) || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            //chuangtouEntities ef = new chuangtouEntities();
            int uid = int.Parse(userID);
            if (uid == 0)
            {
                FormsAuthentication.SignOut();
                return false;
            }
            hx_member_table user = ef.hx_member_table.Where(c => c.registerid == uid).FirstOrDefault();
            if (user == null)
                return false;
            int appID = p == "android" ? 654321 : (p == "ios" ? 123456 : 0);
            var appAuthor = ef.ApplicationAuthorizations.Where(c => c.AppId == appID).FirstOrDefault();
            if (appAuthor == null)
                return false;
            string localKey = Utils.MD5(userID + appAuthor.AppSafeCode);
            if (key.ToLower() != localKey.ToLower())
                return false;

            string res = login(user.mobile, user.password, "", 1, true).ToString();
            registerBackData loginRes = JsonConvert.DeserializeObject<registerBackData>(res);
            return loginRes.rs == "y";
        }
        /// <summary>
        /// 注册登录返回值模板
        /// </summary>
        class registerBackData { public string rs { get; set; } public string url { get; set; } public string error { get; set; } }

        /// <summary>
        /// 获取可用的抽奖次数
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="amount">规则金额</param>
        /// <returns>可用抽奖次数</returns>
        public static int GetCanUseTimes(int userID, DateTime startTime, DateTime endTime, int amount)
        {

            //chuangtouEntities ef = new chuangtouEntities();
            //活动规则金额
            //可抽奖次数(规则：抽奖次数按照一次投资金额进行折算，如一次投资1万元，获得10次抽奖机会)
            int CanUseTimes = 0;
            B_Bid_records b_Bid_records = new B_Bid_records();

            //查询本人在活动开始之前的投资次数
            string channelInvestCountSql = "SELECT count(*) from hx_Bid_records where ordstate = 1 and investor_registerid=" + userID + " and invest_time < '" + startTime + "'";
            int channelInvestCount = Convert.ToInt32(DbHelperSQL.GetSingle(channelInvestCountSql));
            //检测本人是否为渠道用户
            string channelType = "";
            bool isChannel = CheckIsChannel(userID, ref channelType);
            //检测是否可以参加活动
            bool canInvest = CheckChannel(userID);
            if (!canInvest) return 0;
            List<hx_Bid_records> records = ef.hx_Bid_records.Where(c => c.ordstate == 1 && c.investor_registerid == userID && c.invest_time >= startTime && c.invest_time <= endTime).ToList();
            //StringBuilder sql = new StringBuilder();
            //sql.AppendFormat(" ORDSTATE = 1 AND INVESTOR_REGISTERID={0} AND INVEST_TIME >='{1}' AND INVEST_TIME<='{2}' AND INVESTMENT_AMOUNT >={3}", userID, startTime, endTime, amount);
            //DataSet ds = b_Bid_records.GetList(sql.ToString());

            if (records != null && records.Count > 0)
            {
                //如果是渠道用户，并且活动开始前未进行过投资，则将活动期间的投资记录中，最早的一次投资记录删掉，不参与可抽奖次数的计算
                if (canInvest && isChannel && channelType == "cps1" && channelInvestCount <= 0) { records.Remove(records.OrderBy(c => c.invest_time).First()); }
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    CanUseTimes += Convert.ToInt32((Convert.ToDecimal(ds.Tables[0].Rows[i]["investment_amount"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["BonusAmt"])).ToString("0")) / amount;
                //}
                //CanUseTimes = records.Sum(c => Convert.ToInt32((c.investment_amount ?? 0 - c.BonusAmt ?? 0).ToString("0")) / amount);
                foreach (var r in records)
                {
                    CanUseTimes += (Convert.ToInt32(((r.investment_amount ?? 0) - (r.BonusAmt ?? 0)).ToString("0")) / amount);
                }
                //records.ForEach(c =>
                //{
                //    CanUseTimes += (Convert.ToInt32((c.investment_amount ?? 0-c.BonusAmt ?? 0).ToString("0")) / amount);
                //});
            }

            //获取用户的抽奖记录（活动日期内）
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            int recordsCount = bllLuckDraw.GetRecordsCount(userID, startTime, endTime);
            CanUseTimes = CanUseTimes >= recordsCount ? CanUseTimes - recordsCount : 0;
            return CanUseTimes;
        }

        /// <summary>
        /// 检查是否在活动时间内
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int CheckActivityTime(DateTime startTime, DateTime endTime, ref string msg)
        {
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
        /// 发放奖励
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="registerID"></param>
        public static void DrawBonus(hx_ActivityTable activity, int registerID, int effectiveDays)
        {
            //chuangtouEntities ef = new chuangtouEntities();
            M_bonus_account_water mbaw = new M_bonus_account_water();
            B_bonus_account_water bbaw = new B_bonus_account_water();

            DateTime dte = DateTime.Now;//当前时间截点
                                        //if (activity.ActEndtime >= dte && activity.ActStarttime <= dte)   //发布的时候要释放
            if (true)
            {
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(activity.ActRule);
                hx_UserAct hua = new hx_UserAct();
                hua.ActTypeId = activity.ActTypeId;
                hua.registerid = registerID;
                hua.RewTypeID = activity.RewTypeID;
                if (activity.RewTypeID == 3)
                {
                    hua.UseLifeLoan = "3-0";
                }
                hua.ActID = activity.ActID;
                hua.Amt = mcp.cash;
                hua.Uselower = mcp.Msplitarr == null || mcp.Msplitarr.Count <= 0 ? 0 : mcp.Msplitarr[0].startAmt;
                hua.Usehight = mcp.Msplitarr == null || mcp.Msplitarr.Count <= 0 ? 0 : mcp.Msplitarr[0].endAmt;
                hua.AmtEndtime = Convert.ToDateTime(dte.AddDays(effectiveDays).ToShortDateString() + " 23:59:59");  //有效期为10天
                hua.AmtUses = 1; //没指定情况下默认为单独使用
                hua.UseState = activity.RewTypeID == 1 ? 5 : 0;
                hua.AmtProid = 0; //未使用默认为0
                hua.ISSmsOne = 0;
                hua.IsSmsThree = 0;
                hua.isSmsFifteen = 0;
                hua.IsSmsSeven = 0;
                hua.isSmsSixteen = 0;
                hua.Title = activity.ActName;
                hua.Createtime = dte;
                hua.OrderID = decimal.Parse(Utils.Createcode());//根据日期生成单号
                ef.hx_UserAct.Add(hua);
                ef.SaveChanges();


                if (activity.RewTypeID == 1)  //当奖品为现金时
                {
                    Transfer tf = new Transfer();
                    M_member_table p = new M_member_table();
                    B_member_table o = new B_member_table();
                    p = o.GetModel(registerID);
                    ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, mcp.cash, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
                    if (retf != null)
                    {
                        if (retf.RespCode == "000")
                        {
                            //3.事务处理操作账户及插入流水
                            #region 验签缓存处理
                            string cachename = retf.OrdId + "ToUserTransfer" + retf.InCustId;
                            if (Utils.GeTThirdCache(cachename) == 0)
                            {
                                Utils.SetThirdCache(cachename);
                                B_usercenter BUC = new B_usercenter();
                                BUC.UpateActToUserTransfer(retf, 0);

                                //增加账户资金流水
                                hx_Capital_account_water Caw = new hx_Capital_account_water();

                                Caw.membertable_registerid = registerID;
                                Caw.income = mcp.cash;
                                Caw.expenditure = 0;
                                Caw.time_of_occurrence = dte;
                                Caw.account_balance = p.available_balance + mcp.cash;
                                Caw.types_Finance = 43;
                                Caw.createtime = dte;
                                Caw.keyid = 0;
                                Caw.remarks = "现金奖励";
                                ef.hx_Capital_account_water.Add(Caw);
                                ef.SaveChanges();
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断当前用户是否可以参加活动
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static bool CheckChannel(int userID)
        {
            bool t = false;
            B_member_table b_member_table = new B_member_table();
            M_member_table investor = new M_member_table();
            investor = b_member_table.GetModel(userID);
            if (investor == null) return false;
            string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where  invpersonid=" + investor.registerid + " ";//查询本人是否已经被邀请注册过

            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
            //if (dtcode.Rows.Count > 0 || (investor != null && !string.IsNullOrWhiteSpace(investor.channel_invitedcode)))
            //{
            int uuid = dtcode.Rows.Count > 0 ? int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()) : 0; //邀请用户id

            //用户等级为渠道 不参与活动
            if (investor != null && investor.useridentity == 4)
            {
                return t;
            }
            int investCount = B_usercenter.GetInvestCountByUserid(investor.registerid);
            //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
            if (uuid != 0)
            {
                M_member_table py = new M_member_table();
                B_member_table oy = new B_member_table();
                py = oy.GetModel(uuid);//推荐人
                if ((py != null && py.useridentity == 4) && investCount <= 1)
                {
                    return t;
                }
            }

            //新渠道机制判断              
            using (ChannelAct channelAct = new ChannelAct())
            {
                //按照渠道类型和投资次数判断是否参与此次活动 
                if (!string.IsNullOrWhiteSpace(investor.channel_invitedcode) && !channelAct.IsParticipateActivity(investor.channel_invitedcode, investCount))
                {
                    return t;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断用户是否为渠道用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="channelType">渠道类型</param>
        /// <returns></returns>
        public static bool CheckIsChannel(int userID, ref string channelType)
        {
            //chuangtouEntities ef = new chuangtouEntities();
            bool t = true;
            B_member_table b_member_table = new B_member_table();
            M_member_table investor = new M_member_table();
            investor = b_member_table.GetModel(userID);
            if (investor == null) return false;
            string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where  invpersonid=" + investor.registerid + " ";//查询本人是否已经被邀请注册过

            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
            //if (dtcode.Rows.Count > 0 || (investor != null && !string.IsNullOrWhiteSpace(investor.channel_invitedcode)))
            //{
            int uuid = dtcode.Rows.Count > 0 ? int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()) : 0; //邀请用户id

            //用户等级为渠道 不参与活动
            if (investor != null && investor.useridentity == 4)
            {
                return t;
            }
            int investCount = B_usercenter.GetInvestCountByUserid(investor.registerid);
            //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
            if (uuid != 0)
            {
                M_member_table py = new M_member_table();
                B_member_table oy = new B_member_table();
                py = oy.GetModel(uuid);//推荐人
                if ((py != null && py.useridentity == 4))
                {
                    return t;
                }
            }

            if (string.IsNullOrWhiteSpace(investor.channel_invitedcode))
            {
                return false;
            }
            string cacheName = "hx_Channel_" + investor.channel_invitedcode;
            hx_Channel channelEnitty = (hx_Channel)Utils.GetThirdCacheObject(cacheName);
            if (channelEnitty == null)
            {
                channelEnitty = ef.hx_Channel.AsNoTracking().Where(p => p.Invitedcode == investor.channel_invitedcode).FirstOrDefault();
            }
            if (channelEnitty != null)
            {
                channelType = channelEnitty.type;
                Utils.SetThirdCacheName(cacheName, channelEnitty, 5);
            }
            return channelEnitty != null;
        }
        #endregion

        #region [双12抽奖活动方法]
        /// <summary>
        /// 抽奖方法
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="amount">规则金额</param>
        /// <returns>提示信息</returns>
        public static string LuckDrawAward(int userID, DateTime startTime, DateTime endTime, int amount)
        {

            string msg = "";
            try
            {
                int state = CheckActivityTime(startTime, endTime, ref msg);
                if (state != 0)
                {
                    return msg = "4;" + msg;
                }
                if (userID <= 0)
                {
                    return msg = "1;请登录后重试";
                }
                if (GetCanUseTimes(userID, startTime, endTime, amount) <= 0) //如果可抽奖次数是0的话，则不能抽奖
                {
                    return msg = "2;抽奖机会已用完，请明天再来";
                }
                #region 规则及数据

                //1-10 --小米手环（2代） --0.10%  --5个   
                //10-35  --京东卡100元  --0.25%     --2个
                //35-535  --现金5元  --5.00%       --100个
                //535-1500  --2%加息券  --9.65%    --193个
                //1500-3500  --1%加息券  --20.00%  --400个
                //3500-6000  --50元抵扣券 --25.00%  --500个
                //6000-10000  --20元抵扣券 --40.00%     --800个
                #endregion

                //奖品编号
                int awardID = 0;
                //奖品标题
                string title = "";

                // 奖品类型   （前端现金1、抵扣券2、加息券3，后台管理现金4、抵扣券1、加息券2、谢谢参与3、实物-4）
                int awardType = -1;

                //轮盘区块编号
                int awardBlockID = 0;
                Random random = new Random();
                //随机数
                int randomRate = random.Next(1, 10001);

                B_LuckDraw bllLuckDraw = new B_LuckDraw();

                //获得用户对象
                M_member_table p = new M_member_table();
                B_member_table o = new B_member_table();
                p = o.GetModel(userID);

                if (randomRate <= 10)
                {
                    awardID = ConfigHelper.GetConfigInt("MiBandWristband");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 5)     //最多5个，如果超过了5个，则获得20元抵扣券
                    {
                        awardType = -4; title = "双12抽奖送小米手环"; awardBlockID = 5;
                        string mailStr = "恭喜您抽到了 '小米手环（第二代）' 奖品，我司客服将于3个工作日与您联系，请您保持手机畅通。";
                        SendSMS.Send_SMS(p.mobile, mailStr);
                    }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 35)
                {
                    awardID = ConfigHelper.GetConfigInt("JDCard");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 2)     //最多2个，如果超过了2个，则获得20元抵扣券
                    {
                        awardType = -4; title = "双12抽奖送京东卡100元"; awardBlockID = 6;
                        string mailStr = "恭喜您抽到了 '京东卡100元' 奖品，我司客服将于3个工作日与您联系，请您保持手机畅通。";
                        SendSMS.Send_SMS(p.mobile, mailStr);
                    }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 535)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardCash5");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 100)   //最多100个，如果超过了100个，则获得20元抵扣券
                    { awardType = 4; title = "双12抽奖送现金5元"; awardBlockID = 1; }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 1500)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardRate2");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 193)   //最多193个，如果超过了193个，则获得20元抵扣券
                    { awardType = 2; title = "双12抽奖送2%加息券"; awardBlockID = 2; }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 3500)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardRate1");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 400)   //最多400个，如果超过了400个，则获得20元抵扣券
                    { awardType = 2; title = "双12抽奖送1%加息券"; awardBlockID = 7; }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 6000)
                {
                    awardID = ConfigHelper.GetConfigInt("AwardBonus50");
                    if (bllLuckDraw.GetCash50RecordsCount(awardID, startTime, endTime) < 500)   //最多500个，如果超过了500个，则获得20元抵扣券
                    { awardType = 1; title = "双12抽奖送50元抵扣券 "; awardBlockID = 4; }
                    else
                    { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }
                }
                else if (randomRate <= 10000)
                { awardID = ConfigHelper.GetConfigInt("AwardBonus20"); awardType = 1; title = "双12抽奖送20元抵扣券"; awardBlockID = 0; }

                ActFacade act = new ActFacade();
                var sourceData = act.GetActivityModel(awardID); //bllASchedule.GetModel(awardID);
                #region 发放现金、抵扣券和加息券
                if (awardType != -4)
                {
                    if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = title, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID, Ldre_OrderID = awardType == 4 ? Utils.Createcode() : null, Ldre_ActivityName = "双12抽奖" }))
                    {
                        //发放奖励
                        //sourceData.ActStarttime = DateTime.Now;
                        //sourceData.ActEndtime = DateTime.Now.AddDays(10);   //有效期10天
                        sourceData.ActTypeId = 2;  //短期活动
                        switch (awardType)  //后台类别规则和前台规则不一样，所以需要处理，具体说明看awardType定义
                        {
                            case 4:
                                sourceData.RewTypeID = 1;
                                break;
                            case 1:
                                sourceData.RewTypeID = 2;
                                break;
                            case 2:
                                sourceData.RewTypeID = 3;
                                break;
                        }
                        sourceData.ActID = awardID;
                        sourceData.ActName = title;
                        DrawBonus(sourceData, userID, 10);
                        return msg = "0;" + awardBlockID.ToString();
                    }
                }

                else if (awardType == -1)
                {
                    return msg = "3;碰到点小问题，刷新一下试试^v^";
                }

                #endregion

                #region 实体奖品的抽奖记录
                if (bllLuckDraw.AddNewRecord(new M_LuckDrawRecord { Ldre_AwardID = awardID, Ldre_AwardName = title, Ldre_AwardType = awardType, Ldre_CreatTime = DateTime.Now, Ldre_UserID = userID, Ldre_ActivityName = "双12抽奖" }))
                {
                    return msg = "0;" + awardBlockID.ToString();
                }
                else
                {
                    return msg = "3;碰到点小问题，刷新一下试试^v^";
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("双12抽奖活动异常日志:" + "msg：" + ex.Message + "   StackTrace" + ex.StackTrace);

                //没有增加任何抽奖记录时的异常状态
                return msg = "3;" + ex.Message;
            }
        }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public static DateTime startTime = Convert.ToDateTime("2016-12-12 00:00:00");
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public static DateTime endTime = Convert.ToDateTime("2016-12-14 23:59:59");

        #endregion


        /// <summary>
        /// 是否显示右上角的返现角标
        /// </summary>
        /// <param name="sTime">返现活动开始时间</param>
        /// <param name="eTime">返现活动结束时间</param>
        /// <param name="sysTime">开标上线时间</param>
        /// <param name="tenderState">标的状态</param>
        /// <param name="targetEndTime">标的募集截止时间</param>
        /// <returns>是否显示角标</returns>
        public static bool GetCurJiaoBiao(DateTime sTime, DateTime eTime, DateTime sysTime, int tenderState, DateTime targetEndTime)
        {
            if (tenderState == 2 && targetEndTime > DateTime.Now)
            {
                return DateTime.Now > sTime && DateTime.Now < eTime;
            }
            else
            {
                return sysTime > sTime && sysTime < eTime;
            }
        }
    }

}
