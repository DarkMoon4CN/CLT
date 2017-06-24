///////////////////////////////////////////////////////////
//Name:数据库模型-数据库连接通用抽象接口
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System.Data;
using System.Collections.Generic;
namespace ChuanglitouP2P.WindowsService
{
    public interface IDatabaseProvider
    {
        DataTable ExecuteSqlReturnDatatable(string sql);

        int ExecuteSql(string sql);

        T ExecuteSqlOneValue<T>(string sql);

        List<T> GetEntityList<T>(string sql) where T : class, new();

        ILogger Logger { set; }

        string ConnectString { get; set; }
    }
}
