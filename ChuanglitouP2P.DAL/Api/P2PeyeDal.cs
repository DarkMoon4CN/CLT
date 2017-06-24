using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.ttnz;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    public class P2PeyeDal : ImplBase
    {
        /// <summary>
        /// Selects the loan list.
        /// </summary>
        /// <param name="status">	
        /// 标的状态:0.正在投标中的借款标;1.已完成(包括还款中和已完成的借款标). 
        /// 状态为1是对应平台满标字段的值检索,状态为0就以平台发标时间字段检索.</param>
        /// <param name="time_from">始时间如:2014-05-09 06:10:00,</param>
        /// <param name="time_to">截止时间如:2014-05-09 06:10:00,</param>
        /// <param name="page_size">每页记录条数.</param>
        /// <param name="page_index">请求的页码.</param>
        /// <returns>List&lt;LoanEntity&gt;.</returns>
        /// 创建者：解志辉 
        public Pagination<LoanEntity> SelectLoanList(int status, string timeFrom, string timeTo, PageParam pageParam)
        {

            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");
            sbWhere.Append(" and borrowing_balance>=50");

            if (status == 0)
            {
                sbWhere.Append(" and tender_state=2 ");
                sbWhere.AppendFormat(" and (CreatedOn BETWEEN '{0}' AND '{1}') ", timeFrom, timeTo);
            }
            if (status == 1)
            {
                // sbWhere.Append(" and ( tender_state=3 or tender_state=4 or tender_state=5)");
                sbWhere.Append(" and ( tender_state=5)");
                sbWhere.AppendFormat(" and (end_time BETWEEN '{0}' AND '{1}') ", timeFrom, timeTo);
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
            parameters[0].Value = "ViewTargetWithRecords";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "targetid";
            parameters[3].Value = pageParam.PageCurrent;
            parameters[4].Value = pageParam.PageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "*";
            //var pageCount=0;
            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {

                pageParam.RecordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                pageParam.PageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                var item = InitEntity(ds.Tables[0]);
                return item != null ? item.AsPagination(pageParam) : null;
            }
            return null;

        }
        /// <summary>
        /// 根据标ID获取投资数据 解志辉
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Pagination<InvestmentEntity> SelectInvestmentList(int pId, PageParam pageParam)
        {

            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");
            sbWhere.Append(" and investment_amount>=10");
            sbWhere.Append(" and targetId=" + pId);



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
            parameters[3].Value = pageParam.PageCurrent;
            parameters[4].Value = pageParam.PageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "*";
            //var pageCount=0;
            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {

                pageParam.RecordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                pageParam.PageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);

                var item = InitInvestmentEntity(ds.Tables[0]);
                return item != null ? item.AsPagination(pageParam) : null;
            }
            return null;
        }

        /// <summary>
        /// 正在投标中的借款标
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>List&lt;BorrowingEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-23 09:42:24
        /// <exception cref="System.NotImplementedException"></exception>
        /// 创 建 者：解志辉
        ///  创建日期：2016-06-23 09:42:06
        public List<BorrowingEntity> SelectLoanList(int status)
        {

            string str = @"select * from ViewTargetWithRecords where tender_state=2 and End_time>getdate()";  
            //var pageCount=0;
            var ds = DbHelper.Query(str);
            var item = InitBorrowingEntity(ds.Tables[0]);
            return item;
        }


        /// <summary>
        /// 封装投资结果集
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static List<InvestmentEntity> InitInvestmentEntity(DataTable dt)
        {

            var entityList = new List<InvestmentEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new InvestmentEntity();

                        var row = dt.Rows[n];

                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.id = dt.Rows[n]["targetid"].ToString();// ConvertHelper.ParseValue(dt.Rows[n]["MemberType"].ToString(), 0);
                        }

                        entity.link = string.Format("http://"+PublicURL.NewPCUrl+"/invest_borrow_{0}.html", entity.id);

                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            entity.username = dt.Rows[n]["username"].ToString().Substring(0, 4) + "*****";

                        }

                        if (ContainsColumn(dt.Columns, "investor_registerid", row))
                        {

                            entity.userid = dt.Rows[n]["investor_registerid"].ToString();
                        }

                        //entity.type = "手动";

                        if (ContainsColumn(dt.Columns, "invest_state", row))
                        {
                            switch (dt.Rows[n]["invest_state"].ToString())
                            {
                                case "1":
                                    entity.status = "成功";
                                    break;
                                case "2":
                                    entity.status = "失败";
                                    break;
                                default:
                                    entity.status = "成功";
                                    break;
                            }

                        }

                        if (ContainsColumn(dt.Columns, "investment_amount", row))
                        {
                            entity.account = ConvertHelper.ParseValue(dt.Rows[n]["investment_amount"].ToString(), 0D);

                        }


                        if (ContainsColumn(dt.Columns, "invest_time", row))
                        {
                            entity.add_time = ConvertHelper.ParseValue(dt.Rows[n]["invest_time"], DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

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
                LoggerHelper.Error(" public List<LoanEntity> InitEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
        /// <summary>
        /// 封装结果集
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static List<LoanEntity> InitEntity(DataTable dt)
        {
            var entityList = new List<LoanEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new LoanEntity();

                        var row = dt.Rows[n];


                        if (ContainsColumn(dt.Columns, "targetid",row))
                        {
                            entity.id = dt.Rows[n]["targetid"].ToString();// ConvertHelper.ParseValue(dt.Rows[n]["MemberType"].ToString(), 0);
                        }
                        entity.platform_name = "创利投";
                        entity.url = string.Format("http://"+PublicURL.NewPCUrl+"/invest_borrow_{0}.html", entity.id);



                        if (ContainsColumn(dt.Columns, "borrowing_title", row))
                        {
                            entity.title = dt.Rows[n]["borrowing_title"].ToString();
                        }


                        if (ContainsColumn(dt.Columns, "username",row))
                        {
                            entity.username = dt.Rows[n]["username"].ToString().Substring(0, 4) + "*****";
                        }


                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {

                            entity.userid = dt.Rows[n]["borrower_registerid"].ToString();
                        }

                        //tender_state  2 复审通过(开标上线)     3 满标 (还款中)     4放款 (还款中)   5 已还清   

                        if (ContainsColumn(dt.Columns, "tender_state", row))
                        {
                            switch (dt.Rows[n]["tender_state"].ToString())
                            {
                                case "3":
                                case "4":
                                case "5":
                                    {
                                        entity.status = "1";
                                    }
                                    break;
                                case "1":
                                    {
                                        entity.status = "0";
                                    }
                                    break;
                                default: { }
                                    break;
                            }
                        }

                        // entity.c_type = ConvertHelper.ParseValue(dt.Rows[n]["project_type_id"].ToString(), 0);
                        entity.c_type = "0";

                        if (ContainsColumn(dt.Columns, "borrowing_balance", row))
                        {
                            entity.amount = ConvertHelper.ParseValue(dt.Rows[n]["borrowing_balance"].ToString(), 0M).ToString();


                        }
                        if (ContainsColumn(dt.Columns, "annual_interest_rate", row))
                        {
                            //  entity.rate = ConvertHelper.ParseValue(dt.Rows[n]["annual_interest_rate"].ToString(), 0D) * 0.01; 

                            entity.rate = (ConvertHelper.ParseValue(dt.Rows[n]["annual_interest_rate"].ToString(), 0D) * 0.01).ToString();

                        }
                        if (ContainsColumn(dt.Columns, "life_of_loan", row))
                        {
                            // entity.period = ConvertHelper.ParseValue(dt.Rows[n]["life_of_loan"].ToString(), 0);
                            entity.period = dt.Rows[n]["life_of_loan"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "unit_day", row))
                        {

                            switch (dt.Rows[n]["unit_day"].ToString())
                            {
                                case "1"://月

                                    entity.p_type = "1";
                                    break;
                                case "3"://天
                                    entity.p_type = "0";
                                    break;
                                default: { }
                                    break;
                            }

                        }


                        if (ContainsColumn(dt.Columns, "payment_options", row))
                        {
                            //1 按月等额本息  3 每月还息，到期还本   4 一次性还本付息(按天计息)
                            switch (dt.Rows[n]["payment_options"].ToString())
                            {
                                case "1":

                                    entity.pay_way = "1";
                                    break;
                                case "3":

                                    entity.pay_way = "2";
                                    break;
                                case "4":

                                    entity.pay_way = "3";
                                    break;
                                default: { }
                                    break;
                            }

                        }

                        switch (dt.Rows[n]["tender_state"].ToString())
                        {
                            case "5":
                                {
                                    entity.process = "1";
                                }
                                break;
                            default:
                                {
                                    var x = ConvertHelper.ParseValue(dt.Rows[n]["H_Repayment_Amt"].ToString(), 0D);
                                    entity.process = ConvertHelper.ParseValue((x / ConvertHelper.ParseValue(entity.amount, 0D)), "");
                                }
                                break;
                        }




                        if (ContainsColumn(dt.Columns, "start_time", row))
                        {
                            entity.start_time = ConvertHelper.ParseValue(dt.Rows[n]["start_time"], DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

                        }

                        if (ContainsColumn(dt.Columns, "end_time", row))
                        {
                            entity.end_time = ConvertHelper.ParseValue(dt.Rows[n]["end_time"], DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

                        }
                        if (ContainsColumn(dt.Columns, "invest_num", row))
                        {
                            // entity.invest_num = ConvertHelper.ParseValue(dt.Rows[n]["invest_num"], 0);
                            entity.invest_num = dt.Rows[n]["invest_num"].ToString();

                        }

                        entity.c_reward = "0.0";
                        entity.guarantee = "0.0";
                        entity.reward = "0.0";


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
                LoggerHelper.Error(" public List<LoanEntity> InitEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
    }
}
