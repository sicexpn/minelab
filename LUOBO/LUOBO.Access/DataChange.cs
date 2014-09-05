using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace LUOBO.Access
{
    public static class DataChange<T>
    {
        public static List<T> FillModel(DataTable dt)
        {
            List<T> l = new List<T>();
            T model = default(T);

            if (dt.Columns[0].ColumnName == "rowId")
            {
                dt.Columns.Remove("rowId");
            }
            foreach (DataRow dr in dt.Rows)
            {
                model = Activator.CreateInstance<T>();

                foreach (DataColumn dc in dr.Table.Columns)
                {
                   
                    PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName.Substring(0, 1).ToUpper() + dc.ColumnName.Substring(1));
                    if (pi != null)
                    {
                        if (dr[dc.ColumnName] != DBNull.Value)
                            pi.SetValue(model, dr[dc.ColumnName], null);
                        //else
                        //pi.SetValue(model, null, null);
                    }
                }
                l.Add(model);
            }

            return l;
        }
        public static T FillEntity(DataRow dr)
        {

            T model = default(T);

            if (dr.Table.Columns[0].ColumnName == "rowId")
            {
                dr.Table.Columns.Remove("rowId");
            }


            model = Activator.CreateInstance<T>();

            foreach (DataColumn dc in dr.Table.Columns)
            {

                PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);
                if (pi != null)
                {
                    if (dr[dc.ColumnName] != DBNull.Value)
                        pi.SetValue(model, dr[dc.ColumnName], null);
                    //else
                    //pi.SetValue(model, null, null);
                }
            }


            return model;


        }
        public static DataRow FillRow(T t, DataRow row)
        {
            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
            {
                System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                string name = pi.Name;

                //if (dt.Columns[name] == null)
                //{
                //    column = new DataColumn(name, pi.PropertyType);
                //    dt.Columns.Add(column);
                //}
                object value = pi.GetValue(t, null);
                if ((name.IndexOf("FK_") < 0) && (name.IndexOf("CHD_") < 0))
                {
                    if (value != null)
                    {
                        if (row.Table.Columns.IndexOf(name) >= 0)
                        {
                            row[name] = value;
                        }
 
                    }
                    else
                    {
                        if (row.Table.Columns.IndexOf(name) >= 0)
                        {
                            row[name] = DBNull.Value;
                        }
                    }
                }
            }
            return row;
        }
        /// <summary>
        /// 将实体类转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_objlist"></param>
        /// <returns></returns>
        public static DataTable Fill(List<T> objlist)
        {
            if (objlist == null || objlist.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (T t in objlist)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        //if (pi.PropertyType.FullName.IndexOf("Decimal") > 0)
                        //{

                        //}
                        //if (pi.PropertyType.FullName.IndexOf("String") > 0)
                        //{

                        //}
                        //if (pi.PropertyType.FullName.IndexOf("Byte") > 0)
                        //{

                        //}
                        //if (pi.PropertyType.FullName.IndexOf("Decimal") > 0)
                        //{

                        //}
                        Type propType = pi.PropertyType;
                        if (propType.IsGenericType &&
                            propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propType = Nullable.GetUnderlyingType(propType);
                        }
                        column = new DataColumn(name, propType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }
            return dt;
        }
    }

   
}
