using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.DAL.Api
{
    public class DataHelper
    {
        public static T GetEntity<T>(DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type type = item.PropertyType;
                            Type underlyingType = Nullable.GetUnderlyingType(type);

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], underlyingType ?? type), null);
                        }

                    }
                }
            }

            return entity;
        }

        public static IList<T> GetEntities<T>(DataTable table) where T : new()
        {
            IList<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    Type type = item.PropertyType;
                    Type underlyingType = Nullable.GetUnderlyingType(type);
                    item.SetValue(entity, Convert.ChangeType(row[item.Name], underlyingType ?? type), null);
                }
                entities.Add(entity);
            }
            return entities;
        }


    }
}
