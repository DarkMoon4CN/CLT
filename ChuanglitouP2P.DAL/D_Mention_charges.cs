using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Mention_charges
	/// </summary>
	public partial class D_Mention_charges
	{
		public D_Mention_charges()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("mention_charges_id", "hx_Mention_charges"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int mention_charges_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_Mention_charges");
			strSql.Append(" where mention_charges_id=@mention_charges_id");
			SqlParameter[] parameters = {
					new SqlParameter("@mention_charges_id", SqlDbType.Int,4)
			};
			parameters[0].Value = mention_charges_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Mention_charges model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_Mention_charges(");
			strSql.Append("mention_charges_name,minimum_amount,maximum_amount,fees,fees_unit)");
			strSql.Append(" values (");
			strSql.Append("@mention_charges_name,@minimum_amount,@maximum_amount,@fees,@fees_unit)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@mention_charges_name", SqlDbType.VarChar,100),
					new SqlParameter("@minimum_amount", SqlDbType.Decimal,17),
					new SqlParameter("@maximum_amount", SqlDbType.Decimal,17),
					new SqlParameter("@fees", SqlDbType.Decimal,17),
					new SqlParameter("@fees_unit", SqlDbType.Int,4)};
			parameters[0].Value = model.mention_charges_name;
			parameters[1].Value = model.minimum_amount;
			parameters[2].Value = model.maximum_amount;
			parameters[3].Value = model.fees;
			parameters[4].Value = model.fees_unit;

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
		public bool Update(M_Mention_charges model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_Mention_charges set ");
			strSql.Append("mention_charges_name=@mention_charges_name,");
			strSql.Append("minimum_amount=@minimum_amount,");
			strSql.Append("maximum_amount=@maximum_amount,");
			strSql.Append("fees=@fees,");
			strSql.Append("fees_unit=@fees_unit");
			strSql.Append(" where mention_charges_id=@mention_charges_id");
			SqlParameter[] parameters = {
					new SqlParameter("@mention_charges_name", SqlDbType.VarChar,100),
					new SqlParameter("@minimum_amount", SqlDbType.Decimal,17),
					new SqlParameter("@maximum_amount", SqlDbType.Decimal,17),
					new SqlParameter("@fees", SqlDbType.Decimal,17),
					new SqlParameter("@fees_unit", SqlDbType.Int,4),
					new SqlParameter("@mention_charges_id", SqlDbType.Int,4)};
			parameters[0].Value = model.mention_charges_name;
			parameters[1].Value = model.minimum_amount;
			parameters[2].Value = model.maximum_amount;
			parameters[3].Value = model.fees;
			parameters[4].Value = model.fees_unit;
			parameters[5].Value = model.mention_charges_id;

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
		public bool Delete(int mention_charges_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Mention_charges ");
			strSql.Append(" where mention_charges_id=@mention_charges_id");
			SqlParameter[] parameters = {
					new SqlParameter("@mention_charges_id", SqlDbType.Int,4)
			};
			parameters[0].Value = mention_charges_id;

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
		public bool DeleteList(string mention_charges_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_Mention_charges ");
			strSql.Append(" where mention_charges_id in ("+mention_charges_idlist + ")  ");
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
		public M_Mention_charges GetModel(int mention_charges_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 mention_charges_id,mention_charges_name,minimum_amount,maximum_amount,fees,fees_unit from hx_Mention_charges ");
			strSql.Append(" where mention_charges_id=@mention_charges_id");
			SqlParameter[] parameters = {
					new SqlParameter("@mention_charges_id", SqlDbType.Int,4)
			};
			parameters[0].Value = mention_charges_id;

			M_Mention_charges model=new M_Mention_charges();
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
		public M_Mention_charges DataRowToModel(DataRow row)
		{
			M_Mention_charges model=new M_Mention_charges();
			if (row != null)
			{
				if(row["mention_charges_id"]!=null && row["mention_charges_id"].ToString()!="")
				{
					model.mention_charges_id=int.Parse(row["mention_charges_id"].ToString());
				}
				if(row["mention_charges_name"]!=null)
				{
					model.mention_charges_name=row["mention_charges_name"].ToString();
				}
				if(row["minimum_amount"]!=null && row["minimum_amount"].ToString()!="")
				{
					model.minimum_amount=decimal.Parse(row["minimum_amount"].ToString());
				}
				if(row["maximum_amount"]!=null && row["maximum_amount"].ToString()!="")
				{
					model.maximum_amount=decimal.Parse(row["maximum_amount"].ToString());
				}
				if(row["fees"]!=null && row["fees"].ToString()!="")
				{
					model.fees=decimal.Parse(row["fees"].ToString());
				}
				if(row["fees_unit"]!=null && row["fees_unit"].ToString()!="")
				{
					model.fees_unit=int.Parse(row["fees_unit"].ToString());
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
			strSql.Append("select mention_charges_id,mention_charges_name,minimum_amount,maximum_amount,fees,fees_unit ");
			strSql.Append(" FROM hx_Mention_charges ");
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
			strSql.Append(" mention_charges_id,mention_charges_name,minimum_amount,maximum_amount,fees,fees_unit ");
			strSql.Append(" FROM hx_Mention_charges ");
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
			strSql.Append("select count(1) FROM hx_Mention_charges ");
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
				strSql.Append("order by T.mention_charges_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_Mention_charges T ");
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
			parameters[0].Value = "hx_Mention_charges";
			parameters[1].Value = "mention_charges_id";
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

