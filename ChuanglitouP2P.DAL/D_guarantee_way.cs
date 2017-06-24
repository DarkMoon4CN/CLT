﻿using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:guarantee_way
	/// </summary>
	public partial class D_guarantee_way
	{
		public D_guarantee_way()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("guarantee_way_id", "guarantee_way"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int guarantee_way_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from guarantee_way");
			strSql.Append(" where guarantee_way_id=@guarantee_way_id");
			SqlParameter[] parameters = {
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4)
			};
			parameters[0].Value = guarantee_way_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_guarantee_way model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into guarantee_way(");
			strSql.Append("guarantee_way_name,amount_guaranteed,createtime)");
			strSql.Append(" values (");
			strSql.Append("@guarantee_way_name,@amount_guaranteed,@createtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@guarantee_way_name", SqlDbType.VarChar,100),
					new SqlParameter("@amount_guaranteed", SqlDbType.Decimal,17),
					new SqlParameter("@createtime", SqlDbType.DateTime)};
			parameters[0].Value = model.guarantee_way_name;
			parameters[1].Value = model.amount_guaranteed;
			parameters[2].Value = model.createtime;

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
		public bool Update(M_guarantee_way model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update guarantee_way set ");
			strSql.Append("guarantee_way_name=@guarantee_way_name,");
			strSql.Append("amount_guaranteed=@amount_guaranteed,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where guarantee_way_id=@guarantee_way_id");
			SqlParameter[] parameters = {
					new SqlParameter("@guarantee_way_name", SqlDbType.VarChar,100),
					new SqlParameter("@amount_guaranteed", SqlDbType.Decimal,17),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4)};
			parameters[0].Value = model.guarantee_way_name;
			parameters[1].Value = model.amount_guaranteed;
			parameters[2].Value = model.createtime;
			parameters[3].Value = model.guarantee_way_id;

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
		public bool Delete(int guarantee_way_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from guarantee_way ");
			strSql.Append(" where guarantee_way_id=@guarantee_way_id");
			SqlParameter[] parameters = {
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4)
			};
			parameters[0].Value = guarantee_way_id;

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
		public bool DeleteList(string guarantee_way_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from guarantee_way ");
			strSql.Append(" where guarantee_way_id in ("+guarantee_way_idlist + ")  ");
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
		public M_guarantee_way GetModel(int guarantee_way_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 guarantee_way_id,guarantee_way_name,amount_guaranteed,createtime from guarantee_way ");
			strSql.Append(" where guarantee_way_id=@guarantee_way_id");
			SqlParameter[] parameters = {
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4)
			};
			parameters[0].Value = guarantee_way_id;

			M_guarantee_way model=new M_guarantee_way();
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
		public M_guarantee_way DataRowToModel(DataRow row)
		{
			M_guarantee_way model=new M_guarantee_way();
			if (row != null)
			{
				if(row["guarantee_way_id"]!=null && row["guarantee_way_id"].ToString()!="")
				{
					model.guarantee_way_id=int.Parse(row["guarantee_way_id"].ToString());
				}
				if(row["guarantee_way_name"]!=null)
				{
					model.guarantee_way_name=row["guarantee_way_name"].ToString();
				}
				if(row["amount_guaranteed"]!=null && row["amount_guaranteed"].ToString()!="")
				{
					model.amount_guaranteed=decimal.Parse(row["amount_guaranteed"].ToString());
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
			strSql.Append("select guarantee_way_id,guarantee_way_name,amount_guaranteed,createtime ");
			strSql.Append(" FROM guarantee_way ");
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
			strSql.Append(" guarantee_way_id,guarantee_way_name,amount_guaranteed,createtime ");
			strSql.Append(" FROM guarantee_way ");
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
			strSql.Append("select count(1) FROM guarantee_way ");
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
				strSql.Append("order by T.guarantee_way_id desc");
			}
			strSql.Append(")AS Row, T.*  from guarantee_way T ");
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
			parameters[0].Value = "guarantee_way";
			parameters[1].Value = "guarantee_way_id";
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

