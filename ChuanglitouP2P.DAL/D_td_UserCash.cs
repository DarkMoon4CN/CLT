
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;

namespace ChuanglitouP2P.DAL
{
    /// <summary>
    /// 数据访问类:td_UserCash
    /// </summary>
    public partial class D_td_UserCash
    {
        public D_td_UserCash()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("UserCashId", "hx_td_UserCash");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserCashId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from hx_td_UserCash");
            strSql.Append(" where UserCashId=@UserCashId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserCashId", SqlDbType.Int,4)
            };
            parameters[0].Value = UserCashId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_td_UserCash model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_td_UserCash(");
            strSql.Append("registerid,UsrCustId,TransAmt,FeeAmt,OrdId,OrdIdTime,OrdIdState,Reason,Remarks,TransState,FeeObjFlag,CashChl)");
            strSql.Append(" values (");
            strSql.Append("@registerid,@UsrCustId,@TransAmt,@FeeAmt,@OrdId,@OrdIdTime,@OrdIdState,@Reason,@Remarks,@TransState,@FeeObjFlag,@CashChl)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.Int,4),
                    new SqlParameter("@UsrCustId", SqlDbType.VarChar,50),
                    new SqlParameter("@TransAmt", SqlDbType.Decimal,17),
                    new SqlParameter("@FeeAmt", SqlDbType.Decimal,17),
                    new SqlParameter("@OrdId", SqlDbType.VarChar,50),
                    new SqlParameter("@OrdIdTime", SqlDbType.DateTime),
                    new SqlParameter("@OrdIdState", SqlDbType.Int,4),

                    new SqlParameter("@Reason", SqlDbType.VarChar,500),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,500),
                    new SqlParameter("@TransState", SqlDbType.Int,4),
                    new SqlParameter("@FeeObjFlag", SqlDbType.VarChar,500),
                    new SqlParameter("@CashChl", SqlDbType.NVarChar,50)
                                        };
            parameters[0].Value = model.registerid;
            parameters[1].Value = model.UsrCustId;
            parameters[2].Value = model.TransAmt;
            parameters[3].Value = model.FeeAmt;
            parameters[4].Value = model.OrdId;
            parameters[5].Value = model.OrdIdTime;
            parameters[6].Value = model.OrdIdState;

            parameters[7].Value = model.Reason;
            parameters[8].Value = model.Remarks;
            parameters[9].Value = model.TransState;
            parameters[10].Value = model.FeeObjFlag;
            parameters[11].Value = model.CashChl;

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
        public bool Update(M_td_UserCash model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hx_td_UserCash set ");
            strSql.Append("registerid=@registerid,");
            strSql.Append("UsrCustId=@UsrCustId,");
            strSql.Append("TransAmt=@TransAmt,");
            strSql.Append("FeeAmt=@FeeAmt,");
            strSql.Append("OrdId=@OrdId,");
            strSql.Append("OrdIdTime=@OrdIdTime,");
            strSql.Append("OrdIdState=@OrdIdState,");
            strSql.Append("OperTime=@OperTime,");
            strSql.Append("Reason=@Reason,");
            strSql.Append("Remarks=@Remarks,");
            strSql.Append("TransState=@TransState,");
            strSql.Append("CashChl=@CashChl");
            strSql.Append(" where UserCashId=@UserCashId");
            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.Int,4),
                    new SqlParameter("@UsrCustId", SqlDbType.VarChar,50),
                    new SqlParameter("@TransAmt", SqlDbType.Decimal,17),
                    new SqlParameter("@FeeAmt", SqlDbType.Decimal,17),
                    new SqlParameter("@OrdId", SqlDbType.VarChar,50),
                    new SqlParameter("@OrdIdTime", SqlDbType.DateTime),
                    new SqlParameter("@OrdIdState", SqlDbType.Int,4),
                    new SqlParameter("@OperTime", SqlDbType.DateTime),
                    new SqlParameter("@Reason", SqlDbType.VarChar,500),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,500),
                    new SqlParameter("@TransState", SqlDbType.Int,4),
                    new SqlParameter("@UserCashId", SqlDbType.Int,4),
                    new SqlParameter("@CashChl", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.registerid;
            parameters[1].Value = model.UsrCustId;
            parameters[2].Value = model.TransAmt;
            parameters[3].Value = model.FeeAmt;
            parameters[4].Value = model.OrdId;
            parameters[5].Value = model.OrdIdTime;
            parameters[6].Value = model.OrdIdState;
            parameters[7].Value = model.OperTime;
            parameters[8].Value = model.Reason;
            parameters[9].Value = model.Remarks;
            parameters[10].Value = model.TransState;
            parameters[11].Value = model.UserCashId;
            parameters[12].Value = model.CashChl;

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
        public bool Delete(int UserCashId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_td_UserCash ");
            strSql.Append(" where UserCashId=@UserCashId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserCashId", SqlDbType.Int,4)
            };
            parameters[0].Value = UserCashId;

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
        public bool DeleteList(string UserCashIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hx_td_UserCash ");
            strSql.Append(" where UserCashId in (" + UserCashIdlist + ")  ");
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
        public M_td_UserCash GetModel(int UserCashId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from hx_td_UserCash ");
            strSql.Append(" where UserCashId=@UserCashId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserCashId", SqlDbType.Int,4)
            };
            parameters[0].Value = UserCashId;

            M_td_UserCash model = new M_td_UserCash();
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
        public M_td_UserCash DataRowToModel(DataRow row)
        {
            M_td_UserCash model = new M_td_UserCash();
            if (row != null)
            {
                if (row["UserCashId"] != null && row["UserCashId"].ToString() != "")
                {
                    model.UserCashId = int.Parse(row["UserCashId"].ToString());
                }
                if (row["registerid"] != null && row["registerid"].ToString() != "")
                {
                    model.registerid = int.Parse(row["registerid"].ToString());
                }
                if (row["UsrCustId"] != null)
                {
                    model.UsrCustId = row["UsrCustId"].ToString();
                }
                if (row["TransAmt"] != null && row["TransAmt"].ToString() != "")
                {
                    model.TransAmt = decimal.Parse(row["TransAmt"].ToString());
                }
                if (row["FeeAmt"] != null && row["FeeAmt"].ToString() != "")
                {
                    model.FeeAmt = decimal.Parse(row["FeeAmt"].ToString());
                }
                if (row["OrdId"] != null)
                {
                    model.OrdId = row["OrdId"].ToString();
                }
                if (row["OrdIdTime"] != null && row["OrdIdTime"].ToString() != "")
                {
                    model.OrdIdTime = DateTime.Parse(row["OrdIdTime"].ToString());
                }
                if (row["OrdIdState"] != null && row["OrdIdState"].ToString() != "")
                {
                    model.OrdIdState = int.Parse(row["OrdIdState"].ToString());
                }
                if (row["OperTime"] != null && row["OperTime"].ToString() != "")
                {
                    model.OperTime = DateTime.Parse(row["OperTime"].ToString());
                }
                if (row["Reason"] != null)
                {
                    model.Reason = row["Reason"].ToString();
                }
                if (row["Remarks"] != null)
                {
                    model.Remarks = row["Remarks"].ToString();
                }
                if (row["TransState"] != null && row["TransState"].ToString() != "")
                {
                    model.TransState = int.Parse(row["TransState"].ToString());
                }
                if (row["CashChl"] != null && row["CashChl"].ToString() != "")
                {
                    model.CashChl = row["CashChl"].ToString();
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
            strSql.Append("select UserCashId,registerid,UsrCustId,TransAmt,FeeAmt,OrdId,OrdIdTime,OrdIdState,OperTime,Reason,Remarks,TransState ");
            strSql.Append(" FROM hx_td_UserCash ");
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
            strSql.Append(" UserCashId,registerid,UsrCustId,TransAmt,FeeAmt,OrdId,OrdIdTime,OrdIdState,OperTime,Reason,Remarks,TransState ");
            strSql.Append(" FROM hx_td_UserCash ");
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
            strSql.Append("select count(1) FROM hx_td_UserCash ");
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
                strSql.Append("order by T.UserCashId desc");
            }
            strSql.Append(")AS Row, T.*  from hx_td_UserCash T ");
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
			parameters[0].Value = "hx_td_UserCash";
			parameters[1].Value = "UserCashId";
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

