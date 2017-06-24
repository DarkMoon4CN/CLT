using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Project_type
	/// </summary>
	public partial class D_Project_type
	{
		public D_Project_type()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("project_type_id", "hx_Project_type"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int project_type_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_Project_type");
			strSql.Append(" where project_type_id=@project_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@project_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = project_type_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Project_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_Project_type(");
			strSql.Append("project_type_name)");
			strSql.Append(" values (");
			strSql.Append("@project_type_name)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@project_type_name", SqlDbType.VarChar,200)};
			parameters[0].Value = model.project_type_name;

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
		public bool Update(M_Project_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_Project_type set ");
			strSql.Append("project_type_name=@project_type_name");
			strSql.Append(" where project_type_id=@project_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@project_type_name", SqlDbType.VarChar,200),
					new SqlParameter("@project_type_id", SqlDbType.Int,4)};
			parameters[0].Value = model.project_type_name;
			parameters[1].Value = model.project_type_id;

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
		public bool Delete(int project_type_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Project_type ");
			strSql.Append(" where project_type_id=@project_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@project_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = project_type_id;

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
		public bool DeleteList(string project_type_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Project_type ");
			strSql.Append(" where project_type_id in ("+project_type_idlist + ")  ");
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
		public M_Project_type GetModel(int project_type_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 project_type_id,project_type_name from hx_Project_type ");
			strSql.Append(" where project_type_id=@project_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@project_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = project_type_id;

			M_Project_type model=new M_Project_type();
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
		public M_Project_type DataRowToModel(DataRow row)
		{
			M_Project_type model=new M_Project_type();
			if (row != null)
			{
				if(row["project_type_id"]!=null && row["project_type_id"].ToString()!="")
				{
					model.project_type_id=int.Parse(row["project_type_id"].ToString());
				}
				if(row["project_type_name"]!=null)
				{
					model.project_type_name=row["project_type_name"].ToString();
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
			strSql.Append("select project_type_id,project_type_name ");
			strSql.Append(" FROM hx_Project_type ");
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
			strSql.Append(" project_type_id,project_type_name ");
			strSql.Append(" FROM hx_Project_type ");
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
			strSql.Append("select count(1) FROM hx_Project_type ");
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
				strSql.Append("order by T.project_type_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_Project_type T ");
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
			parameters[0].Value = "hx_Project_type";
			parameters[1].Value = "project_type_id";
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

