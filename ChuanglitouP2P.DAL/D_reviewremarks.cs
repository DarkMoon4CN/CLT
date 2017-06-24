
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_reviewremarks
	/// </summary>
	public partial class D_reviewremarks
	{
		public D_reviewremarks()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("reviewid", "hx_td_reviewremarks"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int reviewid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_reviewremarks");
			strSql.Append(" where reviewid=@reviewid");
			SqlParameter[] parameters = {
					new SqlParameter("@reviewid", SqlDbType.Int,4)
			};
			parameters[0].Value = reviewid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_reviewremarks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_reviewremarks(");
			strSql.Append("targetid,tender_state,reviewremarks,reviewtime,admin_operator)");
			strSql.Append(" values (");
			strSql.Append("@targetid,@tender_state,@reviewremarks,@reviewtime,@admin_operator)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@tender_state", SqlDbType.Int,4),
					new SqlParameter("@reviewremarks", SqlDbType.VarChar,1000),
					new SqlParameter("@reviewtime", SqlDbType.DateTime),
					new SqlParameter("@admin_operator", SqlDbType.Int,4)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.tender_state;
			parameters[2].Value = model.reviewremarks;
			parameters[3].Value = model.reviewtime;
			parameters[4].Value = model.admin_operator;

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
		public bool Update(M_reviewremarks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_reviewremarks set ");
			strSql.Append("targetid=@targetid,");
			strSql.Append("tender_state=@tender_state,");
			strSql.Append("reviewremarks=@reviewremarks,");
			strSql.Append("reviewtime=@reviewtime,");
			strSql.Append("admin_operator=@admin_operator");
			strSql.Append(" where reviewid=@reviewid");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@tender_state", SqlDbType.Int,4),
					new SqlParameter("@reviewremarks", SqlDbType.VarChar,1000),
					new SqlParameter("@reviewtime", SqlDbType.DateTime),
					new SqlParameter("@admin_operator", SqlDbType.Int,4),
					new SqlParameter("@reviewid", SqlDbType.Int,4)};
			parameters[0].Value = model.targetid;
			parameters[1].Value = model.tender_state;
			parameters[2].Value = model.reviewremarks;
			parameters[3].Value = model.reviewtime;
			parameters[4].Value = model.admin_operator;
			parameters[5].Value = model.reviewid;

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
		public bool Delete(int reviewid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_reviewremarks ");
			strSql.Append(" where reviewid=@reviewid");
			SqlParameter[] parameters = {
					new SqlParameter("@reviewid", SqlDbType.Int,4)
			};
			parameters[0].Value = reviewid;

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
		public bool DeleteList(string reviewidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_reviewremarks ");
			strSql.Append(" where reviewid in ("+reviewidlist + ")  ");
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
		public M_reviewremarks GetModel(int reviewid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 reviewid,targetid,tender_state,reviewremarks,reviewtime,admin_operator from hx_td_reviewremarks ");
			strSql.Append(" where reviewid=@reviewid");
			SqlParameter[] parameters = {
					new SqlParameter("@reviewid", SqlDbType.Int,4)
			};
			parameters[0].Value = reviewid;

			M_reviewremarks model=new M_reviewremarks();
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
		public M_reviewremarks DataRowToModel(DataRow row)
		{
			M_reviewremarks model=new M_reviewremarks();
			if (row != null)
			{
				if(row["reviewid"]!=null && row["reviewid"].ToString()!="")
				{
					model.reviewid=int.Parse(row["reviewid"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["tender_state"]!=null && row["tender_state"].ToString()!="")
				{
					model.tender_state=int.Parse(row["tender_state"].ToString());
				}
				if(row["reviewremarks"]!=null)
				{
					model.reviewremarks=row["reviewremarks"].ToString();
				}
				if(row["reviewtime"]!=null && row["reviewtime"].ToString()!="")
				{
					model.reviewtime=DateTime.Parse(row["reviewtime"].ToString());
				}
				if(row["admin_operator"]!=null && row["admin_operator"].ToString()!="")
				{
					model.admin_operator=int.Parse(row["admin_operator"].ToString());
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
			strSql.Append("select reviewid,targetid,tender_state,reviewremarks,reviewtime,admin_operator ");
			strSql.Append(" FROM hx_td_reviewremarks ");
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
			strSql.Append(" reviewid,targetid,tender_state,reviewremarks,reviewtime,admin_operator ");
			strSql.Append(" FROM hx_td_reviewremarks ");
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
			strSql.Append("select count(1) FROM hx_td_reviewremarks ");
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
				strSql.Append("order by T.reviewid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_reviewremarks T ");
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
			parameters[0].Value = "hx_td_reviewremarks";
			parameters[1].Value = "reviewid";
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

