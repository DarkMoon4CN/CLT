
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_department
	/// </summary>
	public partial class D_td_department
	{
		public D_td_department()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("department_id", "hx_td_department"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int department_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_department");
			strSql.Append(" where department_id=@department_id");
			SqlParameter[] parameters = {
					new SqlParameter("@department_id", SqlDbType.Int,4)
			};
			parameters[0].Value = department_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_department model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_department(");
			strSql.Append("department_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime)");
			strSql.Append(" values (");
			strSql.Append("@department_name,@parentid,@parentpath,@depath,@rootid,@child,@previd,@nextid,@orderid,@createtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@department_name", SqlDbType.VarChar,20),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentpath", SqlDbType.VarChar,50),
					new SqlParameter("@depath", SqlDbType.Int,4),
					new SqlParameter("@rootid", SqlDbType.Int,4),
					new SqlParameter("@child", SqlDbType.Int,4),
					new SqlParameter("@previd", SqlDbType.Int,4),
					new SqlParameter("@nextid", SqlDbType.Int,4),
					new SqlParameter("@orderid", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime)};
			parameters[0].Value = model.department_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentpath;
			parameters[3].Value = model.depath;
			parameters[4].Value = model.rootid;
			parameters[5].Value = model.child;
			parameters[6].Value = model.previd;
			parameters[7].Value = model.nextid;
			parameters[8].Value = model.orderid;
			parameters[9].Value = model.createtime;

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
        /// 
        public bool Update(M_td_department model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hx_td_department set ");
            strSql.Append("department_name=@department_name");
           
            strSql.Append(" where department_id=@department_id");
            SqlParameter[] parameters = {
					new SqlParameter("@department_name", SqlDbType.VarChar,20),
				
					new SqlParameter("@department_id", SqlDbType.Int,4)};
            parameters[0].Value = model.department_name;
           
            parameters[1].Value = model.department_id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
		public bool Update(M_td_department model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_department set ");
			strSql.Append("department_name=@department_name,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("parentpath=@parentpath,");
			strSql.Append("depath=@depath,");
			strSql.Append("rootid=@rootid,");
			strSql.Append("child=@child,");
			strSql.Append("previd=@previd,");
			strSql.Append("nextid=@nextid,");
			strSql.Append("orderid=@orderid,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where department_id=@department_id");
			SqlParameter[] parameters = {
					new SqlParameter("@department_name", SqlDbType.VarChar,20),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentpath", SqlDbType.VarChar,50),
					new SqlParameter("@depath", SqlDbType.Int,4),
					new SqlParameter("@rootid", SqlDbType.Int,4),
					new SqlParameter("@child", SqlDbType.Int,4),
					new SqlParameter("@previd", SqlDbType.Int,4),
					new SqlParameter("@nextid", SqlDbType.Int,4),
					new SqlParameter("@orderid", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@department_id", SqlDbType.Int,4)};
			parameters[0].Value = model.department_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentpath;
			parameters[3].Value = model.depath;
			parameters[4].Value = model.rootid;
			parameters[5].Value = model.child;
			parameters[6].Value = model.previd;
			parameters[7].Value = model.nextid;
			parameters[8].Value = model.orderid;
			parameters[9].Value = model.createtime;
			parameters[10].Value = model.department_id;

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
         * */

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int department_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_department ");
			strSql.Append(" where department_id=@department_id");
			SqlParameter[] parameters = {
					new SqlParameter("@department_id", SqlDbType.Int,4)
			};
			parameters[0].Value = department_id;

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
		public bool DeleteList(string department_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_department ");
			strSql.Append(" where department_id in ("+department_idlist + ")  ");
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
		public M_td_department GetModel(int department_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 department_id,department_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime from hx_td_department ");
			strSql.Append(" where department_id=@department_id");
			SqlParameter[] parameters = {
					new SqlParameter("@department_id", SqlDbType.Int,4)
			};
			parameters[0].Value = department_id;

			M_td_department model=new M_td_department();
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
		public M_td_department DataRowToModel(DataRow row)
		{
			M_td_department model=new M_td_department();
			if (row != null)
			{
				if(row["department_id"]!=null && row["department_id"].ToString()!="")
				{
					model.department_id=int.Parse(row["department_id"].ToString());
				}
				if(row["department_name"]!=null)
				{
					model.department_name=row["department_name"].ToString();
				}
				if(row["parentid"]!=null && row["parentid"].ToString()!="")
				{
					model.parentid=int.Parse(row["parentid"].ToString());
				}
				if(row["parentpath"]!=null)
				{
					model.parentpath=row["parentpath"].ToString();
				}
				if(row["depath"]!=null && row["depath"].ToString()!="")
				{
					model.depath=int.Parse(row["depath"].ToString());
				}
				if(row["rootid"]!=null && row["rootid"].ToString()!="")
				{
					model.rootid=int.Parse(row["rootid"].ToString());
				}
				if(row["child"]!=null && row["child"].ToString()!="")
				{
					model.child=int.Parse(row["child"].ToString());
				}
				if(row["previd"]!=null && row["previd"].ToString()!="")
				{
					model.previd=int.Parse(row["previd"].ToString());
				}
				if(row["nextid"]!=null && row["nextid"].ToString()!="")
				{
					model.nextid=int.Parse(row["nextid"].ToString());
				}
				if(row["orderid"]!=null && row["orderid"].ToString()!="")
				{
					model.orderid=int.Parse(row["orderid"].ToString());
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
			strSql.Append("select department_id,department_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime ");
			strSql.Append(" FROM hx_td_department ");
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
			strSql.Append(" department_id,department_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime ");
			strSql.Append(" FROM hx_td_department ");
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
			strSql.Append("select count(1) FROM hx_td_department ");
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
				strSql.Append("order by T.department_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_department T ");
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
			parameters[0].Value = "hx_td_department";
			parameters[1].Value = "department_id";
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

