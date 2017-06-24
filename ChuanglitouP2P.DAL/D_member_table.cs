using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:member_table
	/// </summary>
	public partial class D_member_table
	{
		public D_member_table()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("registerid", "hx_member_table"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int registerid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_member_table");
			strSql.Append(" where registerid=@registerid");
			SqlParameter[] parameters = {
					new SqlParameter("@registerid", SqlDbType.Int,4)
			};
			parameters[0].Value = registerid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回在于0登录成功,0登录失败</returns>
        public int CheckLogin(string username, string password)
        {
            try
            {
	            int t = 0;
	            string sql = "select registerid ,username,password,userstate  from hx_member_table where  username ='" + username + "'  or  mobile ='" + username + "' ";
	            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
	            if (dt.Rows.Count == 1)
	            {
	                if (dt.Rows[0]["userstate"].ToString() == "1")
	                {
	                    t = -100;
	                }
	                else
	                {
	                    if (dt.Rows[0]["password"].ToString() == password)
	                    {
	                        t = int.Parse(dt.Rows[0]["registerid"].ToString());
	                    }
	                    else
	                    {
	                        t = 0;
	                    }
	                }
	            }
	            return t;
	        }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            return 0;
        }


        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_member_table model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_member_table(");
            strSql.Append("username,password,mobile,email,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes,invitedcode,Channelsource,Tid,channel_invitedcode)");
			strSql.Append(" values (");
            strSql.Append("@username,@password,@mobile,@email,@realname,@iD_number,@transactionpassword,@istransactionpassword,@ismobile,@isrealname,@isbankcard,@isemail,@userstate,@account_total_assets,@available_balance,@collect_total_amount,@frozen_sum,@open_tonto_account,@tonto_account_user,@usertypes,@invitedcode,@Channelsource,@Tid,@channel_invitedcode)");
            strSql.Append(";select ident_current('hx_member_table')");
            //strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@username", SqlDbType.NVarChar,50),
                    new SqlParameter("@password", SqlDbType.NVarChar,50),
                    new SqlParameter("@mobile", SqlDbType.VarChar,20),
                    new SqlParameter("@email", SqlDbType.NVarChar,50),
                    new SqlParameter("@realname", SqlDbType.NVarChar,30),
                    new SqlParameter("@iD_number", SqlDbType.VarChar,30),
                    new SqlParameter("@transactionpassword", SqlDbType.NVarChar,50),
                    new SqlParameter("@istransactionpassword", SqlDbType.Int,4),
                    new SqlParameter("@ismobile", SqlDbType.Int,4),
                    new SqlParameter("@isrealname", SqlDbType.Int,4),
                    new SqlParameter("@isbankcard", SqlDbType.Int,4),
                    new SqlParameter("@isemail", SqlDbType.Int,4),
                    new SqlParameter("@userstate", SqlDbType.Int,4),
                    new SqlParameter("@account_total_assets", SqlDbType.Decimal,17),
                    new SqlParameter("@available_balance", SqlDbType.Decimal,17),
                    new SqlParameter("@collect_total_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@frozen_sum", SqlDbType.Decimal,17),
                    new SqlParameter("@open_tonto_account", SqlDbType.Int,4),
                    new SqlParameter("@tonto_account_user", SqlDbType.VarChar,30),
                    new SqlParameter("@usertypes",SqlDbType.Int,4),
                    new SqlParameter("@invitedcode", SqlDbType.VarChar,30),
                    new SqlParameter("@Channelsource", SqlDbType.Int,4),
                    new SqlParameter("@Tid", SqlDbType.VarChar,300),
                    new SqlParameter("@channel_invitedcode", SqlDbType.NVarChar,20)};

            
			parameters[0].Value = model.username;
			parameters[1].Value = model.password;
			parameters[2].Value = model.mobile;
			parameters[3].Value = model.email;
			parameters[4].Value = model.realname;
			parameters[5].Value = model.iD_number;
			parameters[6].Value = model.transactionpassword;
			parameters[7].Value = model.istransactionpassword;
			parameters[8].Value = model.ismobile;
			parameters[9].Value = model.isrealname;
			parameters[10].Value = model.isbankcard;
			parameters[11].Value = model.isemail;
			parameters[12].Value = model.userstate;
			parameters[13].Value = model.account_total_assets;
			parameters[14].Value = model.available_balance;
			parameters[15].Value = model.collect_total_amount;
			parameters[16].Value = model.frozen_sum;
			parameters[17].Value = model.open_tonto_account;
			parameters[18].Value = model.tonto_account_user;
            parameters[19].Value = model.usertypes;
            parameters[20].Value = model.invitedcode;
            parameters[21].Value = model.Channelsource;
            parameters[22].Value = model.Tid;
            parameters[23].Value = model.channel_invitedcode;

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
        /// 获取部分自定义字段的会员信息
        /// </summary>
        /// <param name="registerid"></param>
        /// <returns></returns>
        public PartialMemberModel GetPartialModel(int registerid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 registerid,registration_time,isrealname  from hx_member_table ");
            strSql.Append(" where registerid=@registerid");
            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.Int,4)
            };
            parameters[0].Value = registerid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataHelper.GetEntity<PartialMemberModel>(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_member_table model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_member_table set ");
			strSql.Append("username=@username,");
			strSql.Append("password=@password,");
			strSql.Append("mobile=@mobile,");
			strSql.Append("email=@email,");
			strSql.Append("realname=@realname,");
			strSql.Append("iD_number=@iD_number,");
			strSql.Append("transactionpassword=@transactionpassword,");
			strSql.Append("istransactionpassword=@istransactionpassword,");
			strSql.Append("ismobile=@ismobile,");
			strSql.Append("isrealname=@isrealname,");
			strSql.Append("isbankcard=@isbankcard,");
			strSql.Append("isemail=@isemail,");
			strSql.Append("userstate=@userstate,");
			strSql.Append("account_total_assets=@account_total_assets,");
			strSql.Append("available_balance=@available_balance,");
			strSql.Append("collect_total_amount=@collect_total_amount,");
			strSql.Append("frozen_sum=@frozen_sum,");
			strSql.Append("open_tonto_account=@open_tonto_account,");
			strSql.Append("tonto_account_user=@tonto_account_user,");
            strSql.Append("usertypes=@usertypes,");
            
			strSql.Append(" where registerid=@registerid");
			SqlParameter[] parameters = {
					new SqlParameter("@username", SqlDbType.NVarChar,50),
					new SqlParameter("@password", SqlDbType.NVarChar,50),
					new SqlParameter("@mobile", SqlDbType.VarChar,20),
					new SqlParameter("@email", SqlDbType.NVarChar,50),
					new SqlParameter("@realname", SqlDbType.NVarChar,30),
					new SqlParameter("@iD_number", SqlDbType.VarChar,30),
					new SqlParameter("@transactionpassword", SqlDbType.NVarChar,50),
					new SqlParameter("@istransactionpassword", SqlDbType.Int,4),
					new SqlParameter("@ismobile", SqlDbType.Int,4),
					new SqlParameter("@isrealname", SqlDbType.Int,4),
					new SqlParameter("@isbankcard", SqlDbType.Int,4),
					new SqlParameter("@isemail", SqlDbType.Int,4),
					new SqlParameter("@userstate", SqlDbType.Int,4),
					new SqlParameter("@account_total_assets", SqlDbType.Decimal,17),
					new SqlParameter("@available_balance", SqlDbType.Decimal,17),
					new SqlParameter("@collect_total_amount", SqlDbType.Decimal,17),
					new SqlParameter("@frozen_sum", SqlDbType.Decimal,17),
					new SqlParameter("@open_tonto_account", SqlDbType.Int,4),
					new SqlParameter("@tonto_account_user", SqlDbType.VarChar,30),
                    new SqlParameter("@usertypes", SqlDbType.Int,4),                    
					new SqlParameter("@registerid", SqlDbType.Int,4)};
			parameters[0].Value = model.username;
			parameters[1].Value = model.password;
			parameters[2].Value = model.mobile;
			parameters[3].Value = model.email;
			parameters[4].Value = model.realname;
			parameters[5].Value = model.iD_number;
			parameters[6].Value = model.transactionpassword;
			parameters[7].Value = model.istransactionpassword;
			parameters[8].Value = model.ismobile;
			parameters[9].Value = model.isrealname;
			parameters[10].Value = model.isbankcard;
			parameters[11].Value = model.isemail;
			parameters[12].Value = model.userstate;
			parameters[13].Value = model.account_total_assets;
			parameters[14].Value = model.available_balance;
			parameters[15].Value = model.collect_total_amount;
			parameters[16].Value = model.frozen_sum;
			parameters[17].Value = model.open_tonto_account;
			parameters[18].Value = model.tonto_account_user;
            parameters[19].Value = model.usertypes;
			parameters[20].Value = model.registerid;

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
		public bool Delete(int registerid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_member_table ");
			strSql.Append(" where registerid=@registerid");
			SqlParameter[] parameters = {
					new SqlParameter("@registerid", SqlDbType.Int,4)
			};
			parameters[0].Value = registerid;

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
		public bool DeleteList(string registeridlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_member_table ");
			strSql.Append(" where registerid in ("+registeridlist + ")  ");
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
		public M_member_table GetModel(int registerid)
		{
			
			StringBuilder strSql=new StringBuilder();

            strSql.Append("select  top 1 registerid,username,registration_time,password,mobile,email,UsrCustId,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes,invitedcode,UsrId,useridentity,Channelsource,Tid,lastlogintime,lastloginIP,registration_time,LostInvitation,channel_invitedcode from hx_member_table ");
			strSql.Append(" where registerid=@registerid");
			SqlParameter[] parameters = {
					new SqlParameter("@registerid", SqlDbType.Int,4)
			};
			parameters[0].Value = registerid;

			M_member_table model=new M_member_table();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
                return DataHelper.GetEntity<M_member_table>(ds.Tables[0]);
				//return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_member_table GetModel(string username)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 registerid,username,registration_time,password,mobile,email,UsrCustId,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes,invitedcode,UsrId,useridentity,Channelsource,Tid,lastlogintime,lastloginIP,registration_time,LostInvitation from hx_member_table ");
            strSql.Append(" where username=@username  or  mobile=@username");
            SqlParameter[] parameters = {
                    new SqlParameter("@username", SqlDbType.VarChar,200)
            };
            parameters[0].Value = username;

            M_member_table model = new M_member_table();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体,根据汇付客户号
        /// </summary>
        public M_member_table GetModelByUsrCustid(string usrCustid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 registerid,username,registration_time,password,mobile,email,UsrCustId,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes,invitedcode,UsrId,useridentity,Channelsource,Tid,lastlogintime,lastloginIP,registration_time,LostInvitation from hx_member_table ");
            strSql.Append(" where UsrCustId=@UsrCustId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsrCustId", SqlDbType.VarChar,16)
            };
            parameters[0].Value = usrCustid;
            M_member_table model = new M_member_table();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
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
        public M_member_table DataRowToModel(DataRow row)
		{
			M_member_table model=new M_member_table();
			if (row != null)
			{
				if(row["registerid"]!=null && row["registerid"].ToString()!="")
				{
					model.registerid=int.Parse(row["registerid"].ToString());
				}
				if(row["username"]!=null)
				{
					model.username=row["username"].ToString();
				}
				if(row["password"]!=null)
				{
					model.password=row["password"].ToString();
				}
				if(row["mobile"]!=null)
				{
					model.mobile=row["mobile"].ToString();
				}

                if (row["UsrCustId"] != null)
				{
                    model.UsrCustId = row["UsrCustId"].ToString();
				}
                                

				if(row["email"]!=null)
				{
					model.email=row["email"].ToString();
				}
				if(row["realname"]!=null)
				{
					model.realname=row["realname"].ToString();
				}
				if(row["iD_number"]!=null)
				{
					model.iD_number=row["iD_number"].ToString();
				}
				if(row["transactionpassword"]!=null)
				{
					model.transactionpassword=row["transactionpassword"].ToString();
				}
				if(row["istransactionpassword"]!=null && row["istransactionpassword"].ToString()!="")
				{
					model.istransactionpassword=int.Parse(row["istransactionpassword"].ToString());
				}
				if(row["ismobile"]!=null && row["ismobile"].ToString()!="")
				{
					model.ismobile=int.Parse(row["ismobile"].ToString());
				}
				if(row["isrealname"]!=null && row["isrealname"].ToString()!="")
				{
					model.isrealname=int.Parse(row["isrealname"].ToString());
				}
				if(row["isbankcard"]!=null && row["isbankcard"].ToString()!="")
				{
					model.isbankcard=int.Parse(row["isbankcard"].ToString());
				}
				if(row["isemail"]!=null && row["isemail"].ToString()!="")
				{
					model.isemail=int.Parse(row["isemail"].ToString());
				}
				if(row["userstate"]!=null && row["userstate"].ToString()!="")
				{
					model.userstate=int.Parse(row["userstate"].ToString());
				}
				if(row["account_total_assets"]!=null && row["account_total_assets"].ToString()!="")
				{
					model.account_total_assets=decimal.Parse(row["account_total_assets"].ToString());
				}
				if(row["available_balance"]!=null && row["available_balance"].ToString()!="")
				{
					model.available_balance=decimal.Parse(row["available_balance"].ToString());
				}
				if(row["collect_total_amount"]!=null && row["collect_total_amount"].ToString()!="")
				{
					model.collect_total_amount=decimal.Parse(row["collect_total_amount"].ToString());
				}
				if(row["frozen_sum"]!=null && row["frozen_sum"].ToString()!="")
				{
					model.frozen_sum=decimal.Parse(row["frozen_sum"].ToString());
				}
				if(row["open_tonto_account"]!=null && row["open_tonto_account"].ToString()!="")
				{
					model.open_tonto_account=int.Parse(row["open_tonto_account"].ToString());
				}
				if(row["tonto_account_user"]!=null)
				{
					model.tonto_account_user=row["tonto_account_user"].ToString();
				}

                if (row["usertypes"] != null)
                {
                    model.usertypes = int.Parse(row["usertypes"].ToString());
                }

                if (row["invitedcode"] != null)
				{
                    model.invitedcode = row["invitedcode"].ToString();
				}


                if (row["UsrId"] != null)
				{
                    model.UsrId = row["UsrId"].ToString();
				}

                if (row["useridentity"] != null)
                {
                    model.useridentity = int.Parse(row["useridentity"].ToString());
                }

                if (row["Channelsource"] != null && row["Channelsource"].ToString() != "")
                {
                    model.Channelsource = int.Parse(row["Channelsource"].ToString());
                }

                if (row["Tid"] != null)
                {
                    model.Tid = row["Tid"].ToString();
                }
                if (row["registration_time"] != null)
                {
                    model.CreatedOn = row["registration_time"].ToString();
                }

                if (row["lastlogintime"] != null)
                {
                    model.lastlogintime = Convert.ToDateTime(string.IsNullOrEmpty(row["lastlogintime"].ToString()) ? "2016-1-1" : row["lastlogintime"].ToString());
                }

                if (row["lastloginIP"] != null)
                {
                    model.lastloginIP = row["lastloginIP"].ToString();
                }
                if (row["registration_time"] != null)
                {
                    model.Registration_time = Convert.ToDateTime(string.IsNullOrEmpty(row["registration_time"].ToString()) ? "2016-1-1" : row["registration_time"].ToString());
                }
                if (row["LostInvitation"] != null && row["LostInvitation"].ToString() != "")
                {
                    model.LostInvitation = int.Parse(row["LostInvitation"].ToString());
                }

                if (row["registration_time"] != null)
                {
                    model.Registration_time = Convert.ToDateTime(string.IsNullOrEmpty(row["registration_time"].ToString()) ? "2016-9-10" : row["registration_time"].ToString());
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
			strSql.Append("select registerid,username,password,mobile,email,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes ");
			strSql.Append(" FROM hx_member_table ");
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
            strSql.Append(" registerid,username,password,mobile,email,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes ");
			strSql.Append(" FROM hx_member_table ");
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
			strSql.Append("select count(1) FROM hx_member_table ");
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
				strSql.Append("order by T.registerid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_member_table T ");
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
			parameters[0].Value = "hx_member_table";
			parameters[1].Value = "registerid";
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

