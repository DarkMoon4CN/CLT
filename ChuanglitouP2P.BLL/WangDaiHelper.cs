using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL
{
    public class WangDaiHelper
    {

        /// <summary>
        /// 获得满标的借款标
        /// </summary>
        /// <returns></returns>
        public DataTable GetBorrowTargetList(string date, int page, int pageSize, out int totalCount, out double totalAmount)
        {
            DataTable dt = new DataTable();
            DataTable dttotalcount = new DataTable();
            try
            {

                StringBuilder sbsql = new StringBuilder();
                sbsql.Append("select top " + pageSize.ToString() + " * from(");
                sbsql.Append("select row_number() over (order by a.targetid asc) as RowNumber,targetid,borrower_registerid,borrowing_title,borrowing_balance,fundraising_amount,annual_interest_rate,life_of_loan,unit_day,project_type_id,payment_options, end_time from  hx_borrowing_target a");
                sbsql.Append(" where ");
                sbsql.Append("end_time>='" + date + " 00:00:00' and end_time<='" + date + " 23:59:59'");
                sbsql.Append(" and (tender_state=4 or tender_state=5)");
                sbsql.Append(" and a.borrowing_balance=(select sum(investment_amount) from hx_bid_records where targetid=a.targetid)");
                sbsql.Append(") as temp where RowNumber >" + (pageSize * (page - 1)).ToString());

                string sql = "select Count(*) as 'totalCount',sum(borrowing_balance) as 'totalAmount' from hx_borrowing_target a where end_time>='" + date + " 00:00:00' and end_time<='" + date + " 23:59:59' and (tender_state=4 or tender_state=5) and a.borrowing_balance=(select sum(investment_amount) from hx_bid_records where targetid=a.targetid)";
                dttotalcount = DbHelperSQL.GET_DataTable_List(sql);

                dt = DbHelperSQL.GET_DataTable_List(sbsql.ToString());

                totalCount = 0;

                totalAmount = 0.00;

                if (dttotalcount.Rows.Count > 0)
                {
                    if (dttotalcount.Rows[0][0] != null)
                    {
                        totalCount =Convert.ToInt32(dttotalcount.Rows[0][0]);
                    }
                    if (dttotalcount.Rows[0][1] != null)
                    {
                        totalAmount = Convert.ToDouble(dttotalcount.Rows[0][1]);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("网贷之家查询当天满标借款标，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace);
                totalCount = 0;
                totalAmount = 0;
                return null;
            }

        }

        public DataTable GetBidRecordList(int targetid)
        {
            DataTable dt = new DataTable();
            try
            {
                if (targetid > 0)
                {
                    string sql = "select investor_registerid,investment_amount,invest_time from hx_bid_records where targetid=" + targetid.ToString();
                    dt = DbHelperSQL.GET_DataTable_List(sql);
                }

            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("网贷之家查询投标记录，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace);
            }
            return dt;
        }
    }
}
