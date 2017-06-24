using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.UserAddress;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class BonusDal.
    /// </summary>
    public class BonusDal : ImplBase
    {
        /// <summary>
        /// 获取用户 余额+优惠券
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberInvestEntity.</returns>
        public MemberInvestEntity SelectBalance(int userId)
        {
            MemberInvestEntity ent = new MemberInvestEntity();
            #region 获取用户余额


            var sql = string.Format("SELECT available_balance FROM dbo.hx_member_table WHERE registerid={0};", userId);

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var memberEnt = InitMemberEntity(ds.Tables[0]);
                if (memberEnt != null)
                {
                    ent.userId = userId;
                    ent.userBalance = memberEnt.available_balance;

                    return ent;
                }
            }
            return null;
            #endregion
        }



        /// <summary>
        /// 获取用户 余额+优惠券
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberInvestEntity.</returns>
        public ActivityBonusEntity SelectBonus(int userId, string sltd)
        {
            ActivityBonusEntity ent = new ActivityBonusEntity();
            #region 获取用户余额


            var sql = string.Format("" +
                                    "SELECT bonus_account_id,activity_schedule_name,amount_of_reward,use_lower_limit from bonus_account where membertable_registerid={0} and reward_state=0;" + "SELECT LogId, ActivityName, AddRate FROM[dbo].[ActivityLogs] WHERE UserId = {0} AND UseStatus = 0", userId);

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {

                ent.bonus = InitBonusList(ds.Tables[0], sltd);
                ent.addRate = InitActivityLogs(ds.Tables[1], sltd);
                return ent;

            }
            return null;
            #endregion
        }

        /// <summary>
        /// 获取用户奖励(优惠券 代金券)列表.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="rewardState">0 未使用   1已使用  2 已过期 3 锁定中 -1 查询全部</param>
        /// <returns>Entities.Response.BasePage&lt;List&lt;Entities.Response.Bonus.BonusEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 09:58:27
        /// <exception cref="System.NotImplementedException"></exception>
        public BasePage<List<BonusEntity>> SelectBonuses(int pageIndex, int pageSize, int userId, int rewardState)
        {
            BasePage<List<BonusEntity>> page = new BasePage<List<BonusEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (userId > 0)
            {
                sbWhere.Append(" and membertable_registerid = " + userId);

            }
            if (rewardState >= 0)
            {
                sbWhere.Append(" and reward_state = " + rewardState);
            }


            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@OrderField", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };
            parameters[0].Value = "bonus_account";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "bonus_account_id";
            parameters[3].Value = "bonus_account_id";
            parameters[4].Value = pageIndex;
            parameters[5].Value = pageSize;
            parameters[6].Value = 1;//desc
            parameters[7].Value = "bonus_account_id,activity_schedule_name,amount_of_reward,use_lower_limit,reward_state,start_date,end_date,entry_time";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitBonusList(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }
        /// <summary>
        /// 获取个人抵扣券、加息券列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面容量</param>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        public BasePage<List<OwnBonusEntity>> SelectOwnBonuses(int pageIndex, int pageSize, int userID)
        {
            BasePage<List<OwnBonusEntity>> page = new BasePage<List<OwnBonusEntity>>();
            const string proc = @"procPagination";
            var sbWhere = new StringBuilder(" 1>0 AND RewTypeID>1 ");
            sbWhere.AppendFormat(" and registerID={0} ", userID);
            var recordCount = new SqlParameter("@recordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };//@recordCount int OUTPUT--总记录数(存储过程输出参数)
            IDataParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.NVarChar,255),//@tblName NVarChar(255), --表名
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,1500),//@strWhere  NVarChar(1500), --查询条件(注意: 不要加where)
                    new SqlParameter("@prKey", SqlDbType.NVarChar,255),//@prKey NVarChar(255), --主键
                    new SqlParameter("@pageIndex", SqlDbType.Int,4),//@pageIndex  int, --页码
                    new SqlParameter("@pageSize", SqlDbType.Int,4),//@pageSize   int, --页尺寸
                    new SqlParameter("@fldName", SqlDbType.NVarChar,255),//@fldName NVarChar(255), --排序的字段名
                    new SqlParameter("@sort", SqlDbType.NVarChar,4),//@sort NVarChar(255), --排序的方法
                    new SqlParameter("@strGetFields", SqlDbType.NVarChar,1000),//@strGetFields NVarChar(1000), --需要返回的列
                    recordCount  };
            parameters[0].Value = "V_OwnBonuses";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "UserAct";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = "UseState,CreateTime";
            parameters[6].Value = "asc";
            parameters[7].Value = @"UserAct,RewTypeID,Amt,CONVERT(varchar(100), CreateTime, 102)+'-'+CONVERT(varchar(100), AmtEndtime, 102) ActTime,TypeName,UseState,UseLifeLoan,'' as UseLifeLoanMessage,ISNULL( case when RewTypeID=2 then '单笔投资满'+Convert(varchar,Uselower)+'元可使用' when RewTypeID=3 then '单笔投资仅限使用一张加息券' end,'') Remark";
            //Uselower,case when AmtEndtime <= getdate() then 2 else UseState end UseState,ActName,
            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = (page.recordCount + pageSize - 1) / pageSize;
                    var item = DataHelper.GetEntities<OwnBonusEntity>(ds.Tables[0]).ToList();
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }
        /// <summary>
        /// 获取个人抵扣券、加息券详情
        /// </summary>
        /// <param name="userAct">券编号</param>
        /// <returns></returns>
        public OwnBonusDetailEntity SelectOwnBonusesDetail(int userAct)
        {
            OwnBonusDetailEntity page = new OwnBonusDetailEntity();
            string sql = @"select ISNULL(ActName,'') ActName,ISNULL( case when RewTypeID=2 then '单笔投资满'+Convert(varchar,Uselower)+'元可使用' when RewTypeID=3 then '单笔投资仅限使用一张加息券' end,'') Remark,
                            ISNULL(CONVERT(varchar(100), CreateTime, 102)+'-'+CONVERT(varchar(100), AmtEndtime, 102),'' ) AmtEndTime,RewTypeID,UseLifeLoan,'' as UseLifeLoanMessage
                            from V_OwnBonuses
                            where userAct = @userAct";//待后期优化修改值命名问题
            var ds = DbHelper.Query(sql, new SqlParameter[] { new SqlParameter { ParameterName = "userAct", Value = userAct } });
            if (DataSetIsNotNull(ds))
            {
                var item = DataHelper.GetEntity<OwnBonusDetailEntity>(ds.Tables[0]);
                page = item;
                return page;
            }
            return null;
        }
        
        /// <summary>
        /// 新版获取用户代金券+加息券 刘佳
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberInvestEntity.</returns>
        public ActivityBonusEntity SelectActivity(int userId, string sltd)
        {
            ActivityBonusEntity ent = new ActivityBonusEntity();

            #region 获取用户代金券+加息券
            //按照加息券的过期时间显示--贾磊同意修改
            //var sql = string.Format("SELECT ua.UserAct as bonus_account_id,ta.ActName as activity_schedule_name,ua.Amt as amount_of_reward,ua.Uselower as use_lower_limit,UseLifeLoan FROM  dbo.hx_UserAct as ua inner join dbo.hx_RewardType as at on ua.RewTypeID = at.RewTypeID inner join dbo.hx_ActivityTable as ta on ua.ActID = ta.ActID  WHERE ua.registerid = {0} AND ta.ActStarttime <= GETDATE() AND ta.ActEndtime >= GETDATE() AND ua.UseState = 0 AND ua.RewTypeID = 2 ; " + "SELECT  ua.UserAct as LogId,ta.ActName as ActivityName,ua.Amt as AddRate,UseLifeLoan FROM  dbo.hx_UserAct as ua inner join dbo.hx_RewardType as at on ua.RewTypeID = at.RewTypeID inner join dbo.hx_ActivityTable as ta on ua.ActID = ta.ActID  WHERE ua.registerid = {0} AND ta.ActStarttime <= GETDATE() AND ta.ActEndtime >= GETDATE() AND ua.UseState = 0 AND ua.RewTypeID =3", userId); //使用状态 UseState:  0未使用 1已使用 2已过期 3锁定中;   奖励类型RewTypeID:1现金   2抵扣券    3加息券

            var sql = string.Format("SELECT ua.UserAct as bonus_account_id,ta.ActName as activity_schedule_name,ua.Amt as amount_of_reward,ua.Uselower as use_lower_limit,isnull(UseLifeLoan,'') as UseLifeLoan FROM  dbo.hx_UserAct as ua inner join dbo.hx_RewardType as at on ua.RewTypeID = at.RewTypeID inner join dbo.hx_ActivityTable as ta on ua.ActID = ta.ActID  WHERE ua.registerid = {0} AND ua.AmtEndtime>= '{1}' AND ua.UseState = 0 AND ua.RewTypeID = 2 ; " + "SELECT  ua.UserAct as LogId,ta.ActName as ActivityName,ua.Amt as AddRate,isnull(UseLifeLoan,'') as UseLifeLoan FROM  dbo.hx_UserAct as ua inner join dbo.hx_RewardType as at on ua.RewTypeID = at.RewTypeID inner join dbo.hx_ActivityTable as ta on ua.ActID = ta.ActID  WHERE ua.registerid = {0} AND ua.AmtEndtime>= '{1}' AND ua.UseState = 0 AND ua.RewTypeID =3", userId,DateTime.Now.ToString("yyyy-MM-dd")); //使用状态 UseState:  0未使用 1已使用 2已过期 3锁定中;   奖励类型RewTypeID:1现金   2抵扣券    3加息券

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                ent.bonus = InitBonusList(ds.Tables[0], sltd);//代金券（红包）
                ent.addRate = InitActivityLogs(ds.Tables[1], sltd);//加息券
                return ent;
            }
            return null;
            #endregion
        }

        /// <summary>
        /// 获取用户常用地址
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>NormalAreaEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 16:51:07
        public ResponseUserAddressEntity SelectUserAddress(int userId)
        {
            var sql = "SELECT provinceId,provinceName,cityId,cityName,countyId,countyName,detailAddress,userName,mobile,zipCode FROM [UserAddress] WHERE userId=@userId";

            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;


            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitUserAddressEntityList(ds.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 通过id获取优惠券实体
        /// </summary>
        /// <param name="bonusId">The bonus identifier.</param>
        /// <returns>BonusEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 09:26:26
        public List<BonusEntity> SelectBonusById(int userId, string bonusId)
        {
            var sql =
                string.Format(
                    "SELECT  a.registerid AS membertable_registerid, a.UserAct AS bonus_account_id,ISNULL(t.ActName,'') AS activity_schedule_name,a.Amt AS amount_of_reward , a.Uselower as use_lower_limit  , ISNULL(t.ActState, 0)  AS reward_state   FROM[dbo].[hx_UserAct] a LEFT JOIN dbo.hx_ActivityTable t ON a.UserAct = t.ActID WHERE  a.UserAct IN ({0}) and  a.registerid={1} and  a.UseState=0",
                    bonusId, userId);
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitBonusList(ds.Tables[0]);
            }
            return null;
        }

    }
}
