using System.Data;
using System.Reflection;

namespace Sales_Inventory.Helper
{
    public static class CommonHelper
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr.IsNull(column.ColumnName) ? null : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
