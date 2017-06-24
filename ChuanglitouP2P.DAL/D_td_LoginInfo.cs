
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_LoginInfo
	/// </summary>
	public partial class D_td_LoginInfo
	{
		public D_td_LoginInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "hx_td_LoginInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_LoginInfo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_LoginInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_LoginInfo(");
			strSql.Append("AdminUserName,Pwd,LoginTime,LoginIP,LoginSuccess)");
			strSql.Append(" values (");
			strSql.Append("@AdminUserName,@Pwd,@LoginTime,@LoginIP,@LoginSuccess)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@AdminUserName", SqlDbType.VarChar,50),
					new SqlParameter("@Pwd", SqlDbType.VarChar,50),
					new SqlParameter("@LoginTime", SqlDbType.DateTime),
					new SqlParameter("@LoginIP", SqlDbType.VarChar,20),
					new SqlParameter("@LoginSuccess", SqlDbType.Int,4)};
			parameters[0].Value = model.AdminUserName;
			parameters[1].Value = model.Pwd;
			parameters[2].Value = model.LoginTime;
			parameters[3].Value = model.LoginIP;
			parameters[4].Value = model.LoginSuccess;

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
		public bool Update(M_td_LoginInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_LoginInfo set ");
			strSql.Append("AdminUserName=@AdminUserName,");
			strSql.Append("Pwd=@Pwd,");
			strSql.Append("LoginTime=@LoginTime,");
			strSql.Append("LoginIP=@LoginIP,");
			strSql.Append("LoginSuccess=@LoginSuccess");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@AdminUserName", SqlDbType.VarChar,50),
					new SqlParameter("@Pwd", SqlDbType.VarChar,50),
					new SqlParameter("@LoginTime", SqlDbType.DateTime),
					new SqlParameter("@LoginIP", SqlDbType.VarChar,20),
					new SqlParameter("@LoginSuccess", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.AdminUserName;
			parameters[1].Value = model.Pwd;
			parameters[2].Value = model.LoginTime;
			parameters[3].Value = model.LoginIP;
			parameters[4].Value = model.LoginSuccess;
			parameters[5].Value = model.id;

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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_LoginInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_LoginInfo ");
			strSql.Append(" where id in ("+idlist + ")  ");
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
		public M_td_LoginInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,AdminUserName,Pwd,LoginTime,LoginIP,LoginSuccess from hx_td_LoginInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			M_td_LoginInfo model=new M_td_LoginInfo();
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
		public M_td_LoginInfo DataRowToModel(DataRow row)
		{
			M_td_LoginInfo model=new M_td_LoginInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["AdminUserName"]!=null)
				{
					model.AdminUserName=row["AdminUserName"].ToString();
				}
				if(row["Pwd"]!=null)
				{
					model.Pwd=row["Pwd"].ToString();
				}
				if(row["LoginTime"]!=null && row["LoginTime"].ToString()!="")
				{
					model.LoginTime=DateTime.Parse(row["LoginTime"].ToString());
				}
				if(row["LoginIP"]!=null)
				{
					model.LoginIP=row["LoginIP"].ToString();
				}
				if(row["LoginSuccess"]!=null && row["LoginSuccess"].ToString()!="")
				{
					model.LoginSuccess=int.Parse(row["LoginSuccess"].ToString());
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
			strSql.Append("select id,AdminUserName,Pwd,LoginTime,LoginIP,LoginSuccess ");
			strSql.Append(" FROM hx_td_LoginInfo ");
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
			strSql.Append(" id,AdminUserName,Pwd,LoginTime,LoginIP,LoginSuccess ");
			strSql.Append(" FROM hx_td_LoginInfo ");
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
			strSql.Append("select count(1) FROM hx_td_LoginInfo ");
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
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_LoginInfo T ");
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
			parameters[0].Value = "hx_td_LoginInfo";
			parameters[1].Value = "id";
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

