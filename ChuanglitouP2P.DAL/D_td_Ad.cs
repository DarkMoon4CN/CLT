
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_Ad
	/// </summary>
	public partial class D_td_Ad
	{
		public D_td_Ad()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Adid", "hx_td_Ad"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Adid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_Ad");
			strSql.Append(" where Adid=@Adid");
			SqlParameter[] parameters = {
					new SqlParameter("@Adid", SqlDbType.Int,4)
			};
			parameters[0].Value = Adid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_Ad model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_Ad(");
            strSql.Append("AdName,Adcreatetime,AdState,AdTypeId,AdPath,AdLink)");
			strSql.Append(" values (");
            strSql.Append("@AdName,@Adcreatetime,@AdState,@AdTypeId,@AdPath,@AdLink)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@AdName", SqlDbType.VarChar,50),
					new SqlParameter("@Adcreatetime", SqlDbType.DateTime),
					new SqlParameter("@AdState", SqlDbType.Int,4),
					new SqlParameter("@AdTypeId", SqlDbType.Int,4),
					new SqlParameter("@AdPath", SqlDbType.VarChar,100),
					new SqlParameter("@AdLink", SqlDbType.VarChar,100)};
			parameters[0].Value = model.AdName;
			parameters[1].Value = model.Adcreatetime;
			parameters[2].Value = model.AdState;
            parameters[3].Value = model.AdTypeId;
			parameters[4].Value = model.AdPath;
			parameters[5].Value = model.AdLink;

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
		public bool Update(M_td_Ad model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_Ad set ");
			strSql.Append("AdName=@AdName,");
			//strSql.Append("Adcreatetime=@Adcreatetime,");
			strSql.Append("AdState=@AdState,");
			strSql.Append("AdTypeId=@AdTypeId,");
			strSql.Append("AdPath=@AdPath,");
			strSql.Append("AdLink=@AdLink");
			strSql.Append(" where Adid=@Adid");
			SqlParameter[] parameters = {
					new SqlParameter("@AdName", SqlDbType.VarChar,50),
					//new SqlParameter("@Adcreatetime", SqlDbType.DateTime),
					new SqlParameter("@AdState", SqlDbType.Int,4),
					new SqlParameter("@AdTypeId", SqlDbType.Int,4),
					new SqlParameter("@AdPath", SqlDbType.VarChar,100),
					new SqlParameter("@AdLink", SqlDbType.VarChar,100),
					new SqlParameter("@Adid", SqlDbType.Int,4)};
			parameters[0].Value = model.AdName;
			//parameters[1].Value = model.Adcreatetime;
			parameters[1].Value = model.AdState;
			parameters[2].Value = model.AdTypeId;
			parameters[3].Value = model.AdPath;
			parameters[4].Value = model.AdLink;
			parameters[5].Value = model.Adid;

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
		public bool Delete(int Adid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Ad ");
			strSql.Append(" where Adid=@Adid");
			SqlParameter[] parameters = {
					new SqlParameter("@Adid", SqlDbType.Int,4)
			};
			parameters[0].Value = Adid;

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
		public bool DeleteList(string Adidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Ad ");
			strSql.Append(" where Adid in ("+Adidlist + ")  ");
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
		public M_td_Ad GetModel(int Adid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Adid,AdName,Adcreatetime,AdState,AdTypeId,AdPath,AdLink from hx_td_Ad ");
			strSql.Append(" where Adid=@Adid");
			SqlParameter[] parameters = {
					new SqlParameter("@Adid", SqlDbType.Int,4)
			};
			parameters[0].Value = Adid;

			M_td_Ad model=new M_td_Ad();
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
		public M_td_Ad DataRowToModel(DataRow row)
		{
			M_td_Ad model=new M_td_Ad();
			if (row != null)
			{
				if(row["Adid"]!=null && row["Adid"].ToString()!="")
				{
					model.Adid=int.Parse(row["Adid"].ToString());
				}
				if(row["AdName"]!=null)
				{
					model.AdName=row["AdName"].ToString();
				}
				if(row["Adcreatetime"]!=null && row["Adcreatetime"].ToString()!="")
				{
					model.Adcreatetime=DateTime.Parse(row["Adcreatetime"].ToString());
				}
				if(row["AdState"]!=null && row["AdState"].ToString()!="")
				{
					model.AdState=int.Parse(row["AdState"].ToString());
				}
				if(row["AdTypeId"]!=null)
				{
                    model.AdTypeId =int.Parse( row["AdTypeId"].ToString());
				}
				if(row["AdPath"]!=null)
				{
					model.AdPath=row["AdPath"].ToString();
				}
				if(row["AdLink"]!=null)
				{
					model.AdLink=row["AdLink"].ToString();
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
			strSql.Append("select Adid,AdName,Adcreatetime,AdState,AdTypeId,AdPath,AdLink ");
			strSql.Append(" FROM hx_td_Ad ");
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
			strSql.Append(" Adid,AdName,Adcreatetime,AdState,AdTypeId,AdPath,AdLink ");
			strSql.Append(" FROM hx_td_Ad ");
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
			strSql.Append("select count(1) FROM hx_td_Ad ");
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
				strSql.Append("order by T.Adid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_Ad T ");
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
			parameters[0].Value = "hx_td_Ad";
			parameters[1].Value = "Adid";
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

