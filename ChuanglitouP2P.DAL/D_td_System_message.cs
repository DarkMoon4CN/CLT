
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_System_message
	/// </summary>
	public partial class D_td_System_message
	{
		public D_td_System_message()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Messageid", "hx_td_System_message"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Messageid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_System_message");
			strSql.Append(" where Messageid=@Messageid");
			SqlParameter[] parameters = {
					new SqlParameter("@Messageid", SqlDbType.Int,4)
			};
			parameters[0].Value = Messageid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_System_message model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_System_message(");
            strSql.Append("MTitle,PubTime,MContext,Mstate,MUrl,MReg,Mtype)");
			strSql.Append(" values (");
            strSql.Append("@MTitle,@PubTime,@MContext,@Mstate,@MUrl,@MReg,@Mtype)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MTitle", SqlDbType.VarChar,255),
					new SqlParameter("@PubTime", SqlDbType.DateTime),
					new SqlParameter("@MContext", SqlDbType.VarChar,800),
					new SqlParameter("@Mstate", SqlDbType.Int,4),
					new SqlParameter("@MUrl", SqlDbType.VarChar,255),
					new SqlParameter("@MReg", SqlDbType.Int,4),
                    new SqlParameter("@Mtype", SqlDbType.Int,4)   };
			parameters[0].Value = model.MTitle;
			parameters[1].Value = model.PubTime;
			parameters[2].Value = model.MContext;
			parameters[3].Value = model.Mstate;
			parameters[4].Value = model.MUrl;
			parameters[5].Value = model.MReg;
            parameters[6].Value = model.Mtype;

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
		public bool Update(M_td_System_message model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_System_message set ");
			strSql.Append("MTitle=@MTitle,");
			strSql.Append("PubTime=@PubTime,");
			strSql.Append("MContext=@MContext,");
			strSql.Append("Mstate=@Mstate,");
			strSql.Append("MUrl=@MUrl,");
			strSql.Append("MReg=@MReg");
			strSql.Append(" where Messageid=@Messageid");
			SqlParameter[] parameters = {
					new SqlParameter("@MTitle", SqlDbType.VarChar,255),
					new SqlParameter("@PubTime", SqlDbType.DateTime),
					new SqlParameter("@MContext", SqlDbType.VarChar,800),
					new SqlParameter("@Mstate", SqlDbType.Int,4),
					new SqlParameter("@MUrl", SqlDbType.VarChar,255),
					new SqlParameter("@MReg", SqlDbType.Int,4),
					new SqlParameter("@Messageid", SqlDbType.Int,4)};
			parameters[0].Value = model.MTitle;
			parameters[1].Value = model.PubTime;
			parameters[2].Value = model.MContext;
			parameters[3].Value = model.Mstate;
			parameters[4].Value = model.MUrl;
			parameters[5].Value = model.MReg;
			parameters[6].Value = model.Messageid;

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
		public bool Delete(int Messageid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_System_message ");
			strSql.Append(" where Messageid=@Messageid");
			SqlParameter[] parameters = {
					new SqlParameter("@Messageid", SqlDbType.Int,4)
			};
			parameters[0].Value = Messageid;

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
		public bool DeleteList(string Messageidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_System_message ");
			strSql.Append(" where Messageid in ("+Messageidlist + ")  ");
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
		public M_td_System_message GetModel(int Messageid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Messageid,MTitle,PubTime,MContext,Mstate,MUrl,MReg from hx_td_System_message ");
			strSql.Append(" where Messageid=@Messageid");
			SqlParameter[] parameters = {
					new SqlParameter("@Messageid", SqlDbType.Int,4)
			};
			parameters[0].Value = Messageid;

			M_td_System_message model=new M_td_System_message();
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
		public M_td_System_message DataRowToModel(DataRow row)
		{
			M_td_System_message model=new M_td_System_message();
			if (row != null)
			{
				if(row["Messageid"]!=null && row["Messageid"].ToString()!="")
				{
					model.Messageid=int.Parse(row["Messageid"].ToString());
				}
				if(row["MTitle"]!=null)
				{
					model.MTitle=row["MTitle"].ToString();
				}
				if(row["PubTime"]!=null && row["PubTime"].ToString()!="")
				{
					model.PubTime=DateTime.Parse(row["PubTime"].ToString());
				}
				if(row["MContext"]!=null)
				{
					model.MContext=row["MContext"].ToString();
				}
				if(row["Mstate"]!=null && row["Mstate"].ToString()!="")
				{
					model.Mstate=int.Parse(row["Mstate"].ToString());
				}
				if(row["MUrl"]!=null)
				{
					model.MUrl=row["MUrl"].ToString();
				}
				if(row["MReg"]!=null && row["MReg"].ToString()!="")
				{
					model.MReg=int.Parse(row["MReg"].ToString());
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
			strSql.Append("select Messageid,MTitle,PubTime,MContext,Mstate,MUrl,MReg ");
			strSql.Append(" FROM hx_td_System_message ");
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
			strSql.Append(" Messageid,MTitle,PubTime,MContext,Mstate,MUrl,MReg ");
			strSql.Append(" FROM hx_td_System_message ");
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
			strSql.Append("select count(1) FROM hx_td_System_message ");
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
				strSql.Append("order by T.Messageid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_System_message T ");
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
			parameters[0].Value = "hx_td_System_message";
			parameters[1].Value = "Messageid";
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

