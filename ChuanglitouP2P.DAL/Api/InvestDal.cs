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
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model.Invest;

namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class InvestDal.
    /// </summary>
    public class InvestDal : ImplBase
    {
        /// <summary>
        /// 获取投资列表--解志辉.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>InvestEntity.</returns>
        public List<InvestEntity> SelectInvests(int userId)
        {
            var sql = "select * from ViewInvestList where investMemberId=@userId";

            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = userId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitInvestList(ds.Tables[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取投资列表--解志辉.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="timeFrom">The time from.</param>
        /// <param name="timeTo">The time to.</param>
        /// <returns>BasePage&lt;List&lt;InvestEntity&gt;&gt;.</returns>
        public BasePage<List<InvestEntity>> SelectInvests(int pageIndex, int pageSize, int userId, string timeFrom, string timeTo)
        {
            BasePage<List<InvestEntity>> page = new BasePage<List<InvestEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (userId > 0)
            {
                sbWhere.Append(" and investMemberId = " + userId);
                sbWhere.Append(" and ordstate=1");

            }
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                sbWhere.AppendFormat(" and (createdOn BETWEEN '{0}' AND '{1}') ", timeFrom, timeTo);
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
            parameters[0].Value = "ViewInvestList";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "recordId";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "*";

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
        /// 获取投资详情--解志辉
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>InvestEntity.</returns>
        public InvestEntity SelectInvestDetail(int recordId)
        {
            var sql = "select * from ViewInvestDetail where recordId=@recordId";
            SqlParameter[] parameters = {
                    new SqlParameter("@recordId", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = recordId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitInvestEntity(ds.Tables[0]);
            }
            return null;
        }

        public BasePage<List<InvestRecordEntity>> SelectInvestRecordsByID(int pageIndex, int pageSize, int recordId)
        {
            BasePage<List<InvestRecordEntity>> page = new BasePage<List<InvestRecordEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (recordId > 0)
            {
                sbWhere.Append(" and targetid = " + recordId);
                sbWhere.Append(" and ordstate =1");

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
            parameters[0].Value = "ViewInvestRecord";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "bid_records_id";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "username,investment_amount,invest_time";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitInvestRecordList(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }

        /// <summary>
        /// 获取累计邀请注册人数--刘佳
        /// </summary>
        /// <param name="Invpeopleid">邀请人ID</param>
        /// <returns></returns>
        public int SelectInvitationRegisterCount(int Invpeopleid)
        {
            var sql = "SELECT COUNT(invitationid) FROM hx_td_Userinvitation where Invpeopleid=@Invpeopleid";
            SqlParameter[] parameters = {
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4)
            };
            parameters[0].Value = Invpeopleid;
            return ConvertHelper.ParseValue(DbHelper.GetSingle(sql, parameters), 0);
        }
        /// <summary>
        /// 根据邀请人ID获取邀请码
        /// </summary>
        /// <param name="Invpeopleid">邀请人ID</param>
        /// <returns></returns>
        public string SelectInvitedCode(int Invpeopleid)
        {
            var sql = "SELECT invitedcode FROM hx_member_table where registerid=@Invpeopleid";
            SqlParameter[] parameters = {
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4)
            };
            parameters[0].Value = Invpeopleid;
            return ConvertHelper.ParseValue(DbHelper.GetSingle(sql, parameters), "");
        }

        /// <summary>
        /// 获取累计邀请信息--刘佳
        /// </summary>
        /// <param name="Invpeopleid">邀请人ID</param>
        /// <returns></returns>
        public InvestCountEntity SelectInvitationInvestCount(int Invpeopleid)
        {
            var sql = "SELECT  COUNT(invitationid) FROM hx_td_Userinvitation where Invpeopleid=@Invpeopleid and invpersonid IN(SELECT DISTINCT investor_registerid from hx_Bid_records )";
            SqlParameter[] parameters = {
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4)
            };
            parameters[0].Value = Invpeopleid;
            int investCount = ConvertHelper.ParseValue(DbHelper.GetSingle(sql, parameters), 0);

            var ent = new InvestCountEntity();
            ent.InvestCount = investCount;//获取累计邀请投资人数
            ent.RegisterCount = SelectInvitationRegisterCount(Invpeopleid);//获取累计邀请注册人数
            ent.strLink = SelectInvitedCode(Invpeopleid);//获取邀请码
            return ent;
        }

        public InvitationDetail SelectInvitationDetail(int Invpeopleid)
        {
            var sql = "select  b.username as UserName,isnull(sum(c.investment_amount),0.00) as InvestedAmount,(select top 1 invest_time from hx_Bid_records where hx_Bid_records.investor_registerid = a.invpersonid order by invest_time desc) as invest_time, b.registration_time from hx_td_Userinvitation a left join hx_member_table b on a.invpersonid = b.registerid left join hx_Bid_records c on a.invpersonid = c.investor_registerid where a.Invpeopleid = @Invpeopleid group by a.invpersonid,b.username,b.registration_time order by invest_time desc,b.registration_time desc";
            SqlParameter[] parameters = {
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4)
            };
            parameters[0].Value = Invpeopleid;
            var ent = new InvitationDetail() { InvitationTotalAmount = "0.00", InvitedInvestedPeopleCount = "0", InvitedPeopleCount = "0" };
            var ds = DbHelper.Query(sql, parameters);
            decimal invitationTotalAmount = 0.00M;
            int invitedPeopleCount = 0, invitedInvestedPeopleCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                InitInvitationDetailList(ds.Tables[0], ref invitedPeopleCount, ref invitedInvestedPeopleCount, ref invitationTotalAmount);
            ent.InvitationTotalAmount = invitationTotalAmount.ToString("f2") + "元";
            ent.InvitedPeopleCount = invitedPeopleCount.ToString() + "人";
            ent.InvitedInvestedPeopleCount = invitedInvestedPeopleCount.ToString() + "人";
            return ent;
        }

        public BasePage<List<UserInvestedStatistics>> SelectInvitationDetailPage(int Invpeopleid, int pageIndex, int pageSize = 12)
        {
            BasePage<List<UserInvestedStatistics>> result = new BasePage<List<UserInvestedStatistics>>() { pageCount = 0, recordCount = 0, rows = new List<UserInvestedStatistics>() };
            var sql = "select  b.username as UserName,isnull(sum(c.investment_amount),0.00) as InvestedAmount,(select top 1 invest_time from hx_Bid_records where hx_Bid_records.investor_registerid = a.invpersonid order by invest_time desc) as invest_time, b.registration_time from hx_td_Userinvitation a left join hx_member_table b on a.invpersonid = b.registerid left join hx_Bid_records c on a.invpersonid = c.investor_registerid where a.Invpeopleid = @Invpeopleid group by a.invpersonid,b.username,b.registration_time order by invest_time desc,b.registration_time desc";
            SqlParameter[] parameters = {
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4)
            };
            parameters[0].Value = Invpeopleid;
            var ent = new InvitationDetail() { InvitationTotalAmount = "0.00", InvitedInvestedPeopleCount = "0", InvitedPeopleCount = "0" };
            var ds = DbHelper.Query(sql, parameters);
            decimal invitationTotalAmount = 0.00M;
            int invitedPeopleCount = 0, invitedInvestedPeopleCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var items = InitInvitationDetailList(ds.Tables[0], ref invitedPeopleCount, ref invitedInvestedPeopleCount, ref invitationTotalAmount);
                if (items.Count > 0)
                {
                    result.pageCount = items.Count / pageSize;
                    if (items.Count % pageSize > 0)
                        result.pageCount += 1;
                    result.recordCount = items.Count;
                    if (pageIndex < 1)
                        pageIndex = 1;
                    result.rows = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// 回款计划--解志辉
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BasePage<List<ResponseInvestIncomeEntity>> SelectIncomeList(int pageIndex, int pageSize, int userId)
        {
            BasePage<List<ResponseInvestIncomeEntity>> page = new BasePage<List<ResponseInvestIncomeEntity>>();
            const string proc = @"procPagination";
            var sbWhere = new StringBuilder(" 1>0");
            sbWhere.Append("and investor_registerid = " + userId);

            var recordCount = new SqlParameter("@recordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.NVarChar,255),
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@prKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@pageIndex", SqlDbType.Int,4),
                    new SqlParameter("@pageSize", SqlDbType.Int,4),
                    new SqlParameter("@fldName", SqlDbType.NVarChar,255),
                    new SqlParameter("@sort", SqlDbType.NVarChar,4),
                    new SqlParameter("@strGetFields", SqlDbType.NVarChar,1000),
                    recordCount  };

            parameters[0].Value = "ViewIncomeList";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "income_statement_id";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = "interest_payment_date desc,invest_time";
            parameters[6].Value = "desc";//desc
            parameters[7].Value = "investment_amount,interest_payment_date,interest_payment_date as day,repayment_amount,payment_status,repayment_period";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = (page.recordCount + pageSize - 1) / pageSize;
                    var item = InitResponseInvestIncomeEntity(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }
            }
            return null;
        }

        public decimal SelectTotalIncome(int userId)
        {
            var sql = "SELECT SUM(repayment_amount) AS totalInvest FROM ViewIncomeList WHERE investor_registerid=" + userId;
            var res = DbHelper.GetSingle(sql);

            return ConvertHelper.ParseValue(res, 0M);
        }
    }
}
