
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_web_Ad_type
	/// </summary>
	public partial class D_td_web_Ad_type
	{
		public D_td_web_Ad_type()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("AdTypeId", "hx_td_web_Ad_type"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int AdTypeId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_web_Ad_type");
			strSql.Append(" where AdTypeId=@AdTypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@AdTypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = AdTypeId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_web_Ad_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_web_Ad_type(");
			strSql.Append("AdtypeName,createtime)");
			strSql.Append(" values (");
			strSql.Append("@AdtypeName,@createtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@AdtypeName", SqlDbType.VarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime)};
			parameters[0].Value = model.AdtypeName;
			parameters[1].Value = model.createtime;

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
		public bool Update(M_td_web_Ad_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_web_Ad_type set ");
			strSql.Append("AdtypeName=@AdtypeName,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where AdTypeId=@AdTypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@AdtypeName", SqlDbType.VarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@AdTypeId", SqlDbType.Int,4)};
			parameters[0].Value = model.AdtypeName;
			parameters[1].Value = model.createtime;
			parameters[2].Value = model.AdTypeId;

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
		public bool Delete(int AdTypeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_web_Ad_type ");
			strSql.Append(" where AdTypeId=@AdTypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@AdTypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = AdTypeId;

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
		public bool DeleteList(string AdTypeIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_web_Ad_type ");
			strSql.Append(" where AdTypeId in ("+AdTypeIdlist + ")  ");
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
		public M_td_web_Ad_type GetModel(int AdTypeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 AdTypeId,AdtypeName,createtime from hx_td_web_Ad_type ");
			strSql.Append(" where AdTypeId=@AdTypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@AdTypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = AdTypeId;

			M_td_web_Ad_type model=new M_td_web_Ad_type();
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
		public M_td_web_Ad_type DataRowToModel(DataRow row)
		{
			M_td_web_Ad_type model=new M_td_web_Ad_type();
			if (row != null)
			{
				if(row["AdTypeId"]!=null && row["AdTypeId"].ToString()!="")
				{
					model.AdTypeId=int.Parse(row["AdTypeId"].ToString());
				}
				if(row["AdtypeName"]!=null)
				{
					model.AdtypeName=row["AdtypeName"].ToString();
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
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
			strSql.Append("select AdTypeId,AdtypeName,createtime ");
			strSql.Append(" FROM hx_td_web_Ad_type ");
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
			strSql.Append(" AdTypeId,AdtypeName,createtime ");
			strSql.Append(" FROM hx_td_web_Ad_type ");
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
			strSql.Append("select count(1) FROM hx_td_web_Ad_type ");
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
				strSql.Append("order by T.AdTypeId desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_web_Ad_type T ");
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
			parameters[0].Value = "hx_td_web_Ad_type";
			parameters[1].Value = "AdTypeId";
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

