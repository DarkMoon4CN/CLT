
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_Links
	/// </summary>
	public partial class D_td_Links
	{
		public D_td_Links()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Linkid", "hx_td_Links"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Linkid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_Links");
			strSql.Append(" where Linkid=@Linkid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Linkid", SqlDbType.Int,4)			};
			parameters[0].Value = Linkid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(M_td_Links model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_Links(");
			strSql.Append("Linkname,LinkUrl,LinkType,LinkLogo,createtime,Linkstate)");
			strSql.Append(" values (");
			strSql.Append("@Linkname,@LinkUrl,@LinkType,@LinkLogo,@createtime,@Linkstate)");
			SqlParameter[] parameters = {
					
					new SqlParameter("@Linkname", SqlDbType.VarChar,100),
					new SqlParameter("@LinkUrl", SqlDbType.VarChar,100),
					new SqlParameter("@LinkType", SqlDbType.Int,4),
					new SqlParameter("@LinkLogo", SqlDbType.VarChar,300),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@Linkstate", SqlDbType.Int,4)};
			
			parameters[0].Value = model.Linkname;
			parameters[1].Value = model.LinkUrl;
			parameters[2].Value = model.LinkType;
			parameters[3].Value = model.LinkLogo;
			parameters[4].Value = model.createtime;
			parameters[5].Value = model.Linkstate;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(M_td_Links model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_Links set ");
			strSql.Append("Linkname=@Linkname,");
			strSql.Append("LinkUrl=@LinkUrl,");
			strSql.Append("LinkType=@LinkType,");
			strSql.Append("LinkLogo=@LinkLogo,");
			strSql.Append("createtime=@createtime,");
			strSql.Append("Linkstate=@Linkstate");
			strSql.Append(" where Linkid=@Linkid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Linkname", SqlDbType.VarChar,100),
					new SqlParameter("@LinkUrl", SqlDbType.VarChar,100),
					new SqlParameter("@LinkType", SqlDbType.Int,4),
					new SqlParameter("@LinkLogo", SqlDbType.VarChar,300),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@Linkstate", SqlDbType.Int,4),
					new SqlParameter("@Linkid", SqlDbType.Int,4)};
			parameters[0].Value = model.Linkname;
			parameters[1].Value = model.LinkUrl;
			parameters[2].Value = model.LinkType;
			parameters[3].Value = model.LinkLogo;
			parameters[4].Value = model.createtime;
			parameters[5].Value = model.Linkstate;
			parameters[6].Value = model.Linkid;

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
		public bool Delete(int Linkid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Links ");
			strSql.Append(" where Linkid=@Linkid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Linkid", SqlDbType.Int,4)			};
			parameters[0].Value = Linkid;

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
		public bool DeleteList(string Linkidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_Links ");
			strSql.Append(" where Linkid in ("+Linkidlist + ")  ");
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
		public M_td_Links GetModel(int Linkid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Linkid,Linkname,LinkUrl,LinkType,LinkLogo,createtime,Linkstate from hx_td_Links ");
			strSql.Append(" where Linkid=@Linkid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Linkid", SqlDbType.Int,4)			};
			parameters[0].Value = Linkid;

			M_td_Links model=new M_td_Links();
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
		public M_td_Links DataRowToModel(DataRow row)
		{
			M_td_Links model=new M_td_Links();
			if (row != null)
			{
				if(row["Linkid"]!=null && row["Linkid"].ToString()!="")
				{
					model.Linkid=int.Parse(row["Linkid"].ToString());
				}
				if(row["Linkname"]!=null)
				{
					model.Linkname=row["Linkname"].ToString();
				}
				if(row["LinkUrl"]!=null)
				{
					model.LinkUrl=row["LinkUrl"].ToString();
				}
				if(row["LinkType"]!=null && row["LinkType"].ToString()!="")
				{
					model.LinkType=int.Parse(row["LinkType"].ToString());
				}
				if(row["LinkLogo"]!=null)
				{
					model.LinkLogo=row["LinkLogo"].ToString();
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
				}
				if(row["Linkstate"]!=null && row["Linkstate"].ToString()!="")
				{
					model.Linkstate=int.Parse(row["Linkstate"].ToString());
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
			strSql.Append("select Linkid,Linkname,LinkUrl,LinkType,LinkLogo,createtime,Linkstate ");
			strSql.Append(" FROM hx_td_Links ");
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
			strSql.Append(" Linkid,Linkname,LinkUrl,LinkType,LinkLogo,createtime,Linkstate ");
			strSql.Append(" FROM hx_td_Links ");
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
			strSql.Append("select count(1) FROM hx_td_Links ");
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
				strSql.Append("order by T.Linkid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_Links T ");
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
			parameters[0].Value = "hx_td_Links";
			parameters[1].Value = "Linkid";
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

