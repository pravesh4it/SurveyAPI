namespace ABC.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Reflection;

    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            var list = new List<T>();

            if (dataTable == null)
                return list;

            foreach (DataRow row in dataTable.Rows)
            {
                T obj = new T();

                foreach (DataColumn column in dataTable.Columns)
                {
                    // property name same as column name
                    PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                    if (prop == null)
                        continue;

                    object value = row[column];
                    if (value == DBNull.Value)
                    {
                        // DBNull -> skip (leave default / null)
                        continue;
                    }

                    // If DB returns empty string for date etc. treat as null
                    if (value is string s && string.IsNullOrWhiteSpace(s))
                    {
                        continue;
                    }

                    Type targetType = prop.PropertyType;
                    Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                    try
                    {
                        object safeValue = ChangeType(value, underlyingType);
                        prop.SetValue(obj, safeValue, null);
                    }
                    catch
                    {
                        // conversion failed — skip assigning or you can log
                        // don't rethrow to avoid breaking whole mapping for one bad cell
                    }
                }
                list.Add(obj);
            }

            return list;
        }

        // Helper: convert value to a target non-nullable type (underlying type)
        private static object ChangeType(object value, Type targetType)
        {
            if (targetType == typeof(Guid))
            {
                if (value is Guid) return value;
                return Guid.Parse(value.ToString());
            }

            if (targetType == typeof(DateTime))
            {
                if (value is DateTime dt) return dt;

                // Try parse with invariant culture (handles formats like "2025-10-03 12:10:08.8076669")
                if (DateTime.TryParse(value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDt))
                    return parsedDt;

                // try default parse as fallback
                return DateTime.Parse(value.ToString());
            }

            if (targetType.IsEnum)
            {
                if (value is string) return Enum.Parse(targetType, value.ToString(), ignoreCase: true);
                return Enum.ToObject(targetType, value);
            }

            // Use TypeDescriptor for general conversions (handles numeric types, bool, etc.)
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
            {
                return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            }

            // Fallback to ChangeType (for primitives)
            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
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
