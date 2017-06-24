using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_frozen
	/// </summary>
	public partial class D_td_frozen
	{
		public D_td_frozen()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Frozenid", "hx_td_frozen"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Frozenid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_frozen");
			strSql.Append(" where Frozenid=@Frozenid");
			SqlParameter[] parameters = {
					new SqlParameter("@Frozenid", SqlDbType.Int,4)
			};
			parameters[0].Value = Frozenid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_frozen model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_frozen(");
            strSql.Append("MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,targetid,UsrCustId,bid_records_id)");
			strSql.Append(" values (");
            strSql.Append("@MBT_Registerid,@FrozenidAmount,@FrozenidNo,@FrozenState,@FrozenDate,@targetid,@UsrCustId,@bid_records_id)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MBT_Registerid", SqlDbType.Int,4),
					new SqlParameter("@FrozenidAmount", SqlDbType.Decimal,17),
					new SqlParameter("@FrozenidNo", SqlDbType.VarChar,20),
					new SqlParameter("@FrozenState", SqlDbType.Int,4),
					new SqlParameter("@FrozenDate", SqlDbType.DateTime),
					new SqlParameter("@targetid", SqlDbType.Int,4),
                    new SqlParameter("@UsrCustId", SqlDbType.VarChar,200),
                    new SqlParameter("@bid_records_id", SqlDbType.Int,4)};
			parameters[0].Value = model.MBT_Registerid;
			parameters[1].Value = model.FrozenidAmount;
			parameters[2].Value = model.FrozenidNo;
			parameters[3].Value = model.FrozenState;
			parameters[4].Value = model.FrozenDate;
			parameters[5].Value = model.targetid;
            parameters[6].Value = model.UsrCustId;
            parameters[7].Value = model.bid_records_id;
            

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
		public bool Update(M_td_frozen model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_frozen set ");
			strSql.Append("MBT_Registerid=@MBT_Registerid,");
			strSql.Append("FrozenidAmount=@FrozenidAmount,");
			strSql.Append("FrozenidNo=@FrozenidNo,");
			strSql.Append("FrozenState=@FrozenState,");
			strSql.Append("FrozenDate=@FrozenDate,");
			strSql.Append("targetid=@targetid");
			strSql.Append(" where Frozenid=@Frozenid");
			SqlParameter[] parameters = {
					new SqlParameter("@MBT_Registerid", SqlDbType.Int,4),
					new SqlParameter("@FrozenidAmount", SqlDbType.Decimal,17),
					new SqlParameter("@FrozenidNo", SqlDbType.VarChar,20),
					new SqlParameter("@FrozenState", SqlDbType.Int,4),
					new SqlParameter("@FrozenDate", SqlDbType.DateTime),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@Frozenid", SqlDbType.Int,4)};
			parameters[0].Value = model.MBT_Registerid;
			parameters[1].Value = model.FrozenidAmount;
			parameters[2].Value = model.FrozenidNo;
			parameters[3].Value = model.FrozenState;
			parameters[4].Value = model.FrozenDate;
			parameters[5].Value = model.targetid;
			parameters[6].Value = model.Frozenid;

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
		public bool Delete(int Frozenid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_frozen ");
			strSql.Append(" where Frozenid=@Frozenid");
			SqlParameter[] parameters = {
					new SqlParameter("@Frozenid", SqlDbType.Int,4)
			};
			parameters[0].Value = Frozenid;

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
		public bool DeleteList(string Frozenidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_frozen ");
			strSql.Append(" where Frozenid in ("+Frozenidlist + ")  ");
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
		public M_td_frozen GetModel(int Frozenid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Frozenid,MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,targetid from hx_td_frozen ");
			strSql.Append(" where Frozenid=@Frozenid");
			SqlParameter[] parameters = {
					new SqlParameter("@Frozenid", SqlDbType.Int,4)
			};
			parameters[0].Value = Frozenid;

			M_td_frozen model=new M_td_frozen();
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
		public M_td_frozen DataRowToModel(DataRow row)
		{
			M_td_frozen model=new M_td_frozen();
			if (row != null)
			{
				if(row["Frozenid"]!=null && row["Frozenid"].ToString()!="")
				{
					model.Frozenid=int.Parse(row["Frozenid"].ToString());
				}
				if(row["MBT_Registerid"]!=null && row["MBT_Registerid"].ToString()!="")
				{
					model.MBT_Registerid=int.Parse(row["MBT_Registerid"].ToString());
				}
				if(row["FrozenidAmount"]!=null && row["FrozenidAmount"].ToString()!="")
				{
					model.FrozenidAmount=decimal.Parse(row["FrozenidAmount"].ToString());
				}
				if(row["FrozenidNo"]!=null)
				{
					model.FrozenidNo=row["FrozenidNo"].ToString();
				}
				if(row["FrozenState"]!=null && row["FrozenState"].ToString()!="")
				{
					model.FrozenState=int.Parse(row["FrozenState"].ToString());
				}
				if(row["FrozenDate"]!=null && row["FrozenDate"].ToString()!="")
				{
					model.FrozenDate=DateTime.Parse(row["FrozenDate"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
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
			strSql.Append("select Frozenid,MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,targetid ");
			strSql.Append(" FROM hx_td_frozen ");
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
			strSql.Append(" Frozenid,MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,targetid ");
			strSql.Append(" FROM hx_td_frozen ");
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
			strSql.Append("select count(1) FROM hx_td_frozen ");
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
				strSql.Append("order by T.Frozenid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_frozen T ");
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
			parameters[0].Value = "hx_td_frozen";
			parameters[1].Value = "Frozenid";
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

