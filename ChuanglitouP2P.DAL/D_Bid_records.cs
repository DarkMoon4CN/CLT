using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
    /// <summary>
    /// 数据访问类:Bid_records
    /// </summary>
    public partial class D_Bid_records
    {
        public D_Bid_records()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("bid_records_id", "hx_Bid_records");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int bid_records_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from hx_Bid_records");
            strSql.Append(" where bid_records_id=@bid_records_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@bid_records_id", SqlDbType.Int,4)
            };
            parameters[0].Value = bid_records_id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_Bid_records model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_Bid_records(");
            strSql.Append("borrower_registerid,targetid,loan_number,annual_interest_rate,current_period,investment_amount,value_date,investment_maturity,invest_time,invest_state,flow_return,repayment_amount,repayment_period,investor_registerid,payment_status,withoutinterest,invitationcode,OrdId,JiaxiNum,BonusAmt)");
            strSql.Append(" values (");
            strSql.Append("@borrower_registerid,@targetid,@loan_number,@annual_interest_rate,@current_period,@investment_amount,@value_date,@investment_maturity,@invest_time,@invest_state,@flow_return,@repayment_amount,@repayment_period,@investor_registerid,@payment_status,@withoutinterest,@invitationcode,@OrdId,@JiaxiNum,@BonusAmt)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
                    new SqlParameter("@targetid", SqlDbType.Int,4),
                    new SqlParameter("@loan_number", SqlDbType.Decimal,18),
                    new SqlParameter("@annual_interest_rate", SqlDbType.Decimal,17),
                    new SqlParameter("@current_period", SqlDbType.Int,4),
                    new SqlParameter("@investment_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@value_date", SqlDbType.DateTime),
                    new SqlParameter("@investment_maturity", SqlDbType.DateTime),
                    new SqlParameter("@invest_time", SqlDbType.DateTime),
                    new SqlParameter("@invest_state", SqlDbType.Int,4),
                    new SqlParameter("@flow_return", SqlDbType.Int,4),
                    new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@repayment_period", SqlDbType.DateTime),
                    new SqlParameter("@investor_registerid", SqlDbType.Int,4),
                    new SqlParameter("@payment_status", SqlDbType.Int,4),
                    new SqlParameter("@withoutinterest", SqlDbType.Decimal,17),
                    new SqlParameter("@invitationcode", SqlDbType.VarChar,50),
                    new SqlParameter("@OrdId", SqlDbType.Decimal,20),
                    new SqlParameter("@JiaxiNum", SqlDbType.Decimal,20),
                    new SqlParameter("@BonusAmt", SqlDbType.Decimal,20)};
            parameters[0].Value = model.borrower_registerid;
            parameters[1].Value = model.targetid;
            parameters[2].Value = model.loan_number;
            parameters[3].Value = model.annual_interest_rate;
            parameters[4].Value = model.current_period;
            parameters[5].Value = model.investment_amount;
            parameters[6].Value = model.value_date;
            parameters[7].Value = model.investment_maturity;
            parameters[8].Value = model.invest_time;
            parameters[9].Value = model.invest_state;
            parameters[10].Value = model.flow_return;
            parameters[11].Value = model.repayment_amount;
            parameters[12].Value = model.repayment_period;
            parameters[13].Value = model.investor_registerid;
            parameters[14].Value = model.payment_status;
            parameters[15].Value = model.withoutinterest;
            parameters[16].Value = model.invitationcode;
            parameters[17].Value = model.OrdId;
            parameters[18].Value = model.JiaxiNum;
            parameters[19].Value = model.BonusAmt;


            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(M_Bid_records model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hx_Bid_records set ");
            strSql.Append("borrower_registerid=@borrower_registerid,");
            strSql.Append("targetid=@targetid,");
            strSql.Append("loan_number=@loan_number,");
            strSql.Append("annual_interest_rate=@annual_interest_rate,");
            strSql.Append("current_period=@current_period,");
            strSql.Append("investment_amount=@investment_amount,");
            strSql.Append("value_date=@value_date,");
            strSql.Append("investment_maturity=@investment_maturity,");
            strSql.Append("invest_time=@invest_time,");
            strSql.Append("invest_state=@invest_state,");
            strSql.Append("flow_return=@flow_return,");
            strSql.Append("repayment_amount=@repayment_amount,");
            strSql.Append("repayment_period=@repayment_period,");
            strSql.Append("investor_registerid=@investor_registerid,");
            strSql.Append("payment_status=@payment_status");
            strSql.Append(" where bid_records_id=@bid_records_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
                    new SqlParameter("@targetid", SqlDbType.Int,4),
                    new SqlParameter("@loan_number", SqlDbType.Decimal,18),
                    new SqlParameter("@annual_interest_rate", SqlDbType.Decimal,17),
                    new SqlParameter("@current_period", SqlDbType.Int,4),
                    new SqlParameter("@investment_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@value_date", SqlDbType.DateTime),
                    new SqlParameter("@investment_maturity", SqlDbType.DateTime),
                    new SqlParameter("@invest_time", SqlDbType.DateTime),
                    new SqlParameter("@invest_state", SqlDbType.Int,4),
                    new SqlParameter("@flow_return", SqlDbType.Int,4),
                    new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@repayment_period", SqlDbType.DateTime),
                    new SqlParameter("@investor_registerid", SqlDbType.Int,4),
                    new SqlParameter("@payment_status", SqlDbType.Int,4),
                    new SqlParameter("@bid_records_id", SqlDbType.Int,4)};
            parameters[0].Value = model.borrower_registerid;
            parameters[1].Value = model.targetid;
            parameters[2].Value = model.loan_number;
            parameters[3].Value = model.annual_interest_rate;
            parameters[4].Value = model.current_period;
            parameters[5].Value = model.investment_amount;
            parameters[6].Value = model.value_date;
            parameters[7].Value = model.investment_maturity;
            parameters[8].Value = model.invest_time;
            parameters[9].Value = model.invest_state;
            parameters[10].Value = model.flow_return;
            parameters[11].Value = model.repayment_amount;
            parameters[12].Value = model.repayment_period;
            parameters[13].Value = model.investor_registerid;
            parameters[14].Value = model.payment_status;
            parameters[15].Value = model.bid_records_id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int bid_records_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_Bid_records ");
            strSql.Append(" where bid_records_id=@bid_records_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@bid_records_id", SqlDbType.Int,4)
            };
            parameters[0].Value = bid_records_id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string bid_records_idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_Bid_records ");
            strSql.Append(" where bid_records_id in (" + bid_records_idlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_Bid_records GetModel(int bid_records_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 bid_records_id,borrower_registerid,targetid,loan_number,annual_interest_rate,current_period,investment_amount,value_date,investment_maturity,invest_time,invest_state,flow_return,repayment_amount,repayment_period,investor_registerid,payment_status from hx_Bid_records ");
            strSql.Append(" where bid_records_id=@bid_records_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@bid_records_id", SqlDbType.Int,4)
            };
            parameters[0].Value = bid_records_id;

            M_Bid_records model = new M_Bid_records();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_Bid_records DataRowToModel(DataRow row)
        {
            M_Bid_records model = new M_Bid_records();
            if (row != null)
            {
                if (row["bid_records_id"] != null && row["bid_records_id"].ToString() != "")
                {
                    model.bid_records_id = int.Parse(row["bid_records_id"].ToString());
                }
                if (row["borrower_registerid"] != null && row["borrower_registerid"].ToString() != "")
                {
                    model.borrower_registerid = int.Parse(row["borrower_registerid"].ToString());
                }
                if (row["targetid"] != null && row["targetid"].ToString() != "")
                {
                    model.targetid = int.Parse(row["targetid"].ToString());
                }
                if (row["loan_number"] != null && row["loan_number"].ToString() != "")
                {
                    model.loan_number = decimal.Parse(row["loan_number"].ToString());
                }
                if (row["annual_interest_rate"] != null && row["annual_interest_rate"].ToString() != "")
                {
                    model.annual_interest_rate = decimal.Parse(row["annual_interest_rate"].ToString());
                }
                if (row["current_period"] != null && row["current_period"].ToString() != "")
                {
                    model.current_period = int.Parse(row["current_period"].ToString());
                }
                if (row["investment_amount"] != null && row["investment_amount"].ToString() != "")
                {
                    model.investment_amount = decimal.Parse(row["investment_amount"].ToString());
                }
                if (row["value_date"] != null && row["value_date"].ToString() != "")
                {
                    model.value_date = DateTime.Parse(row["value_date"].ToString());
                }
                if (row["investment_maturity"] != null && row["investment_maturity"].ToString() != "")
                {
                    model.investment_maturity = DateTime.Parse(row["investment_maturity"].ToString());
                }
                if (row["invest_time"] != null && row["invest_time"].ToString() != "")
                {
                    model.invest_time = DateTime.Parse(row["invest_time"].ToString());
                }
                if (row["invest_state"] != null && row["invest_state"].ToString() != "")
                {
                    model.invest_state = int.Parse(row["invest_state"].ToString());
                }
                if (row["flow_return"] != null && row["flow_return"].ToString() != "")
                {
                    model.flow_return = int.Parse(row["flow_return"].ToString());
                }
                if (row["repayment_amount"] != null && row["repayment_amount"].ToString() != "")
                {
                    model.repayment_amount = decimal.Parse(row["repayment_amount"].ToString());
                }
                if (row["repayment_period"] != null && row["repayment_period"].ToString() != "")
                {
                    model.repayment_period = DateTime.Parse(row["repayment_period"].ToString());
                }
                if (row["investor_registerid"] != null && row["investor_registerid"].ToString() != "")
                {
                    model.investor_registerid = int.Parse(row["investor_registerid"].ToString());
                }
                if (row["payment_status"] != null && row["payment_status"].ToString() != "")
                {
                    model.payment_status = int.Parse(row["payment_status"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT BID_RECORDS_ID,BORROWER_REGISTERID,TARGETID,LOAN_NUMBER,ANNUAL_INTEREST_RATE,CURRENT_PERIOD,INVESTMENT_AMOUNT,VALUE_DATE,INVESTMENT_MATURITY,INVEST_TIME,INVEST_STATE,FLOW_RETURN,REPAYMENT_AMOUNT,REPAYMENT_PERIOD,INVESTOR_REGISTERID,PAYMENT_STATUS,BONUSAMT ");
            strSql.Append(" FROM hx_Bid_records ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" bid_records_id,borrower_registerid,targetid,loan_number,annual_interest_rate,current_period,investment_amount,value_date,investment_maturity,invest_time,invest_state,flow_return,repayment_amount,repayment_period,investor_registerid,payment_status ");
            strSql.Append(" FROM hx_Bid_records ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM hx_Bid_records ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.bid_records_id desc");
            }
            strSql.Append(")AS Row, T.*  from hx_Bid_records T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public int GetInvestedCount(int userId, int targetId)
        {
            int result = 0;
            string sql = "select count(0) as investedCount from hx_Bid_records where investor_registerid=" + userId.ToString() + " and targetid=" + targetId.ToString() + " and ordstate=1";
            object obj = DbHelperSQL.GetSingle(sql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
            return result;
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "hx_Bid_records";
			parameters[1].Value = "bid_records_id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

