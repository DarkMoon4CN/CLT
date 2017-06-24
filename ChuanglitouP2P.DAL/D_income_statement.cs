using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System.Collections;
using System.Collections.Generic;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:income_statement
	/// </summary>
	public partial class D_income_statement
	{
		public D_income_statement()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("income_statement_id", "hx_income_statement"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int income_statement_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_income_statement");
			strSql.Append(" where income_statement_id=@income_statement_id");
			SqlParameter[] parameters = {
					new SqlParameter("@income_statement_id", SqlDbType.Int,4)
			};
			parameters[0].Value = income_statement_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_income_statement model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_income_statement(");
            strSql.Append("targetid,bid_records_id,loan_number,borrower_registerid,annual_revenue,investment_amount,invest_time,current_investment_period,value_date,interest_payment_date,repayment_amount,investor_registerid,payment_status,interestpayment,OutCustId,InCustId,BidOrderid,Principal,interestDay,TotalInstallments)");
			strSql.Append(" values (");
            strSql.Append("@targetid,@bid_records_id,@loan_number,@borrower_registerid,@annual_revenue,@investment_amount,@invest_time,@current_investment_period,@value_date,@interest_payment_date,@repayment_amount,@investor_registerid,@payment_status,@interestpayment,@OutCustId,@InCustId,@BidOrderid,@Principal,@interestDay,@TotalInstallments)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@bid_records_id", SqlDbType.Int,4),
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@annual_revenue", SqlDbType.Decimal,17),
					new SqlParameter("@investment_amount", SqlDbType.Decimal,17),
					new SqlParameter("@invest_time", SqlDbType.DateTime),
					new SqlParameter("@current_investment_period", SqlDbType.Int,4),
					new SqlParameter("@value_date", SqlDbType.DateTime),
					new SqlParameter("@interest_payment_date", SqlDbType.DateTime),
					new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
				
					new SqlParameter("@investor_registerid", SqlDbType.Int,4),
					new SqlParameter("@payment_status", SqlDbType.Int,4),
                    new SqlParameter("@interestpayment", SqlDbType.Decimal,17), 
                    new SqlParameter("@OutCustId", SqlDbType.VarChar,50),
                    new SqlParameter("@InCustId", SqlDbType.VarChar,50),
                    new SqlParameter("@BidOrderid", SqlDbType.Decimal,20),
                    new SqlParameter("@Principal", SqlDbType.Decimal,17),
                    new SqlParameter("@interestDay", SqlDbType.Int,4),
                    new SqlParameter("@TotalInstallments", SqlDbType.Int,4)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.bid_records_id;
			parameters[2].Value = model.loan_number;
			parameters[3].Value = model.borrower_registerid;
			parameters[4].Value = model.annual_revenue;
			parameters[5].Value = model.investment_amount;
			parameters[6].Value = model.invest_time;
			parameters[7].Value = model.current_investment_period;
			parameters[8].Value = model.value_date;
			parameters[9].Value = model.interest_payment_date;
			parameters[10].Value = model.repayment_amount;
		
			parameters[11].Value = model.investor_registerid;
			parameters[12].Value = model.payment_status;
            parameters[13].Value = model.interestpayment;
            parameters[14].Value = model.OutCustId;
            parameters[15].Value = model.InCustId;
            parameters[16].Value = model.BidOrderid;
            parameters[17].Value = model.Principal;
            parameters[18].Value = model.interestDay;
            parameters[19].Value = model.TotalInstallments;
            

            
            List<string> slist = new List<string>();
            object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
		
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
		public bool Update(M_income_statement model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_income_statement set ");
			strSql.Append("targetid=@targetid,");
			strSql.Append("bid_records_id=@bid_records_id,");
			strSql.Append("loan_number=@loan_number,");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("annual_revenue=@annual_revenue,");
			strSql.Append("investment_amount=@investment_amount,");
			strSql.Append("invest_time=@invest_time,");
			strSql.Append("current_investment_period=@current_investment_period,");
			strSql.Append("value_date=@value_date,");
			strSql.Append("interest_payment_date=@interest_payment_date,");
			strSql.Append("repayment_amount=@repayment_amount,");
			strSql.Append("repayment_period=@repayment_period,");
			strSql.Append("investor_registerid=@investor_registerid,");
			strSql.Append("payment_status=@payment_status");
			strSql.Append(" where income_statement_id=@income_statement_id");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@bid_records_id", SqlDbType.Int,4),
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@annual_revenue", SqlDbType.Decimal,17),
					new SqlParameter("@investment_amount", SqlDbType.Decimal,17),
					new SqlParameter("@invest_time", SqlDbType.DateTime),
					new SqlParameter("@current_investment_period", SqlDbType.Int,4),
					new SqlParameter("@value_date", SqlDbType.DateTime),
					new SqlParameter("@interest_payment_date", SqlDbType.DateTime),
					new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
					new SqlParameter("@repayment_period", SqlDbType.DateTime),
					new SqlParameter("@investor_registerid", SqlDbType.Int,4),
					new SqlParameter("@payment_status", SqlDbType.Int,4),
					new SqlParameter("@income_statement_id", SqlDbType.Int,4)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.bid_records_id;
			parameters[2].Value = model.loan_number;
			parameters[3].Value = model.borrower_registerid;
			parameters[4].Value = model.annual_revenue;
			parameters[5].Value = model.investment_amount;
			parameters[6].Value = model.invest_time;
			parameters[7].Value = model.current_investment_period;
			parameters[8].Value = model.value_date;
			parameters[9].Value = model.interest_payment_date;
			parameters[10].Value = model.repayment_amount;
			parameters[11].Value = model.repayment_period;
			parameters[12].Value = model.investor_registerid;
			parameters[13].Value = model.payment_status;
			parameters[14].Value = model.income_statement_id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int income_statement_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_income_statement ");
			strSql.Append(" where income_statement_id=@income_statement_id");
			SqlParameter[] parameters = {
					new SqlParameter("@income_statement_id", SqlDbType.Int,4)
			};
			parameters[0].Value = income_statement_id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string income_statement_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_income_statement ");
			strSql.Append(" where income_statement_id in ("+income_statement_idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public M_income_statement GetModel(int income_statement_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 income_statement_id,targetid,bid_records_id,loan_number,borrower_registerid,annual_revenue,investment_amount,invest_time,current_investment_period,value_date,interest_payment_date,repayment_amount,repayment_period,investor_registerid,payment_status from hx_income_statement ");
			strSql.Append(" where income_statement_id=@income_statement_id");
			SqlParameter[] parameters = {
					new SqlParameter("@income_statement_id", SqlDbType.Int,4)
			};
			parameters[0].Value = income_statement_id;

			M_income_statement model=new M_income_statement();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public M_income_statement DataRowToModel(DataRow row)
		{
			M_income_statement model=new M_income_statement();
			if (row != null)
			{
				if(row["income_statement_id"]!=null && row["income_statement_id"].ToString()!="")
				{
					model.income_statement_id=int.Parse(row["income_statement_id"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["bid_records_id"]!=null && row["bid_records_id"].ToString()!="")
				{
					model.bid_records_id=int.Parse(row["bid_records_id"].ToString());
				}
				if(row["loan_number"]!=null && row["loan_number"].ToString()!="")
				{
					model.loan_number=decimal.Parse(row["loan_number"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["annual_revenue"]!=null && row["annual_revenue"].ToString()!="")
				{
					model.annual_revenue=decimal.Parse(row["annual_revenue"].ToString());
				}
				if(row["investment_amount"]!=null && row["investment_amount"].ToString()!="")
				{
					model.investment_amount=decimal.Parse(row["investment_amount"].ToString());
				}
				if(row["invest_time"]!=null && row["invest_time"].ToString()!="")
				{
					model.invest_time=DateTime.Parse(row["invest_time"].ToString());
				}
				if(row["current_investment_period"]!=null && row["current_investment_period"].ToString()!="")
				{
					model.current_investment_period=int.Parse(row["current_investment_period"].ToString());
				}
				if(row["value_date"]!=null && row["value_date"].ToString()!="")
				{
					model.value_date=DateTime.Parse(row["value_date"].ToString());
				}
				if(row["interest_payment_date"]!=null && row["interest_payment_date"].ToString()!="")
				{
					model.interest_payment_date=DateTime.Parse(row["interest_payment_date"].ToString());
				}
				if(row["repayment_amount"]!=null && row["repayment_amount"].ToString()!="")
				{
					model.repayment_amount=decimal.Parse(row["repayment_amount"].ToString());
				}
				if(row["repayment_period"]!=null && row["repayment_period"].ToString()!="")
				{
					model.repayment_period=DateTime.Parse(row["repayment_period"].ToString());
				}
				if(row["investor_registerid"]!=null && row["investor_registerid"].ToString()!="")
				{
					model.investor_registerid=int.Parse(row["investor_registerid"].ToString());
				}
				if(row["payment_status"]!=null && row["payment_status"].ToString()!="")
				{
					model.payment_status=int.Parse(row["payment_status"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select income_statement_id,targetid,bid_records_id,loan_number,borrower_registerid,annual_revenue,investment_amount,invest_time,current_investment_period,value_date,interest_payment_date,repayment_amount,repayment_period,investor_registerid,payment_status ");
			strSql.Append(" FROM hx_income_statement ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" income_statement_id,targetid,bid_records_id,loan_number,borrower_registerid,annual_revenue,investment_amount,invest_time,current_investment_period,value_date,interest_payment_date,repayment_amount,repayment_period,investor_registerid,payment_status ");
			strSql.Append(" FROM hx_income_statement ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM hx_income_statement ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.income_statement_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_income_statement T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
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
			parameters[0].Value = "hx_income_statement";
			parameters[1].Value = "income_statement_id";
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

