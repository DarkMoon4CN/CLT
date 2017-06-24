///////////////////////////////////////////////////////////
//Name:数据库模型-数据库连接提供类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace ChuanglitouP2P.WindowsService
{
    public class SqlServerProvider : IDatabaseProvider
    {
        ILogger logger = null;

        public string ConnectString
        {
            get; set;
        }

        public ILogger Logger
        {
            set
            {
                logger = value;
            }
        }

        public int ExecuteSql(string sql)
        {
            int result = -1;
            using (SqlConnection con = new SqlConnection(ConnectString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    cmd.Dispose();
                    cmd = null;
                }
                catch (Exception ex)
                {
                    logger.WriteException(sql, ex);
                    throw ex;
                }
            }
            return result;
        }

        public T ExecuteSqlOneValue<T>(string sql)
        {
            DataTable result = ExecuteSqlReturnDatatable(sql);
            return (T)result.Rows[0][0];
        }

        public DataTable ExecuteSqlReturnDatatable(string sql)
        {
            DataTable result = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectString))
            {
                try
                {
                    SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                    ada.Fill(result);
                    ada.Dispose();
                    ada = null;
                }
                catch (Exception ex)
                {
                    logger.WriteException(sql, ex);
                    throw ex;
                }
            }
            return result;
        }

        public List<T> GetEntityList<T>(string sql) where T : class, new()
        {
            List<T> results = new List<T>();
            try
            {
                DataTable items = ExecuteSqlReturnDatatable(sql);
                if (items.Rows.Count < 1) return results;
                Type type = typeof(T);
                foreach (DataRow dr in items.Rows)
                {
                    T t = new T();
                    foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public))
                    {
                        if (items.Columns.Contains(propertyInfo.Name))
                        {
                            var value = dr[propertyInfo.Name];
                            if (value == null || value is DBNull) continue;
                            propertyInfo.SetValue(t, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteException(sql, ex);
                throw ex;
            }
            return results;
        }
    }
}
