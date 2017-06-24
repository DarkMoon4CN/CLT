using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Capital;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class MemberDal.
    /// </summary>
    public class CapitalDal : ImplBase
    {
        /// <summary>
        /// 获取用户资金总体概括.【资产总计 累计收益 待收收益 已收收益  累计投资】
        /// </summary>
        /// <param name="userId">用户id.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 13:10:31
        public MemberEntity SelectMemberFundsInformation(int userId)
        {
            var sql = @"select available_balance  from hx_member_table  WHERE  registerid =@registerid";
            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = userId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = InitMemberEntity(ds.Tables[0]);
                FillMemberEntityAccountTotalAssets(ref ent, userId);
                ent.totalGains = GetTotalGains(userId);//累计赚取总额
                ent.receivedIncome = GetInterest(userId);//已收收益
                ent.receivingIncome = GetUnpaidInterest(userId);//待收收益
                ent.totalInverstAmount = GetTotalInverstAmount(userId); //累计投资
                return ent;
            }
            return null;
        }


        /// <summary>
        /// 获取用户资金流水列表
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;CapitalAccountWater&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 15:24:43
        public BasePage<List<CapitalAccountWater>> SelectFundWater(int pageIndex, int pageSize, int userId)
        {
            BasePage<List<CapitalAccountWater>> page = new BasePage<List<CapitalAccountWater>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (userId > 0)
            {
                sbWhere.Append(" and membertable_registerid = " + userId);

            }
            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };
            parameters[0].Value = "hx_Capital_account_water";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "account_water_id";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "income,expenditure,time_of_occurrence,account_balance,types_Finance";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitCapitalAccountWater(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;

        }

        /// <summary>
        /// 资金总计接口.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-18 17:23:55
        public MemberEntity TotalCapital(int userId)
        {
            ////可用余额 投资冻结
            string sql = "select  available_balance,frozen_sum  from  hx_member_table where registerid=@userId";
            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;
            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = InitMemberEntity(ds.Tables[0]);
                FillMemberEntityAccountTotalAssets(ref ent, userId);
                return ent;
            }
            return null;
        }

        /// <summary>
        /// 累计投资接口
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;InvestEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-01 10:21:21
        public BasePage<List<InvestEntity>> SelectTotalInvest(int pageIndex, int pageSize, int userId)
        {
            BasePage<List<InvestEntity>> page = new BasePage<List<InvestEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (userId > 0)
            {
                sbWhere.Append(" and investMemberId = " + userId);

            }
            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };
            parameters[0].Value = "ViewInvestDetail";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "recordId";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "recordId,targetTitle,investAmount";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitInvestList(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }

        /// <summary>
        /// 获取提现记录
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;ResponsePresentEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 17:31:08 
        public BasePage<List<PresentEntity>> SelectPresentRecord(int pageIndex, int pageSize, int userId)
        {
            BasePage<List<PresentEntity>> page = new BasePage<List<PresentEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (userId > 0)
            {
                sbWhere.Append(" and registerid = " + userId);
                sbWhere.Append(" and OpenBankId is not null");

            }
            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };
            parameters[0].Value = "hx_td_UserCash";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "UserCashId";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "OrdIdTime,OrdIdState,TransAmt";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitPresentList(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }

        /// <summary>
        /// 累计收益接口
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 18:11:19
        public MemberEntity SelectTotalGains(int userId)
        {
            var ent = new MemberEntity
            {
                interestIncome = GetTotalGains(userId),
                activityReward = GetBonuses(userId),
                couponIncome = 0M,
                claimsIncome = 0M
            };
            return ent;
        }
        /// <summary>
        /// 获取提现总金额--解志辉
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal SelectTotalPresent(int userId)
        {
            var sql = "SELECT  SUM(TransAmt) FROM hx_td_UserCash WHERE OrdIdState=3 AND registerid=" + userId;
            var res = DbHelper.GetSingle(sql);

            return ConvertHelper.ParseValue(res, 0M);
        }

        /// <summary>
        /// 填充 资产总计 + 待收本金 + 待收收益 + 奖励金额
        /// <remarks>调用前必须已经调用了 <see cref="InitMemberEntity"/> 方法。</remarks>
        /// </summary>
        /// <param name="entity">需要填充的实体</param>
        /// <param name="userId">用户唯一标识</param>
        public void FillMemberEntityAccountTotalAssets(ref MemberEntity entity, int userId)
        {
            entity.receivingIncome = GetUnpaidInterest(userId);//待收收益

            entity.totalAmount = GetPrincipal(userId);//待收本金

            entity.bonusAmount = GetBonuses(userId);//奖励金额

            //资产总计 = 账户余额 + 待收本金 + 待收收益 + 冻结金额 + 奖励金额
            entity.account_total_assets = entity.available_balance + entity.totalAmount + entity.receivingIncome + entity.frozen_sum + entity.bonusAmount;
        }
    }
}
