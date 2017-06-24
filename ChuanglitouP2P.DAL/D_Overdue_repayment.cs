using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Overdue_repayment
	/// </summary>
	public partial class D_Overdue_repayment
	{
		public D_Overdue_repayment()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("overdue_repayment_id", "Overdue_repayment"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int overdue_repayment_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Overdue_repayment");
			strSql.Append(" where overdue_repayment_id=@overdue_repayment_id");
			SqlParameter[] parameters = {
					new SqlParameter("@overdue_repayment_id", SqlDbType.Int,4)
			};
			parameters[0].Value = overdue_repayment_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Overdue_repayment model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Overdue_repayment(");
			strSql.Append("repayment_plan_id,membertable_registerid,targetid,loan_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,actual_repayment_time,overdue_days,interest_state)");
			strSql.Append(" values (");
			strSql.Append("@repayment_plan_id,@membertable_registerid,@targetid,@loan_management_fee,@ordinary_overdue_management_fees,@seriously_overdue_management_fees,@ordinary_overdue_penalty,@seriously_overdue_penalty,@actual_repayment_time,@overdue_days,@interest_state)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4),
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@loan_management_fee", SqlDbType.Decimal,17),
					new SqlParameter("@ordinary_overdue_management_fees", SqlDbType.Decimal,17),
					new SqlParameter("@seriously_overdue_management_fees", SqlDbType.Decimal,17),
					new SqlParameter("@ordinary_overdue_penalty", SqlDbType.Decimal,17),
					new SqlParameter("@seriously_overdue_penalty", SqlDbType.Decimal,17),
					new SqlParameter("@actual_repayment_time", SqlDbType.DateTime),
					new SqlParameter("@overdue_days", SqlDbType.Int,4),
					new SqlParameter("@interest_state", SqlDbType.Int,4)};
			parameters[0].Value = model.repayment_plan_id;
			parameters[1].Value = model.membertable_registerid;
			parameters[2].Value = model.targetid;
			parameters[3].Value = model.loan_management_fee;
			parameters[4].Value = model.ordinary_overdue_management_fees;
			parameters[5].Value = model.seriously_overdue_management_fees;
			parameters[6].Value = model.ordinary_overdue_penalty;
			parameters[7].Value = model.seriously_overdue_penalty;
			parameters[8].Value = model.actual_repayment_time;
			parameters[9].Value = model.overdue_days;
			parameters[10].Value = model.interest_state;

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
		public bool Update(M_Overdue_repayment model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Overdue_repayment set ");
			strSql.Append("repayment_plan_id=@repayment_plan_id,");
			strSql.Append("membertable_registerid=@membertable_registerid,");
			strSql.Append("targetid=@targetid,");
			strSql.Append("loan_management_fee=@loan_management_fee,");
			strSql.Append("ordinary_overdue_management_fees=@ordinary_overdue_management_fees,");
			strSql.Append("seriously_overdue_management_fees=@seriously_overdue_management_fees,");
			strSql.Append("ordinary_overdue_penalty=@ordinary_overdue_penalty,");
			strSql.Append("seriously_overdue_penalty=@seriously_overdue_penalty,");
			strSql.Append("actual_repayment_time=@actual_repayment_time,");
			strSql.Append("overdue_days=@overdue_days,");
			strSql.Append("interest_state=@interest_state");
			strSql.Append(" where overdue_repayment_id=@overdue_repayment_id");
			SqlParameter[] parameters = {
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4),
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@loan_management_fee", SqlDbType.Decimal,17),
					new SqlParameter("@ordinary_overdue_management_fees", SqlDbType.Decimal,17),
					new SqlParameter("@seriously_overdue_management_fees", SqlDbType.Decimal,17),
					new SqlParameter("@ordinary_overdue_penalty", SqlDbType.Decimal,17),
					new SqlParameter("@seriously_overdue_penalty", SqlDbType.Decimal,17),
					new SqlParameter("@actual_repayment_time", SqlDbType.DateTime),
					new SqlParameter("@overdue_days", SqlDbType.Int,4),
					new SqlParameter("@interest_state", SqlDbType.Int,4),
					new SqlParameter("@overdue_repayment_id", SqlDbType.Int,4)};
			parameters[0].Value = model.repayment_plan_id;
			parameters[1].Value = model.membertable_registerid;
			parameters[2].Value = model.targetid;
			parameters[3].Value = model.loan_management_fee;
			parameters[4].Value = model.ordinary_overdue_management_fees;
			parameters[5].Value = model.seriously_overdue_management_fees;
			parameters[6].Value = model.ordinary_overdue_penalty;
			parameters[7].Value = model.seriously_overdue_penalty;
			parameters[8].Value = model.actual_repayment_time;
			parameters[9].Value = model.overdue_days;
			parameters[10].Value = model.interest_state;
			parameters[11].Value = model.overdue_repayment_id;

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
		public bool Delete(int overdue_repayment_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Overdue_repayment ");
			strSql.Append(" where overdue_repayment_id=@overdue_repayment_id");
			SqlParameter[] parameters = {
					new SqlParameter("@overdue_repayment_id", SqlDbType.Int,4)
			};
			parameters[0].Value = overdue_repayment_id;

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
		public bool DeleteList(string overdue_repayment_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Overdue_repayment ");
			strSql.Append(" where overdue_repayment_id in ("+overdue_repayment_idlist + ")  ");
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
		public M_Overdue_repayment GetModel(int overdue_repayment_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 overdue_repayment_id,repayment_plan_id,membertable_registerid,targetid,loan_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,actual_repayment_time,overdue_days,interest_state from Overdue_repayment ");
			strSql.Append(" where overdue_repayment_id=@overdue_repayment_id");
			SqlParameter[] parameters = {
					new SqlParameter("@overdue_repayment_id", SqlDbType.Int,4)
			};
			parameters[0].Value = overdue_repayment_id;

			M_Overdue_repayment model=new M_Overdue_repayment();
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
		public M_Overdue_repayment DataRowToModel(DataRow row)
		{
			M_Overdue_repayment model=new M_Overdue_repayment();
			if (row != null)
			{
				if(row["overdue_repayment_id"]!=null && row["overdue_repayment_id"].ToString()!="")
				{
					model.overdue_repayment_id=int.Parse(row["overdue_repayment_id"].ToString());
				}
				if(row["repayment_plan_id"]!=null && row["repayment_plan_id"].ToString()!="")
				{
					model.repayment_plan_id=int.Parse(row["repayment_plan_id"].ToString());
				}
				if(row["membertable_registerid"]!=null && row["membertable_registerid"].ToString()!="")
				{
					model.membertable_registerid=int.Parse(row["membertable_registerid"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["loan_management_fee"]!=null && row["loan_management_fee"].ToString()!="")
				{
					model.loan_management_fee=decimal.Parse(row["loan_management_fee"].ToString());
				}
				if(row["ordinary_overdue_management_fees"]!=null && row["ordinary_overdue_management_fees"].ToString()!="")
				{
					model.ordinary_overdue_management_fees=decimal.Parse(row["ordinary_overdue_management_fees"].ToString());
				}
				if(row["seriously_overdue_management_fees"]!=null && row["seriously_overdue_management_fees"].ToString()!="")
				{
					model.seriously_overdue_management_fees=decimal.Parse(row["seriously_overdue_management_fees"].ToString());
				}
				if(row["ordinary_overdue_penalty"]!=null && row["ordinary_overdue_penalty"].ToString()!="")
				{
					model.ordinary_overdue_penalty=decimal.Parse(row["ordinary_overdue_penalty"].ToString());
				}
				if(row["seriously_overdue_penalty"]!=null && row["seriously_overdue_penalty"].ToString()!="")
				{
					model.seriously_overdue_penalty=decimal.Parse(row["seriously_overdue_penalty"].ToString());
				}
				if(row["actual_repayment_time"]!=null && row["actual_repayment_time"].ToString()!="")
				{
					model.actual_repayment_time=DateTime.Parse(row["actual_repayment_time"].ToString());
				}
				if(row["overdue_days"]!=null && row["overdue_days"].ToString()!="")
				{
					model.overdue_days=int.Parse(row["overdue_days"].ToString());
				}
				if(row["interest_state"]!=null && row["interest_state"].ToString()!="")
				{
					model.interest_state=int.Parse(row["interest_state"].ToString());
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
			strSql.Append("select overdue_repayment_id,repayment_plan_id,membertable_registerid,targetid,loan_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,actual_repayment_time,overdue_days,interest_state ");
			strSql.Append(" FROM Overdue_repayment ");
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
			strSql.Append(" overdue_repayment_id,repayment_plan_id,membertable_registerid,targetid,loan_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,actual_repayment_time,overdue_days,interest_state ");
			strSql.Append(" FROM Overdue_repayment ");
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
			strSql.Append("select count(1) FROM Overdue_repayment ");
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
				strSql.Append("order by T.overdue_repayment_id desc");
			}
			strSql.Append(")AS Row, T.*  from Overdue_repayment T ");
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
			parameters[0].Value = "Overdue_repayment";
			parameters[1].Value = "overdue_repayment_id";
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

