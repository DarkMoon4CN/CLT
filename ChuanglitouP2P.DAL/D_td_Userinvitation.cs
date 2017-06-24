
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
    /// <summary>
    /// 数据访问类:td_Userinvitation
    /// </summary>
    public partial class D_td_Userinvitation
    {
        public D_td_Userinvitation()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("invitationid", "hx_td_Userinvitation");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int invitationid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from hx_td_Userinvitation");
            strSql.Append(" where invitationid=@invitationid");
            SqlParameter[] parameters = {
                    new SqlParameter("@invitationid", SqlDbType.Int,4)
            };
            parameters[0].Value = invitationid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_td_Userinvitation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_td_Userinvitation(");
            strSql.Append("invcode,invtime,invpersonid,Invpeopleid,InvitesStates,Invitereward,UserAct)");
            strSql.Append(" values (");
            strSql.Append("@invcode,@invtime,@invpersonid,@Invpeopleid,@InvitesStates,@Invitereward,@UserAct)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@invcode", SqlDbType.VarChar,50),
                    new SqlParameter("@invtime", SqlDbType.DateTime),
                    new SqlParameter("@invpersonid", SqlDbType.Int,4),
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4),
                    new SqlParameter("@InvitesStates", SqlDbType.Int,4),
                    new SqlParameter("@Invitereward", SqlDbType.Decimal,5),
                    new SqlParameter("@UserAct", SqlDbType.Int,4)
            };
            parameters[0].Value = model.invcode;
            parameters[1].Value = model.invtime;
            parameters[2].Value = model.invpersonid;
            parameters[3].Value = model.Invpeopleid;
            parameters[4].Value = model.InvitesStates;
            parameters[5].Value = model.Invitereward;
            parameters[6].Value = model.UserAct;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(M_td_Userinvitation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hx_td_Userinvitation set ");
            strSql.Append("invcode=@invcode,");
            strSql.Append("invtime=@invtime,");
            strSql.Append("invpersonid=@invpersonid,");
            strSql.Append("Invpeopleid=@Invpeopleid,");
            strSql.Append("InvitesStates=@InvitesStates,");
            strSql.Append("Invitereward=@Invitereward");
            strSql.Append(" where invitationid=@invitationid");
            SqlParameter[] parameters = {
                    new SqlParameter("@invcode", SqlDbType.VarChar,50),
                    new SqlParameter("@invtime", SqlDbType.DateTime),
                    new SqlParameter("@invpersonid", SqlDbType.Int,4),
                    new SqlParameter("@Invpeopleid", SqlDbType.Int,4),
                    new SqlParameter("@InvitesStates", SqlDbType.Int,4),
                    new SqlParameter("@Invitereward", SqlDbType.Decimal,5),
                    new SqlParameter("@invitationid", SqlDbType.Int,4)};
            parameters[0].Value = model.invcode;
            parameters[1].Value = model.invtime;
            parameters[2].Value = model.invpersonid;
            parameters[3].Value = model.Invpeopleid;
            parameters[4].Value = model.InvitesStates;
            parameters[5].Value = model.Invitereward;
            parameters[6].Value = model.invitationid;

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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int invitationid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_td_Userinvitation ");
            strSql.Append(" where invitationid=@invitationid");
            SqlParameter[] parameters = {
                    new SqlParameter("@invitationid", SqlDbType.Int,4)
            };
            parameters[0].Value = invitationid;

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
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string invitationidlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_td_Userinvitation ");
            strSql.Append(" where invitationid in (" + invitationidlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public M_td_Userinvitation GetModel(int invitationid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 invitationid,invcode,invtime,invpersonid,Invpeopleid,InvitesStates,Invitereward from hx_td_Userinvitation ");
            strSql.Append(" where invitationid=@invitationid");
            SqlParameter[] parameters = {
                    new SqlParameter("@invitationid", SqlDbType.Int,4)
            };
            parameters[0].Value = invitationid;

            M_td_Userinvitation model = new M_td_Userinvitation();
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
        public M_td_Userinvitation DataRowToModel(DataRow row)
        {
            M_td_Userinvitation model = new M_td_Userinvitation();
            if (row != null)
            {
                if (row["invitationid"] != null && row["invitationid"].ToString() != "")
                {
                    model.invitationid = int.Parse(row["invitationid"].ToString());
                }
                if (row["invcode"] != null)
                {
                    model.invcode = row["invcode"].ToString();
                }
                if (row["invtime"] != null && row["invtime"].ToString() != "")
                {
                    model.invtime = DateTime.Parse(row["invtime"].ToString());
                }
                if (row["invpersonid"] != null && row["invpersonid"].ToString() != "")
                {
                    model.invpersonid = int.Parse(row["invpersonid"].ToString());
                }
                if (row["Invpeopleid"] != null && row["Invpeopleid"].ToString() != "")
                {
                    model.Invpeopleid = int.Parse(row["Invpeopleid"].ToString());
                }
                if (row["InvitesStates"] != null && row["InvitesStates"].ToString() != "")
                {
                    model.InvitesStates = int.Parse(row["InvitesStates"].ToString());
                }
                if (row["Invitereward"] != null && row["Invitereward"].ToString() != "")
                {
                    model.Invitereward = decimal.Parse(row["Invitereward"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select invitationid,invcode,invtime,invpersonid,Invpeopleid,InvitesStates,Invitereward ");
            strSql.Append(" FROM hx_td_Userinvitation ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" invitationid,invcode,invtime,invpersonid,Invpeopleid,InvitesStates,Invitereward ");
            strSql.Append(" FROM hx_td_Userinvitation ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM hx_td_Userinvitation ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.invitationid desc");
            }
            strSql.Append(")AS Row, T.*  from hx_td_Userinvitation T ");
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
			parameters[0].Value = "hx_td_Userinvitation";
			parameters[1].Value = "invitationid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 获取邀请的好友，注册且投资的个数
        /// </summary>
        public int GetTotalCanUseCount(DateTime startTime, int registerID)
        {
            //获取邀请的好友，注册且投资的个数
            string strSql = @"
                        select count(distinct investor_registerid) from [hx_Bid_records] c
                        join hx_td_Userinvitation d
                        on c.investor_registerid = d.invpersonid
                        join [dbo].[hx_member_table] e
                        on c.investor_registerid = e.registerid
						join [hx_member_table] f
						on d.Invpeopleid = f.registerid
                        where e.registration_time > @StartTime and d.Invpeopleid = @registerID and c.ordstate != 0 and  f.useridentity != 4";
            SqlParameter[] parameters = {
                    new SqlParameter("@StartTime", SqlDbType.DateTime),
                    new SqlParameter("@registerID", SqlDbType.Int)
            };
            parameters[0].Value = startTime;
            parameters[1].Value = registerID;
            DataSet ds = DbHelperSQL.Query(strSql, parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        #endregion  ExtensionMethod
    }
}

