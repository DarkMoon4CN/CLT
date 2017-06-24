
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_Phone_records
	/// </summary>
	public partial class D_td_Phone_records
	{
		public D_td_Phone_records()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("recordsid", "hx_td_Phone_records"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int recordsid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_Phone_records");
			strSql.Append(" where recordsid=@recordsid");
			SqlParameter[] parameters = {
					new SqlParameter("@recordsid", SqlDbType.Int,4)
			};
			parameters[0].Value = recordsid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_Phone_records model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_Phone_records(");
			strSql.Append("recordcontext,recordtime,registerid,adminid)");
			strSql.Append(" values (");
			strSql.Append("@recordcontext,@recordtime,@registerid,@adminid)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@recordcontext", SqlDbType.VarChar,-1),
					new SqlParameter("@recordtime", SqlDbType.DateTime),
					new SqlParameter("@registerid", SqlDbType.Int,4),
					new SqlParameter("@adminid", SqlDbType.Int,4)};
			parameters[0].Value = model.recordcontext;
			parameters[1].Value = model.recordtime;
			parameters[2].Value = model.registerid;
			parameters[3].Value = model.adminid;

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
		public bool Update(M_td_Phone_records model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_Phone_records set ");
			strSql.Append("recordcontext=@recordcontext,");
			strSql.Append("recordtime=@recordtime,");
			strSql.Append("registerid=@registerid,");
			strSql.Append("adminid=@adminid");
			strSql.Append(" where recordsid=@recordsid");
			SqlParameter[] parameters = {
					new SqlParameter("@recordcontext", SqlDbType.VarChar,-1),
					new SqlParameter("@recordtime", SqlDbType.DateTime),
					new SqlParameter("@registerid", SqlDbType.Int,4),
					new SqlParameter("@adminid", SqlDbType.Int,4),
					new SqlParameter("@recordsid", SqlDbType.Int,4)};
			parameters[0].Value = model.recordcontext;
			parameters[1].Value = model.recordtime;
			parameters[2].Value = model.registerid;
			parameters[3].Value = model.adminid;
			parameters[4].Value = model.recordsid;

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
		public bool Delete(int recordsid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Phone_records ");
			strSql.Append(" where recordsid=@recordsid");
			SqlParameter[] parameters = {
					new SqlParameter("@recordsid", SqlDbType.Int,4)
			};
			parameters[0].Value = recordsid;

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
		public bool DeleteList(string recordsidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Phone_records ");
			strSql.Append(" where recordsid in ("+recordsidlist + ")  ");
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
		public M_td_Phone_records GetModel(int recordsid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 recordsid,recordcontext,recordtime,registerid,adminid from hx_td_Phone_records ");
			strSql.Append(" where recordsid=@recordsid");
			SqlParameter[] parameters = {
					new SqlParameter("@recordsid", SqlDbType.Int,4)
			};
			parameters[0].Value = recordsid;

			M_td_Phone_records model=new M_td_Phone_records();
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
		public M_td_Phone_records DataRowToModel(DataRow row)
		{
			M_td_Phone_records model=new M_td_Phone_records();
			if (row != null)
			{
				if(row["recordsid"]!=null && row["recordsid"].ToString()!="")
				{
					model.recordsid=int.Parse(row["recordsid"].ToString());
				}
				if(row["recordcontext"]!=null)
				{
					model.recordcontext=row["recordcontext"].ToString();
				}
				if(row["recordtime"]!=null && row["recordtime"].ToString()!="")
				{
					model.recordtime=DateTime.Parse(row["recordtime"].ToString());
				}
				if(row["registerid"]!=null && row["registerid"].ToString()!="")
				{
					model.registerid=int.Parse(row["registerid"].ToString());
				}
				if(row["adminid"]!=null && row["adminid"].ToString()!="")
				{
					model.adminid=int.Parse(row["adminid"].ToString());
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
			strSql.Append("select recordsid,recordcontext,recordtime,registerid,adminid ");
			strSql.Append(" FROM hx_td_Phone_records ");
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
			strSql.Append(" recordsid,recordcontext,recordtime,registerid,adminid ");
			strSql.Append(" FROM hx_td_Phone_records ");
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
			strSql.Append("select count(1) FROM hx_td_Phone_records ");
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
				strSql.Append("order by T.recordsid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_Phone_records T ");
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
			parameters[0].Value = "hx_td_Phone_records";
			parameters[1].Value = "recordsid";
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

