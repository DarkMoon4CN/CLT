using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ChuangLiTou.Core.Helpers.Util;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{
    public class DbHelper
    {
        //���ݿ������ַ���(web.config������)	
        public static readonly string ConnectionString = Settings.Instance.ProjectDatabase;

        #region ���÷���

        /// <summary>
        /// �ж��Ƿ����ĳ���ĳ���ֶ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="columnName">������</param>
        /// <returns>�Ƿ����</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" +
                         columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        public static int GetMaxID(string fieldName, string tableName)
        {
            string strsql = "select max(" + fieldName + ")+1 from " + tableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            return int.Parse(obj.ToString());
        }

        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        }

        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool TabExists(string tableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + tableName +
                            "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U')";
            object obj = GetSingle(strsql);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        }

        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            if (cmdParms == null)
            {
                LoggerHelper.Error("cmdParms is null");
                return false;
            }
            var obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            return true;
        }

        #endregion
        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// Make stored procedure param.
        /// </summary>
        /// <param name="paramName">Name of param.</param>
        /// <param name="dbType">Param type.</param>
        /// <param name="size">Param size.</param>
        /// <param name="direction">Parm direction.</param>
        /// <param name="value">Param value.</param>
        /// <returns>New parameter.</returns>
        public static SqlParameter MakeParam(string paramName, SqlDbType dbType, Int32 size, ParameterDirection direction, object value)
        {
            SqlParameter param;

            if (size > 0)
                param = new SqlParameter(paramName, dbType, size);
            else
                param = new SqlParameter(paramName, dbType);

            param.Direction = direction;


            if (dbType == SqlDbType.DateTime && Convert.ToDateTime(value) == System.DateTime.MinValue)
            {
                value = null;
            }

            if (!(direction == ParameterDirection.Output && value == null))
            {
                if (value == null)
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = value;
                }
            }

            return param;
        }


        #region  ִ�м�SQL���

        /// <summary>
        /// ִ��SQL��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="sqlString">SQL���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSql(string sqlString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        var rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        LoggerHelper.Error("throw exception when execute Sql:" + sqlString + " connection string is:" + ConnectionString, e);

                        connection.Close();
                    }
                }
            }
            return 0;
        }

        public static int ExecuteSqlByTime(string sqlString, int times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        LoggerHelper.Error("throw exception when execute Sql by time:" + sqlString + " connection string is:" + ConnectionString, e);
                        connection.Close();
                        return 0;
                    }
                }
            }
        } /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// ִ��Sql��Oracle�λ������
        /// </summary>
        /// <param name="list">SQL�������б�</param>
        /// <param name="oracleCmdSqlList">Oracle�������б�</param>
        /// <returns>ִ�н�� 0-����SQL�������ʧ�� -1 ����Oracle�������ʧ�� 1-��������ִ�гɹ�</returns>
        public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand { Connection = conn };
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (var myDe in list)
                    {
                        string cmdText = myDe.CommandText;
                        var cmdParms = (SqlParameter[])myDe.Parameters;
                        PrepareCommand(cmd, conn, tx, cmdText, cmdParms);
                        if (myDe.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (myDe.CommandText.ToLower().IndexOf("count(", StringComparison.Ordinal) == -1)
                            {
                                tx.Rollback();
                                LoggerHelper.Error("Υ��Ҫ��" + myDe.CommandText + "�������select count(..�ĸ�ʽ");
                                //throw new Exception("Υ��Ҫ��" + myDe.CommandText + "�������select count(..�ĸ�ʽ");
                                return 0;
                            }

                            var obj = cmd.ExecuteScalar();
                            var isHave = Convert.ToInt32(obj) > 0;
                            if (isHave)
                            {
                                //�����¼�
                                myDe.OnSolicitationEvent();
                            }
                        }
                        if (myDe.EffentNextType == EffentNextType.WhenHaveContine ||
                            myDe.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (myDe.CommandText.ToLower().IndexOf("count(", StringComparison.Ordinal) == -1)
                            {
                                tx.Rollback();
                                LoggerHelper.Error("SQL:Υ��Ҫ��" + myDe.CommandText + "�������select count(..�ĸ�ʽ");

                                //throw new Exception("SQL:Υ��Ҫ��" + myDe.CommandText + "�������select count(..�ĸ�ʽ");
                                return 0;
                            }

                            object obj = cmd.ExecuteScalar();
                            bool isHave = Convert.ToInt32(obj) > 0;

                            if (myDe.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                            {
                                tx.Rollback();
                                LoggerHelper.Error("SQL:Υ��Ҫ��" + myDe.CommandText + "����ֵ�������0");

                                //throw new Exception("SQL:Υ��Ҫ��" + myDe.CommandText + "����ֵ�������0");
                                return 0;
                            }
                            if (myDe.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                            {
                                tx.Rollback();
                                LoggerHelper.Error("SQL:Υ��Ҫ��" + myDe.CommandText + "����ֵ�������0");

                                //throw new Exception("SQL:Υ��Ҫ��" + myDe.CommandText + "����ֵ�������0");
                                return 0;
                            }
                            continue;
                        }
                        int val = cmd.ExecuteNonQuery();
                        if (myDe.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                        {
                            tx.Rollback();
                            LoggerHelper.Error("SQL:Υ��Ҫ��" + myDe.CommandText + "������Ӱ����");

                            // throw new Exception("SQL:Υ��Ҫ��" + myDe.CommandText + "������Ӱ����");
                            return 0;
                        }
                        cmd.Parameters.Clear();
                    }

                    tx.Commit();
                    return 1;
                }
                catch (SqlException e)
                {
                    tx.Rollback();
                    LoggerHelper.Error("SqlException ִ�лع�������", e);
                    return 0;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    LoggerHelper.Error("Exception ִ�лع�������", e);
                    return 0;
                }
            }
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="sqlStringList">����SQL���</param>		
        public static int ExecuteSqlTran(List<String> sqlStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand { Connection = conn };
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    var count = 0;
                    foreach (var strsql in sqlStringList.Where(strsql => strsql.Trim().Length > 1))
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error("Exception��", ex);
                    tx.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
        /// </summary>
        /// <param name="sqlString">SQL���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSql(string sqlString, string content)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(sqlString, connection);
                var myParameter = new SqlParameter("@content",
                                                   SqlDbType.NText) { Value = content };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    LoggerHelper.Error("SqlException��" + sqlString, e);
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
        /// </summary>
        /// <param name="sqlString">SQL���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static object ExecuteSqlGet(string sqlString, string content)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(sqlString, connection);
                var myParameter = new SqlParameter("@content",
                                                   SqlDbType.NText) { Value = content };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (SqlException e)
                {
                    LoggerHelper.Error("SqlException��" + sqlString, e);
                    return null;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(strSQL, connection);
                var myParameter = new SqlParameter("@fs",
                                                   SqlDbType.Image) { Value = fs };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    LoggerHelper.Error("SqlException��" + strSQL, e);
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="sqlString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
        public static object GetSingle(string sqlString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        LoggerHelper.Error("SqlException��" + sqlString, e);
                        return null;
                    }
                }
            }
        }

        public static object GetSingle(string sqlString, int times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        object obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        LoggerHelper.Error("SqlException��" + sqlString, e);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
        /// </summary>
        /// <param name="strSQL">��ѯ���</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException e)
            {
                LoggerHelper.Error("SqlException��" + strSQL, e);

                return null;
            }
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="sqlString">��ѯ���</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    //LoggerHelper.Info(sqlString);
                    command.Fill(ds, "R");
                }
                catch (SqlException ex)
                {

                    LoggerHelper.Error("sql:" + sqlString + " SqlException��", ex);
                    return null;
                }
                return ds;
            }
        }
        public static DataSet Query(string sqlString, string conn)
        {
            using (var connection = new SqlConnection(conn))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    //LoggerHelper.Info(sqlString);
                    command.Fill(ds, "R");
                }
                catch (SqlException ex)
                {

                    LoggerHelper.Error("sql:" + sqlString + " SqlException��", ex);
                    return null;
                }
                return ds;
            }
        }

        public static DataSet Query(string sqlString, int times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.SelectCommand.CommandTimeout = times;
                    command.Fill(ds, "ds");
                    //LoggerHelper.Info(sqlString);
                }
                catch (SqlException ex)
                {
                    LoggerHelper.Error("SqlException��" + sqlString, ex);
                    return null;
                }
                return ds;
            }
        }

        #endregion

        #region ִ�д�������SQL���

        /// <summary>
        /// ִ��SQL��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="sqlString">SQL���</param>
        /// <param name="cmdParms"> </param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSql(string sqlString, params SqlParameter[] cmdParms)
        {
            if (cmdParms == null)
            {
                LoggerHelper.Error("cmdParms is null");
                return 0;
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        //LoggerHelper.Info(sqlString);
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        LoggerHelper.Error("SqlException:", e);
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="sqlStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
        public static void ExecuteSqlTran(Hashtable sqlStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //LoggerHelper.Info(sqlStringList);
                        //ѭ��
                        foreach (DictionaryEntry myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            var cmdParms = (SqlParameter[])myDe.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        LoggerHelper.Error("ExecuteSqlTran Exception:", e);
                    }
                }
            }
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="cmdList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //LoggerHelper.Info(cmdList);
                        int count = 0;
                        //ѭ��
                        foreach (CommandInfo myDe in cmdList)
                        {
                            string cmdText = myDe.CommandText;
                            var cmdParms = (SqlParameter[])myDe.Parameters;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);

                            if (myDe.EffentNextType == EffentNextType.WhenHaveContine ||
                                myDe.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDe.CommandText.ToLower().IndexOf("count(", StringComparison.Ordinal) == -1)
                                {
                                    trans.Rollback();
                                    LoggerHelper.Error("public static int ExecuteSqlTran(List<CommandInfo> cmdList) throw exception,trans.Rollback:" + myDe.CommandText);
                                    return 0;
                                }

                                object obj = cmd.ExecuteScalar();
                                bool isHave = Convert.ToInt32(obj) > 0;

                                if (myDe.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    LoggerHelper.Error("public static int ExecuteSqlTran(List<CommandInfo> cmdList) throw exception,trans.Rollback:" + myDe.EffentNextType);

                                    return 0;
                                }
                                if (myDe.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    LoggerHelper.Error("public static int ExecuteSqlTran(List<CommandInfo> cmdList) throw exception,trans.Rollback:" + myDe.EffentNextType);

                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDe.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                LoggerHelper.Error("public static int ExecuteSqlTran(List<CommandInfo> cmdList) throw exception,trans.Rollback:" + myDe.EffentNextType);
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        LoggerHelper.Error("public static int ExecuteSqlTran(List<CommandInfo> cmdList) throw exception,trans.Rollback:", e);
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="sqlStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> sqlStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //LoggerHelper.Info(sqlStringList);
                        int indentity = 0;
                        //ѭ��
                        foreach (var myDe in sqlStringList)
                        {
                            string cmdText = myDe.CommandText;
                            var cmdParms = (SqlParameter[])myDe.Parameters;
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        LoggerHelper.Error(" public static void ExecuteSqlTranWithIndentity(List<CommandInfo> sqlStringList) throw exception,trans.Rollback:", e);
                    }
                }
            }
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="sqlStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable sqlStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //LoggerHelper.Info(sqlStringList);
                        int indentity = 0;
                        //ѭ��
                        foreach (DictionaryEntry myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            var cmdParms = (SqlParameter[])myDe.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="sqlString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
        public static object GetSingle(string sqlString, params SqlParameter[] cmdParms)
        {
            if (cmdParms == null)
            {
                LoggerHelper.Error(" public static object GetSingle cmdParms is null");
                return null;
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                       // LoggerHelper.Info(sqlString);
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        LoggerHelper.Error(" public static object GetSingle SqlException��" + sqlString, e);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
        /// </summary>
        /// <param name="sqlString">��ѯ���</param>
        /// <param name="cmdParms"> </param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlString, params SqlParameter[] cmdParms)
        {
            if (cmdParms == null)
            {
                LoggerHelper.Error(" public static object ExecuteReader cmdParms is null");
                return null;
            }
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand();
            try
            {
               // LoggerHelper.Info(sqlString);
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
            {
                LoggerHelper.Error(" public static object ExecuteReader SqlException:" + sqlString, e);
                return null;
            }
            //			finally
            //			{
            //				cmd.Dispose();
            //				connection.Close();
            //			}	
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="sqlString">��ѯ���</param>
        /// <param name="cmdParms"> </param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString, params SqlParameter[] cmdParms)
        {
            if (cmdParms == null)
            {
                LoggerHelper.Error("  public static DataSet Query cmdParms is null");
                return null;
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                //LoggerHelper.Info(sqlString);
                var cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        LoggerHelper.Error("  public static DataSet Query SqlException��" + sqlString, ex);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText,
                                           IEnumerable<SqlParameter> cmdParms)
        {

            if (cmdParms == null)
            {
                LoggerHelper.Warning("private static void PrepareCommand cmdParms is null");

            }
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text; //cmdType; 
            if (cmdParms != null)
            {
                foreach (var parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region �洢���̲���

        /// <summary>
        /// ִ�д洢���̣����ش洢����return ��ֵ 
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>��ȡ�洢����return ��ֵ</returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            try
            {

                //LoggerHelper.Info("run procedure ��" + storedProcName);
                var connection = new SqlConnection(ConnectionString);
                connection.Open();
                var command = BuildQueryCommand(connection, storedProcName, parameters);

                command.Parameters.Add(new SqlParameter("ReturnValue",
                                                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                                                false, 0, 0, string.Empty, DataRowVersion.Default, null));
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                var result = (int)command.Parameters["ReturnValue"].Value;
         
                return result; 
            }
            catch (Exception e)
            {
                LoggerHelper.Error("public static int RunProcedure(string storedProcName, IDataParameter[] parameters)��", e);
                return 0;
            }
        }


        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="tableName">DataSet����еı���</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            try
            {


                using (var connection = new SqlConnection(ConnectionString))
                {
                   // LoggerHelper.Info("run procedure ��" + storedProcName);
                    var dataSet = new DataSet();
                    connection.Open();
                    var sqlDA = new SqlDataAdapter { SelectCommand = BuildQueryCommand(connection, storedProcName, parameters) };
                    sqlDA.Fill(dataSet, tableName);
                    connection.Close();
                    return dataSet;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(" public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)��", e);
                return null;
            }
        }

        public static DataSet RunProcedure(string conn, string storedProcName, IDataParameter[] parameters, string tableName)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    //LoggerHelper.Info("run procedure ��" + storedProcName);
                    var dataSet = new DataSet();
                    connection.Open();
                    var sqlDA = new SqlDataAdapter { SelectCommand = BuildQueryCommand(connection, storedProcName, parameters) };
                    sqlDA.Fill(dataSet, tableName);
                    connection.Close();
                    return dataSet;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(" public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)��", e);
                return null;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName,
                                           int times)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    //LoggerHelper.Info("run procedure ��" + storedProcName);
                    var dataSet = new DataSet();
                    connection.Open();
                    var sqlDA = new SqlDataAdapter { SelectCommand = BuildQueryCommand(connection, storedProcName, parameters) };
                    sqlDA.SelectCommand.CommandTimeout = times;
                    sqlDA.Fill(dataSet, tableName);
                    connection.Close();
                    return dataSet;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error("public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName,int times)��", e);
                return null;
            }
        }


        /// <summary>
        /// ���� SqlCommand ����(��������һ���������������һ������ֵ)
        /// </summary>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName,
                                                    IEnumerable<IDataParameter> parameters)
        {
            var command = new SqlCommand(storedProcName, connection) { CommandType = CommandType.StoredProcedure };
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // ���δ����ֵ���������,���������DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// ִ�д洢���̣�����Ӱ�������		
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="rowsAffected">Ӱ�������</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    //LoggerHelper.Info("run procedure ��" + storedProcName);
                    int result;
                    connection.Open();
                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    //Connection.Close();
                    return result;

                }
                catch (Exception e)
                {
                    LoggerHelper.Error(" public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)��", e);

                }
                rowsAffected = 0;
                return rowsAffected;
            }

        }
        public static int RunProcedure(string conn, string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (var connection = new SqlConnection(conn))
            {
                try
                {

                    //LoggerHelper.Info("run procedure ��" + storedProcName);
                    int result;
                    connection.Open();
                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    //Connection.Close();
                    return result;

                }
                catch (Exception e)
                {
                    LoggerHelper.Error(" public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)��", e);

                }
                rowsAffected = 0;
                return rowsAffected;
            }

        }

        /// <summary>
        /// ���� SqlCommand ����ʵ��(��������һ������ֵ)	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlCommand ����ʵ��</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName,
                                                  IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                                                    SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                                                    false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        #endregion
        /// <summary>
        /// Execute SQL Command
        /// </summary>
        /// <param name="sqlString">SQL Command</param>
        /// <param name="sqlParams">SQL Command Parameters; when no params, the param is null</param>
        /// <returns>Execute number of row affected</returns>
        public static int ExecuteScalar(string sqlString, SqlParameter[] sqlParams)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    //LoggerHelper.Info("run ExecuteScalar ��" + sqlString);
                    connection.Open();
                    var command = new SqlCommand(sqlString, connection);
                    //command.CommandType = CommandType.StoredProcedure;

                    if (sqlParams != null)
                    {
                        foreach (SqlParameter param in sqlParams)
                        {
                            command.Parameters.Add(param);
                        }
                    }
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    LoggerHelper.Error("  public static int ExecuteScalar��" + sqlString, ex);
                    return -1;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }




        public static int ExecuteNonQuery(string connString, string sqlString, CommandType cmdType, params SqlParameter[] parameters)
        {
            var val = 0;
            using (var connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, connection, null, cmdType, sqlString, parameters);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

            }
            return val;
        }
    }
}


