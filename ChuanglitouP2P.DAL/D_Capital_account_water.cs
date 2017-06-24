using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Capital_account_water
	/// </summary>
	public partial class D_Capital_account_water
	{
		public D_Capital_account_water()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("account_water_id", "hx_Capital_account_water"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int account_water_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_Capital_account_water");
			strSql.Append(" where account_water_id=@account_water_id");
			SqlParameter[] parameters = {
					new SqlParameter("@account_water_id", SqlDbType.Int,4)
			};
			parameters[0].Value = account_water_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Capital_account_water model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_Capital_account_water(");
            strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
			strSql.Append(" values (");
            strSql.Append("@membertable_registerid,@income,@expenditure,@time_of_occurrence,@account_balance,@types_Finance,@createtime,@keyid,@remarks)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@income", SqlDbType.Decimal,17),
					new SqlParameter("@expenditure", SqlDbType.Decimal,17),
					new SqlParameter("@time_of_occurrence", SqlDbType.DateTime),
					new SqlParameter("@account_balance", SqlDbType.Decimal,17),
					new SqlParameter("@types_Finance", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@keyid", SqlDbType.Int,4),  
                    new SqlParameter("@remarks", SqlDbType.VarChar,3000) };
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.income;
			parameters[2].Value = model.expenditure;
			parameters[3].Value = model.time_of_occurrence;
			parameters[4].Value = model.account_balance;
			parameters[5].Value = model.types_Finance;
			parameters[6].Value = model.createtime;
            parameters[7].Value = model.keyid;
            parameters[8].Value = model.remarks;


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
		public bool Update(M_Capital_account_water model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_Capital_account_water set ");
			strSql.Append("membertable_registerid=@membertable_registerid,");
			strSql.Append("income=@income,");
			strSql.Append("expenditure=@expenditure,");
			strSql.Append("time_of_occurrence=@time_of_occurrence,");
			strSql.Append("account_balance=@account_balance,");
			strSql.Append("types_Finance=@types_Finance,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where account_water_id=@account_water_id");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@income", SqlDbType.Decimal,17),
					new SqlParameter("@expenditure", SqlDbType.Decimal,17),
					new SqlParameter("@time_of_occurrence", SqlDbType.DateTime),
					new SqlParameter("@account_balance", SqlDbType.Decimal,17),
					new SqlParameter("@types_Finance", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@account_water_id", SqlDbType.Int,4)};
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.income;
			parameters[2].Value = model.expenditure;
			parameters[3].Value = model.time_of_occurrence;
			parameters[4].Value = model.account_balance;
			parameters[5].Value = model.types_Finance;
			parameters[6].Value = model.createtime;
			parameters[7].Value = model.account_water_id;

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
		public bool Delete(int account_water_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Capital_account_water ");
			strSql.Append(" where account_water_id=@account_water_id");
			SqlParameter[] parameters = {
					new SqlParameter("@account_water_id", SqlDbType.Int,4)
			};
			parameters[0].Value = account_water_id;

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
		public bool DeleteList(string account_water_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Capital_account_water ");
			strSql.Append(" where account_water_id in ("+account_water_idlist + ")  ");
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
		public M_Capital_account_water GetModel(int account_water_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 account_water_id,membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime from hx_Capital_account_water ");
			strSql.Append(" where account_water_id=@account_water_id");
			SqlParameter[] parameters = {
					new SqlParameter("@account_water_id", SqlDbType.Int,4)
			};
			parameters[0].Value = account_water_id;

			M_Capital_account_water model=new M_Capital_account_water();
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
		public M_Capital_account_water DataRowToModel(DataRow row)
		{
			M_Capital_account_water model=new M_Capital_account_water();
			if (row != null)
			{
				if(row["account_water_id"]!=null && row["account_water_id"].ToString()!="")
				{
					model.account_water_id=int.Parse(row["account_water_id"].ToString());
				}
				if(row["membertable_registerid"]!=null && row["membertable_registerid"].ToString()!="")
				{
					model.membertable_registerid=int.Parse(row["membertable_registerid"].ToString());
				}
				if(row["income"]!=null && row["income"].ToString()!="")
				{
					model.income=decimal.Parse(row["income"].ToString());
				}
				if(row["expenditure"]!=null && row["expenditure"].ToString()!="")
				{
					model.expenditure=decimal.Parse(row["expenditure"].ToString());
				}
				if(row["time_of_occurrence"]!=null && row["time_of_occurrence"].ToString()!="")
				{
					model.time_of_occurrence=DateTime.Parse(row["time_of_occurrence"].ToString());
				}
				if(row["account_balance"]!=null && row["account_balance"].ToString()!="")
				{
					model.account_balance=decimal.Parse(row["account_balance"].ToString());
				}
				if(row["types_Finance"]!=null && row["types_Finance"].ToString()!="")
				{
					model.types_Finance=int.Parse(row["types_Finance"].ToString());
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
			strSql.Append("select account_water_id,membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime ");
			strSql.Append(" FROM hx_Capital_account_water ");
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
			strSql.Append(" account_water_id,membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime ");
			strSql.Append(" FROM hx_Capital_account_water ");
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
			strSql.Append("select count(1) FROM hx_Capital_account_water ");
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
				strSql.Append("order by T.account_water_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_Capital_account_water T ");
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
			parameters[0].Value = "hx_Capital_account_water";
			parameters[1].Value = "account_water_id";
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

