using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
   
	/// <summary>
	/// 数据访问类:Activity_schedule
	/// </summary>
	public partial class D_Activity_schedule
	{
		public D_Activity_schedule()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("activity_schedule_id", "hx_Activity_schedule"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int activity_schedule_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_Activity_schedule");
			strSql.Append(" where activity_schedule_id=@activity_schedule_id");
			SqlParameter[] parameters = {
					new SqlParameter("@activity_schedule_id", SqlDbType.Int,4)
			};
			parameters[0].Value = activity_schedule_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

       
       

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Activity_schedule model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_Activity_schedule(");
			strSql.Append("activity_schedule_name,amount_of_reward,use_lower_limit,reward,start_date,end_date,entry_time)");
			strSql.Append(" values (");
			strSql.Append("@activity_schedule_name,@amount_of_reward,@use_lower_limit,@reward,@start_date,@end_date,@entry_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@activity_schedule_name", SqlDbType.VarChar,50),
					new SqlParameter("@amount_of_reward", SqlDbType.Decimal,17),
					new SqlParameter("@use_lower_limit", SqlDbType.Decimal,17),
					new SqlParameter("@reward", SqlDbType.Int,4),
					new SqlParameter("@start_date", SqlDbType.DateTime),
					new SqlParameter("@end_date", SqlDbType.DateTime),
					new SqlParameter("@entry_time", SqlDbType.DateTime)};
			parameters[0].Value = model.activity_schedule_name;
			parameters[1].Value = model.amount_of_reward;
			parameters[2].Value = model.use_lower_limit;
			parameters[3].Value = model.reward;
			parameters[4].Value = model.start_date;
			parameters[5].Value = model.end_date;
			parameters[6].Value = model.entry_time;

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
		public bool Update(M_Activity_schedule model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_Activity_schedule set ");
			strSql.Append("activity_schedule_name=@activity_schedule_name,");
			strSql.Append("amount_of_reward=@amount_of_reward,");
			strSql.Append("use_lower_limit=@use_lower_limit,");
			strSql.Append("reward=@reward,");
			strSql.Append("start_date=@start_date,");
			strSql.Append("end_date=@end_date");
			strSql.Append(" where activity_schedule_id=@activity_schedule_id");
			SqlParameter[] parameters = {
					new SqlParameter("@activity_schedule_name", SqlDbType.VarChar,50),
					new SqlParameter("@amount_of_reward", SqlDbType.Decimal,17),
					new SqlParameter("@use_lower_limit", SqlDbType.Decimal,17),
					new SqlParameter("@reward", SqlDbType.Int,4),
					new SqlParameter("@start_date", SqlDbType.DateTime),
					new SqlParameter("@end_date", SqlDbType.DateTime),					
					new SqlParameter("@activity_schedule_id", SqlDbType.Int,4)};
			parameters[0].Value = model.activity_schedule_name;
			parameters[1].Value = model.amount_of_reward;
			parameters[2].Value = model.use_lower_limit;
			parameters[3].Value = model.reward;
			parameters[4].Value = model.start_date;
			parameters[5].Value = model.end_date;
			parameters[6].Value = model.activity_schedule_id;
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
		public bool Delete(int activity_schedule_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Activity_schedule ");
			strSql.Append(" where activity_schedule_id=@activity_schedule_id");
			SqlParameter[] parameters = {
					new SqlParameter("@activity_schedule_id", SqlDbType.Int,4)
			};
			parameters[0].Value = activity_schedule_id;

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
		public bool DeleteList(string activity_schedule_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Activity_schedule ");
			strSql.Append(" where activity_schedule_id in ("+activity_schedule_idlist + ")  ");
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
		public M_Activity_schedule GetModel(int activity_schedule_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 activity_schedule_id,activity_schedule_name,amount_of_reward,use_lower_limit,reward,start_date,end_date,entry_time from hx_Activity_schedule ");
			strSql.Append(" where activity_schedule_id=@activity_schedule_id");
			SqlParameter[] parameters = {
					new SqlParameter("@activity_schedule_id", SqlDbType.Int,4)
			};
			parameters[0].Value = activity_schedule_id;

			M_Activity_schedule model=new M_Activity_schedule();
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
		public M_Activity_schedule DataRowToModel(DataRow row)
		{
			M_Activity_schedule model=new M_Activity_schedule();
			if (row != null)
			{
				if(row["activity_schedule_id"]!=null && row["activity_schedule_id"].ToString()!="")
				{
					model.activity_schedule_id=int.Parse(row["activity_schedule_id"].ToString());
				}
				if(row["activity_schedule_name"]!=null)
				{
					model.activity_schedule_name=row["activity_schedule_name"].ToString();
				}
				if(row["amount_of_reward"]!=null && row["amount_of_reward"].ToString()!="")
				{
					model.amount_of_reward=decimal.Parse(row["amount_of_reward"].ToString());
				}
				if(row["use_lower_limit"]!=null && row["use_lower_limit"].ToString()!="")
				{
					model.use_lower_limit=decimal.Parse(row["use_lower_limit"].ToString());
				}
				if(row["reward"]!=null && row["reward"].ToString()!="")
				{
					model.reward=int.Parse(row["reward"].ToString());
				}
				if(row["start_date"]!=null && row["start_date"].ToString()!="")
				{
					model.start_date=DateTime.Parse(row["start_date"].ToString());
				}
				if(row["end_date"]!=null && row["end_date"].ToString()!="")
				{
					model.end_date=DateTime.Parse(row["end_date"].ToString());
				}
				if(row["entry_time"]!=null && row["entry_time"].ToString()!="")
				{
					model.entry_time=DateTime.Parse(row["entry_time"].ToString());
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
			strSql.Append("select activity_schedule_id,activity_schedule_name,amount_of_reward,use_lower_limit,reward,start_date,end_date,entry_time ");
			strSql.Append(" FROM hx_Activity_schedule ");
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
			strSql.Append(" activity_schedule_id,activity_schedule_name,amount_of_reward,use_lower_limit,reward,start_date,end_date,entry_time ");
			strSql.Append(" FROM hx_Activity_schedule ");
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
			strSql.Append("select count(1) FROM hx_Activity_schedule ");
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
				strSql.Append("order by T.activity_schedule_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_Activity_schedule T ");
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
			parameters[0].Value = "hx_Activity_schedule";
			parameters[1].Value = "activity_schedule_id";
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

