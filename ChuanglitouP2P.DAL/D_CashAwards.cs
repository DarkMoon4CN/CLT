
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:CashAwards
	/// </summary>
	public partial class D_CashAwards
	{
		public D_CashAwards()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("CashAwardsid", "hx_CashAwards"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int CashAwardsid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_CashAwards");
			strSql.Append(" where CashAwardsid=@CashAwardsid");
			SqlParameter[] parameters = {
					new SqlParameter("@CashAwardsid", SqlDbType.Int,4)
			};
			parameters[0].Value = CashAwardsid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_CashAwards model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_CashAwards(");
			strSql.Append("membertable_registerid,proid,Amounts,AwardsTime,targetid,OrdId,OrdIdstate,UsrCustId)");
			strSql.Append(" values (");
			strSql.Append("@membertable_registerid,@proid,@Amounts,@AwardsTime,@targetid,@OrdId,@OrdIdstate,@UsrCustId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@proid", SqlDbType.Int,4),
					new SqlParameter("@Amounts", SqlDbType.Decimal,5),
					new SqlParameter("@AwardsTime", SqlDbType.DateTime),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@OrdId", SqlDbType.Decimal,13),
					new SqlParameter("@OrdIdstate", SqlDbType.Int,4),
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,50)};
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.proid;
			parameters[2].Value = model.Amounts;
			parameters[3].Value = model.AwardsTime;
			parameters[4].Value = model.targetid;
			parameters[5].Value = model.OrdId;
			parameters[6].Value = model.OrdIdstate;
			parameters[7].Value = model.UsrCustId;

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
		public bool Update(M_CashAwards model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_CashAwards set ");
			strSql.Append("membertable_registerid=@membertable_registerid,");
			strSql.Append("proid=@proid,");
			strSql.Append("Amounts=@Amounts,");
			strSql.Append("AwardsTime=@AwardsTime,");
			strSql.Append("targetid=@targetid,");
			strSql.Append("OrdId=@OrdId,");
			strSql.Append("OrdIdstate=@OrdIdstate,");
			strSql.Append("UsrCustId=@UsrCustId");
			strSql.Append(" where CashAwardsid=@CashAwardsid");
			SqlParameter[] parameters = {
					new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
					new SqlParameter("@proid", SqlDbType.Int,4),
					new SqlParameter("@Amounts", SqlDbType.Decimal,5),
					new SqlParameter("@AwardsTime", SqlDbType.DateTime),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@OrdId", SqlDbType.Decimal,13),
					new SqlParameter("@OrdIdstate", SqlDbType.Int,4),
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,50),
					new SqlParameter("@CashAwardsid", SqlDbType.Int,4)};
			parameters[0].Value = model.membertable_registerid;
			parameters[1].Value = model.proid;
			parameters[2].Value = model.Amounts;
			parameters[3].Value = model.AwardsTime;
			parameters[4].Value = model.targetid;
			parameters[5].Value = model.OrdId;
			parameters[6].Value = model.OrdIdstate;
			parameters[7].Value = model.UsrCustId;
			parameters[8].Value = model.CashAwardsid;

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
		public bool Delete(int CashAwardsid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_CashAwards ");
			strSql.Append(" where CashAwardsid=@CashAwardsid");
			SqlParameter[] parameters = {
					new SqlParameter("@CashAwardsid", SqlDbType.Int,4)
			};
			parameters[0].Value = CashAwardsid;

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
		public bool DeleteList(string CashAwardsidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_CashAwards ");
			strSql.Append(" where CashAwardsid in ("+CashAwardsidlist + ")  ");
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
		public M_CashAwards GetModel(int CashAwardsid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 CashAwardsid,membertable_registerid,proid,Amounts,AwardsTime,targetid,OrdId,OrdIdstate,UsrCustId from hx_CashAwards ");
			strSql.Append(" where CashAwardsid=@CashAwardsid");
			SqlParameter[] parameters = {
					new SqlParameter("@CashAwardsid", SqlDbType.Int,4)
			};
			parameters[0].Value = CashAwardsid;

			M_CashAwards model=new M_CashAwards();
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
		public M_CashAwards DataRowToModel(DataRow row)
		{
			M_CashAwards model=new M_CashAwards();
			if (row != null)
			{
				if(row["CashAwardsid"]!=null && row["CashAwardsid"].ToString()!="")
				{
					model.CashAwardsid=int.Parse(row["CashAwardsid"].ToString());
				}
				if(row["membertable_registerid"]!=null && row["membertable_registerid"].ToString()!="")
				{
					model.membertable_registerid=int.Parse(row["membertable_registerid"].ToString());
				}
				if(row["proid"]!=null && row["proid"].ToString()!="")
				{
					model.proid=int.Parse(row["proid"].ToString());
				}
				if(row["Amounts"]!=null && row["Amounts"].ToString()!="")
				{
					model.Amounts=decimal.Parse(row["Amounts"].ToString());
				}
				if(row["AwardsTime"]!=null && row["AwardsTime"].ToString()!="")
				{
					model.AwardsTime=DateTime.Parse(row["AwardsTime"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["OrdId"]!=null && row["OrdId"].ToString()!="")
				{
					model.OrdId=decimal.Parse(row["OrdId"].ToString());
				}
				if(row["OrdIdstate"]!=null && row["OrdIdstate"].ToString()!="")
				{
					model.OrdIdstate=int.Parse(row["OrdIdstate"].ToString());
				}
				if(row["UsrCustId"]!=null)
				{
					model.UsrCustId=row["UsrCustId"].ToString();
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
			strSql.Append("select CashAwardsid,membertable_registerid,proid,Amounts,AwardsTime,targetid,OrdId,OrdIdstate,UsrCustId ");
			strSql.Append(" FROM hx_CashAwards ");
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
			strSql.Append(" CashAwardsid,membertable_registerid,proid,Amounts,AwardsTime,targetid,OrdId,OrdIdstate,UsrCustId ");
			strSql.Append(" FROM hx_CashAwards ");
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
			strSql.Append("select count(1) FROM hx_CashAwards ");
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
				strSql.Append("order by T.CashAwardsid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_CashAwards T ");
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
			parameters[0].Value = "hx_CashAwards";
			parameters[1].Value = "CashAwardsid";
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

