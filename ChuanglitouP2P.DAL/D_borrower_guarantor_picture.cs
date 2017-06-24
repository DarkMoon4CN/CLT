using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:borrower_guarantor_picture
	/// </summary>
	public partial class D_borrower_guarantor_picture
	{
		public D_borrower_guarantor_picture()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("borrower_guarantor_picture_id", "hx_borrower_guarantor_picture"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int borrower_guarantor_picture_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_borrower_guarantor_picture");
			strSql.Append(" where borrower_guarantor_picture_id=@borrower_guarantor_picture_id");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_guarantor_picture_id", SqlDbType.Int,4)
			};
			parameters[0].Value = borrower_guarantor_picture_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_borrower_guarantor_picture model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_borrower_guarantor_picture(");
			strSql.Append("borrower_registerid,targetid,type_picture,picture_path,picture_name,uploadtime)");
			strSql.Append(" values (");
			strSql.Append("@borrower_registerid,@targetid,@type_picture,@picture_path,@picture_name,@uploadtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@type_picture", SqlDbType.Int,4),
					new SqlParameter("@picture_path", SqlDbType.VarChar,200),
					new SqlParameter("@picture_name", SqlDbType.VarChar,200),
					new SqlParameter("@uploadtime", SqlDbType.DateTime)};
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.type_picture;
			parameters[3].Value = model.picture_path;
			parameters[4].Value = model.picture_name;
			parameters[5].Value = model.uploadtime;

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
		public bool Update(M_borrower_guarantor_picture model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_borrower_guarantor_picture set ");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("targetid=@targetid,");
			strSql.Append("type_picture=@type_picture,");
			strSql.Append("picture_path=@picture_path,");
			strSql.Append("picture_name=@picture_name,");
			strSql.Append("uploadtime=@uploadtime");
			strSql.Append(" where borrower_guarantor_picture_id=@borrower_guarantor_picture_id");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@type_picture", SqlDbType.Int,4),
					new SqlParameter("@picture_path", SqlDbType.VarChar,200),
					new SqlParameter("@picture_name", SqlDbType.VarChar,200),
					new SqlParameter("@uploadtime", SqlDbType.DateTime),
					new SqlParameter("@borrower_guarantor_picture_id", SqlDbType.Int,4)};
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.type_picture;
			parameters[3].Value = model.picture_path;
			parameters[4].Value = model.picture_name;
			parameters[5].Value = model.uploadtime;
			parameters[6].Value = model.borrower_guarantor_picture_id;

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
		public bool Delete(int borrower_guarantor_picture_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_borrower_guarantor_picture ");
			strSql.Append(" where borrower_guarantor_picture_id=@borrower_guarantor_picture_id");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_guarantor_picture_id", SqlDbType.Int,4)
			};
			parameters[0].Value = borrower_guarantor_picture_id;

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
		public bool DeleteList(string borrower_guarantor_picture_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_borrower_guarantor_picture ");
			strSql.Append(" where borrower_guarantor_picture_id in ("+borrower_guarantor_picture_idlist + ")  ");
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
		public M_borrower_guarantor_picture GetModel(int borrower_guarantor_picture_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 borrower_guarantor_picture_id,borrower_registerid,targetid,type_picture,picture_path,picture_name,uploadtime from hx_borrower_guarantor_picture ");
			strSql.Append(" where borrower_guarantor_picture_id=@borrower_guarantor_picture_id");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_guarantor_picture_id", SqlDbType.Int,4)
			};
			parameters[0].Value = borrower_guarantor_picture_id;

			M_borrower_guarantor_picture model=new M_borrower_guarantor_picture();
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
		public M_borrower_guarantor_picture DataRowToModel(DataRow row)
		{
			M_borrower_guarantor_picture model=new M_borrower_guarantor_picture();
			if (row != null)
			{
				if(row["borrower_guarantor_picture_id"]!=null && row["borrower_guarantor_picture_id"].ToString()!="")
				{
					model.borrower_guarantor_picture_id=int.Parse(row["borrower_guarantor_picture_id"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["type_picture"]!=null && row["type_picture"].ToString()!="")
				{
					model.type_picture=int.Parse(row["type_picture"].ToString());
				}
				if(row["picture_path"]!=null)
				{
					model.picture_path=row["picture_path"].ToString();
				}
				if(row["picture_name"]!=null)
				{
					model.picture_name=row["picture_name"].ToString();
				}
				if(row["uploadtime"]!=null && row["uploadtime"].ToString()!="")
				{
					model.uploadtime=DateTime.Parse(row["uploadtime"].ToString());
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
			strSql.Append("select borrower_guarantor_picture_id,borrower_registerid,targetid,type_picture,picture_path,picture_name,uploadtime ");
			strSql.Append(" FROM hx_borrower_guarantor_picture ");
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
			strSql.Append(" borrower_guarantor_picture_id,borrower_registerid,targetid,type_picture,picture_path,picture_name,uploadtime ");
			strSql.Append(" FROM hx_borrower_guarantor_picture ");
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
			strSql.Append("select count(1) FROM hx_borrower_guarantor_picture ");
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
				strSql.Append("order by T.borrower_guarantor_picture_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_borrower_guarantor_picture T ");
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
			parameters[0].Value = "hx_borrower_guarantor_picture";
			parameters[1].Value = "borrower_guarantor_picture_id";
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

