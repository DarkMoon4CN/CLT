using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ChuanglitouP2P.Common
{
    #region 数据库操作类
    public class YY_Sql
    {
        SqlCommand L_Comm;
        protected int iState = 0;
        /// <summary>
        /// 构造函数，初始化数据库连接对象
        /// </summary>
        public YY_Sql()
        {
        }



        public static SqlConnection createCon()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString());
        }



        /// <summary>
        /// 定义一个结构体(设置存储过程中的参数的基本属性)
        /// </summary>
        public struct YX_ProceducreParameter
        {
            public string L_strPName;//参数名
            public SqlDbType L_dbtPType;//参数数据类型
            public int L_iPSize;//参数长度
            public object L_obPValue;//参数值
            public ParameterDirection L_pdDirection;//参数是否为输出
        }
        /// <summary>
        /// 执行传入的存储过程名，返回SqlDataReader对象
        /// </summary>
        /// <param name="strProName">存储过程名称</param>
        /// <param name="stProParameters">存储过程变量</param>
        /// <param name="DR">返回的SqlDataReader对象</param>
        /// <returns></returns>
        public SqlDataReader YX_PROCEDURE_DataReader(string strProName, YX_ProceducreParameter[] stProParameters)
        {
            SqlConnection con = createCon();
            con.Open();
            SqlDataReader DR = null;
            try
            {
                L_Comm = new SqlCommand(strProName, con);//SqlCommand L_Comm;
                L_Comm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < stProParameters.Length; ++i)//注意不是“i++”！
                {
                    YX_ProceducreParameter stProPar = stProParameters[i];
                    SqlParameter sp = new SqlParameter(stProPar.L_strPName, stProPar.L_dbtPType, stProPar.L_iPSize);
                    sp.Direction = stProPar.L_pdDirection;
                    sp.Value = stProPar.L_obPValue;
                    L_Comm.Parameters.Add(sp);
                }
                DR = L_Comm.ExecuteReader(CommandBehavior.CloseConnection);
                iState = 0;
            }
            catch
            {
                iState = -1;
            }
            con.Close();
            return DR;
        }
        /// <summary>
        /// 执行存储过程操作,没有返回结果
        /// </summary>
        /// <param name="strProName">存储过程名称</param>
        /// <param name="stProParameters">存储过程变量</param>
        /// <returns></returns>
        public int YX_PROCEDURE_NonQuery(string strProName, YX_ProceducreParameter[] stProParameters)
        {
            SqlConnection con = createCon();
            con.Open();
            try
            {
                L_Comm = new SqlCommand(strProName, con);
                L_Comm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < stProParameters.Length; i++)
                {
                    YX_ProceducreParameter stProPar = stProParameters[i];
                    SqlParameter sp = new SqlParameter(stProPar.L_strPName, stProPar.L_dbtPType, stProPar.L_iPSize);
                    sp.Direction = stProPar.L_pdDirection;
                    sp.Value = stProPar.L_obPValue;
                    L_Comm.Parameters.Add(sp);
                }
                L_Comm.ExecuteNonQuery();
                iState = 0;
                // L_Comm.Dispose();
            }
            catch
            {
                iState = -1;
            }
            finally
            {
                con.Close();
            }
            return iState;
        }
        /// <summary>
        /// 执行传入的存储过程名和参数结构体,返回DataSet对象
        /// </summary>
        /// <param name="strProName"></param>
        /// <param name="stProParameters"></param>
        /// <returns></returns>
        public DataSet YX_PROCEDURE_DataSet(string strProName, YX_ProceducreParameter[] stProParameters)
        {
            SqlConnection con = createCon();
            con.Open();
            DataSet ds = new DataSet();
            L_Comm = new SqlCommand(strProName, con);
            L_Comm.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < stProParameters.Length; ++i)//注意，不是“i++”！
            {
                YX_ProceducreParameter stProPar = stProParameters[i];
                SqlParameter sp = new SqlParameter(stProPar.L_strPName, stProPar.L_dbtPType, stProPar.L_iPSize);
                sp.Direction = stProPar.L_pdDirection;
                sp.Value = stProPar.L_obPValue;
                L_Comm.Parameters.Add(sp);
            }
            SqlDataAdapter da = new SqlDataAdapter(L_Comm);
            try
            {
                da.Fill(ds);
            }
            catch
            {

            }
            finally
            {
                da.Dispose();
                con.Close();

            }
            return ds;
        }
        /// <summary>
        /// 通过表名和ID的值进行查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="ID">ID的值</param>
        /// <param name="tab">需要查询的表名</param>
        /// <returns></returns>
        public SqlDataReader Re_dr(int ID, string tab)
        {
            SqlConnection con = createCon();
            con.Open();
            string sql = "select * from " + tab + " where ID=" + ID;
            SqlCommand Comm = new SqlCommand(sql, con);
            SqlDataReader Dr = Comm.ExecuteReader(CommandBehavior.CloseConnection);



            return Dr;
        }


        public SqlDataReader ExecuteReader(string query)
        {
            SqlConnection con = createCon();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            if (query.StartsWith("SELECT") | query.StartsWith("select"))
            {
                cmd.CommandType = CommandType.Text;
            }
            else
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception ee)
            {
                con.Close();
                throw ee;
            }

        }
        /// <summary>
        /// 执行传入的SQL语句，返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader Re_dr(string sql)
        {
            SqlConnection con = createCon();
            con.Open();
            SqlCommand Comm = new SqlCommand(sql, con);
            SqlDataReader Dr = Comm.ExecuteReader(CommandBehavior.CloseConnection);


            return Dr;

        }
        /// <summary>
        /// 返回查询结果的数据集
        /// </summary>
        /// <param name="ZSStr"></param>
        /// <returns></returns>
        public DataSet CreatDataSet(string ZSStr)
        {
            SqlConnection con = createCon();
            con.Open();
            SqlDataAdapter sdr = new SqlDataAdapter(ZSStr, con);
            DataSet ds = new DataSet();
            sdr.Fill(ds);
            sdr.Dispose();
            con.Close();
            return ds;
        }
        /// <summary>
        /// 返回执行结果的首行首列字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Re_String(string sql)
        {
            string result = "";
            SqlConnection con = createCon();
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            result = Convert.ToString(cmd.ExecuteScalar());
            con.Close();

            return result;
        }
        /// <summary>
        /// 执行SQL语句，不返回结果
        /// </summary>
        /// <param name="sql"></param>
        public void YX_ExecSql(string sql)
        {

            SqlConnection con = createCon();
            con.Open();
            try
            {
                SqlCommand Comm_S = new SqlCommand(sql, con);
                Comm_S.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                HttpContext.Current.Response.Write(ee.Message);
            }
            finally
            {
                con.Close();

            }
        }
        /// <summary>
        /// 执行SQL语句，不返回结果
        /// </summary>
        /// <param name="sql"></param>
        public void Executesql(string sql)
        {
            SqlConnection con = createCon();
            con.Open();
            try
            {
                SqlCommand Comm_S = new SqlCommand(sql, con);
                Comm_S.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                HttpContext.Current.Response.Write(ee.Message);
            }
            finally
            {
                con.Close();

            }
        }
        /// <summary>
        /// 返回执行SQL语句的结果是否成功
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExSql(string sql)
        {
            SqlConnection con = createCon();
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();

            }
        }
        /// <summary>
        /// 返回SQL语句执行结果的首行首列（此函数返回结果为数字）
        /// </summary>
        /// <param name="sql"></param>
        public int YX_Execint(string sql)
        {
            SqlConnection con = createCon();
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
            catch (Exception ee)
            {
                HttpContext.Current.Response.Write(ee.ToString());
            }
            finally
            {
                con.Close();

            }
            return num;
        }
        /// <summary>
        /// 返回SQL语句执行结果的首行首列（此函数返回结果为object对象）
        /// </summary>
        /// <param name="sql"></param>
        public object YX_GetObj(string sql)
        {
            SqlConnection con = createCon();
            con.Open();
            object obj = null;
            try
            {
                SqlCommand Comm_S = new SqlCommand(sql, con);
                obj = Comm_S.ExecuteScalar();
            }
            catch (Exception ee)
            {
                HttpContext.Current.Response.Write(ee.ToString());
            }
            finally
            {
                con.Close();
            }
            return obj;
        }


        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlConnection con = YY_Sql.createCon();
            con.Open();
            try
            {
                sda.SelectCommand = new SqlCommand(sql, con);
                DataSet ds = new DataSet();
                sda.Fill(ds, "Order1List");
                dt = ds.Tables["Order1List"];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                sda.Dispose();
                con.Close();
            }
            return dt;
        }

    }
    #endregion
}
