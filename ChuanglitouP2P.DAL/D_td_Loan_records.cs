
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_Loan_records
	/// </summary>
	public partial class D_td_Loan_records
	{
		public D_td_Loan_records()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Loan_records_id", "hx_td_Loan_records"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Loan_records_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_Loan_records");
			strSql.Append(" where Loan_records_id=@Loan_records_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Loan_records_id", SqlDbType.Int,4)
			};
			parameters[0].Value = Loan_records_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_Loan_records model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_Loan_records(");
			strSql.Append("targetid,borrower_registerid,investor_registerid,LoanAMT,LoanOrdId,LoanDate,Free,DivDetails,SubOrdid,unFreezeOrdId,FreezeTrxId)");
			strSql.Append(" values (");
			strSql.Append("@targetid,@borrower_registerid,@investor_registerid,@LoanAMT,@LoanOrdId,@LoanDate,@Free,@DivDetails,@SubOrdid,@unFreezeOrdId,@FreezeTrxId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@investor_registerid", SqlDbType.Int,4),
					new SqlParameter("@LoanAMT", SqlDbType.Decimal,9),
					new SqlParameter("@LoanOrdId", SqlDbType.VarChar,50),
					new SqlParameter("@LoanDate", SqlDbType.DateTime),
					new SqlParameter("@Free", SqlDbType.Decimal,9),
					new SqlParameter("@DivDetails", SqlDbType.VarChar,-1),
					new SqlParameter("@SubOrdid", SqlDbType.VarChar,50),
					new SqlParameter("@unFreezeOrdId", SqlDbType.VarChar,50),
					new SqlParameter("@FreezeTrxId", SqlDbType.VarChar,50)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.borrower_registerid;
			parameters[2].Value = model.investor_registerid;
			parameters[3].Value = model.LoanAMT;
			parameters[4].Value = model.LoanOrdId;
			parameters[5].Value = model.LoanDate;
			parameters[6].Value = model.Free;
			parameters[7].Value = model.DivDetails;
			parameters[8].Value = model.SubOrdid;
			parameters[9].Value = model.unFreezeOrdId;
			parameters[10].Value = model.FreezeTrxId;

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
		public bool Update(M_td_Loan_records model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_Loan_records set ");
			strSql.Append("targetid=@targetid,");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("investor_registerid=@investor_registerid,");
			strSql.Append("LoanAMT=@LoanAMT,");
			strSql.Append("LoanOrdId=@LoanOrdId,");
			strSql.Append("LoanDate=@LoanDate,");
			strSql.Append("Free=@Free,");
			strSql.Append("DivDetails=@DivDetails,");
			strSql.Append("SubOrdid=@SubOrdid,");
			strSql.Append("unFreezeOrdId=@unFreezeOrdId,");
			strSql.Append("FreezeTrxId=@FreezeTrxId");
			strSql.Append(" where Loan_records_id=@Loan_records_id");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@investor_registerid", SqlDbType.Int,4),
					new SqlParameter("@LoanAMT", SqlDbType.Decimal,9),
					new SqlParameter("@LoanOrdId", SqlDbType.VarChar,50),
					new SqlParameter("@LoanDate", SqlDbType.DateTime),
					new SqlParameter("@Free", SqlDbType.Decimal,9),
					new SqlParameter("@DivDetails", SqlDbType.VarChar,-1),
					new SqlParameter("@SubOrdid", SqlDbType.VarChar,50),
					new SqlParameter("@unFreezeOrdId", SqlDbType.VarChar,50),
					new SqlParameter("@FreezeTrxId", SqlDbType.VarChar,50),
					new SqlParameter("@Loan_records_id", SqlDbType.Int,4)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.borrower_registerid;
			parameters[2].Value = model.investor_registerid;
			parameters[3].Value = model.LoanAMT;
			parameters[4].Value = model.LoanOrdId;
			parameters[5].Value = model.LoanDate;
			parameters[6].Value = model.Free;
			parameters[7].Value = model.DivDetails;
			parameters[8].Value = model.SubOrdid;
			parameters[9].Value = model.unFreezeOrdId;
			parameters[10].Value = model.FreezeTrxId;
			parameters[11].Value = model.Loan_records_id;

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
		public bool Delete(int Loan_records_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Loan_records ");
			strSql.Append(" where Loan_records_id=@Loan_records_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Loan_records_id", SqlDbType.Int,4)
			};
			parameters[0].Value = Loan_records_id;

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
		public bool DeleteList(string Loan_records_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Loan_records ");
			strSql.Append(" where Loan_records_id in ("+Loan_records_idlist + ")  ");
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
		public M_td_Loan_records GetModel(int Loan_records_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Loan_records_id,targetid,borrower_registerid,investor_registerid,LoanAMT,LoanOrdId,LoanDate,Free,DivDetails,SubOrdid,unFreezeOrdId,FreezeTrxId from hx_td_Loan_records ");
			strSql.Append(" where Loan_records_id=@Loan_records_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Loan_records_id", SqlDbType.Int,4)
			};
			parameters[0].Value = Loan_records_id;

			M_td_Loan_records model=new M_td_Loan_records();
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
		public M_td_Loan_records DataRowToModel(DataRow row)
		{
			M_td_Loan_records model=new M_td_Loan_records();
			if (row != null)
			{
				if(row["Loan_records_id"]!=null && row["Loan_records_id"].ToString()!="")
				{
					model.Loan_records_id=int.Parse(row["Loan_records_id"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["investor_registerid"]!=null && row["investor_registerid"].ToString()!="")
				{
					model.investor_registerid=int.Parse(row["investor_registerid"].ToString());
				}
				if(row["LoanAMT"]!=null && row["LoanAMT"].ToString()!="")
				{
					model.LoanAMT=decimal.Parse(row["LoanAMT"].ToString());
				}
				if(row["LoanOrdId"]!=null)
				{
					model.LoanOrdId=row["LoanOrdId"].ToString();
				}
				if(row["LoanDate"]!=null && row["LoanDate"].ToString()!="")
				{
					model.LoanDate=DateTime.Parse(row["LoanDate"].ToString());
				}
				if(row["Free"]!=null && row["Free"].ToString()!="")
				{
					model.Free=decimal.Parse(row["Free"].ToString());
				}
				if(row["DivDetails"]!=null)
				{
					model.DivDetails=row["DivDetails"].ToString();
				}
				if(row["SubOrdid"]!=null)
				{
					model.SubOrdid=row["SubOrdid"].ToString();
				}
				if(row["unFreezeOrdId"]!=null)
				{
					model.unFreezeOrdId=row["unFreezeOrdId"].ToString();
				}
				if(row["FreezeTrxId"]!=null)
				{
					model.FreezeTrxId=row["FreezeTrxId"].ToString();
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
			strSql.Append("select Loan_records_id,targetid,borrower_registerid,investor_registerid,LoanAMT,LoanOrdId,LoanDate,Free,DivDetails,SubOrdid,unFreezeOrdId,FreezeTrxId ");
			strSql.Append(" FROM hx_td_Loan_records ");
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
			strSql.Append(" Loan_records_id,targetid,borrower_registerid,investor_registerid,LoanAMT,LoanOrdId,LoanDate,Free,DivDetails,SubOrdid,unFreezeOrdId,FreezeTrxId ");
			strSql.Append(" FROM hx_td_Loan_records ");
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
			strSql.Append("select count(1) FROM hx_td_Loan_records ");
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
				strSql.Append("order by T.Loan_records_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_Loan_records T ");
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
			parameters[0].Value = "hx_td_Loan_records";
			parameters[1].Value = "Loan_records_id";
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

