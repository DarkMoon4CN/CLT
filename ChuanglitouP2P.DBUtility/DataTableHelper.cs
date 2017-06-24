using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
namespace ChuanglitouP2P.DBUtility
{
    public static class DataTableHelper
    {
        public static DataTable CreateDataTableSimple(List<string> columnNames)
        {
            DataTable dataTable = new DataTable();
            foreach (string colName in columnNames)
            {
                DataColumn dc = new DataColumn(colName, Type.GetType("System.String"));
                dataTable.Columns.Add(dc);
            }
            dataTable.TableName = "results";
            return dataTable;
        }
    }
}
