using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 活动解析操作类-- 邀请好友活动奖励
    /// </summary>
    class ActInvite : ActBase
    {
        /// <summary>
        /// 邀请奖历 投资成功奖励
        /// </summary>
        /// <param name="dt"></param>
        protected internal void SendBonusForInviteAfterInvest(DataTable dt, string targetPlatform)
        //(int registerid, string invcode, int invcount, decimal investAmt, string userName, string borrowing_title, decimal bonusAmt)
        {
            if (dt.Rows.Count <= 0)
            {
                return;
            }
            //投资金额
            decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            int registerid = int.Parse(dt.Rows[0]["investor_registerid"].ToString()); //投资人ID
            int bid_records_id = int.Parse(dt.Rows[0]["bid_records_id"].ToString());
            int life_of_loan = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
            int unit_day = int.Parse(dt.Rows[0]["unit_day"].ToString());
            if (unit_day != 1) { life_of_loan = 0; }
            string borrowing_title = dt.Rows[0]["borrowing_title"].ToString();
            string userName = dt.Rows[0]["username"].ToString();
            string invcode = dt.Rows[0]["invitationcode"].ToString();
            int targetid = int.Parse(dt.Rows[0]["targetid"].ToString());//标的ID
            //本次投标使用的奖励金额
            decimal bonusAmt = decimal.Parse(dt.Rows[0]["bonusAmt"].ToString());
            B_bonus_account_water bbaw = new B_bonus_account_water();
            int invcount = B_usercenter.GetInvestCountByUserid(registerid);//投资次数

            string uid = registerid.ToString();
            LogInfo.WriteLog("投资人ID：" + registerid + "； /*邀请码*/:" + invcode);
            #region MyRegion  邀请奖历 投资成功奖励   暂不启用false  待重新梳理帐号fangjianmin
            if (invcode != null && invcode != "")// && false
            {
                DateTime dte = DateTime.Now;
                string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where invcode='" + invcode + "' and invpersonid=" + uid + " ";//查询本人是否已经被邀请注册过
                LogInfo.WriteLog("投资人ID：" + registerid + "；查询本人是否已经被邀请注册过:" + codesql);
                DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
                if (dtcode.Rows.Count > 0)
                {
                    int uuid = int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()); //邀请用户id
                    B_member_table oy = new B_member_table();
                    M_member_table py = new M_member_table();
                    //获取 邀请用户身份渠道用户不执行
                    py = oy.GetModel(uuid);
                    if (py.useridentity != 4)
                    {
                        DateTime invtime = Convert.ToDateTime(dtcode.Rows[0]["invtime"].ToString()); //邀请时间

                        DateTime nowdate = DateTime.Now;
                        DateTime startdate = new DateTime(2017, 1, 6, 0, 00, 00);
                        DateTime enddate = new DateTime(2017, 3, 31, 23, 59, 59);
                        if ((nowdate > startdate && nowdate < enddate) && (invtime > startdate && invtime < enddate))
                        {
                            #region 1月6日活动，奖励邀请人首投满2000返现10元
                            if (!string.IsNullOrWhiteSpace(py.UsrCustId))
                            {
                                B_borrowing_target bbt = new B_borrowing_target();
                                M_borrowing_target mbt = new M_borrowing_target();
                                mbt = bbt.GetModel(targetid);
                                if (mbt.project_type_id != 6)//排除新手标
                                {


                                    string log = "用户" + registerid + "活动奖励邀请人首投满2000返现10元: ";
                                    decimal amtc = 0;
                                    DataTable dtstAmt = B_usercenter.GetInvestCountByUseridNew(registerid);
                                    if (dtstAmt.Rows.Count > 0)
                                    {
                                        log += "<br> 投资次数" + dtstAmt.Rows.Count + "次";
                                        if (dtstAmt.Rows.Count == 1)
                                        {
                                            amtc = decimal.Parse(dtstAmt.Rows[0]["InvCount_Amt"].ToString());
                                            log += "<br>投资金额" + amtc;
                                            if (amtc >= 2000)
                                            {
                                                hx_UserAct huact = InviteActCashNew(uuid, targetPlatform, amtc, 4, 1, 10, 5)[0];

                                                if (huact != null && huact.ActID != null)
                                                {
                                                    log += "<br>奖励发放成功";
                                                    #region MyRegion  奖励流水
                                                    string awardDescription = string.Format("邀请好友首投满2000成功,获得{0}{1}", Convert.ToDouble(huact.Amt.ToString()), GetBunusDescription(huact.RewTypeID));
                                                    AddBonusAccoutWater(huact.UserAct, uuid, decimal.Parse(huact.Amt.ToString()), awardDescription, registerid); //registerid 被邀请人 uuid邀请人
                                                    #endregion MyRegion  奖励流水

                                                    #region MyRegion  系统消息
                                                    string MContext = string.Format("尊敬的用户:邀请好友{0}首投{1}金额为{2}，获得{3}{4}如有问题可咨询创利投的客服！", userName, borrowing_title, investAmt, decimal.Parse(huact.Amt.ToString()), GetBunusDescription(huact.RewTypeID));
                                                    AddSytemMessage(uuid, "邀请好友首投满2000成功奖励", MContext);
                                                    #endregion

                                                    #region 现金流水信息
                                                    if (huact.RewTypeID == 1)
                                                    {
                                                        B_usercenter o = new B_usercenter();
                                                        decimal di = o.GetUsridAvailable_balance(uuid);
                                                        //di = di + decimal.Parse(huact.Amt.ToString());
                                                        StringBuilder strSql = new StringBuilder();
                                                        strSql.Append("insert into hx_Capital_account_water(");
                                                        strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                                        strSql.Append(" values (");
                                                        strSql.Append("" + uuid + "," + decimal.Parse(huact.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.邀请奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + awardDescription + "')");
                                                        DbHelperSQL.RunSql(strSql.ToString());
                                                        strSql.Clear();
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                    LogInfo.WriteLog(log);
                                }
                            }
                            #endregion
                            #region 受邀好友首次成功投资给予活动奖励   常规邀请
                            //获取标的期限
                            life_of_loan = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
                            unit_day = int.Parse(dt.Rows[0]["unit_day"].ToString());
                            int lol = 0;
                            if (unit_day == 1)
                            {
                                lol = life_of_loan;
                            }
                            #region 邀请好友投资成功返现（奖励邀请人）
                            if (!string.IsNullOrWhiteSpace(py.UsrCustId))
                            {
                                hx_UserAct hut = InviteActBonus(uuid, targetPlatform, int.Parse(dtcode.Rows[0]["invpersonid"].ToString()), investAmt, 1, 4, 1, 0, 5, lol, "邀请好友投资成功返现")[0];//
                                if (hut != null && hut.ActID != null)
                                {
                                    #region MyRegion  奖励流水
                                    string awardDescription = string.Format("邀请好友投资成功,获得{0}{1}", Convert.ToDouble(hut.Amt.ToString()), GetBunusDescription(hut.RewTypeID));
                                    AddBonusAccoutWater(hut.UserAct, uuid, decimal.Parse(hut.Amt.ToString()), awardDescription, registerid); //registerid 被邀请人 uuid邀请人
                                    #endregion MyRegion  奖励流水

                                    #region MyRegion  系统消息
                                    string MContext = string.Format("尊敬的用户:邀请好友{0}投资{1}金额为{2}，获得{3}{4}如有问题可咨询创利投的客服！", userName, borrowing_title, investAmt, hut.Amt.ToString(), GetBunusDescription(hut.RewTypeID));
                                    AddSytemMessage(uuid, "邀请好友投资成功奖励", MContext);
                                    #endregion

                                    #region 现金流水信息
                                    if (hut.RewTypeID == 1)
                                    {
                                        B_usercenter o = new B_usercenter();
                                        decimal di = o.GetUsridAvailable_balance(uuid);
                                        //di = di + decimal.Parse(hut.Amt.ToString());
                                        StringBuilder strSql = new StringBuilder();
                                        strSql.Append("insert into hx_Capital_account_water(");
                                        strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                        strSql.Append(" values (");
                                        strSql.Append("" + uuid + "," + decimal.Parse(hut.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.邀请奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + awardDescription + "')");
                                        DbHelperSQL.RunSql(strSql.ToString());
                                        strSql.Clear();
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            #region 被邀请投资成功返现（奖励被邀请人）
                            int byquuid = int.Parse(dtcode.Rows[0]["invpersonid"].ToString());
                            hx_UserAct byhut = InviteActBonus(byquuid, targetPlatform, 0, investAmt, 1, 4, 1, 0, 5, lol, "被邀请投资返现")[0];//
                            if (byhut != null && byhut.ActID != null)
                            {
                                #region MyRegion  奖励流水
                                string awardDescription = string.Format("被邀请投资成功,获得{0}{1}", Convert.ToDouble(byhut.Amt.ToString()), GetBunusDescription(byhut.RewTypeID));
                                AddBonusAccoutWater(byhut.UserAct, byquuid, decimal.Parse(byhut.Amt.ToString()), awardDescription, registerid); //registerid 被邀请人 uuid邀请人
                                #endregion MyRegion  奖励流水

                                #region MyRegion  系统消息
                                string MContext = string.Format("尊敬的用户:被邀请投资{0}金额为{1}，获得{2}{3}如有问题可咨询创利投的客服！", borrowing_title, investAmt, byhut.Amt.ToString(), GetBunusDescription(byhut.RewTypeID));
                                AddSytemMessage(byquuid, "被邀请投资成功奖励", MContext);
                                #endregion

                                #region 现金流水信息
                                if (byhut.RewTypeID == 1)
                                {
                                    B_usercenter o = new B_usercenter();
                                    decimal di = o.GetUsridAvailable_balance(byquuid);
                                    //di = di + decimal.Parse(byhut.Amt.ToString());
                                    StringBuilder strSql = new StringBuilder();
                                    strSql.Append("insert into hx_Capital_account_water(");
                                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                    strSql.Append(" values (");
                                    strSql.Append("" + byquuid + "," + decimal.Parse(byhut.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.邀请奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + awardDescription + "')");
                                    DbHelperSQL.RunSql(strSql.ToString());
                                    strSql.Clear();
                                }
                                #endregion
                            }
                            #endregion
                            #endregion
                        }



                        #region 这里现金返现  ??? 投资金额为-1 作了单独处理。注册？？ RewTypeID-1 ？？？
                        //hx_UserAct ua = InviteActBonus(uuid, int.Parse(dtcode.Rows[0]["invpersonid"].ToString()), -1M, 1, 4, 1, 0, 5)[0];//???
                        #endregion
                        //}
                        //else if (invcount > 1)
                        //{
                        //    #region 受邀用户续投奖励 邀请活动正常续投
                        //    int biyaoUsrid = int.Parse(dtcode.Rows[0]["invpersonid"].ToString());
                        //    decimal totAmt = B_usercenter.GetInviUserTotalAmt(uuid, biyaoUsrid);//????这里需要取出受邀用户奖总数
                        //    // hx_UserAct hut = aci.InviteActCash(uuid, biyaoUsrid, decimal.Parse(p.TransAmt), 4, 2, 1, totAmt, 5);
                        //    hx_UserAct hut = InviteActBonus(uuid, int.Parse(dtcode.Rows[0]["invpersonid"].ToString()), investAmt - bonusAmt, 1, 4, 1, totAmt, 5)[0];//????
                        //    if (hut != null && hut.ActID != null)
                        //    {
                        //        #region MyRegion  奖励流水
                        //        string awardDescription = string.Format("邀请好友续投成功获得{0}{1}", hut.Amt.ToString(), GetBunusDescription(hut.RewTypeID));
                        //        AddBonusAccoutWater(hut.UserAct, uuid, decimal.Parse(hut.Amt.ToString()), awardDescription, registerid); //registerid 被邀请人 uuid邀请人
                        //        #endregion MyRegion  奖励流水

                        //        #region MyRegion  系统消息
                        //        string MContext = string.Format("尊敬的用户:邀请好友续投{1}金额为{2}，获得{3}{4}如有问题可咨询创利投的客服！", userName, borrowing_title, investAmt, hut.Amt.ToString(), GetBunusDescription(hut.RewTypeID));
                        //        AddSytemMessage(uuid, "邀请好友续投奖励", MContext);
                        //        #endregion                                  

                        //        #region 现金奖励 资金流水信息
                        //        if (hut.RewTypeID == 1)
                        //        {
                        //            B_usercenter o = new B_usercenter();
                        //            decimal di = o.GetUsridAvailable_balance(uuid);
                        //            // di = di + decimal.Parse(hut.Amt.ToString());
                        //            StringBuilder strSql = new StringBuilder();
                        //            strSql.Append("insert into hx_Capital_account_water(");
                        //            strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                        //            strSql.Append(" values (");
                        //            strSql.Append("" + uuid + "," + decimal.Parse(hut.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.邀请奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + awardDescription + "')");
                        //            LogInfo.WriteLog("续投奖励流水语句:" + strSql.ToString());
                        //            DbHelperSQL.RunSql(strSql.ToString());
                        //            strSql.Clear();
                        //        }
                        //        #endregion
                        //    }
                        //    #endregion
                        //}
                        /*
                            DbHelperSQL.RunSql(" update hx_td_Userinvitation  set InvitesStates=1  where invcode='" + invcode + "' and invpersonid=" + uid + " and  InvitesStates=2 ");
                            LogInfo.WriteLog(" 后台更新数据邀请状态 update hx_td_Userinvitation  set InvitesStates=1  where invcode='" + invcode + "' and invpersonid=" + uid + " and  InvitesStates=2 ");
                       */
                    }
                }
            }
            #endregion

        }
        /// <summary>
        /// 邀请注册奖励活动
        /// </summary>
        /// <param name="registerid">注册用户id</param>
        /// <param name="ActUser">面向用户 0不限  1首次投资用户 2非首投用户 3每标首投用户 4每标最大投资用户 5所有投资用户</param>
        protected internal void SendBonusForInviteAfterRegister(int registerid, string targetPlatform, int ActUser = 0)
        {
            #region MyRegion   //邀请奖励处理逻辑,暂无邀请奖励
            bool flag = true;
            if (flag)
            {
                if (registerid < 0)
                {
                    return;
                }
                DateTime dte = DateTime.Now;
                string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where invpersonid=" + registerid.ToString();//查询本人是否已经被邀请注册过 invcode='" + invcode + "' and 
                LogInfo.WriteLog("codesql2:" + codesql);
                DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
                //邀请奖励处理逻辑
                if (dtcode.Rows.Count > 0)
                {
                    B_member_table oy = new B_member_table();
                    M_member_table py = new M_member_table();
                    //获取邀请用户id
                    int invpeopleid = int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString());
                    py = oy.GetModel(invpeopleid);
                    if (py.useridentity != 4 && py.iD_number != "")//渠道用户不执行
                    {
                        DateTime nowdate = DateTime.Now;
                        DateTime startdate = new DateTime(2017, 1, 6, 0, 00, 00);
                        DateTime enddate = new DateTime(2017, 3, 31, 23, 59, 59);
                        DateTime invtime = Convert.ToDateTime(dtcode.Rows[0]["invtime"].ToString()); //邀请时间

                        #region 生成奖励，记录流水

                        //??待优化到奖励内部记录
                        if ((nowdate > startdate && nowdate < enddate) && (invtime > startdate && invtime < enddate))
                        {
                            List<hx_UserAct> hut = InviteActBonus(py.registerid, targetPlatform, registerid, -1M, 0, 4, 1, 0, 5);
                            string strlog = "用户：" + py.registerid + ",邀请奖励返回实体：" + hut.Count;
                            if (hut.Count > 0)
                            {
                                foreach (hx_UserAct ua in hut)
                                {
                                    if (ua.Amt != null && ua.Amt > 0)
                                    {
                                        string sql = "update hx_td_Userinvitation set Invitereward=" + ua.Amt + ",UserAct= " + ua.UserAct + " where invpersonid=" + int.Parse(dtcode.Rows[0]["invpersonid"].ToString());
                                        strlog += "；更新邀请表:" + sql;
                                        DbHelperSQL.ExecuteSql(sql);

                                        #region MyRegion  记录奖励流水表
                                        string awardDescription = string.Format("邀请好友注册成功,获得{0}{1}", Convert.ToDouble(ua.Amt.ToString()), GetBunusDescription(ua.RewTypeID));
                                        AddBonusAccoutWater(ua.UserAct, py.registerid, decimal.Parse(ua.Amt.ToString()), awardDescription, registerid);
                                        #endregion
                                        strlog += "；" + awardDescription;
                                        #region MyRegion  发送系统消息
                                        string MContext = string.Format("尊敬的用户：您邀请好友注册成功,获得{0}{1}如有问题可咨询创利投的客服！", ua.Amt.ToString(), GetBunusDescription(ua.RewTypeID));
                                        AddSytemMessage(py.registerid, "邀请好友注册奖励", MContext);
                                        #endregion
                                        strlog += "；ua.RewTypeID：" + ua.RewTypeID;
                                        if (ua.RewTypeID == 1)
                                        {
                                            #region 资金流水信息
                                            B_usercenter o = new B_usercenter();
                                            decimal di = o.GetUsridAvailable_balance(py.registerid);
                                            //di = di + decimal.Parse(ua.Amt.ToString());

                                            StringBuilder strSql = new StringBuilder();
                                            strSql.Append("insert into hx_Capital_account_water(");
                                            strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                            strSql.Append(" values (");
                                            strSql.Append("" + py.registerid + "," + decimal.Parse(ua.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.邀请奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + awardDescription + "')");
                                            strlog += "；邀请好友注册奖励流水语句:" + strSql.ToString();
                                            DbHelperSQL.RunSql(strSql.ToString());
                                            strSql.Clear();
                                            #endregion
                                        }
                                    }
                                }
                            }
                            LogInfo.WriteLog(strlog);
                        }
                        #endregion

                        #region 11月活动，每邀请5位好友实名注册，奖励邀请人2%加息券一张

                        if ((nowdate > startdate && nowdate < enddate) && (invtime > startdate && invtime < enddate))
                        {
                            string log = "11月活动用户" + invpeopleid + "每邀请5位好友，当前好友(" + registerid + ")实名注册奖励邀请人2%加息券一张: ";
                            //查询活动期间内邀请人累计邀请实名注册人数
                            string strSQL = string.Format("select COUNT(mt.registerid) recount from hx_td_Userinvitation ut inner join hx_member_table mt on ut.invpersonid=mt.registerid where mt.iD_number <> '' and mt.useridentity <> 4 and mt.registration_time >= '{0}' and mt.registration_time <= '{1}' and ut.Invpeopleid={2}", startdate.ToString("yyyy-MM-dd HH:mm:ss"), enddate.ToString("yyyy-MM-dd HH:mm:ss"), invpeopleid);
                            log += "查询活动期间邀请实名人数sql:" + strSQL;
                            DataTable dtcount = DbHelperSQL.GET_DataTable_List(strSQL);
                            if (dtcount.Rows.Count > 0)
                            {

                                int recount = int.Parse(dtcount.Rows[0]["recount"].ToString());
                                log += "<br>累计实名人数：" + recount;
                                if (recount > 0)
                                {
                                    if (recount % 5 == 0)
                                    {
                                        log += "<br>实名人数" + dtcount.Rows.Count + "人，执行奖励操作";
                                        List<hx_UserAct> t = new List<hx_UserAct>();
                                        hx_ActivityTable hat = new hx_ActivityTable();
                                        hat = GetActTableInfo(4, targetPlatform, ActUser, 1, 1); //赋值11月活动RewTypeID为1 ，主要获取活动起止时间，真正发放奖励hua.RewTypeID是3 加息券
                                        if (hat != null && hat.ActID > 0)
                                        {
                                            hx_UserAct hua = new hx_UserAct();
                                            hua.ActTypeId = hat.ActTypeId; //活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送
                                            hua.registerid = invpeopleid; //邀请人ID
                                            hua.RewTypeID = 3;//奖励类型id 1现金/2抵扣券/3加息券
                                            hua.ActID = hat.ActID;
                                            hua.Amt = 2;//2%加息券
                                            hua.Uselower = 100;
                                            hua.Usehight = 0;
                                            hua.AmtEndtime  = DateTime.Now.Date.AddDays(31).AddSeconds(-1);// DateTime.Parse(hat.ActEndtime.ToString());
                                            hua.AmtUses = 1; //使用要求 1单独使用,2组合使用
                                            hua.UseState = 0;  //使用状态 0 未使用 1 已使用 2已过期 3 锁定中
                                            hua.AmtProid = 0; //未使用默认为0
                                            hua.ISSmsOne = 0;
                                            hua.IsSmsThree = 0;
                                            hua.isSmsFifteen = 0;
                                            hua.IsSmsSeven = 0;
                                            hua.isSmsSixteen = 0;
                                            hua.Createtime = DateTime.Now;
                                            hua.Title = "11月活动每邀请5位好友送2%加息券";
                                            hua.UseLifeLoan = "3-0";
                                            ef.hx_UserAct.Add(hua);
                                            int i = ef.SaveChanges();
                                            if (i > 0)
                                            {
                                                log += "<br>2%加息券奖励发放成功";
                                                if (hua.Amt != null && hua.Amt > 0)
                                                {
                                                    #region MyRegion  发送系统消息
                                                    string MContext = string.Format("尊敬的用户：您邀请{0}好友实名注册成功,获得{1}{2}如有问题可咨询创利投的客服！", recount, hua.Amt.ToString(), GetBunusDescription(hua.RewTypeID));
                                                    AddSytemMessage(registerid, "邀请好友注册奖励", MContext);
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            LogInfo.WriteLog(log);
                        }
                        #endregion
                        #region 11月活动，被邀请人实名注册，奖励2%加息券一张
                        if ((nowdate > startdate && nowdate < enddate) && (invtime > startdate && invtime < enddate))
                        {
                            string log = "11月活动，被邀请人(" + registerid + ")实名注册奖励2%加息券一张: ";

                            List<hx_UserAct> t = new List<hx_UserAct>();
                            hx_ActivityTable hat = new hx_ActivityTable();
                            hat = GetActTableInfo(4, targetPlatform, ActUser, 1, 1); //赋值11月活动RewTypeID为1 ，主要获取活动起止时间，真正发放奖励hua.RewTypeID是3 加息券
                            if (hat != null && hat.ActID > 0)
                            {
                                hx_UserAct hua = new hx_UserAct();
                                hua.ActTypeId = hat.ActTypeId; //活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送
                                hua.registerid = registerid; //注册人ID
                                hua.RewTypeID = 3;//奖励类型id 1现金/2抵扣券/3加息券
                                hua.ActID = hat.ActID;
                                hua.Amt = 2;//2%加息券
                                hua.Uselower = 100;
                                hua.Usehight = 0;
                                hua.AmtEndtime = DateTime.Now.Date.AddDays(31).AddSeconds(-1);// DateTime.Parse(hat.ActEndtime.ToString());
                                hua.AmtUses = 1; //使用要求 1单独使用,2组合使用
                                hua.UseState = 0;  //使用状态 0 未使用 1 已使用 2已过期 3 锁定中 
                                hua.AmtProid = 0; //未使用默认为0
                                hua.ISSmsOne = 0;
                                hua.IsSmsThree = 0;
                                hua.isSmsFifteen = 0;
                                hua.IsSmsSeven = 0;
                                hua.isSmsSixteen = 0;
                                hua.Createtime = DateTime.Now;
                                hua.Title = "11月活动实名注册送2%加息券";
                                hua.UseLifeLoan = "3-0";
                                //待添加加息券使用标的
                                ef.hx_UserAct.Add(hua);
                                int i = ef.SaveChanges();
                                if (i > 0)
                                {
                                    log += "<br>2%加息券奖励发放成功";
                                    if (hua.Amt != null && hua.Amt > 0)
                                    {
                                        #region MyRegion  发送系统消息
                                        string MContext = string.Format("尊敬的用户：您实名注册成功,获得{0}{1}如有问题可咨询创利投的客服！", hua.Amt.ToString(), GetBunusDescription(hua.RewTypeID));
                                        AddSytemMessage(registerid, "邀请好友注册奖励", MContext);
                                        #endregion

                                    }
                                }
                            }
                            LogInfo.WriteLog(log);
                        }
                        #endregion
                    }
                }
            }
            #endregion
        }

        #region 好友邀请 活动奖励
        /// <summary>
        /// 好友邀请 活动奖励
        /// </summary>
        /// <param name="Registerid">邀请人ID</param>
        /// <param name="InvitedRegisterid">被邀请人ID</param>
        /// <param name="InvestAmt">投次金额</param>
        /// <param name="RewTypeID">奖励类型????</param>
        /// <param name="ActTypeId">活动类型</param>
        /// <param name="require"></param>
        /// <param name="totalAmt">奖励金额</param>
        /// <param name="ActUser">面向用户</param>
        /// <param name="LifeOfLoan">借款期限</param>
        /// <returns></returns>
        private List<hx_UserAct> InviteActBonus(int Registerid, string targetPlatform, int InvitedRegisterid, decimal InvestAmt, int RewTypeID, int ActTypeId = 1, int require = 1, decimal totalAmt = 0M, int ActUser = 0, int LifeOfLoan = 0, string YaoQingType = "")
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = new hx_ActivityTable();
            hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1, RewTypeID);
            if (hat != null && hat.ActID > 0)
            {
                if (hat.RewTypeID == 1) //现金奖励
                {
                    t = InviteActCash(Registerid, targetPlatform, InvitedRegisterid, InvestAmt, int.Parse(hat.ActTypeId.ToString()), RewTypeID, require, totalAmt, ActUser, LifeOfLoan, YaoQingType);
                }
                else if (hat.RewTypeID == 2) //抵扣券奖励
                {
                    t = InviteActCoupon(Registerid, targetPlatform, InvestAmt, int.Parse(hat.ActTypeId.ToString()), ActUser);
                }
                else if (hat.RewTypeID == 3) //加息券奖励
                {
                    t = InviteActInterestRatesPlus(Registerid, targetPlatform, InvestAmt, int.Parse(hat.ActTypeId.ToString()), ActUser);
                }
            }
            return t;
        }
        #endregion

        #region 邀请好友 现金奖励
        /// <summary>
        /// 邀请好友 现金奖励
        /// </summary>
        /// <param name="Registerid">邀请人用户id</param>
        /// <param name="biyaoUsrid">被邀请用户id</param>
        /// <param name="InvestAmt">投资金额</param>
        /// <param name="ActTypeId">活动类型</param>
        /// <param name="RewTypeID">奖励类型</param>
        /// <param name="require">活动规则条件</param>
        /// <param name="totalAmt">已获得奖励金额</param>
        /// <param name="ActUser">面向用户对象</param>
        /// <param name="LifeOfLoan">借款期限</param>
        /// <returns></returns>   
        private List<hx_UserAct> InviteActCash(int Registerid, string targetPlatform, int biyaoUsrid, decimal InvestAmt, int ActTypeId, int RewTypeID, int require = 1, decimal totalAmt = 0M, int ActUser = 0, int LifeOfLoan = 0, string YaoQingType = "")
        {   //RewTypeID=1
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = new hx_ActivityTable();

            hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1, RewTypeID);
            if (hat != null)
            {
                #region 邀请好友现金奖励
                string ActRule = hat.ActRule;
                List<MAmtList> mlist = new List<MAmtList>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                MActCash mc = new MActCash();
                mc = js.Deserialize<MActCash>(ActRule);
                decimal actamt = 0M;
                totalAmt = B_usercenter.GetTopAmtCount(hat.ActID);
                //检查奖历邀请总佣金
                if (mc.TopAmt > totalAmt)
                {
                    if (mc.require == 1 && InvestAmt == -1M) //受邀好友注册成功发放红包---//首次成功投资
                    {
                        actamt = mc.Cash;
                        //发放红包
                        //hx_UserAct hua = new hx_UserAct();
                        //hua.RewTypeID = 2;
                        //t.Add(hua);

                        hx_UserAct hua = new hx_UserAct();
                        hua.ActTypeId = hat.ActTypeId;
                        hua.registerid = Registerid;
                        hua.RewTypeID = 2;//hat.RewTypeID;
                        hua.ActID = hat.ActID;
                        hua.Amt = actamt;//item.cashAmt;
                        hua.Uselower = 500; //item.startAmt;
                        hua.Usehight = 0;//item.endAmt;
                        hua.AmtEndtime = hua.AmtEndtime = DateTime.Now.Date.AddDays(31).AddSeconds(-1);// hat.ActEndtime;//item.endTime;
                        hua.AmtUses = 2;//mcp.Uses; //没指定情况下默认为单独使用
                        hua.UseState = 0;  //现金未转账
                        hua.AmtProid = 0; //未使用默认为0
                        hua.ISSmsOne = 0;
                        hua.IsSmsThree = 0;
                        hua.isSmsFifteen = 0;
                        hua.IsSmsSeven = 0;
                        hua.isSmsSixteen = 0;
                        hua.Createtime = DateTime.Now;
                        hua.Title = hat.ActName;
                        hua.UseLifeLoan = "0-0";//string.IsNullOrWhiteSpace(item.UseLifeLoan) ? "" : item.UseLifeLoan;
                        ef.hx_UserAct.Add(hua);
                        int i = ef.SaveChanges();
                        t.Add(hua);
                    }
                    else if (mc.require1 == 1 && InvestAmt > 0) //续投按一定金额赠送
                    {
                        actamt = GetYaoActAMT(mc, InvestAmt, LifeOfLoan);//奖励邀请人与被邀请人返现金额（邀请人与被邀请人返现规则一直）


                        #region 奖励
                        if (actamt > 0 && actamt <= mc.TopAmt1)//大于 0里写入对应的奖励数据
                        {
                            hx_UserAct hua = new hx_UserAct();
                            hua.ActTypeId = hat.ActTypeId;
                            hua.registerid = Registerid;
                            hua.RewTypeID = hat.RewTypeID;
                            hua.ActID = hat.ActID;
                            hua.Amt = actamt;
                            hua.Uselower = 0.00M;
                            hua.Usehight = 0.00M;
                            hua.UseState = 5;  //现金未转账
                            hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString());
                            hua.OrderID = decimal.Parse(Utils.Createcode());
                            hua.AmtUses = 1; //没指定情况下默认为单独使用                       
                            hua.AmtProid = 0; //未使用默认为0
                            hua.ISSmsOne = 0;
                            hua.IsSmsThree = 0;
                            hua.isSmsFifteen = 0;
                            hua.IsSmsSeven = 0;
                            hua.isSmsSixteen = 0;
                            hua.Createtime = DateTime.Now;
                            hua.Title = YaoQingType;// "邀请好友返现";
                            ef.hx_UserAct.Add(hua);
                            int i = ef.SaveChanges();
                            if (i > 0)
                            {
                                //录入成功，后进行转账操作
                                //1.获取用户对向
                                M_member_table p = new M_member_table();
                                B_member_table o = new B_member_table();
                                p = o.GetModel(Registerid);
                                if (p != null)
                                {
                                    //2.调用商户向用户转账接口
                                    Transfer tf = new Transfer();
                                    ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
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
                                                int ic = BUC.UpateActToUserTransfer(retf, 0);
                                                if (ic > 0)
                                                {
                                                    //处理 资金流水信息 奖励流水  系统消息  
                                                    t.Add(hua); //????
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }


                }
                #endregion
            }
            return t;
        }
        #endregion

        #region 邀请好友 抵扣券奖励
        /// <summary>
        /// 常规活动 抵扣券
        /// </summary>
        /// <param name="registerid"></param>
        /// <param name="investAmt"></param>
        /// <returns></returns>
        private List<hx_UserAct> InviteActCoupon(int registerid, string targetPlatform, decimal investAmt, int ActTypeId, int ActUser = 0)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1);
            if (hat != null)
            {
                hx_UserAct hua = new hx_UserAct();
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //获取用户活动表对象
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        #endregion

        #region 邀请好友活动  加息券奖励
        /// <summary>
        /// 活动加息券
        /// </summary>
        /// <param name="registerid"></param>
        /// <param name="investAmt"></param>
        /// <param name="ActTypeId">活动类型</param>
        /// <param name="ActUser"></param>
        /// <returns></returns>
        private List<hx_UserAct> InviteActInterestRatesPlus(int registerid, string targetPlatform, decimal investAmt, int ActTypeId = 3, int ActUser = 0)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1);
            if (hat != null)
            {
                hx_UserAct hua = new hx_UserAct();
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //获取用户活动表对象
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        #endregion

        #region 计算邀请奖励金额，按投资比例赠送???? 加标期限
        protected decimal GetYaoActAMT(MActCash mc, decimal InvestAmt, int lifeLoan = 0)
        {
            decimal amt = 0.00M;

            foreach (MAmtList item in mc.MAmtList)
            {
                if ((item.startAmt <= InvestAmt || InvestAmt == -1) && (item.endAmt == 0 || item.endAmt >= InvestAmt) && (lifeLoan == 0 || item.LifeLoan == 0 || item.LifeLoan == lifeLoan))
                {
                    if (mc.require == 1)  //按百分比赠送  
                    {
                        amt = (item.percent / 100) * InvestAmt;
                        if (amt > mc.TopAmt1 && mc.TopAmt1 != 0)//大于单比封顶金额
                        {
                            amt = mc.TopAmt1;
                        }
                    }
                    else if (mc.require == 2)//2 投投资赠送
                    {
                        amt = item.percent;
                    }
                    break;
                }
            }

            return amt;
        }
        #endregion

        #region 邀请好友 现金奖励(11月活动被邀请人投资满2000奖励邀请人10元现金)
        /// <summary>
        /// 邀请好友 现金奖励
        /// </summary>
        /// <param name="Registerid">邀请人用户id</param>
        /// <param name="InvestAmt">投资金额</param>
        /// <param name="ActTypeId">活动类型（1新人注册，3常规活动，4邀请奖励）</param>
        /// <param name="RewTypeID">奖励类型(1现金，2抵扣券，3加息券)</param>
        /// <param name="totalAmt">奖励金额</param>
        /// <param name="ActUser">面向用户对象</param>
        /// <returns></returns>   
        private List<hx_UserAct> InviteActCashNew(int Registerid, string targetPlatform, decimal InvestAmt, int ActTypeId, int RewTypeID, decimal totalAmt = 0M, int ActUser = 0)
        {   //RewTypeID=1
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = new hx_ActivityTable();

            hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1, RewTypeID);
            if (hat != null)
            {
                #region 邀请好友现金奖励
                decimal actamt = 0M;

                if (InvestAmt >= 2000) //
                {
                    actamt = totalAmt;//奖励邀请人
                }
                #region 奖励
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
                    hua.UseState = 5;  //现金未转账
                    hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString());
                    hua.OrderID = decimal.Parse(Utils.Createcode());
                    hua.AmtUses = 1; //没指定情况下默认为单独使用                       
                    hua.AmtProid = 0; //未使用默认为0
                    hua.ISSmsOne = 0;
                    hua.IsSmsThree = 0;
                    hua.isSmsFifteen = 0;
                    hua.IsSmsSeven = 0;
                    hua.isSmsSixteen = 0;
                    hua.Createtime = DateTime.Now;
                    hua.Title = "邀请好友首投满2000送10元现金";
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    if (i > 0)
                    {
                        //录入成功，后进行转账操作
                        //1.获取用户对向
                        M_member_table p = new M_member_table();
                        B_member_table o = new B_member_table();
                        p = o.GetModel(Registerid);
                        if (p != null)
                        {
                            //2.调用商户向用户转账接口
                            Transfer tf = new Transfer();
                            ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
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
                                        int ic = BUC.UpateActToUserTransfer(retf, 0);
                                        if (ic > 0)
                                        {
                                            //处理 资金流水信息 奖励流水  系统消息  
                                            t.Add(hua); //????
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion
            }
            return t;
        }
        #endregion
    }
}
