
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_web_type
	/// </summary>
	public partial class D_td_web_type
	{
		public D_td_web_type()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("menu_id", "hx_td_web_type"); 
		}


        /// <summary>
        /// 排序最大id
        /// </summary>
        /// <returns></returns>
        public int GetMaxIdOrderid(int typeid)
        {
            string sql = "select max(orderid) as orderid   from hx_td_web_type where  parentid ="+typeid.ToString();

            return DbHelperSQL.Execint(sql);

        }




		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int menu_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_web_type");
			strSql.Append(" where menu_id=@menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@menu_id", SqlDbType.Int,4)
			};
			parameters[0].Value = menu_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_web_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_web_type(");
			strSql.Append("menu_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime,path1,path2,path3,path4)");
			strSql.Append(" values (");
			strSql.Append("@menu_name,@parentid,@parentpath,@depath,@rootid,@child,@previd,@nextid,@orderid,@createtime,@path1,@path2,@path3,@path4)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@menu_name", SqlDbType.VarChar,50),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentpath", SqlDbType.VarChar,50),
					new SqlParameter("@depath", SqlDbType.Int,4),
					new SqlParameter("@rootid", SqlDbType.Int,4),
					new SqlParameter("@child", SqlDbType.Int,4),
					new SqlParameter("@previd", SqlDbType.Int,4),
					new SqlParameter("@nextid", SqlDbType.Int,4),
					new SqlParameter("@orderid", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@path1", SqlDbType.VarChar,150),
					new SqlParameter("@path2", SqlDbType.VarChar,150),
					new SqlParameter("@path3", SqlDbType.VarChar,150),
					new SqlParameter("@path4", SqlDbType.VarChar,150)};
			parameters[0].Value = model.menu_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentpath;
			parameters[3].Value = model.depath;
			parameters[4].Value = model.rootid;
			parameters[5].Value = model.child;
			parameters[6].Value = model.previd;
			parameters[7].Value = model.nextid;
			parameters[8].Value = model.orderid;
			parameters[9].Value = model.createtime;
			parameters[10].Value = model.path1;
			parameters[11].Value = model.path2;
			parameters[12].Value = model.path3;
			parameters[13].Value = model.path4;

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
		public bool Update(M_td_web_type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_web_type set ");
			strSql.Append("menu_name=@menu_name,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("parentpath=@parentpath,");
			strSql.Append("depath=@depath,");
			strSql.Append("rootid=@rootid,");
			strSql.Append("child=@child,");
			strSql.Append("previd=@previd,");
			strSql.Append("nextid=@nextid,");
			strSql.Append("orderid=@orderid,");
			strSql.Append("createtime=@createtime,");
			strSql.Append("path1=@path1,");
			strSql.Append("path2=@path2,");
			strSql.Append("path3=@path3,");
			strSql.Append("path4=@path4");
			strSql.Append(" where menu_id=@menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@menu_name", SqlDbType.VarChar,50),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentpath", SqlDbType.VarChar,50),
					new SqlParameter("@depath", SqlDbType.Int,4),
					new SqlParameter("@rootid", SqlDbType.Int,4),
					new SqlParameter("@child", SqlDbType.Int,4),
					new SqlParameter("@previd", SqlDbType.Int,4),
					new SqlParameter("@nextid", SqlDbType.Int,4),
					new SqlParameter("@orderid", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@path1", SqlDbType.VarChar,150),
					new SqlParameter("@path2", SqlDbType.VarChar,150),
					new SqlParameter("@path3", SqlDbType.VarChar,150),
					new SqlParameter("@path4", SqlDbType.VarChar,150),
					new SqlParameter("@menu_id", SqlDbType.Int,4)};
			parameters[0].Value = model.menu_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentpath;
			parameters[3].Value = model.depath;
			parameters[4].Value = model.rootid;
			parameters[5].Value = model.child;
			parameters[6].Value = model.previd;
			parameters[7].Value = model.nextid;
			parameters[8].Value = model.orderid;
			parameters[9].Value = model.createtime;
			parameters[10].Value = model.path1;
			parameters[11].Value = model.path2;
			parameters[12].Value = model.path3;
			parameters[13].Value = model.path4;
			parameters[14].Value = model.menu_id;

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
		public bool Delete(int menu_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_web_type ");
			strSql.Append(" where menu_id=@menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@menu_id", SqlDbType.Int,4)
			};
			parameters[0].Value = menu_id;

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
		public bool DeleteList(string menu_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_web_type ");
			strSql.Append(" where menu_id in ("+menu_idlist + ")  ");
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
		public M_td_web_type GetModel(int menu_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 menu_id,menu_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime,path1,path2,path3,path4 from hx_td_web_type ");
			strSql.Append(" where menu_id=@menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@menu_id", SqlDbType.Int,4)
			};
			parameters[0].Value = menu_id;

			M_td_web_type model=new M_td_web_type();
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
		public M_td_web_type DataRowToModel(DataRow row)
		{
			M_td_web_type model=new M_td_web_type();
			if (row != null)
			{
				if(row["menu_id"]!=null && row["menu_id"].ToString()!="")
				{
					model.menu_id=int.Parse(row["menu_id"].ToString());
				}
				if(row["menu_name"]!=null)
				{
					model.menu_name=row["menu_name"].ToString();
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
				if(row["path1"]!=null)
				{
					model.path1=row["path1"].ToString();
				}
				if(row["path2"]!=null)
				{
					model.path2=row["path2"].ToString();
				}
				if(row["path3"]!=null)
				{
					model.path3=row["path3"].ToString();
				}
				if(row["path4"]!=null)
				{
					model.path4=row["path4"].ToString();
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
			strSql.Append("select menu_id,menu_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime,path1,path2,path3,path4 ");
			strSql.Append(" FROM hx_td_web_type ");
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
			strSql.Append(" menu_id,menu_name,parentid,parentpath,depath,rootid,child,previd,nextid,orderid,createtime,path1,path2,path3,path4 ");
			strSql.Append(" FROM hx_td_web_type ");
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
			strSql.Append("select count(1) FROM hx_td_web_type ");
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
				strSql.Append("order by T.menu_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_web_type T ");
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
			parameters[0].Value = "hx_td_web_type";
			parameters[1].Value = "menu_id";
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

