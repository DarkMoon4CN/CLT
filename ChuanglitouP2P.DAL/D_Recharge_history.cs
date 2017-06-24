using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Recharge_history
	/// </summary>
	public partial class D_Recharge_history
	{
		public D_Recharge_history()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("recharge_history_id", "hx_Recharge_history"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int recharge_history_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_Recharge_history");
			strSql.Append(" where recharge_history_id=@recharge_history_id");
			SqlParameter[] parameters = {
					new SqlParameter("@recharge_history_id", SqlDbType.Int,4)
			};
			parameters[0].Value = recharge_history_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Recharge_history model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_Recharge_history(");
			strSql.Append("membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank)");
			strSql.Append(" values (");
			strSql.Append("@membertable_registerid,@recharge_amount,@recharge_time,@account_amount,@order_No,@recharge_condition,@recharge_bank)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@recharge_amount", SqlDbType.Decimal,17),
					new SqlParameter("@recharge_time", SqlDbType.DateTime),
					new SqlParameter("@account_amount", SqlDbType.Decimal,17),
					new SqlParameter("@order_No", SqlDbType.VarChar,30),
					new SqlParameter("@recharge_condition", SqlDbType.Int,4),
					new SqlParameter("@recharge_bank", SqlDbType.VarChar,100)};
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.recharge_amount;
			parameters[2].Value = model.recharge_time;
			parameters[3].Value = model.account_amount;
			parameters[4].Value = model.order_No;
			parameters[5].Value = model.recharge_condition;
			parameters[6].Value = model.recharge_bank;

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
		public bool Update(M_Recharge_history model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_Recharge_history set ");
			strSql.Append("membertable_registerid=@membertable_registerid,");
			strSql.Append("recharge_amount=@recharge_amount,");
			strSql.Append("recharge_time=@recharge_time,");
			strSql.Append("account_amount=@account_amount,");
			strSql.Append("order_No=@order_No,");
			strSql.Append("recharge_condition=@recharge_condition,");
			strSql.Append("recharge_bank=@recharge_bank");
			strSql.Append(" where recharge_history_id=@recharge_history_id");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@recharge_amount", SqlDbType.Decimal,17),
					new SqlParameter("@recharge_time", SqlDbType.DateTime),
					new SqlParameter("@account_amount", SqlDbType.Decimal,17),
					new SqlParameter("@order_No", SqlDbType.VarChar,30),
					new SqlParameter("@recharge_condition", SqlDbType.Int,4),
					new SqlParameter("@recharge_bank", SqlDbType.VarChar,100),
					new SqlParameter("@recharge_history_id", SqlDbType.Int,4)};
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.recharge_amount;
			parameters[2].Value = model.recharge_time;
			parameters[3].Value = model.account_amount;
			parameters[4].Value = model.order_No;
			parameters[5].Value = model.recharge_condition;
			parameters[6].Value = model.recharge_bank;
			parameters[7].Value = model.recharge_history_id;

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
		public bool Delete(int recharge_history_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Recharge_history ");
			strSql.Append(" where recharge_history_id=@recharge_history_id");
			SqlParameter[] parameters = {
					new SqlParameter("@recharge_history_id", SqlDbType.Int,4)
			};
			parameters[0].Value = recharge_history_id;

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
		public bool DeleteList(string recharge_history_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Recharge_history ");
			strSql.Append(" where recharge_history_id in ("+recharge_history_idlist + ")  ");
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
		public M_Recharge_history GetModel(int recharge_history_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 recharge_history_id,membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank from hx_Recharge_history ");
			strSql.Append(" where recharge_history_id=@recharge_history_id");
			SqlParameter[] parameters = {
					new SqlParameter("@recharge_history_id", SqlDbType.Int,4)
			};
			parameters[0].Value = recharge_history_id;

			M_Recharge_history model=new M_Recharge_history();
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
		public M_Recharge_history DataRowToModel(DataRow row)
		{
			M_Recharge_history model=new M_Recharge_history();
			if (row != null)
			{
				if(row["recharge_history_id"]!=null && row["recharge_history_id"].ToString()!="")
				{
					model.recharge_history_id=int.Parse(row["recharge_history_id"].ToString());
				}
				if(row["membertable_registerid"]!=null && row["membertable_registerid"].ToString()!="")
				{
					model.membertable_registerid=int.Parse(row["membertable_registerid"].ToString());
				}
				if(row["recharge_amount"]!=null && row["recharge_amount"].ToString()!="")
				{
					model.recharge_amount=decimal.Parse(row["recharge_amount"].ToString());
				}
				if(row["recharge_time"]!=null && row["recharge_time"].ToString()!="")
				{
					model.recharge_time=DateTime.Parse(row["recharge_time"].ToString());
				}
				if(row["account_amount"]!=null && row["account_amount"].ToString()!="")
				{
					model.account_amount=decimal.Parse(row["account_amount"].ToString());
				}
				if(row["order_No"]!=null)
				{
					model.order_No=row["order_No"].ToString();
				}
				if(row["recharge_condition"]!=null && row["recharge_condition"].ToString()!="")
				{
					model.recharge_condition=int.Parse(row["recharge_condition"].ToString());
				}
				if(row["recharge_bank"]!=null)
				{
					model.recharge_bank=row["recharge_bank"].ToString();
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
			strSql.Append("select recharge_history_id,membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank ");
			strSql.Append(" FROM hx_Recharge_history ");
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
			strSql.Append(" recharge_history_id,membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank ");
			strSql.Append(" FROM hx_Recharge_history ");
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
			strSql.Append("select count(1) FROM hx_Recharge_history ");
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
				strSql.Append("order by T.recharge_history_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_Recharge_history T ");
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
			parameters[0].Value = "hx_Recharge_history";
			parameters[1].Value = "recharge_history_id";
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

