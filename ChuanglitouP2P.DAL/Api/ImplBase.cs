#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>LogicBase ©2013 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2013/3/22 11:09:42</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Capital;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.Record;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuangLiTou.Core.Entities.Response.UserAddress;
using ChuangLiTou.Core.Entities.ttnz;
using ChuanglitouP2P.Model.Invest;
using ChuangLiTou.Core.Entities.Response.AdNews;
using ChuangLiTou.Core.Entities.Response.Bank;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL.Api
{
    [Serializable]
    public class ImplBase
    {

        #region 实体封装

        #region 优惠券实体
        protected BonusEntity InitBonusEntity(DataTable dt)
        {
            return InitBonusList(dt).FirstOrDefault();
        }
        protected static List<BonusEntity> InitBonusList(DataTable dt, string sltd = "")
        {
            var entityList = new List<BonusEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    if (sltd != "" && sltd.StartsWith("b_", StringComparison.Ordinal))
                    {
                        sltd = sltd.Replace("b_", ",");
                        sltd = sltd + ",";
                    }

                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new BonusEntity();
                        var row = dt.Rows[n];

                        if (ContainsColumn(dt.Columns, "bonus_account_id", row))
                        {
                            entity.bonus_account_id = ConvertHelper.ParseValue(dt.Rows[n]["bonus_account_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "reward_state", row))
                        {
                            entity.reward_state = ConvertHelper.ParseValue(dt.Rows[n]["reward_state"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "reward_remarks", row))
                        {
                            entity.reward_remarks = dt.Rows[n]["reward_remarks"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "entry_time", row))
                        {
                            entity.entry_time = ConvertHelper.ParseValue(dt.Rows[n]["entry_time"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "act_state", row))
                        {
                            entity.act_state = ConvertHelper.ParseValue(dt.Rows[n]["act_state"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "proid", row))
                        {
                            entity.proid = ConvertHelper.ParseValue(dt.Rows[n]["proid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "activity_schedule_id", row))
                        {
                            entity.activity_schedule_id = ConvertHelper.ParseValue(dt.Rows[n]["activity_schedule_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "membertable_registerid", row))
                        {
                            entity.membertable_registerid = ConvertHelper.ParseValue(dt.Rows[n]["membertable_registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "activity_schedule_name", row))
                        {
                            entity.activity_schedule_name = dt.Rows[n]["activity_schedule_name"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "amount_of_reward", row))
                        {
                            entity.amount_of_reward = ConvertHelper.ParseValue(dt.Rows[n]["amount_of_reward"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "use_lower_limit", row))
                        {
                            entity.use_lower_limit = ConvertHelper.ParseValue(dt.Rows[n]["use_lower_limit"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "reward", row))
                        {
                            entity.reward = ConvertHelper.ParseValue(dt.Rows[n]["reward"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "start_date", row))
                        {
                            entity.start_date = ConvertHelper.ParseValue(dt.Rows[n]["start_date"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "end_date", row))
                        {
                            entity.end_date = ConvertHelper.ParseValue(dt.Rows[n]["end_date"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "UseLifeLoan", row))
                        {
                            if (dt.Rows[n]["UseLifeLoan"] == null)
                                entity.UseLifeLoan = "0-0";
                            else
                                entity.UseLifeLoan = dt.Rows[n]["UseLifeLoan"].ToString();
                        }
                        //b_12,43

                        if (sltd.IndexOf("," + entity.bonus_account_id + ",", StringComparison.Ordinal) != -1)
                        {
                            entity.selectedItem = 1;
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<BonusEntity> InitBonusEntityList(DataTable dt) throw exception:" + ex.ToString());
            }

            return entityList;
        }
        #endregion

        protected static List<RateBonusEntity> InitActivityLogs(DataTable dt, string sltd = "")
        {
            var entityList = new List<RateBonusEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {

                    if (sltd != "" && sltd.StartsWith("r_", StringComparison.Ordinal))
                    {
                        sltd = sltd.Replace("r_", ",");
                        sltd = sltd + ",";
                    }

                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new RateBonusEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "LogId", row))
                        {
                            entity.LogId = ConvertHelper.ParseValue(dt.Rows[n]["LogId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "UsedRecordId", row))
                        {
                            entity.UsedRecordId = ConvertHelper.ParseValue(dt.Rows[n]["UsedRecordId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "UsedTargetId", row))
                        {
                            entity.UsedTargetId = ConvertHelper.ParseValue(dt.Rows[n]["UsedTargetId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AddRate", row))
                        {
                            entity.AddRate = ConvertHelper.ParseValue(dt.Rows[n]["AddRate"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "UseStatus", row))
                        {
                            entity.UseStatus = ConvertHelper.ParseValue(dt.Rows[n]["UseStatus"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "CreateOn", row))
                        {
                            entity.CreateOn = ConvertHelper.ParseValue(dt.Rows[n]["CreateOn"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "UserId", row))
                        {
                            entity.UserId = ConvertHelper.ParseValue(dt.Rows[n]["UserId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "ActivityId", row))
                        {
                            entity.ActivityId = ConvertHelper.ParseValue(dt.Rows[n]["ActivityId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "ActType", row))
                        {
                            entity.ActType = ConvertHelper.ParseValue(dt.Rows[n]["ActType"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "ActivityName", row))
                        {
                            entity.ActivityName = dt.Rows[n]["ActivityName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "UseBeginOn", row))
                        {
                            entity.UseBeginOn = ConvertHelper.ParseValue(dt.Rows[n]["UseBeginOn"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "UseEndOn", row))
                        {
                            entity.UseEndOn = ConvertHelper.ParseValue(dt.Rows[n]["UseEndOn"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "RecordId", row))
                        {
                            entity.RecordId = ConvertHelper.ParseValue(dt.Rows[n]["RecordId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "TargetId", row))
                        {
                            entity.TargetId = ConvertHelper.ParseValue(dt.Rows[n]["TargetId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "UseLifeLoan", row))
                        {
                            entity.UseLifeLoan = dt.Rows[n]["UseLifeLoan"].ToString();
                        }
                        //r_12,43
                        if (sltd.IndexOf("," + entity.LogId + ",", StringComparison.Ordinal) != -1)
                        {
                            entity.selectedItem = 1;
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<BonusEntity> InitBonusEntityList(DataTable dt) throw exception:" + ex.ToString());
            }

            return entityList;
        }

        protected static List<NewsEntity> InitNewsEntity(DataTable dt)
        {
            var entityList = new List<NewsEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new NewsEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "newid", row))
                        {
                            entity.newid = ConvertHelper.ParseValue(dt.Rows[n]["newid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "parentid", row))
                        {
                            entity.parentid = ConvertHelper.ParseValue(dt.Rows[n]["parentid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "rootid", row))
                        {
                            entity.rootid = ConvertHelper.ParseValue(dt.Rows[n]["rootid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "path1", row))
                        {
                            entity.path1 = dt.Rows[n]["path1"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "orderid", row))
                        {
                            entity.orderid = ConvertHelper.ParseValue(dt.Rows[n]["orderid"].ToString(), 0);
                        }
                        entity.jumpUrl = Settings.Instance.SiteDomain + "/H5/News/Index?id=" + entity.newid;

                        if (ContainsColumn(dt.Columns, "topmenuname", row))
                        {
                            entity.topmenuname = dt.Rows[n]["topmenuname"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "comm", row))
                        {
                            entity.comm = ConvertHelper.ParseValue(dt.Rows[n]["comm"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "listcomm", row))
                        {
                            entity.listcomm = ConvertHelper.ParseValue(dt.Rows[n]["listcomm"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "newimg", row) && dt.Rows[n]["newimg"] != null)
                        {
                            entity.newimg = dt.Rows[n]["newimg"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "web_Type_menu_id", row))
                        {
                            entity.web_Type_menu_id = ConvertHelper.ParseValue(dt.Rows[n]["web_Type_menu_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "News_title", row))
                        {
                            entity.News_title = dt.Rows[n]["News_title"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "News_Key", row))
                        {
                            entity.News_Key = dt.Rows[n]["News_Key"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "news_Des", row))
                        {
                            entity.news_Des = dt.Rows[n]["news_Des"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "context", row))
                        {
                            // entity.context = HttpHelper.RemoveHtml(dt.Rows[n]["context"].ToString());
                            entity.context = (dt.Rows[n]["context"].ToString().Replace("src=\"/", string.Format(" width='100%' src=\"{0}/", "http://" + PublicURL.NewPCUrl + "")));
                        }
                        if (ContainsColumn(dt.Columns, "createtime", row))
                        {
                            entity.createtime = ConvertHelper.ParseValue(dt.Rows[n]["createtime"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "adminuserid", row))
                        {
                            entity.adminuserid = ConvertHelper.ParseValue(dt.Rows[n]["adminuserid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "menu_name", row))
                        {
                            entity.menu_name = dt.Rows[n]["menu_name"].ToString();
                        }

                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<Maticsoft.Model.V_type_news> InitMaticsoft.Model.V_type_news(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        protected static List<AdEntity> InitAdEntityList(DataTable dt)
        {
            var entityList = new List<AdEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new AdEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "Adid", row))
                        {
                            entity.Adid = ConvertHelper.ParseValue(dt.Rows[n]["Adid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AdName", row))
                        {
                            entity.AdName = dt.Rows[n]["AdName"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "Adcreatetime", row))
                        {
                            entity.Adcreatetime = ConvertHelper.ParseValue(dt.Rows[n]["Adcreatetime"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "AdState", row))
                        {
                            entity.AdState = ConvertHelper.ParseValue(dt.Rows[n]["AdState"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "AdTypeId", row))
                        {
                            entity.AdTypeId = ConvertHelper.ParseValue(dt.Rows[n]["AdTypeId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AdPath", row))
                        {
                            entity.AdPath = Settings.Instance.ImagesDomain + dt.Rows[n]["AdPath"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "AdLink", row))
                        {
                            entity.AdLink = dt.Rows[n]["AdLink"].ToString();
                        }

                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<AdEntity> InitAdEntityList(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }


        protected RecordEntity InitRecordEntity(DataTable dt)
        {
            return InitRecordList(dt).FirstOrDefault();
        }
        protected static List<RecordEntity> InitRecordList(DataTable dt)
        {
            var entityList = new List<RecordEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new RecordEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "bid_records_id", row))
                        {
                            entity.bid_records_id = ConvertHelper.ParseValue(dt.Rows[n]["bid_records_id"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "invest_time", row))
                        {
                            entity.invest_time = ConvertHelper.ParseValue(dt.Rows[n]["invest_time"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "invest_state", row))
                        {
                            entity.invest_state = ConvertHelper.ParseValue(dt.Rows[n]["invest_state"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "flow_return", row))
                        {
                            entity.flow_return = ConvertHelper.ParseValue(dt.Rows[n]["flow_return"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "repayment_amount", row))
                        {
                            entity.repayment_amount = ConvertHelper.ParseValue(dt.Rows[n]["repayment_amount"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "repayment_period", row))
                        {
                            entity.repayment_period = ConvertHelper.ParseValue(dt.Rows[n]["repayment_period"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "investor_registerid", row))
                        {
                            entity.investor_registerid = ConvertHelper.ParseValue(dt.Rows[n]["investor_registerid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "payment_status", row))
                        {
                            entity.payment_status = ConvertHelper.ParseValue(dt.Rows[n]["payment_status"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "withoutinterest", row))
                        {
                            entity.withoutinterest = ConvertHelper.ParseValue(dt.Rows[n]["withoutinterest"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "haveinterest", row))
                        {
                            entity.haveinterest = ConvertHelper.ParseValue(dt.Rows[n]["haveinterest"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "contractid", row))
                        {
                            entity.contractid = ConvertHelper.ParseValue(dt.Rows[n]["contractid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {
                            entity.borrower_registerid = ConvertHelper.ParseValue(dt.Rows[n]["borrower_registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "contractpath", row))
                        {
                            entity.contractpath = dt.Rows[n]["contractpath"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "invitationcode", row))
                        {
                            entity.invitationcode = dt.Rows[n]["invitationcode"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "OrdId", row))
                        {
                            entity.OrdId = ConvertHelper.ParseValue(dt.Rows[n]["OrdId"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "ordstate", row))
                        {
                            entity.ordstate = ConvertHelper.ParseValue(dt.Rows[n]["ordstate"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "IsLoans", row))
                        {
                            entity.IsLoans = ConvertHelper.ParseValue(dt.Rows[n]["IsLoans"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "loan_number", row))
                        {
                            entity.loan_number = ConvertHelper.ParseValue(dt.Rows[n]["loan_number"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "annual_interest_rate", row))
                        {
                            entity.annual_interest_rate = ConvertHelper.ParseValue(dt.Rows[n]["annual_interest_rate"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "current_period", row))
                        {
                            entity.current_period = ConvertHelper.ParseValue(dt.Rows[n]["current_period"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "investment_amount", row))
                        {
                            entity.investment_amount = ConvertHelper.ParseValue(dt.Rows[n]["investment_amount"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "value_date", row))
                        {
                            entity.value_date = ConvertHelper.ParseValue(dt.Rows[n]["value_date"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "investment_maturity", row))
                        {
                            entity.investment_maturity = ConvertHelper.ParseValue(dt.Rows[n]["investment_maturity"].ToString(), DateTime.MinValue);
                        }

                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<RecordEntity> InitRecordList(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        #region 用户实体
        protected MemberEntity InitMemberEntity(DataTable dt)
        {
            return InitMemberList(dt).FirstOrDefault();
        }
        protected List<MemberEntity> InitMemberList(DataTable dt)
        {
            var entityList = new List<MemberEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var model = new MemberEntity();

                        var row = dt.Rows[n];

                        if (ContainsColumn(dt.Columns, "registerid", row))
                        {
                            model.registerid = ConvertHelper.ParseValue(row["registerid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            model.username = row["username"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "password", row))
                        {
                            model.password = row["password"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "mobile", row))
                        {
                            model.mobile = row["mobile"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "email", row))
                        {
                            model.email = row["email"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "realname", row))
                        {
                            model.realname = row["realname"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "UsrCustId", row))
                        {
                            model.UsrCustId = row["UsrCustId"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "iD_number", row))
                        {
                            model.iD_number = row["iD_number"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "transactionpassword", row))
                        {
                            model.transactionpassword = row["transactionpassword"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "istransactionpassword", row))
                        {
                            model.istransactionpassword = ConvertHelper.ParseValue(row["istransactionpassword"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "ismobile", row))
                        {
                            model.ismobile = ConvertHelper.ParseValue(row["ismobile"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "isrealname", row))
                        {
                            model.isrealname = ConvertHelper.ParseValue(row["isrealname"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "isbankcard", row))
                        {
                            model.isbankcard = ConvertHelper.ParseValue(row["isbankcard"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "isemail", row))
                        {
                            model.isemail = ConvertHelper.ParseValue(row["isemail"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "userstate", row))
                        {
                            model.userstate = ConvertHelper.ParseValue(row["userstate"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "account_total_assets", row))
                        {
                            model.account_total_assets = ConvertHelper.ParseValue(row["account_total_assets"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "available_balance", row))
                        {
                            model.available_balance = ConvertHelper.ParseValue(row["available_balance"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "collect_total_amount", row))
                        {
                            model.collect_total_amount = ConvertHelper.ParseValue(row["collect_total_amount"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "frozen_sum", row))
                        {
                            model.frozen_sum = ConvertHelper.ParseValue(row["frozen_sum"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "open_tonto_account", row))
                        {
                            model.open_tonto_account = ConvertHelper.ParseValue(row["open_tonto_account"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "tonto_account_user", row))
                        {
                            model.tonto_account_user = row["tonto_account_user"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "usertypes", row))
                        {
                            model.usertypes = int.Parse(row["usertypes"].ToString());
                        }
                        if (ContainsColumn(dt.Columns, "registration_time", row))
                        {
                            model.registration_time = ConvertHelper.ParseValue(row["registration_time"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "lastlogintime", row))
                        {
                            model.lastlogintime = ConvertHelper.ParseValue(row["lastlogintime"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "lastloginIP", row))
                        {
                            model.lastloginIP = row["lastloginIP"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "customserviceid", row))
                        {
                            model.customserviceid = ConvertHelper.ParseValue(row["customserviceid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "homephone", row))
                        {
                            model.homephone = row["homephone"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "contactaddress", row))
                        {
                            model.contactaddress = row["contactaddress"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "zipcode", row))
                        {
                            model.zipcode = row["zipcode"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "qq", row))
                        {
                            model.qq = row["qq"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "msn", row))
                        {
                            model.msn = row["msn"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "invitedcode", row))
                        {
                            model.invitedcode = row["invitedcode"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "UsrId", row))
                        {
                            model.UsrId = row["UsrId"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "useridentity", row))
                        {
                            model.useridentity = int.Parse(row["useridentity"].ToString());
                        }
                        if (ContainsColumn(dt.Columns, "CopName", row))
                        {
                            model.CopName = row["CopName"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "Channelsource", row))
                        {
                            model.Channelsource = ConvertHelper.ParseValue(row["Channelsource"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "Tid", row))
                        {
                            model.Tid = row["Tid"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "WXAppId", row))
                        {
                            model.WXAppId = row["WXAppId"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "userPhotoUrl", row))
                        {
                            model.UserPhotoVirtualPath = row["userPhotoUrl"].ToString();
                            model.userPhotoUrl = Settings.Instance.ImagesAvater + row["userPhotoUrl"].ToString();
                        }
                        entityList.Add(model);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(" public List<LoanEntity> InitEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
        #endregion

        #region 用户投资实体

        protected InvestEntity InitInvestEntity(DataTable dt)
        {
            return InitInvestList(dt).FirstOrDefault();
        }

        protected static List<InvestEntity> InitInvestList(DataTable dt)
        {
            var entityList = new List<InvestEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new InvestEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "recordId", row))
                        {
                            entity.recordId = ConvertHelper.ParseValue(dt.Rows[n]["recordId"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "fundTotalAmount", row))
                        {
                            entity.fundTotalAmount = ConvertHelper.ParseValue(dt.Rows[n]["fundTotalAmount"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "investAmount", row))
                        {
                            entity.investAmount = ConvertHelper.ParseValue(dt.Rows[n]["investAmount"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "paymentOption", row))
                        {
                            entity.paymentOption = ConvertHelper.ParseValue(dt.Rows[n]["paymentOption"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "paymentStatus", row))
                        {
                            entity.paymentStatus = ConvertHelper.ParseValue(dt.Rows[n]["paymentStatus"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "rateBeginOn", row))
                        {
                            entity.rateBeginOn = ConvertHelper.ParseValue(dt.Rows[n]["rateBeginOn"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "valueDate", row))
                        {
                            entity.valueDate = ConvertHelper.ParseValue(dt.Rows[n]["valueDate"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "investMaturity", row))
                        {
                            entity.investMaturity = ConvertHelper.ParseValue(dt.Rows[n]["investMaturity"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "paymentDate", row))
                        {
                            entity.paymentDate = ConvertHelper.ParseValue(dt.Rows[n]["paymentDate"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "createdOn", row))
                        {
                            entity.createdOn = ConvertHelper.ParseValue(dt.Rows[n]["createdOn"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "investNumber", row))
                        {
                            entity.investNumber = ConvertHelper.ParseValue(dt.Rows[n]["investNumber"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "targetId", row))
                        {
                            entity.targetId = ConvertHelper.ParseValue(dt.Rows[n]["targetId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "targetTitle", row))
                        {
                            entity.targetTitle = dt.Rows[n]["targetTitle"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "investMemberId", row))
                        {
                            entity.investMemberId = ConvertHelper.ParseValue(dt.Rows[n]["investMemberId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "investMemberName", row))
                        {
                            entity.investMemberName = dt.Rows[n]["investMemberName"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "deadLine", row))
                        {
                            entity.deadLine = ConvertHelper.ParseValue(dt.Rows[n]["deadLine"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "unitDay", row))
                        {
                            entity.unitDay = ConvertHelper.ParseValue(dt.Rows[n]["unitDay"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "rate", row))
                        {
                            entity.rate = ConvertHelper.ParseValue(dt.Rows[n]["rate"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "borrowTotalAmount", row))
                        {
                            entity.borrowTotalAmount = ConvertHelper.ParseValue(dt.Rows[n]["borrowTotalAmount"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "receivableInterest", row))
                        {
                            entity.receivableInterest = ConvertHelper.ParseValue(dt.Rows[n]["receivableInterest"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "guarantee_way_id", row))
                        {
                            entity.guarantee_way_id = ConvertHelper.ParseValue(dt.Rows[n]["guarantee_way_id"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "month_payment_date", row))
                        {
                            entity.month_payment_date = ConvertHelper.ParseValue(dt.Rows[n]["month_payment_date"].ToString(), 0);
                        }
                        entity.jiaxiNum = dt.Rows[n]["jiaxiNum"] == null ? 0 : Convert.ToDecimal(dt.Rows[n]["jiaxiNum"]);
                        entity.BonusAmt = dt.Rows[n]["BonusAmt"] == null ? 0 : Convert.ToDecimal(dt.Rows[n]["BonusAmt"]);
                        entityList.Add(entity);

                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<InvestEntity> InitInvestEntityList(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }



        protected static List<InvestRecordEntity> InitInvestRecordList(DataTable dt)
        {
            var entityList = new List<InvestRecordEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new InvestRecordEntity();
                        var row = dt.Rows[n];

                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            entity.phone = dt.Rows[n]["username"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "investment_amount", row))
                        {
                            entity.investMoney = ConvertHelper.ParseValue(dt.Rows[n]["investment_amount"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "invest_time", row))
                        {
                            entity.investTime = ConvertHelper.ParseValue(dt.Rows[n]["invest_time"].ToString(), DateTime.MinValue);
                        }
                        entityList.Add(entity);

                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<InvestEntity> InitInvestEntityList(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        #endregion

        #region 资金流水

        protected static List<CapitalAccountWater> InitCapitalAccountWater(DataTable dt)
        {
            var entityList = new List<CapitalAccountWater>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new CapitalAccountWater();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "account_water_id", row))
                        {
                            entity.account_water_id = ConvertHelper.ParseValue(dt.Rows[n]["account_water_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "remarks", row))
                        {
                            entity.remarks = dt.Rows[n]["remarks"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "membertable_registerid", row))
                        {
                            entity.membertable_registerid = ConvertHelper.ParseValue(dt.Rows[n]["membertable_registerid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "income", row))
                        {
                            entity.income = ConvertHelper.ParseValue(dt.Rows[n]["income"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "expenditure", row))
                        {
                            entity.expenditure = ConvertHelper.ParseValue(dt.Rows[n]["expenditure"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "time_of_occurrence", row))
                        {
                            entity.time_of_occurrence = ConvertHelper.ParseValue(dt.Rows[n]["time_of_occurrence"].ToString(), DateTime.MinValue).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (ContainsColumn(dt.Columns, "account_balance", row))
                        {
                            entity.account_balance = ConvertHelper.ParseValue(dt.Rows[n]["account_balance"].ToString(), 0M);
                        }
                        if (ContainsColumn(dt.Columns, "types_Finance", row))
                        {
                            entity.types_Finance = ConvertHelper.ParseValue(dt.Rows[n]["types_Finance"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "createtime", row))
                        {
                            entity.createtime = ConvertHelper.ParseValue(dt.Rows[n]["createtime"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "keyid", row))
                        {
                            entity.keyid = ConvertHelper.ParseValue(dt.Rows[n]["keyid"].ToString(), 0);
                        }

                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<CapitalAccountWater> InitCapitalAccountWater(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
        #endregion

        #endregion


        protected List<BorrowingEntity> InitBorrowingEntity(DataTable dt)
        {
            var entityList = new List<BorrowingEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {

                        if (
                            (dt.Rows[n]["unit_day"].ToString() == "1") && (dt.Rows[n]["life_of_loan"].ToString() == "1")
                            ||
                            (dt.Rows[n]["unit_day"].ToString() == "3") &&
                            (dt.Rows[n]["life_of_loan"].ToString() == "30" ||
                             dt.Rows[n]["life_of_loan"].ToString() == "31"))
                        {
                            var entity = new BorrowingEntity();
                            var row = dt.Rows[n];

                            if (ContainsColumn(dt.Columns, "borrowing_title", row))
                            {
                                //  entity.bname = System.Web.HttpUtility.UrlEncode(dt.Rows[n]["borrowing_title"].ToString());
                                entity.bname = dt.Rows[n]["borrowing_title"].ToString();
                            }

                            if (ContainsColumn(dt.Columns, "borrowing_balance", row))
                            {
                                entity.jkje = ConvertHelper.ParseValue(dt.Rows[n]["borrowing_balance"].ToString(), 0M).ToString();
                            }

                            if (ContainsColumn(dt.Columns, "minimum", row))
                            {
                                entity.qtje = dt.Rows[n]["minimum"].ToString();
                            }

                            //borrowing_balance - fundraising_amount;
                            if (ContainsColumn(dt.Columns, "fundraising_amount", row))
                            {

                                entity.ktje = (ConvertHelper.ParseValue(dt.Rows[n]["borrowing_balance"].ToString(), 0M) -
                                    ConvertHelper.ParseValue(dt.Rows[n]["fundraising_amount"].ToString(), 0M)).ToString()
                                    ;
                            }

                            if (ContainsColumn(dt.Columns, "annual_interest_rate", row))
                            {


                                entity.nhsy = dt.Rows[n]["annual_interest_rate"].ToString();

                            }

                            if (ContainsColumn(dt.Columns, "unit_day", row))
                            {

                                switch (dt.Rows[n]["unit_day"].ToString())
                                {
                                    case "1"://月

                                        entity.dkqx = dt.Rows[n]["life_of_loan"] + "月";
                                        break;
                                    case "3"://天
                                        entity.dkqx = dt.Rows[n]["life_of_loan"] + "天";
                                        break;
                                    default:
                                        { }
                                        break;
                                }
                            }

                            if (ContainsColumn(dt.Columns, "payment_options", row))
                            {
                                //1 按月等额本息  3 每月还息，到期还本   4 一次性还本付息(按天计息)
                                switch (dt.Rows[n]["payment_options"].ToString())
                                {
                                    case "1":

                                        entity.hkfs = "按月等额本息";
                                        break;
                                    case "3":
                                        entity.hkfs = "每月还息,到期还本";
                                        break;
                                    case "4":
                                        entity.hkfs = "一次性还本付息(按天计息)";
                                        break;
                                    default:
                                        { }
                                        break;
                                }

                            }

                            switch (dt.Rows[n]["tender_state"].ToString())
                            {
                                case "5":
                                    {
                                        entity.tzjd = "100";
                                    }
                                    break;
                                default:
                                    {


                                        var x = ConvertHelper.ParseValue(dt.Rows[n]["fundraising_amount"].ToString(), 0D);
                                        entity.tzjd = ConvertHelper.ParseValue((x / ConvertHelper.ParseValue(entity.jkje, 0D)) * 100, "");
                                    }
                                    break;
                            }
                            entity.key = EncryptHelper.GetMd5Str32("chuanglitou@ttnznet", "x");

                            entityList.Add(entity);
                        }

                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(" public List<LoanEntity> InitEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
        protected static List<PresentEntity> InitPresentList(DataTable dt)
        {
            var entityList = new List<PresentEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new PresentEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "UserCashId", row))
                        {
                            entity.UserCashId = ConvertHelper.ParseValue(dt.Rows[n]["UserCashId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "Reason", row))
                        {
                            entity.Reason = dt.Rows[n]["Reason"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "Remarks", row))
                        {
                            entity.Remarks = dt.Rows[n]["Remarks"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "TransState", row))
                        {
                            entity.TransState = ConvertHelper.ParseValue(dt.Rows[n]["TransState"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "OpenAcctId", row))
                        {
                            entity.OpenAcctId = dt.Rows[n]["OpenAcctId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "OpenBankId", row))
                        {
                            entity.OpenBankId = dt.Rows[n]["OpenBankId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "FeeObjFlag", row))
                        {
                            entity.FeeObjFlag = dt.Rows[n]["FeeObjFlag"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "registerid", row))
                        {
                            entity.registerid = ConvertHelper.ParseValue(dt.Rows[n]["registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "UsrCustId", row))
                        {
                            entity.UsrCustId = dt.Rows[n]["UsrCustId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "TransAmt", row))
                        {
                            entity.TransAmt = ConvertHelper.ParseValue(dt.Rows[n]["TransAmt"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "FeeAmt", row))
                        {
                            entity.FeeAmt = ConvertHelper.ParseValue(dt.Rows[n]["FeeAmt"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "OrdId", row))
                        {
                            entity.OrdId = dt.Rows[n]["OrdId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "OrdIdTime", row))
                        {
                            entity.OrdIdTime = ConvertHelper.ParseValue(dt.Rows[n]["OrdIdTime"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "OrdIdState", row))
                        {
                            entity.OrdIdState = ConvertHelper.ParseValue(dt.Rows[n]["OrdIdState"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "OperTime", row))
                        {
                            entity.OperTime = ConvertHelper.ParseValue(dt.Rows[n]["OperTime"].ToString(), DateTime.MinValue);
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<Maticsoft.Model.hx_td_UserCash> InitMaticsoft.Model.hx_td_UserCash(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        protected static bool ReaderExists(SqlDataReader dr, string columnName)
        {
            var schemaTable = dr.GetSchemaTable();
            if (schemaTable != null)
            {
                schemaTable.DefaultView.RowFilter = "ColumnName= '" + columnName + "'";

                return (schemaTable.DefaultView.Count > 0);
            }
            return false;
        }
        protected static bool ContainsColumn(DataColumnCollection columns, string columnName, DataRow row)
        {
            var cts = columns.Contains(columnName);

            if (cts)
            {
                if (row[columnName] != null)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断dataset是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>true表示dataset不为空</returns>
        protected static bool DataSetIsNotNull(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 从数据库里返回唯一的邀请码
        /// </summary>
        /// <returns></returns>
        public static string Getinvitedcode()
        {

            string code = "", sql = "";

            code = Utils.RndNumChar(8);

            sql = "select invitedcode from hx_member_table where invitedcode='" + code + "'";

            int df = DbHelper.ExecuteSql(sql);

            if (df <= 0)
            {
                return code;
            }
            Getinvitedcode();

            return code;
        }
        /// <summary>
        /// 获取短信内容
        /// </summary>
        /// <param name="SEtype"></param>
        /// <returns></returns>
        public static List<SmsEmailEntity> SelectSmsEmailList(int type, int messageId)
        {

            var strSql = new StringBuilder();
            strSql.Append(
                "SELECT SmsEmailId,SmsEname,SEContext FROM hx_td_SMSEmail where 1=1 ");
            if (type > 0)
            {
                strSql.AppendFormat(" and SEtype={0}", type);
            }
            if (messageId > 0)
            {
                strSql.AppendFormat(" and SmsEmailId={0}", messageId);
            }

            var cacheKey = CacheHelper.GetCacheKeyByParam(new IComparable[] { "dtmsmemail", type, messageId }).ToLower();
            var objEntity = CacheHelper.GetCache(cacheKey);

            if (objEntity == null)
            {
                var ds = DbHelper.Query(strSql.ToString());

                if (DataSetIsNotNull(ds))
                {
                    var lst = InitSmsEmailEntity(ds.Tables[0]);

                    CacheHelper.SetCache(cacheKey, lst);
                    return lst;
                }
                return null;
            }
            return objEntity as List<SmsEmailEntity>;
        }
        public SmsEmailEntity SelectSmsEmailEntity(int type, int messageId)
        {
            try
            {

                return SelectSmsEmailList(type, messageId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("public static SmsEmailEntity GetSmsEmailEntity(int type, int messageId)" + ex);
                return null;
            }
        }
        protected static List<SmsEmailEntity> InitSmsEmailEntity(DataTable dt)
        {
            var entityList = new List<SmsEmailEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new SmsEmailEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "SmsEmailId", row))
                        {
                            entity.SmsEmailId = ConvertHelper.ParseValue(dt.Rows[n]["SmsEmailId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "SmsEname", row))
                        {
                            entity.SmsEname = dt.Rows[n]["SmsEname"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "SEContext", row))
                        {
                            entity.SEContext = dt.Rows[n]["SEContext"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "SEtype", row))
                        {
                            entity.SEtype = ConvertHelper.ParseValue(dt.Rows[n]["SEtype"].ToString(), 0);
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(" protected static List<SmsEmailEntity> InitSmsEmailEntity(DataTable dt) Exception:", ex);
            }

            return entityList;
        }

        protected static List<NormalAreaEntity> InitNormalAreaEntityList(DataTable dt)
        {
            var entityList = new List<NormalAreaEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new NormalAreaEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "AreaId", row))
                        {
                            entity.AreaId = ConvertHelper.ParseValue(dt.Rows[n]["AreaId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AreaCode", row))
                        {
                            entity.AreaCode = dt.Rows[n]["AreaCode"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "AreaName", row))
                        {
                            entity.AreaName = dt.Rows[n]["AreaName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "ParentId", row))
                        {
                            entity.ParentId = ConvertHelper.ParseValue(dt.Rows[n]["ParentId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AreaLevel", row))
                        {
                            entity.AreaLevel = ConvertHelper.ParseValue(dt.Rows[n]["AreaLevel"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AreaOrder", row))
                        {
                            entity.AreaOrder = ConvertHelper.ParseValue(dt.Rows[n]["AreaOrder"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "AreaNameEn", row))
                        {
                            entity.AreaNameEn = dt.Rows[n]["AreaNameEn"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "AreaShortNameEn", row))
                        {
                            entity.AreaShortNameEn = dt.Rows[n]["AreaShortNameEn"].ToString();
                        }

                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<NormalAreaEntity> InitNormalAreaEntityList(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        /// <summary>
        /// Initializes the user address entity list.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>List&lt;ResponseUserAddressEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-23 09:45:34
        protected static List<ResponseUserAddressEntity> InitUserAddressEntityList(DataTable dt)
        {
            var entityList = new List<ResponseUserAddressEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new ResponseUserAddressEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "userId", row))
                        {
                            entity.userId = ConvertHelper.ParseValue(dt.Rows[n]["userId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "updatedOn", row))
                        {
                            entity.updatedOn = ConvertHelper.ParseValue(dt.Rows[n]["updatedOn"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "provinceId", row))
                        {
                            entity.provinceId = ConvertHelper.ParseValue(dt.Rows[n]["provinceId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "provinceName", row))
                        {
                            entity.provinceName = dt.Rows[n]["provinceName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "cityId", row))
                        {
                            entity.cityId = ConvertHelper.ParseValue(dt.Rows[n]["cityId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "cityName", row))
                        {
                            entity.cityName = dt.Rows[n]["cityName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "countyId", row))
                        {
                            entity.countyId = ConvertHelper.ParseValue(dt.Rows[n]["countyId"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "countyName", row))
                        {
                            entity.countyName = dt.Rows[n]["countyName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "detailAddress", row))
                        {
                            entity.detailAddress = dt.Rows[n]["detailAddress"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "createdOn", row))
                        {
                            entity.createdOn = ConvertHelper.ParseValue(dt.Rows[n]["createdOn"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "userName", row))
                        {
                            entity.userName = dt.Rows[n]["userName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "mobile", row))
                        {
                            entity.mobile = dt.Rows[n]["mobile"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "zipCode", row))
                        {
                            entity.zipCode = dt.Rows[n]["zipCode"].ToString();
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<Maticsoft.Model.UserAddress> InitMaticsoft.Model.UserAddress(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        #region 公用方法
        /// <summary>
        /// 获取可用奖励
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        protected decimal GetBonuses(int userid)
        {
            decimal dec = 0.00M;
            //注需要过期时间处理         
            string sql = "select COALESCE(sum(amt),0) as amount_of_reward from hx_UserAct where registerid=" + userid.ToString() + " and UseState = 0 and AmtEndtime >='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and RewTypeID=2";
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var dt = ds.Tables[0];
                dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());
            }
            return dec;
        }

        /// <summary>
        /// 累计赚取
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:47:42
        protected decimal GetTotalGains(int userid)
        {
            decimal dec = 0M;
            //获取已赚利息
            decimal yinvs = GetInterest(userid);
            //未付利息按天计算
            decimal unPay = GetUnpaidInterest(userid);

            dec = dec + yinvs + unPay;
            if (dec == 0M)
            {
                dec = decimal.Parse(getAllDailyrevenue(userid, DateTime.Now));
            }


            return dec;
        }

        protected decimal GetTotalGains2(int userid)
        {
            decimal dec = 0M;
            string sql = "select  COALESCE(sum(haveinterest),0)  as haveinterest   from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid.ToString() + " and  haveinterest>0 and tender_state between 2 and 5 ";
            var ds = DbHelper.Query(sql);
            dec = decimal.Parse(ds.Tables[0].Rows[0]["haveinterest"].ToString());
            return dec;
        }

        /// <summary>
        /// 每个项目未付的利息(按日计息) 以当天为准计算.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 13:27:51
        protected decimal GetUnpaidInterest(int userId)
        {
            decimal dec = 0M;
            string sql = "select  COALESCE(sum(withoutinterest),0) as withoutinterest from V_hx_Bid_records_borrowing_target  where investor_registerid=" + userId.ToString() + "  and tender_state between 2 and 5 and payment_status=0 and ordstate=1";
            var ds = DbHelper.Query(sql);
            dec = decimal.Parse(ds.Tables[0].Rows[0]["withoutinterest"].ToString());
            return dec;
        }

        /// <summary>
        /// 获取用户所有的有效投资项目id
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>DataTable.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 09:46:32
        public DataSet GetTargetIds(int userid)
        {
            string sql = "SELECT DISTINCT(targetid),bid_records_id FROM dbo.hx_Bid_records r WHERE ordstate=1 AND investor_registerid=@userId AND haveinterest>=0 AND EXISTS (SELECT COUNT(1) FROM dbo.hx_borrowing_target t WHERE t.targetid=r.targetid AND ( t.tender_state between 2 and 5))";

            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userid;
            var ds = DbHelper.Query(sql, parameters);
            return ds;
        }
        /// <summary>
        /// 累计投资.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 13:53:08
        public decimal GetTotalInverstAmount(int userId)
        {
            decimal dec = 0.00M;
            string sql = @"SELECT ISNULL(SUM(investment_amount),0) AS investment_amount FROM dbo.hx_Bid_records WHERE investor_registerid=@userId AND ordstate=1";
            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;
            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dec = dec + decimal.Parse(dt.Rows[0]["investment_amount"].ToString());

                }
            }
            return dec;
        }

        /// <summary>
        /// 返回已赚利息.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 09:25:55
        protected decimal GetInterest(int userid)
        {
            decimal dec = 0.00M;

            string sql = "SELECT  ISNULL(SUM(haveinterest),0)  AS totalInterest FROM dbo.hx_Bid_records r WHERE ordstate=1 AND investor_registerid=@userId AND haveinterest>0 AND EXISTS (SELECT COUNT(1) FROM dbo.hx_borrowing_target t WHERE t.targetid=r.targetid AND ( t.tender_state between 2 and 5))";
            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userid;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                dec = decimal.Parse(ds.Tables[0].Rows[0]["totalInterest"].ToString());
            }
            return dec;
        }

        /// <summary>
        /// 待收本金
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetPrincipal(int userid)
        {
            decimal dec = 0.00M;
            string sql = "select  COALESCE(sum(investment_amount),0)  as investment_amount from hx_Bid_records where investor_registerid=" + userid.ToString() + " and payment_status=0 and ordstate=1 and IsLoans=1";
            DataTable dt = DbHelper.Query(sql).Tables[0];
            dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());


            return dec;
        }
        /// <summary>
        /// 新动态获取累积赚取金额
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="datetime1">The datetime1.</param>
        /// <returns>System.String.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:47:36
        public string getAllDailyrevenue(int userid, DateTime datetime1)
        {

            decimal invest = 0.00M;
            string sql = "select targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options,invest_time,repayment_period from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid + "   and   tender_state between 2 and 5  group by targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options,invest_time,repayment_period";

            // string sql = "select targetid, investment_amount,annual_interest_rate,payment_options from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid.ToString() + "   and  repayment_period>'" + datetime1.ToString() + "'";
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    InvestmentParameters mp = new InvestmentParameters();
                    mp.Amount = decimal.Parse(dt.Rows[i]["investment_amount"].ToString());
                    mp.Circle = 1;
                    mp.CircleType = 3;
                    mp.NominalYearRate = double.Parse(dt.Rows[i]["annual_interest_rate"].ToString());
                    mp.OverheadsRate = 0f;
                    mp.RepaymentMode = int.Parse(dt.Rows[i]["payment_options"].ToString());
                    mp.RewardRate = 0f;
                    mp.IsThirtyDayMonth = false;
                    mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    mp.Payinterest = 1;
                    mp.InvestObject = 1;
                    List<InvestmentReceiveRecordInfo> records = InvestCalculator.CalculateReceiveRecord(mp);
                    decimal dayinvst = 0.0M;
                    foreach (InvestmentReceiveRecordInfo pr in records)
                    {
                        dayinvst = Math.Round(pr.Interest, 2);
                    }



                    DateTime repaydate = DateTime.Parse(dt.Rows[i]["repayment_period"].ToString());

                    if (DateTime.Compare(repaydate, datetime1) <= 0)
                    {

                        long allday = Settings.Instance.DateDiff("Day", DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")), DateTime.Parse(repaydate.ToString("yyyy-MM-dd")));

                        int ady = int.Parse(allday.ToString());
                        if (ady <= 0)
                        {
                            ady = 1;
                        }

                        dayinvst = dayinvst * (ady + 1);

                    }
                    else
                    {
                        long allday = Settings.Instance.DateDiff("Day", DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")), DateTime.Parse(datetime1.ToString("yyyy-MM-dd")));

                        int ady = int.Parse(allday.ToString());
                        if (ady <= 0)
                        {
                            ady = 1;
                        }


                        if (DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")) == DateTime.Parse(datetime1.ToString("yyyy-MM-dd")))
                        {
                            dayinvst = dayinvst * (ady);
                        }
                        else
                        {
                            dayinvst = dayinvst * (ady + 1);
                        }
                    }
                    invest = invest + dayinvst;
                }
            }
            return Math.Round(invest, 2).ToString(); ;
        }


        /// <summary>
        /// 通过项目id 和投资用户id 计算出未付利息
        /// </summary>
        /// <param name="TargetId">The target identifier.</param>
        /// <param name="userid">The userid.</param>
        /// <param name="bid_records_id">The bid_records_id.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 09:59:21
        public decimal GetUnpaidInterest(string TargetId, int userid, string bid_records_id)
        {
            decimal dec = 0M;
            //实现业务逻辑 获得上一次付息日，然后获取当天与下次付息日的时间，按天付息计算出未付利息

            string sql = "select top 1 targetid, investment_amount,annual_revenue,invest_time,value_date,payment_options from V_borrowing_Bid_records_income_statement where investor_registerid=" + userid.ToString() + " and payment_status=0  and targetid=" + TargetId + " and bid_records_id = " + bid_records_id + "  order by value_date asc";
            //string sql = "select top 1 targetid, investment_amount,annual_revenue,invest_time,value_date,payment_options from V_borrowing_Bid_records_income_statement_uc where investor_registerid=" + userid.ToString() + " and payment_status=0  and targetid=" + TargetId + " and bid_records_id = " + bid_records_id + "  order by value_date asc";


            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DateTime invest_time = DateTime.Parse(dt.Rows[0]["invest_time"].ToString());

                    DateTime value_date = DateTime.Parse(dt.Rows[0]["value_date"].ToString());


                    DateTime nows = DateTime.Now;

                    if (DateTime.Compare(invest_time, nows) <= 0)
                    {
                        // Response.Write(Utils.DateDiff("Day", invest_time, nows).ToString());
                        //判断是否超过一个月  如果超过一个月则 按付息日记，不超一个月则按投资日计

                        long allday = Settings.Instance.DateDiff("Day", invest_time, nows);

                        InvestmentParameters mp = new InvestmentParameters();
                        mp.Amount = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                        mp.Circle = 1;
                        mp.CircleType = 3;
                        mp.NominalYearRate = double.Parse(dt.Rows[0]["annual_revenue"].ToString());
                        mp.OverheadsRate = 0f;
                        mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                        mp.RewardRate = 0f;
                        mp.IsThirtyDayMonth = false;
                        /*
                          mp.InvestDate = DateTime.Parse(invest_time.ToString("yyyy-MM-dd"));
                          mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                         */

                        mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                        mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        mp.Payinterest = 1;
                        mp.InvestObject = 1;
                        List<InvestmentReceiveRecordInfo> records = InvestCalculator.CalculateReceiveRecord(mp);
                        foreach (InvestmentReceiveRecordInfo pr in records)
                        {
                            dec = dec + decimal.Parse(Settings.Instance.GetWebConvertdisp(pr.Interest, 2, false));
                        }

                        int ady = int.Parse(allday.ToString());
                        if (ady <= 0)
                        {
                            ady = 1;
                        }

                        dec = dec * ady;



                        //Response.Write("fdfdsf:" + invest.ToString());



                        ////  int days = DateTime.DaysInMonth(invest_time.Year, invest_time.Month);


                        //  Response.Write(Utils.DateDiff("Day", invest_time, value_date).ToString());


                    }



                }
            }


            return dec;
        }

        #endregion

        protected static List<BankEntity> InitBankEntityList(DataTable dt)
        {
            var entityList = new List<BankEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new BankEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "Bankid", row))
                        {
                            entity.Bankid = ConvertHelper.ParseValue(dt.Rows[n]["Bankid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "BankName", row))
                        {
                            entity.BankName = dt.Rows[n]["BankName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "OpenBankId", row))
                        {
                            entity.OpenBankId = dt.Rows[n]["OpenBankId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "CardImage", row))
                        {
                            entity.CardImage = dt.Rows[n]["CardImage"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "Isquick", row))
                        {
                            entity.Isquick = ConvertHelper.ParseValue(dt.Rows[n]["Isquick"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "Isordinary", row))
                        {
                            entity.Isordinary = ConvertHelper.ParseValue(dt.Rows[n]["Isordinary"].ToString(), 0);
                        }

                        entity.AmountLimitPerTrade = entity.AmountLimitPerDay = entity.AmountLimitPerMonth = 0;
                        if (ContainsColumn(dt.Columns, "SingleTransQuota", row))
                        {
                            entity.AmountLimitPerTrade = dt.Rows[n]["SingleTransQuota"] == null ? 0 : Convert.ToInt32(dt.Rows[n]["SingleTransQuota"]);
                        }

                        if (ContainsColumn(dt.Columns, "CardDailyTransQuota", row))
                        {
                            entity.AmountLimitPerDay = dt.Rows[n]["CardDailyTransQuota"] == null ? 0 : Convert.ToInt32(dt.Rows[n]["CardDailyTransQuota"]);
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<Maticsoft.Model.hx_td_Bank> InitMaticsoft.Model.hx_td_Bank(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        protected static List<MemberBankEntity> InitUsrBindCardEntityList(DataTable dt)
        {
            var entityList = new List<MemberBankEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new MemberBankEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "UsrBindCardID", row))
                        {
                            entity.UsrBindCardID = ConvertHelper.ParseValue(dt.Rows[n]["UsrBindCardID"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "UsrCustId", row))
                        {
                            entity.UsrCustId = dt.Rows[n]["UsrCustId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "OpenAcctId", row))
                        {
                            entity.OpenAcctId = dt.Rows[n]["OpenAcctId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "RealName", row))
                        {
                            entity.RealName = dt.Rows[n]["RealName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "BankName", row))
                        {
                            entity.BankName = dt.Rows[n]["BankName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "CardImage", row))
                        {
                            entity.CardImage = Settings.Instance.ImagesDomain.Replace(" ", "") + dt.Rows[n]["CardImage"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "CanImmediateWithdrawal", row))
                        {
                            entity.CanImmediateWithdrawal = (dt.Rows[n]["CanImmediateWithdrawal"] is DBNull || dt.Rows[n]["CanImmediateWithdrawal"] == null) ? false : Convert.ToBoolean(dt.Rows[n]["CanImmediateWithdrawal"]);
                        }

                        if (ContainsColumn(dt.Columns, "OpenBankId", row))
                        {
                            entity.OpenBankId = dt.Rows[n]["OpenBankId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "defCard", row))
                        {
                            entity.defCard = ConvertHelper.ParseValue(dt.Rows[n]["defCard"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "BindCardType", row))
                        {
                            entity.BindCardType = ConvertHelper.ParseValue(dt.Rows[n]["BindCardType"].ToString(), 0);
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<Maticsoft.Model.hx_UsrBindCardC> InitMaticsoft.Model.hx_UsrBindCardC(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        protected static List<ResponseInvestIncomeEntity> InitResponseInvestIncomeEntity(DataTable dt)
        {
            var entityList = new List<ResponseInvestIncomeEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new ResponseInvestIncomeEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "current_investment_period", row))
                        {
                            entity.current_investment_period = ConvertHelper.ParseValue(dt.Rows[n]["current_investment_period"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "value_date", row))
                        {
                            entity.value_date = ConvertHelper.ParseValue(dt.Rows[n]["value_date"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "day", row))
                        {
                            entity.day = ConvertHelper.ParseValue(dt.Rows[n]["day"].ToString(), DateTime.MinValue).ToString("dd");
                        }

                        if (ContainsColumn(dt.Columns, "repayment_amount", row))
                        {
                            entity.repayment_amount = ConvertHelper.ParseValue(dt.Rows[n]["repayment_amount"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "repayment_period", row))
                        {
                            entity.repayment_period = ConvertHelper.ParseValue(dt.Rows[n]["repayment_period"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "interest_payment_date", row))
                        {
                            entity.interest_payment_date = ConvertHelper.ParseValue(dt.Rows[n]["interest_payment_date"].ToString(), DateTime.MinValue);
                        }

                        if (ContainsColumn(dt.Columns, "investor_registerid", row))
                        {
                            entity.investor_registerid = ConvertHelper.ParseValue(dt.Rows[n]["investor_registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "bid_records_id", row))
                        {
                            entity.bid_records_id = ConvertHelper.ParseValue(dt.Rows[n]["bid_records_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "annual_interest_rate", row))
                        {
                            entity.annual_interest_rate = ConvertHelper.ParseValue(dt.Rows[n]["annual_interest_rate"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "registerid", row))
                        {
                            entity.registerid = ConvertHelper.ParseValue(dt.Rows[n]["registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "loan_number", row))
                        {
                            entity.loan_number = ConvertHelper.ParseValue(dt.Rows[n]["loan_number"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "available_balance", row))
                        {
                            entity.available_balance = ConvertHelper.ParseValue(dt.Rows[n]["available_balance"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "payment_options", row))
                        {
                            entity.payment_options = ConvertHelper.ParseValue(dt.Rows[n]["payment_options"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "OutCustId", row))
                        {
                            entity.OutCustId = dt.Rows[n]["OutCustId"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "loan_management_fee", row))
                        {
                            entity.loan_management_fee = ConvertHelper.ParseValue(dt.Rows[n]["loan_management_fee"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "Principal", row))
                        {
                            entity.Principal = ConvertHelper.ParseValue(dt.Rows[n]["Principal"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "interestDay", row))
                        {
                            entity.interestDay = ConvertHelper.ParseValue(dt.Rows[n]["interestDay"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "TotalInstallments", row))
                        {
                            entity.TotalInstallments = ConvertHelper.ParseValue(dt.Rows[n]["TotalInstallments"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "ordstate", row))
                        {
                            entity.ordstate = ConvertHelper.ParseValue(dt.Rows[n]["ordstate"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            entity.username = dt.Rows[n]["username"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "realname", row))
                        {
                            entity.realname = dt.Rows[n]["realname"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "borrowing_title", row))
                        {
                            entity.borrowing_title = dt.Rows[n]["borrowing_title"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "mobile", row))
                        {
                            entity.mobile = dt.Rows[n]["mobile"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "orderid", row))
                        {
                            entity.orderid = dt.Rows[n]["orderid"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "borrowing_balance", row))
                        {
                            entity.borrowing_balance = ConvertHelper.ParseValue(dt.Rows[n]["borrowing_balance"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "investment_amount", row))
                        {
                            entity.investment_amount = ConvertHelper.ParseValue(dt.Rows[n]["investment_amount"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "income_statement_id", row))
                        {
                            entity.income_statement_id = ConvertHelper.ParseValue(dt.Rows[n]["income_statement_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {
                            entity.borrower_registerid = ConvertHelper.ParseValue(dt.Rows[n]["borrower_registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "annual_revenue", row))
                        {
                            entity.annual_revenue = ConvertHelper.ParseValue(dt.Rows[n]["annual_revenue"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "invest_time", row))
                        {
                            entity.invest_time = ConvertHelper.ParseValue(dt.Rows[n]["invest_time"].ToString(), DateTime.MinValue);
                        }

                        if (entity.repayment_period == null || entity.repayment_period == DateTime.MinValue)
                        {
                            entity.payment_status = 2;  //待还款

                            if (DateTime.Now > entity.interest_payment_date.AddDays(1))
                            {
                                entity.payment_status = 5; //延迟（待还款）
                            }
                        }
                        else
                        {
                            //已还款
                            //预期还款时间 与 实际还款时间比较 0 相等 <0  interest_payment_date 小 ||| >0  interest_payment_date 大
                            var x = DateTime.Compare(entity.interest_payment_date, entity.repayment_period);

                            if (x == 0)
                            {
                                entity.payment_status = 4;//已还款
                            }
                            if (x < 0)
                            {
                                entity.payment_status = 3;//延迟（已还款） 
                            }
                            if (x > 0)
                            {
                                entity.payment_status = 1;//提前还款
                            }
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<ResponseInvestIncomeEntity> InitResponseInvestIncomeEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        protected static List<UserInvestedStatistics> InitInvitationDetailList(DataTable dt, ref int invitedPeopleCount, ref int invitedInvestedPeopleCount, ref Decimal invitationTotalAmount)
        {
            List<UserInvestedStatistics> entityList = new List<UserInvestedStatistics>();
            try
            {
                invitedPeopleCount = invitedInvestedPeopleCount = 0;
                invitationTotalAmount = 0.00M;
                int rowsCount = dt.Rows.Count;
                invitedPeopleCount = rowsCount;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new UserInvestedStatistics();
                        var row = dt.Rows[n];
                        entity.No = (n + 1).ToString();
                        if (ContainsColumn(dt.Columns, "UserName", row))
                        {
                            entity.UserName = dt.Rows[n]["UserName"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "InvestedAmount", row))
                        {
                            entity.InvestedAmount = dt.Rows[n]["InvestedAmount"].ToString();
                            if (!string.IsNullOrWhiteSpace(entity.InvestedAmount))
                            {
                                decimal temp = 0.00M;
                                if (decimal.TryParse(entity.InvestedAmount, out temp))
                                {
                                    if (temp > 0)
                                    {
                                        invitationTotalAmount += temp;
                                        invitedInvestedPeopleCount += 1;
                                    }
                                }
                            }
                        }
                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("protected static List<ResponseInvestIncomeEntity> InitResponseInvestIncomeEntity(DataTable dt) throw exception:", ex);
            }
            return entityList;
        }
    }
}
