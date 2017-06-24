
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:UsrBindCard
	/// </summary>
	public partial class D_UsrBindCard
	{
		public D_UsrBindCard()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
            return DbHelperSQL.GetMaxID("UsrBindCardID", "hx_UsrBindCardC"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(string UsrCustId)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select count(OpenAcctId) from hx_UsrBindCardC");
            strSql.Append(" where UsrCustId=@UsrCustId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,50)			};
            parameters[0].Value = UsrCustId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(M_UsrBindCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_UsrBindCardC(");
			strSql.Append("UsrCustId,OpenAcctId,OpenBankId,defCard)");
			strSql.Append(" values (");
			strSql.Append("@UsrCustId,@OpenAcctId,@OpenBankId,@defCard)");
			SqlParameter[] parameters = {
							
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,50),
					new SqlParameter("@OpenAcctId", SqlDbType.VarChar,50),
					new SqlParameter("@OpenBankId", SqlDbType.VarChar,50),
					new SqlParameter("@defCard", SqlDbType.Int,4)};
		
			parameters[0].Value = model.UsrCustId;
			parameters[1].Value = model.OpenAcctId;
			parameters[2].Value = model.OpenBankId;
			parameters[3].Value = model.defCard;

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
		public bool Update(M_UsrBindCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_UsrBindCardC set ");
			strSql.Append("registerid=@registerid,");
			strSql.Append("UsrCustId=@UsrCustId,");
			strSql.Append("OpenAcctId=@OpenAcctId,");
			strSql.Append("OpenBankId=@OpenBankId,");
			strSql.Append("defCard=@defCard");
			strSql.Append(" where UsrBindCardID=@UsrBindCardID ");
			SqlParameter[] parameters = {
					new SqlParameter("@registerid", SqlDbType.Int,4),
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,50),
					new SqlParameter("@OpenAcctId", SqlDbType.VarChar,50),
					new SqlParameter("@OpenBankId", SqlDbType.VarChar,50),
					new SqlParameter("@defCard", SqlDbType.Int,4),
					new SqlParameter("@UsrBindCardID", SqlDbType.Int,4)};
			parameters[0].Value = model.registerid;
			parameters[1].Value = model.UsrCustId;
			parameters[2].Value = model.OpenAcctId;
			parameters[3].Value = model.OpenBankId;
			parameters[4].Value = model.defCard;
			parameters[5].Value = model.UsrBindCardID;

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
		public bool Delete(int UsrBindCardID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_UsrBindCardC ");
			strSql.Append(" where UsrBindCardID=@UsrBindCardID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsrBindCardID", SqlDbType.Int,4)			};
			parameters[0].Value = UsrBindCardID;

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
		public bool DeleteList(string UsrBindCardIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_UsrBindCardC ");
			strSql.Append(" where UsrBindCardID in ("+UsrBindCardIDlist + ")  ");
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
        public M_UsrBindCard GetModel(string UsrCustId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UsrBindCardID,UsrCustId,OpenAcctId,OpenBankId,defCard from hx_UsrBindCardC ");
            strSql.Append(" where UsrCustId=@UsrCustId  and defCard=1 ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsrCustId", SqlDbType.VarChar,100)			};
            parameters[0].Value = UsrCustId;

			M_UsrBindCard model=new M_UsrBindCard();
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
		public M_UsrBindCard DataRowToModel(DataRow row)
		{
			M_UsrBindCard model=new M_UsrBindCard();
			if (row != null)
			{
				if(row["UsrBindCardID"]!=null && row["UsrBindCardID"].ToString()!="")
				{
					model.UsrBindCardID=int.Parse(row["UsrBindCardID"].ToString());
				}
				
				if(row["UsrCustId"]!=null)
				{
					model.UsrCustId=row["UsrCustId"].ToString();
				}
				if(row["OpenAcctId"]!=null)
				{
					model.OpenAcctId=row["OpenAcctId"].ToString();
				}
				if(row["OpenBankId"]!=null)
				{
					model.OpenBankId=row["OpenBankId"].ToString();
				}
				if(row["defCard"]!=null && row["defCard"].ToString()!="")
				{
					model.defCard=int.Parse(row["defCard"].ToString());
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
			strSql.Append("select UsrBindCardID,registerid,UsrCustId,OpenAcctId,OpenBankId,defCard ");
			strSql.Append(" FROM hx_UsrBindCardC ");
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
			strSql.Append(" UsrBindCardID,registerid,UsrCustId,OpenAcctId,OpenBankId,defCard ");
			strSql.Append(" FROM hx_UsrBindCardC ");
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
			strSql.Append("select count(1) FROM hx_UsrBindCardC ");
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
				strSql.Append("order by T.UsrBindCardID desc");
			}
			strSql.Append(")AS Row, T.*  from hx_UsrBindCardC T ");
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
			parameters[0].Value = "hx_UsrBindCardC";
			parameters[1].Value = "UsrBindCardID";
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

