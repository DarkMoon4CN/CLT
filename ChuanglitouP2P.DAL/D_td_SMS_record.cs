
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_SMS_record
	/// </summary>
	public partial class D_td_SMS_record
	{
		public D_td_SMS_record()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("sms_record_id", "hx_td_SMS_record"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int sms_record_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_SMS_record");
			strSql.Append(" where sms_record_id=@sms_record_id");
			SqlParameter[] parameters = {
					new SqlParameter("@sms_record_id", SqlDbType.Int,4)
			};
			parameters[0].Value = sms_record_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_SMS_record model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_SMS_record(");
            strSql.Append("senduserid,phone_number,smscontext,smstype,sendtime,orderid,vcode,ip)");
			strSql.Append(" values (");
            strSql.Append("@senduserid,@phone_number,@smscontext,@smstype,@sendtime,@orderid,@vcode,@ip)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@senduserid", SqlDbType.Int,4),
					new SqlParameter("@phone_number", SqlDbType.VarChar,11),
					new SqlParameter("@smscontext", SqlDbType.VarChar,4000),
					new SqlParameter("@smstype", SqlDbType.Int,4),
					new SqlParameter("@sendtime", SqlDbType.DateTime),
                    new SqlParameter("@orderid", SqlDbType.Decimal,28),
                    new SqlParameter("@vcode", SqlDbType.VarChar,50),
                    new SqlParameter("@ip", SqlDbType.VarChar,50)};
			parameters[0].Value = model.senduserid;
			parameters[1].Value = model.phone_number;
			parameters[2].Value = model.smscontext;
			parameters[3].Value = model.smstype;
			parameters[4].Value = model.sendtime;
            parameters[5].Value = model.orderid;
            parameters[6].Value = model.vcode;
            parameters[7].Value = model.ip;

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
		public bool Update(M_td_SMS_record model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_SMS_record set ");
			strSql.Append("senduserid=@senduserid,");
			strSql.Append("phone_number=@phone_number,");
			strSql.Append("smscontext=@smscontext,");
			strSql.Append("smstype=@smstype,");
			strSql.Append("sendtime=@sendtime");
			strSql.Append(" where sms_record_id=@sms_record_id");
			SqlParameter[] parameters = {
					new SqlParameter("@senduserid", SqlDbType.Int,4),
					new SqlParameter("@phone_number", SqlDbType.VarChar,11),
					new SqlParameter("@smscontext", SqlDbType.VarChar,4000),
					new SqlParameter("@smstype", SqlDbType.Int,4),
					new SqlParameter("@sendtime", SqlDbType.DateTime),
					new SqlParameter("@sms_record_id", SqlDbType.Int,4)};
			parameters[0].Value = model.senduserid;
			parameters[1].Value = model.phone_number;
			parameters[2].Value = model.smscontext;
			parameters[3].Value = model.smstype;
			parameters[4].Value = model.sendtime;
			parameters[5].Value = model.sms_record_id;

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
		public bool Delete(int sms_record_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_SMS_record ");
			strSql.Append(" where sms_record_id=@sms_record_id");
			SqlParameter[] parameters = {
					new SqlParameter("@sms_record_id", SqlDbType.Int,4)
			};
			parameters[0].Value = sms_record_id;

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
		public bool DeleteList(string sms_record_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_SMS_record ");
			strSql.Append(" where sms_record_id in ("+sms_record_idlist + ")  ");
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
		public M_td_SMS_record GetModel(int sms_record_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 sms_record_id,senduserid,phone_number,smscontext,smstype,sendtime from hx_td_SMS_record ");
			strSql.Append(" where sms_record_id=@sms_record_id");
			SqlParameter[] parameters = {
					new SqlParameter("@sms_record_id", SqlDbType.Int,4)
			};
			parameters[0].Value = sms_record_id;

			M_td_SMS_record model=new M_td_SMS_record();
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
		public M_td_SMS_record DataRowToModel(DataRow row)
		{
			M_td_SMS_record model=new M_td_SMS_record();
			if (row != null)
			{
				if(row["sms_record_id"]!=null && row["sms_record_id"].ToString()!="")
				{
					model.sms_record_id=int.Parse(row["sms_record_id"].ToString());
				}
				if(row["senduserid"]!=null && row["senduserid"].ToString()!="")
				{
					model.senduserid=int.Parse(row["senduserid"].ToString());
				}
				if(row["phone_number"]!=null)
				{
					model.phone_number=row["phone_number"].ToString();
				}
				if(row["smscontext"]!=null)
				{
					model.smscontext=row["smscontext"].ToString();
				}
				if(row["smstype"]!=null && row["smstype"].ToString()!="")
				{
					model.smstype=int.Parse(row["smstype"].ToString());
				}
				if(row["sendtime"]!=null && row["sendtime"].ToString()!="")
				{
					model.sendtime=DateTime.Parse(row["sendtime"].ToString());
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
			strSql.Append("select sms_record_id,senduserid,phone_number,smscontext,smstype,sendtime ");
			strSql.Append(" FROM hx_td_SMS_record ");
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
			strSql.Append(" sms_record_id,senduserid,phone_number,smscontext,smstype,sendtime ");
			strSql.Append(" FROM hx_td_SMS_record ");
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
			strSql.Append("select count(1) FROM hx_td_SMS_record ");
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
				strSql.Append("order by T.sms_record_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_SMS_record T ");
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
			parameters[0].Value = "hx_td_SMS_record";
			parameters[1].Value = "sms_record_id";
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

