using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChuanglitouP2P.DBUtility;

namespace ChuanglitouP2P.DAL
{
   public class D_PublicPageList
    {
       public static string connectionString = PubConstant.ConnectionString; 


        /// <summary>
        /// 根据主键获取分页数据列表()
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strGetFields">需要返回的列</param>
        /// <param name="fldName">排序的字段名</param>
        /// <param name="PageSize">页尺寸</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="Sort">排序的方法</param>
        /// <returns>返回结果集</returns>
        public DataTable GetPageIndexListByPage(string tblName, string strGetFields, string fldName, int PageSize, int PageIndex, string strWhere, string Sort, out int RecordCount)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            //try
            //{
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandText = "GetPageIndex";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@tblName", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = tblName;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@strGetFields", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = strGetFields;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@fldName", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = fldName;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PageSize", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = PageSize;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PageIndex", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = PageIndex;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@strWhere", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = strWhere;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@Sort", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = Sort;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@RecordCount", SqlDbType.Int);
                para.Direction = ParameterDirection.Output;
                da.SelectCommand.Parameters.Add(para);

                da.Fill(ds, "pr1");
                dt = ds.Tables["pr1"];
                RecordCount = (Int32)da.SelectCommand.Parameters["@RecordCount"].Value;
            //}
            //catch (Exception e)
            //{
            //    // throw new Exception(e.ToString());
            //    throw new Exception(e.ToString());
            //}
            //finally
            //{
            //    //ds.Clear();
            //    da.Dispose();
            //    con.Close();
            //}
            return dt;

        }

        /// <summary>
        /// 根据存储过程获取分页数据列表
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strGetFields">需要返回的列</param>
        /// <param name="fldName">排序的字段名 多个用,隔开</param>
        /// <param name="PageSize">页尺寸</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="Sort">排序的方法</param>
        /// <returns>返回结果集</returns>
        public DataTable GetListByPage(string tblName, string strGetFields, string fldName, int PageSize, int PageIndex, string strWhere, out int RecordCount)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandText = "GetPageRecord";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@tblName", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = tblName;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@strGetFields", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = strGetFields;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@fldName", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = fldName;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PageSize", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = PageSize;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PageIndex", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = PageIndex;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@strWhere", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = strWhere;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@RecordCount", SqlDbType.Int);
                para.Direction = ParameterDirection.Output;
                da.SelectCommand.Parameters.Add(para);

                da.Fill(ds, "bbs");
                dt = ds.Tables["bbs"];
                RecordCount = (Int32)da.SelectCommand.Parameters["@RecordCount"].Value;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
            return dt;

        }

       /// <summary>
       /// 此过程序用分页排序支持多表 Group by 
       /// </summary>
        /// <param name="TableNames">表名，可以是多个表，但不能用别名</param>
        /// <param name="PrimaryKey">主键，可以为空，但@Order为空时该值不能为空</param>
        /// <param name="Fields">要取出的字段，可以是多个表的字段，可以为空，为空表示select *</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页，0表示第1页</param>
        /// <param name="FilterWhere">条件，可以为空，不用填 where</param>
        /// <param name="GroupBy">分组依据，可以为空，不用填 group by</param>
        /// <param name="Order">排序，可以为空，为空默认按主键升序排列，不用填 order by</param>
       /// <returns></returns>
        public DataTable GetListByPageGroupBy(string TableNames, string PrimaryKey, string Fields, int PageSize, int CurrentPage, string FilterWhere, string GroupBy, string Order)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                con.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandText = "usp_PagingLarge";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter("@TableNames", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = TableNames;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PrimaryKey", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = PrimaryKey;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@Fields", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = Fields;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@PageSize", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = PageSize;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@CurrentPage", SqlDbType.Int);
                para.Direction = ParameterDirection.Input;
                para.Value = CurrentPage;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@Filter", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = FilterWhere;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@Group", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = GroupBy;
                da.SelectCommand.Parameters.Add(para);

                para = new SqlParameter("@Order", SqlDbType.VarChar);
                para.Direction = ParameterDirection.Input;
                para.Value = Order;
                da.SelectCommand.Parameters.Add(para);

                da.Fill(ds, "PageGroupBy");
                dt = ds.Tables["PageGroupBy"];


            }catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ds.Dispose();
                da.Dispose();
                con.Close();
            }



            return dt;
        }










        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string RunSql(string sql)
        {
            string errorInfo = "";
            if (sql != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(sql.ToString(), con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", " ");
                    message = message.Replace("\\", "/");
                    message = message.Replace("\r\n", "\\r\\n");
                    message = message.Replace("\r", "\\r");
                    message = message.Replace("\n", "\\n");
                    errorInfo += message + "<br>";
                }
                con.Close();
            }
            return errorInfo;
        }

        /// <summary>
        /// 执行传入的SQL语句，返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader Re_dr(string sql)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand Comm = new SqlCommand(sql, con);
            SqlDataReader Dr = Comm.ExecuteReader();
            return Dr;
        }

        /// <summary>
        /// 返回SQL语句执行结果的首行首列（此函数返回结果为数字）
        /// </summary>
        /// <param name="sql"></param>
        public int Execint(string sql)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            int num = 0;
            try
            {
                SqlCommand Comm_S = new SqlCommand(sql, con);
                string str = Convert.ToString(Comm_S.ExecuteScalar());
                if (str == null || str == "")
                    num = 0;
                else
                    num = Convert.ToInt32(str.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();

            }
            return num;
        }

        /// <summary>
        /// 返回执行结果的首行首列字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Re_String(string sql)
        {
            string result = "";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            result = Convert.ToString(cmd.ExecuteScalar());
            con.Close();

            return result;
        }





        //public DataTable GetPageList(string tblName, string strGetFields, string fldName, string prKey, int PageSize, int PageIndex, string strWhere, string Sort, out int RecordCount)
        //{ 
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlDataAdapter da = new SqlDataAdapter();
           
        //    da.SelectCommand = new SqlCommand();
        //    da.SelectCommand.Connection = con;
        //    da.SelectCommand.CommandText = "procPagination";
        //    da.SelectCommand.CommandType = CommandType.StoredProcedure;

        //    SqlParameter para = new SqlParameter("@tblName", SqlDbType.VarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = tblName;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@strGetFields", SqlDbType.VarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = strGetFields;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@fldName", SqlDbType.NVarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = fldName;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@prKey", SqlDbType.NVarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = prKey;
        //    da.SelectCommand.Parameters.Add(para);


        //    para = new SqlParameter("@pageSize", SqlDbType.Int);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = PageSize;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@pageIndex", SqlDbType.Int);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = PageIndex;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@strWhere", SqlDbType.NVarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = strWhere;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@sort", SqlDbType.NVarChar);
        //    para.Direction = ParameterDirection.Input;
        //    para.Value = Sort;
        //    da.SelectCommand.Parameters.Add(para);

        //    para = new SqlParameter("@recordCount", SqlDbType.Int);
        //    para.Direction = ParameterDirection.Output;
        //    da.SelectCommand.Parameters.Add(para);

        //    da.Fill(ds, "pr1");
        //    dt = ds.Tables["pr1"];
        //    RecordCount = (Int32)da.SelectCommand.Parameters["@recordCount"].Value;
         
        //    return dt;
        //}
    }
}
