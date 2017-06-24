using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 活动解析操作类--常规投资活动
    /// </summary>
    class ActInvest : ActBase
    {
        #region 投资后按照常规活动进行奖励发放（常规投资、邀请奖励）
        //DataTable dt 投资记录视图 V_hx_Bid_records_borrowing_target
        /// <summary>
        /// 投资后按照常规活动进行奖励发放（常规投资、邀请奖励）
        /// </summary>
        /// <param name="dt"></param>
        protected internal void SendBonusAfterInvest(DataTable dt, string targetPlatform)
        {
            if (dt.Rows.Count <= 0)
            {
                return;
            }
            //投资金额
            decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            int registerid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
            string targetid = dt.Rows[0]["targetid"].ToString();
            //string OrdId = dt.Rows[0]["OrdId"].ToString();
            int bid_records_id = int.Parse(dt.Rows[0]["bid_records_id"].ToString());
            int life_of_loan = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
            int unit_day = int.Parse(dt.Rows[0]["unit_day"].ToString());
            if (unit_day == 1) { life_of_loan = life_of_loan >= 6 ? 6 : (life_of_loan < 3 ? 1 : 3); }
            if (unit_day == 3) { life_of_loan = (life_of_loan / 30) >= 6 ? 6 : ((life_of_loan / 30) < 3 ? ((life_of_loan / 30) >= 1 ? 1 : -1) : 3); }
            string borrowing_title = dt.Rows[0]["borrowing_title"].ToString();
            string userName = dt.Rows[0]["username"].ToString();
            //string mobile = dt.Rows[0]["mobile"].ToString();
            string invcode = dt.Rows[0]["invitationcode"].ToString();
            //本次投标使用的奖励金额
            decimal bonusAmt = decimal.Parse(dt.Rows[0]["bonusAmt"].ToString());
            //项目总借款金额
            decimal borrowing_balance = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());//??
            LogInfo.WriteLog(" /*此处加入活动*/:" + targetid + ";投资人：" + registerid);
            B_bonus_account_water bbaw = new B_bonus_account_water();
            /*此处加入活动*/
            int invcount = B_usercenter.GetInvestCountByUserid(registerid);//投资次数
            //ActFacade act = new ActFacade();
            #region 活动奖励           
            if (invcount == 1)
            {
                #region 首次投资活动奖励
                List<hx_UserAct> hut = GeneralInvestActBonus(registerid, targetPlatform, investAmt - bonusAmt, bid_records_id, 1, life_of_loan);
                if (hut.Count > 0)//合并到奖励中
                {
                    foreach (hx_UserAct item in hut)
                    {
                        if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理 
                        {
                            #region MyRegion  记录奖励流水表
                            string awardDescription = string.Format("首次投资成功获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                            #endregion

                            #region MyRegion  发送系统消息
                            string MContext = string.Format("首次投资{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddSytemMessage(registerid, "首次投资成功", MContext);
                            #endregion
                        }
                    }
                }
                #endregion
            }
            else if (invcount >= 2)
            {
                #region 续投活动奖励
                List<hx_UserAct> hutw = GeneralInvestActBonus(registerid, targetPlatform, investAmt - bonusAmt, bid_records_id, 6, life_of_loan);
                if (hutw.Count > 0)
                {
                    foreach (hx_UserAct item in hutw)
                    {
                        if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                        {
                            #region MyRegion  奖励流水
                            string awardDescription = string.Format("续投成功获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                            #endregion MyRegion  奖励流水

                            #region MyRegion  系统消息
                            string MContext = string.Format("续投{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddSytemMessage(registerid, "续投成功", MContext);
                            #endregion
                        }
                    }
                }
                #endregion

                #region 特殊复投返现活动奖励--回款金额复投
                hutw = GeneralInvestActBonus(registerid, targetPlatform, investAmt - bonusAmt, bid_records_id, 7, life_of_loan);
                if (hutw.Count > 0)
                {
                    foreach (hx_UserAct item in hutw)
                    {
                        if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                        {
                            #region MyRegion  奖励流水
                            string awardDescription = string.Format("回款复投成功获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                            #endregion MyRegion  奖励流水

                            #region MyRegion  系统消息
                            string MContext = string.Format("回款复投{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddSytemMessage(registerid, "回款复投成功", MContext);
                            #endregion
                        }
                    }
                }
                #endregion

            }


            #region 所有用户
            //所有用户
            List<hx_UserAct> hutwc = GeneralInvestActBonus(registerid, targetPlatform, investAmt - bonusAmt, bid_records_id, 5, life_of_loan);

            if (hutwc.Count > 0)
            {
                foreach (hx_UserAct item in hutwc)
                {
                    if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                    {
                        #region MyRegion  奖励流水
                        string awardDescription = string.Format("投资成功获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                        AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                        #endregion MyRegion  奖励流水

                        #region MyRegion  系统消息
                        string MContext = string.Format("投资{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                        AddSytemMessage(registerid, "投资成功", MContext);
                        #endregion
                    }
                }
            }
            #endregion

            #region 投标最大的用户  暂时注释false 有bug,投标结束后才能判断哪个位最大,而不是每次都判断???? 
            DataTable dmax = B_usercenter.Topinvestor(int.Parse(targetid));
            if (dmax.Rows.Count > 0 && false)
            {
                decimal amtc = decimal.Parse(dmax.Rows[0]["InvCount_Amt"].ToString());
                if (borrowing_balance == amtc)//???判断什么 一人满标？
                {
                    List<hx_UserAct> invmax = GeneralInvestActBonus(int.Parse(dmax.Rows[0]["investor_registerid"].ToString()), targetPlatform, decimal.Parse(dmax.Rows[0]["maxamt"].ToString()), bid_records_id, 4, life_of_loan);
                    if (invmax.Count > 0)
                    {
                        foreach (hx_UserAct item in invmax)
                        {
                            if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                            {
                                #region MyRegion  奖励流水
                                string awardDescription = string.Format("投标最大的用户获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                                AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                                #endregion MyRegion  奖励流水

                                #region MyRegion  系统消息
                                string MContext = string.Format("投标最大的用户，投资{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                                AddSytemMessage(registerid, "投标最大的用户奖励", MContext);
                                #endregion
                            }
                        }
                    }
                }
            }
            #endregion

            #region 每标首投用户
            if (B_usercenter.TopNum(targetid) == 1)
            {
                List<hx_UserAct> invFirst = GeneralInvestActBonus(registerid, targetPlatform, investAmt - bonusAmt, bid_records_id, 3, life_of_loan);
                if (invFirst.Count > 0)
                {
                    foreach (hx_UserAct item in invFirst)
                    {
                        if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                        {
                            #region MyRegion  奖励流水
                            string awardDescription = string.Format("每标首投用户获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                            #endregion MyRegion  奖励流水

                            #region MyRegion  系统消息
                            string MContext = string.Format("每标首投用户，投资{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, item.Amt.ToString(), GetBunusDescription(item.RewTypeID));
                            AddSytemMessage(registerid, "每标首投用户奖励", MContext);
                            #endregion
                        }
                    }
                }
            }
            #endregion

            #endregion
        }
        #endregion

        #region 常规活动 投资 ---用户投资活动 获得相应奖励 +  bool UsrFirstInvest()
        /// <summary>
        ///  常规活动 投资 ---用户投资活动 获得相应奖励</summary>
        /// <param name="Registerid">注册用户id</param>
        /// <param name="InvestAmt">投资金额：实际投资金额 已经扣除奖励金额</param>
        /// <param name="bid_records_id">投标记录id</param>
        /// <param name="ActUser">ActUser=0;面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 特殊复投用户=7   </param>
        /// <returns></returns>
        private List<hx_UserAct> GeneralInvestActBonus(int Registerid, string targetPlatform, decimal InvestAmt, int bid_records_id, int ActUser, int lifeLoan = 0)
        {
            ///ActTypeId=3  活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送
            int ActTypeId = 3;
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1);
            //判断是否过期
            if (hat != null && hat.ActStarttime <= DateTime.Now && DateTime.Now <= hat.ActEndtime)
            {
                if (hat.RewTypeID == 1) //现金奖励
                {
                    GeneralActCash(Registerid, InvestAmt, hat, bid_records_id, lifeLoan);
                }
                else if (hat.RewTypeID == 2) //红包奖励
                {
                    t = GeneralActCoupon(Registerid, InvestAmt, hat);
                }
                else if (hat.RewTypeID == 3) //加息券奖励
                {
                    GeneralActInterestRatesPlus(Registerid, InvestAmt, hat);
                }
            }
            return t;
        }
        #endregion

        #region 常规活动 现金奖励+bool GeneralActCash(int Registerid, decimal InvestAmt,hx_ActivityTable hat, int bid_records_id)
        /// <summary>
        /// 常规活动 现金奖励 发放（奖励流水 资金流水 系统消息）
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="investAmt">投资金额</param>
        /// <param name="hat">活动对象</param>
        /// <param name="bid_records_id">投标记录id</param>
        /// <returns>bool 是否发放成功</returns>
        private bool GeneralActCash(int Registerid, decimal InvestAmt, hx_ActivityTable hat, int bid_records_id, int lifeLoan = 0)
        {
            bool t = false;
            //hx_ActivityTable hat = GetActTableInfo(ActTypeId, ActUser, 1);
            if (hat != null)
            {
                B_member_table oy = new B_member_table();
                //M_member_table investor = new M_member_table();
                hx_member_table investor = new hx_member_table();
                investor = ef.hx_member_table.Where(c => c.registerid == Registerid).FirstOrDefault(); //oy.GetModel(Registerid);//被推荐人  也就是投资人

                DateTime dte = DateTime.Now;
                string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where  invpersonid=" + Registerid + " ";//查询本人是否已经被邀请注册过

                DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
                //if (dtcode.Rows.Count > 0 || (investor != null && !string.IsNullOrWhiteSpace(investor.channel_invitedcode)))
                //{
                int uuid = dtcode.Rows.Count > 0 ? int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()) : 0; //邀请用户id

                //用户等级为渠道 不参与活动
                if (investor != null && investor.useridentity == 4)
                {
                    return t;
                }
                int investCount = B_usercenter.GetInvestCountByUserid(Registerid);
                //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
                if (uuid != 0)
                {
                    M_member_table py = new M_member_table();
                    py = oy.GetModel(uuid);//推荐人
                    if ((py != null && py.useridentity == 4) && investCount == 1)
                    {
                        return t;
                    }
                }
                //新渠道机制判断              
                using (ChannelAct channelAct = new ChannelAct())
                {
                    //按照渠道类型和投资次数判断是否参与此次活动 
                    if (!channelAct.IsParticipateActivity(investor.channel_invitedcode, investCount))
                    {
                        return t;
                    }
                }

                //if (((py != null && py.useridentity == 4) || !string.IsNullOrWhiteSpace(investor.channel_invitedcode)) && B_usercenter.GetInvestCountByUserid(Registerid) == 1)
                //{
                //    return t;
                //}
                //if (py.useridentity != 4)//渠道用户不执行 2016-11-8日添加
                //{

                string ActRule = hat.ActRule;
                List<MAmtList> mlist = new List<MAmtList>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                MActCash mc = new MActCash();
                mc = js.Deserialize<MActCash>(ActRule);
                //mlist = js.Deserialize<List<MAmtList>>(ActRule);
                //获取该活动对应的已经发放奖励的人次
                int TopNum = B_usercenter.GetTopNum(hat.ActID);
                //获取该活动对应的已经发放奖励的总金额
                decimal totalAmt = B_usercenter.GetTopAmtCount(hat.ActID);
                if (hat.ActUser == 7)//特殊回款复投奖励类型，根据规则修订奖励基数--投资金额
                {
                    InvestAmt = GetSepcialActAmt(Registerid, bid_records_id, InvestAmt, hat);
                }
                //根据投资金额计算对应现金奖励金额
                decimal actamt = GetActAmt(mc, InvestAmt, TopNum, lifeLoan);
                //检查分发奖励是否超过人数顶限或者总金额上限，上限为0表示不限制，如果超过直接跳过
                if ((mc.TopAmt > totalAmt || mc.TopAmt == 0) && (mc.TopNum > TopNum || mc.TopNum == 0))
                {
                    //t = true;
                    if (actamt > 0)//大于 0里写入对应的奖励数据
                    {
                        hx_UserAct hua = new hx_UserAct();
                        hua.ActTypeId = hat.ActTypeId;
                        hua.registerid = Registerid;
                        hua.RewTypeID = hat.RewTypeID;
                        hua.ActID = hat.ActID;
                        hua.Amt = actamt;
                        hua.Uselower = 0.00M;
                        hua.Usehight = 0.00M;
                        hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString()).AddMonths(1);
                        hua.AmtUses = 1; //没指定情况下默认为单独使用
                        hua.UseState = 5;  //现金未转账
                        hua.UseTime = DateTime.Now;
                        hua.AmtProid = bid_records_id; //未使用默认为0,对于现金奖励该字段存放获得该奖励的投资记录id
                        hua.ISSmsOne = 0;
                        hua.IsSmsThree = 0;
                        hua.isSmsFifteen = 0;
                        hua.IsSmsSeven = 0;
                        hua.isSmsSixteen = 0;
                        hua.OrderID = decimal.Parse(Utils.Createcode());
                        hua.Createtime = DateTime.Now;
                        hua.Title = mc.ActName;
                        hua.UseLifeLoan = "";
                        ef.hx_UserAct.Add(hua);
                        int i = ef.SaveChanges();
                        if (i > 0)
                        {
                            //录入成功，后进行转账操作 1.获取用户对象
                            M_member_table p = new M_member_table();
                            B_member_table o = new B_member_table();
                            p = o.GetModel(Registerid);
                            if (p != null)
                            {
                                //2.调用商户向用户转账接口
                                Transfer tf = new Transfer();
                                ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
                                if (retf != null && retf.RespCode == "000")
                                {
                                    //3.事务处理操作账户及插入流水
                                    #region 验签缓存处理
                                    string cachename = retf.OrdId + "ToUserTransfer" + retf.InCustId;
                                    if (Utils.GeTThirdCache(cachename) == 0)
                                    {
                                        Utils.SetThirdCache(cachename);
                                        B_usercenter BUC = new B_usercenter();
                                        int ic = BUC.UpateActToUserTransfer(retf, bid_records_id);  //用户余额更新
                                        if (ic > 0)
                                        {
                                            string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retf.InCustId + "'";
                                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                            if (dt.Rows.Count > 0)
                                            {
                                                #region 资金流水
                                                B_usercenter ors = new B_usercenter();
                                                decimal di = ors.GetUsridAvailable_balance(int.Parse(dt.Rows[0]["registerid"].ToString()));
                                                // di = di + decimal.Parse(hua.Amt.ToString());
                                                StringBuilder strSql = new StringBuilder();
                                                strSql.Append("insert into hx_Capital_account_water(");
                                                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                                strSql.Append(" values (");
                                                strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + "," + decimal.Parse(hua.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.现金奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "现金奖励" + "')");
                                                DbHelperSQL.RunSql(strSql.ToString());
                                                strSql.Clear();
                                                #endregion

                                                #region MyRegion  记录奖励流水表
                                                string awardDescription = string.Format("{0},已汇入个人账户{1}{2}", hat.ActName, retf.TransAmt, GetBunusDescription(hat.RewTypeID));
                                                AddBonusAccoutWater(int.Parse(hua.UserAct.ToString()), Registerid, decimal.Parse(retf.TransAmt), awardDescription);
                                                #endregion

                                                #region MyRegion  发送系统消息
                                                string MContext = string.Format("尊敬的用户：您好！恭喜您成功参与{0}，获得{1}{2}如有问题可咨询创利投的客服！", hat.ActName, retf.TransAmt, GetBunusDescription(hat.RewTypeID));
                                                AddSytemMessage(Registerid, hat.ActName, MContext);


                                                #endregion
                                                string msg = string.Format("尊敬的客户您好，您已成功投资创利投金服，获得返利{0}元，请登录官网查看。客服热线：010-53732056。", actamt.ToString("0.00"));
                                                SendSMS.Send_SMS(dt.Rows[0]["mobile"].ToString(), msg);
                                            }
                                        }
                                        t = true;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    //}
                }
                //}
            }
            return t;
        }
        #endregion

        #region 常规活动 红包奖励+bool GeneralActCoupon(int registerid, decimal investAmt,hx_ActivityTable hat)
        /// <summary>
        /// 常规活动 红包奖励 发放  未记录系统信息及奖励流水//与加息券方法一致 GeneralActInterestRatesPlus
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="investAmt">投资金额</param>
        /// <param name="hat">活动对象</param>
        /// <returns>list 用户活动奖励记录集合</returns>
        /// <returns></returns>
        private List<hx_UserAct> GeneralActCoupon(int registerid, decimal investAmt, hx_ActivityTable hat)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            //hx_ActivityTable hat = GetActTableInfo(ActTypeId, ActUser, 1);
            if (hat != null)
            {
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //发放红包奖励
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        #endregion

        #region 常规活动 加息券奖励
        /// <summary>
        /// 常规活动 加息券奖励 未记录系统信息及奖励流水 //与红包方法一致GeneralActCoupon
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="investAmt">投资金额</param>
        /// <param name="hat">活动对象</param>
        /// <returns>list 用户活动奖励记录集合</returns>
        /// <returns></returns>
        private List<hx_UserAct> GeneralActInterestRatesPlus(int registerid, decimal investAmt, hx_ActivityTable hat)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            //hx_ActivityTable hat = GetActTableInfo(ActTypeId, ActUser, 1);
            if (hat != null)
            {
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //发放加息券奖励
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        #endregion

        #region 根据活动规则计算特殊回款复投奖励现金金额
        /// <summary>
        /// 根据活动规则计算特殊回款复投奖励现金金额
        /// </summary>
        /// <param name="Registerid">投资人id</param>
        /// <param name="bid_records_id">投标记录id</param>
        /// <param name="InvestAmt">本次投标金额</param>
        /// <param name="hx_ActivityTable"> hx_ActivityTable.ActUser面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 , 特殊复投用户=7</param>
        /// 
        public decimal GetSepcialActAmt(int Registerid, int bid_records_id, decimal InvestAmt, hx_ActivityTable hat = null)
        {
            //获取活动一次开始时间 ;
            DateTime startTime = new DateTime(2016, 10, 31, 13, 35, 0);
            hx_ActivityTable shat = null;
            if (hat != null)
            {
                shat = GetActTableInfoWithoutTimeLimit((int)hat.ActTypeId, (int)hat.ActUser, -1, (int)hat.RewTypeID, true);
            }
            else
            {
                shat = GetActTableInfoWithoutTimeLimit(4, 7, -1, 1, true);
            }
            if (shat != null)
            {
                startTime = (DateTime)shat.ActStarttime;
            }

            decimal CurrentInvestAmt = 0.00M;
            //计算本次计入返现基数的投资金额
            //查找最近30天回款记录(income_statement)
            List<hx_income_statement> incomeList = GetIncomeStatementList(Registerid, 30);
            if (incomeList != null && incomeList.Count > 0)
            {
                List<Behavor> behavorList = new List<Behavor>();
                DateTime? firstRecentPrincipalRepayTime = null;
                foreach (hx_income_statement income in incomeList)
                {
                    Behavor behavor = new Behavor();
                    behavor.type = BehavorType.HK;
                    behavor.time = income.repayment_period;
                    behavor.amount = decimal.Parse(income.Principal.ToString());
                    if (behavor.amount > 0.00M && behavor.time != null && (firstRecentPrincipalRepayTime == null || firstRecentPrincipalRepayTime > (DateTime)behavor.time))
                    {
                        firstRecentPrincipalRepayTime = (DateTime)behavor.time;
                    }
                    behavor.amountExt = decimal.Parse(income.interestpayment.ToString());
                    behavorList.Add(behavor);
                }
                if (firstRecentPrincipalRepayTime == null)
                {//最近30天没有本金回款
                    return 0.00M;
                }
                DateTime time = (DateTime)firstRecentPrincipalRepayTime;
                //查找最近30天第一笔本金回款之后充值记录(recharge)
                List<hx_Recharge_history> rechargeList = GetRechargeList(Registerid, time);
                foreach (hx_Recharge_history recharge in rechargeList)
                {
                    Behavor behavor = new Behavor();
                    behavor.type = BehavorType.CZ;
                    behavor.time = recharge.recharge_time;
                    behavor.amount = decimal.Parse(recharge.recharge_amount.ToString());
                    behavorList.Add(behavor);
                }
                //查找最近30天第一笔回款之后提现记录(cash)
                List<hx_td_UserCash> cashList = GetCashList(Registerid, time);
                foreach (hx_td_UserCash cash in cashList)
                {
                    Behavor behavor = new Behavor();
                    behavor.type = BehavorType.TX;
                    behavor.time = cash.OperTime;
                    behavor.amount = decimal.Parse(cash.TransAmt.ToString());
                    behavorList.Add(behavor);
                }
                //查找最近30天第一笔回款之后投资记录(含红包金额)(bid_records orderstate=1)
                List<hx_Bid_records> investList = GetInvestList(Registerid, time);
                foreach (hx_Bid_records invest in investList)
                {
                    if (invest.bid_records_id == bid_records_id)
                    //不包括本次投标记录
                    {
                        continue;
                    }
                    Behavor behavor = new Behavor();
                    behavor.type = BehavorType.TZ;
                    behavor.time = invest.invest_time;
                    behavor.amount = decimal.Parse(invest.investment_amount.ToString());
                    behavor.amountExt = decimal.Parse(invest.BonusAmt.ToString());
                    behavorList.Add(behavor);
                }
                //查找最近30天第一笔回款之后返现记录(useract,rewardtype=1,createtime)
                List<hx_UserAct> cashRewardList = GetCashRewardList(Registerid, time);
                foreach (hx_UserAct cashReward in cashRewardList)
                {
                    Behavor behavor = new Behavor();
                    behavor.type = BehavorType.FX;
                    behavor.time = cashReward.Createtime;
                    behavor.amount = decimal.Parse(cashReward.Amt.ToString());
                    behavorList.Add(behavor);
                }

                //TODO list 按照时间排序，最近三十天内的第一笔回款之后的操作行为保留，其他排除
                behavorList.OrderBy(p => p.time);

                CurrentInvestAmt = CacuBonusBaseAmount(behavorList, InvestAmt, startTime);
                //return GetActAmt(mc, CurrentInvestAmt, TopNum, lifeLoan);                
            }
            return CurrentInvestAmt;

        }
        private decimal CacuBonusBaseAmount(List<Behavor> list, decimal investMoney, DateTime startTime)
        {
            decimal AvailableRepayMoneyTotal = 0.00M;
            decimal RepayMoneyTotal = 0.00M;
            decimal AccountMoneyTotal = 0.00M;
            //查找最近30天回款记录(income_statement)
            //查找最近30天第一笔回款之后充值记录(recharge)
            //查找最近30天第一笔回款之后提现记录(cash)
            //查找最近30天第一笔回款之后投资记录(含红包金额)(bid_records orderstate=1)
            //查找最近30天第一笔回款之后返现记录(useract,rewardtype=1,createtime)

            //TODO list 按照时间排序，最近三十天内的第一笔回款之后的操作行为保留，其他排除
            //不包括本次投标记录???
            //活动开始前 优先作为回款资金
            //活动开始后。。。
            foreach (Behavor be in list)
            {
                if (be.time < DateTime.Now.AddDays(-31))
                {
                    continue;
                }
                if (be.type == BehavorType.HK)
                {
                    AvailableRepayMoneyTotal += be.amount;
                    RepayMoneyTotal += be.amount;
                    AccountMoneyTotal += be.amount + be.amountExt;
                }
                else if (be.type == BehavorType.CZ)
                {
                    AccountMoneyTotal += be.amount;
                }
                else if (be.type == BehavorType.TX)
                {
                    AccountMoneyTotal -= be.amount;
                    if (AccountMoneyTotal < AvailableRepayMoneyTotal)
                    {//TODO  //客户回款1w+200利息,提现8200,活动开始后投资9K,复投按照9K计算利息;
                     //客户回款1w+2000利息,提现8200,充值7000,活动开始后投资9k,复投按照2k算
                        AvailableRepayMoneyTotal = AccountMoneyTotal;
                    }
                }
                else if (be.type == BehavorType.TZ)
                {
                    if (be.time < startTime)
                    {
                        AccountMoneyTotal -= be.amount;
                        AccountMoneyTotal += be.amountExt;//红包奖励
                        if (AccountMoneyTotal < AvailableRepayMoneyTotal)
                        {
                            AvailableRepayMoneyTotal = AccountMoneyTotal;
                        }
                    }
                    else
                    {
                        AccountMoneyTotal -= be.amount;
                        AccountMoneyTotal += be.amountExt;
                        AvailableRepayMoneyTotal -= be.amount;
                        RepayMoneyTotal -= be.amount;
                    }
                }
                else if (be.type == BehavorType.FX)
                {
                    AccountMoneyTotal += be.amount;
                }
            }
            if (AvailableRepayMoneyTotal < 0)
            {
                AvailableRepayMoneyTotal = 0;
            }

            if (investMoney < AvailableRepayMoneyTotal)
            {
                return investMoney;
            }
            //TODO ---??是否给返利 不确定
            //else if (investMoney > AccountMoneyTotal)//说明账户30天之前就有可用余额
            //{
            //    //TODO ---??是否给返利 不确定
            //    //if (investMoney < RepayMoneyTotal)//投资金额小于总回款额度,以投资金额为准
            //    //{
            //    //    return investMoney;
            //    //}
            //    //else
            //    //{
            //    //    return RepayMoneyTotal;//投资金额大于总回款额度,以总回款额度为准
            //    //}
            //    return 0.00M;
            //}
            else
            {
                return AvailableRepayMoneyTotal;
            }

            // return 0.00M;
        }

        #endregion
        //查找最近30天回款记录(income_statement)
        private List<hx_income_statement> GetIncomeStatementList(int userid, int days)
        {
            var query = ef.hx_income_statement.Where(p => p.investor_registerid == userid && p.payment_status == 1);//recharge_amount,recharge_time
            if (days != 0)
            {
                query = query.Where(p => p.repayment_period != null && DbFunctions.DiffDays(p.repayment_period, DateTime.Now) <= 30);
            }
            List<hx_income_statement> list = query.OrderBy(p => p.repayment_period).ToList();
            return list;
        }

        //查找某天之后的所有充值记录(recharge)
        private List<hx_Recharge_history> GetRechargeList(int userid, DateTime time)
        {
            var query = ef.hx_Recharge_history.Where(p => p.membertable_registerid == userid && p.recharge_condition == 1);//recharge_amount,recharge_time
            if (time != null)
            {
                query = query = query.Where(p => p.recharge_time > time);
            }
            List<hx_Recharge_history> list = query.OrderBy(p => p.recharge_time).ToList();
            return list;
        }
        //查找某天之后提现记录(cash)
        private List<hx_td_UserCash> GetCashList(int userid, DateTime time)
        {
            var query = ef.hx_td_UserCash.Where(p => p.registerid == userid && p.OrdIdState == 3);//select TransAmt,OperTime from hx_td_UserCash where  OrdIdState=3
            if (time != null)
            {
                query = query.Where(p => p.OperTime > time);
            }
            List<hx_td_UserCash> list = query.OrderBy(p => p.OperTime).ToList();
            return list;
        }
        //查找某天之后投资记录(含红包金额)(bid_records orderstate=1)
        private List<hx_Bid_records> GetInvestList(int userid, DateTime time)
        {
            var query = ef.hx_Bid_records.Where(p => p.investor_registerid == userid && p.ordstate == 1);
            if (time != null)
            {
                query = query.Where(p => p.invest_time > time);
            }
            List<hx_Bid_records> list = query.OrderBy(p => p.invest_time).ToList();
            return list;
        }
        //查找某天之后返现记录(useract,rewardtype=1,createtime)
        private List<hx_UserAct> GetCashRewardList(int userid, DateTime time)
        {
            var query = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 1);
            if (time != null)
            {
                query = query.Where(p => p.Createtime > time);
            }
            List<hx_UserAct> list = query.OrderBy(p => p.Createtime).ToList();
            return list;
        }


    }
    public class Behavor
    {
        /// <summary>
        /// 1.充值c 2.投资tz 3.回款h 4.提现t 5返现f
        /// </summary>
        public BehavorType type { get; set; }
        public DateTime? time { get; set; }
        public decimal amount { get; set; }
        /// <summary>
        /// 利息/红包奖励、返现等
        /// </summary>
        public decimal amountExt { get; set; }
    }
    public enum BehavorType
    {
        CZ = 1, TZ = 2, HK = 3, TX = 4, FX = 5, QT = 6
    }
}
