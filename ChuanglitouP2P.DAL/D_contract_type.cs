
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:contract_type
	/// </summary>
	public partial class D_contract_type
	{
		public D_contract_type()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("contract_type_id", "hx_contract_type"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int contract_type_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_contract_type");
			strSql.Append(" where contract_type_id=@contract_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = contract_type_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_contract_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_contract_type(");
			strSql.Append("contract_type_name,createtime)");
			strSql.Append(" values (");
			strSql.Append("@contract_type_name,@createtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_type_name", SqlDbType.VarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime)};
			parameters[0].Value = model.contract_type_name;
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
		public bool Update(M_contract_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_contract_type set ");
			strSql.Append("contract_type_name=@contract_type_name,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where contract_type_id=@contract_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_type_name", SqlDbType.VarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@contract_type_id", SqlDbType.Int,4)};
			parameters[0].Value = model.contract_type_name;
			parameters[1].Value = model.createtime;
			parameters[2].Value = model.contract_type_id;

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
		public bool Delete(int contract_type_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_contract_type ");
			strSql.Append(" where contract_type_id=@contract_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = contract_type_id;

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
		public bool DeleteList(string contract_type_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_contract_type ");
			strSql.Append(" where contract_type_id in ("+contract_type_idlist + ")  ");
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
		public M_contract_type GetModel(int contract_type_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 contract_type_id,contract_type_name,createtime from hx_contract_type ");
			strSql.Append(" where contract_type_id=@contract_type_id");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_type_id", SqlDbType.Int,4)
			};
			parameters[0].Value = contract_type_id;

			M_contract_type model=new M_contract_type();
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
		public M_contract_type DataRowToModel(DataRow row)
		{
			M_contract_type model=new M_contract_type();
			if (row != null)
			{
				if(row["contract_type_id"]!=null && row["contract_type_id"].ToString()!="")
				{
					model.contract_type_id=int.Parse(row["contract_type_id"].ToString());
				}
				if(row["contract_type_name"]!=null)
				{
					model.contract_type_name=row["contract_type_name"].ToString();
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
			strSql.Append("select contract_type_id,contract_type_name,createtime ");
			strSql.Append(" FROM hx_contract_type ");
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
			strSql.Append(" contract_type_id,contract_type_name,createtime ");
			strSql.Append(" FROM hx_contract_type ");
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
			strSql.Append("select count(1) FROM hx_contract_type ");
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
				strSql.Append("order by T.contract_type_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_contract_type T ");
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
			parameters[0].Value = "hx_contract_type";
			parameters[1].Value = "contract_type_id";
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

