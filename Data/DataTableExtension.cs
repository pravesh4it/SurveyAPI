namespace ABC.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            var list = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T obj = new T();
                foreach (DataColumn column in dataTable.Columns)
                {
                    PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                    if (prop != null && row[column] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[column], prop.PropertyType), null);
                    }
                }
                list.Add(obj);
            }

            return list;
        }
        public static T ToSingle<T>(this DataTable dataTable) where T : new()
        {
            if (dataTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("The DataTable does not contain any rows.");
            }

            T obj = new T();
            DataRow row = dataTable.Rows[0]; // Get the first row

            foreach (DataColumn column in dataTable.Columns)
            {
                PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                if (prop != null && row[column] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(row[column], prop.PropertyType), null);
                }
            }

            return obj;
        }
    
    }

}
