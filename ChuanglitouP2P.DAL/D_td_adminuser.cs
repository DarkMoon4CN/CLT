
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.DBUtility;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:td_adminuser
	/// </summary>
	public partial class D_td_adminuser
	{
		public D_td_adminuser()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("adminuserid", "hx_td_adminuser"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int adminuserid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_td_adminuser");
			strSql.Append(" where adminuserid=@adminuserid");
			SqlParameter[] parameters = {
					new SqlParameter("@adminuserid", SqlDbType.Int,4)
			};
			parameters[0].Value = adminuserid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


        /// <summary>
        /// 用户名是否存在
        /// </summary>
        public bool Check_adminuser(string adminuser)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from hx_td_adminuser");
            strSql.Append(" where adminuser=@adminuser");
            SqlParameter[] parameters = {
					new SqlParameter("@adminuser", SqlDbType.VarChar,40)
			};
            parameters[0].Value = adminuser;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
          
        }



        /// <summary>
        /// 检查密码输入是否正确
        /// </summary>
        /// <returns></returns>
        public int Check_userpass(string adminuser, string userpass, string ip)
        {
            int t = 0;

            SqlConnection con = new SqlConnection(PubConstant.ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select adminuserid from hx_td_adminuser where state=1 and adminuser=@adminuser and userpass=@userpass", con);
                SqlParameter para = new SqlParameter("@adminuser", SqlDbType.VarChar, 50);
                para.Value = adminuser;
                cmd.Parameters.Add(para);
                para = new SqlParameter("@userpass", SqlDbType.VarChar, 50);
                para.Value = userpass;
                cmd.Parameters.Add(para);

                t = Convert.ToInt32(cmd.ExecuteScalar());
                if (t > 0)
                {

                    DbHelperSQL.RunSql("update hx_td_adminuser set lastLoginTime=getdate(),lastLoginIP='" + ip + "',loginTimes=loginTimes+1 where adminuserid=" + t.ToString() + "");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }


         


            return t;
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_td_adminuser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_td_adminuser(");
			strSql.Append("adminuser,userpass,state,datetime,trueName,email,province,city,tel,phone_number,lastLoginTime,lastLoginIP,loginTimes,worknum,sex,department_id,area_id)");
			strSql.Append(" values (");
			strSql.Append("@adminuser,@userpass,@state,@datetime,@trueName,@email,@province,@city,@tel,@phone_number,@lastLoginTime,@lastLoginIP,@loginTimes,@worknum,@sex,@department_id,@area_id)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@adminuser", SqlDbType.VarChar,50),
					new SqlParameter("@userpass", SqlDbType.VarChar,50),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@datetime", SqlDbType.DateTime),
					new SqlParameter("@trueName", SqlDbType.VarChar,50),
					new SqlParameter("@email", SqlDbType.VarChar,50),
					new SqlParameter("@province", SqlDbType.VarChar,20),
					new SqlParameter("@city", SqlDbType.VarChar,20),
					new SqlParameter("@tel", SqlDbType.VarChar,20),
					new SqlParameter("@phone_number", SqlDbType.VarChar,20),
					new SqlParameter("@lastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@lastLoginIP", SqlDbType.VarChar,50),
					new SqlParameter("@loginTimes", SqlDbType.Int,4),
					new SqlParameter("@worknum", SqlDbType.VarChar,50),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@department_id", SqlDbType.Int,4),
					new SqlParameter("@area_id", SqlDbType.Int,4)};
			parameters[0].Value = model.adminuser;
			parameters[1].Value = model.userpass;
			parameters[2].Value = model.state;
			parameters[3].Value = model.datetime;
			parameters[4].Value = model.trueName;
			parameters[5].Value = model.email;
			parameters[6].Value = model.province;
			parameters[7].Value = model.city;
			parameters[8].Value = model.tel;
			parameters[9].Value = model.phone_number;
			parameters[10].Value = model.lastLoginTime;
			parameters[11].Value = model.lastLoginIP;
			parameters[12].Value = model.loginTimes;
			parameters[13].Value = model.worknum;
			parameters[14].Value = model.sex;
			parameters[15].Value = model.department_id;
			parameters[16].Value = model.area_id;

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
		public bool Update(M_td_adminuser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_td_adminuser set ");
			strSql.Append("adminuser=@adminuser,");
			strSql.Append("userpass=@userpass,");
			strSql.Append("state=@state,");
			strSql.Append("datetime=@datetime,");
			strSql.Append("trueName=@trueName,");
			strSql.Append("email=@email,");
			strSql.Append("province=@province,");
			strSql.Append("city=@city,");
			strSql.Append("tel=@tel,");
			strSql.Append("phone_number=@phone_number,");
			strSql.Append("lastLoginTime=@lastLoginTime,");
			strSql.Append("lastLoginIP=@lastLoginIP,");
			strSql.Append("loginTimes=@loginTimes,");
			strSql.Append("worknum=@worknum,");
			strSql.Append("sex=@sex,");
			strSql.Append("department_id=@department_id,");
			strSql.Append("area_id=@area_id");
			strSql.Append(" where adminuserid=@adminuserid");
			SqlParameter[] parameters = {
					new SqlParameter("@adminuser", SqlDbType.VarChar,50),
					new SqlParameter("@userpass", SqlDbType.VarChar,50),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@datetime", SqlDbType.DateTime),
					new SqlParameter("@trueName", SqlDbType.VarChar,50),
					new SqlParameter("@email", SqlDbType.VarChar,50),
					new SqlParameter("@province", SqlDbType.VarChar,20),
					new SqlParameter("@city", SqlDbType.VarChar,20),
					new SqlParameter("@tel", SqlDbType.VarChar,20),
					new SqlParameter("@phone_number", SqlDbType.VarChar,20),
					new SqlParameter("@lastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@lastLoginIP", SqlDbType.VarChar,50),
					new SqlParameter("@loginTimes", SqlDbType.Int,4),
					new SqlParameter("@worknum", SqlDbType.VarChar,50),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@department_id", SqlDbType.Int,4),
					new SqlParameter("@area_id", SqlDbType.Int,4),
					new SqlParameter("@adminuserid", SqlDbType.Int,4)};
			parameters[0].Value = model.adminuser;
			parameters[1].Value = model.userpass;
			parameters[2].Value = model.state;
			parameters[3].Value = model.datetime;
			parameters[4].Value = model.trueName;
			parameters[5].Value = model.email;
			parameters[6].Value = model.province;
			parameters[7].Value = model.city;
			parameters[8].Value = model.tel;
			parameters[9].Value = model.phone_number;
			parameters[10].Value = model.lastLoginTime;
			parameters[11].Value = model.lastLoginIP;
			parameters[12].Value = model.loginTimes;
			parameters[13].Value = model.worknum;
			parameters[14].Value = model.sex;
			parameters[15].Value = model.department_id;
			parameters[16].Value = model.area_id;
			parameters[17].Value = model.adminuserid;

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
		public bool Delete(int adminuserid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_adminuser ");
			strSql.Append(" where adminuserid=@adminuserid");
			SqlParameter[] parameters = {
					new SqlParameter("@adminuserid", SqlDbType.Int,4)
			};
			parameters[0].Value = adminuserid;

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
		public bool DeleteList(string adminuseridlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_td_adminuser ");
			strSql.Append(" where adminuserid in ("+adminuseridlist + ")  ");
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
		public M_td_adminuser GetModel(int adminuserid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 adminuserid,adminuser,userpass,state,datetime,trueName,email,province,city,tel,phone_number,lastLoginTime,lastLoginIP,loginTimes,worknum,sex,department_id,area_id from hx_td_adminuser ");
			strSql.Append(" where adminuserid=@adminuserid");
			SqlParameter[] parameters = {
					new SqlParameter("@adminuserid", SqlDbType.Int,4)
			};
			parameters[0].Value = adminuserid;

			M_td_adminuser model=new M_td_adminuser();
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
		public M_td_adminuser DataRowToModel(DataRow row)
		{
			M_td_adminuser model=new M_td_adminuser();
			if (row != null)
			{
				if(row["adminuserid"]!=null && row["adminuserid"].ToString()!="")
				{
					model.adminuserid=int.Parse(row["adminuserid"].ToString());
				}
				if(row["adminuser"]!=null)
				{
					model.adminuser=row["adminuser"].ToString();
				}
				if(row["userpass"]!=null)
				{
					model.userpass=row["userpass"].ToString();
				}
				if(row["state"]!=null && row["state"].ToString()!="")
				{
					model.state=int.Parse(row["state"].ToString());
				}
				if(row["datetime"]!=null && row["datetime"].ToString()!="")
				{
					model.datetime=DateTime.Parse(row["datetime"].ToString());
				}
				if(row["trueName"]!=null)
				{
					model.trueName=row["trueName"].ToString();
				}
				if(row["email"]!=null)
				{
					model.email=row["email"].ToString();
				}
				if(row["province"]!=null)
				{
					model.province=row["province"].ToString();
				}
				if(row["city"]!=null)
				{
					model.city=row["city"].ToString();
				}
				if(row["tel"]!=null)
				{
					model.tel=row["tel"].ToString();
				}
				if(row["phone_number"]!=null)
				{
					model.phone_number=row["phone_number"].ToString();
				}
				if(row["lastLoginTime"]!=null && row["lastLoginTime"].ToString()!="")
				{
					model.lastLoginTime=DateTime.Parse(row["lastLoginTime"].ToString());
				}
				if(row["lastLoginIP"]!=null)
				{
					model.lastLoginIP=row["lastLoginIP"].ToString();
				}
				if(row["loginTimes"]!=null && row["loginTimes"].ToString()!="")
				{
					model.loginTimes=int.Parse(row["loginTimes"].ToString());
				}
				if(row["worknum"]!=null)
				{
					model.worknum=row["worknum"].ToString();
				}
				if(row["sex"]!=null)
				{
					model.sex=row["sex"].ToString();
				}
				if(row["department_id"]!=null && row["department_id"].ToString()!="")
				{
					model.department_id=int.Parse(row["department_id"].ToString());
				}
				if(row["area_id"]!=null && row["area_id"].ToString()!="")
				{
					model.area_id=int.Parse(row["area_id"].ToString());
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
			strSql.Append("select adminuserid,adminuser,userpass,state,datetime,trueName,email,province,city,tel,phone_number,lastLoginTime,lastLoginIP,loginTimes,worknum,sex,department_id,area_id ");
			strSql.Append(" FROM hx_td_adminuser ");
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
			strSql.Append(" adminuserid,adminuser,userpass,state,datetime,trueName,email,province,city,tel,phone_number,lastLoginTime,lastLoginIP,loginTimes,worknum,sex,department_id,area_id ");
			strSql.Append(" FROM hx_td_adminuser ");
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
			strSql.Append("select count(1) FROM hx_td_adminuser ");
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
				strSql.Append("order by T.adminuserid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_td_adminuser T ");
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
			parameters[0].Value = "hx_td_adminuser";
			parameters[1].Value = "adminuserid";
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

