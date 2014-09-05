using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace LUOBO.Helper
{
    public static class PubFun
    {
        /// <summary>
        /// 文件夹内容复制
        /// </summary>
        /// <param name="DirPath">源文件夹路径</param>
        /// <param name="NewPath">目标文件夹路径</param>
        public static void CopyDirectory(String DirPath, String NewPath)
        {
            if (!Directory.Exists(DirPath))
            {
                return;
            }

            if (!Directory.Exists(NewPath))
            {
                Directory.CreateDirectory(NewPath);
            }

            foreach(String ff in Directory.GetFiles(DirPath)){
                File.Copy(ff, NewPath + "\\" + ff.Substring(ff.Replace("\\", "/").LastIndexOf('/') + 1));
            }

            foreach (String dd in Directory.GetDirectories(DirPath))
            {
                CopyDirectory(dd, NewPath + "/" + dd.Substring(dd.Replace("\\", "/").LastIndexOf('/') + 1));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ColumnName">传Null为基本数据类型</param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> list, string ColumnName)
        {
            string result = "";

            if (ColumnName != null)
            {
                foreach (T item in list)
                {
                    if (result != "")
                        result += "\n";
                    PropertyInfo pi = item.GetType().GetProperty(ColumnName);
                    result += pi.GetValue(item, null).ToString();
                }
            }
            else
            {
                foreach (T item in list)
                {
                    if (result != "")
                        result += "\n";
                    result += item.ToString();
                }
            }

            return result;
        }

        public static string ToString<T>(this IEnumerable<T> list, string ColumnName, string SplitChar)
        {
            string result = "";

            if (ColumnName != null)
            {
                foreach (T item in list)
                {
                    if (result != "")
                        result += SplitChar;
                    PropertyInfo pi = item.GetType().GetProperty(ColumnName);
                    result += pi.GetValue(item, null).ToString();
                }
            }
            else
            {
                foreach (T item in list)
                {
                    if (result != "")
                        result += SplitChar;
                    result += item.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 将两个相似的对象数据进行复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ChangeNewItem<T, U>(U source)
        {
            T newitem = Activator.CreateInstance<T>();
            foreach (PropertyInfo info in source.GetType().GetProperties())
            {
                foreach (PropertyInfo targetInfo in newitem.GetType().GetProperties())
                {
                    if (info.Name == targetInfo.Name && info.PropertyType == targetInfo.PropertyType)
                    {
                        if (targetInfo.CanWrite == true)
                        {
                            try
                            {
                                targetInfo.SetValue(newitem, info.GetValue(source, null), null);
                            }
                            catch { }
                        }
                    }
                }
            }
            return newitem;
        }

        /// <summary>
        /// 将两个相似的对象数据进行复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ChangeNewList<T, U>(List<U> source)
        {
            List<T> newlist = Activator.CreateInstance<List<T>>();
            T newitem;
            foreach (U item in source)
            {
                newitem = Activator.CreateInstance<T>();
                foreach (PropertyInfo info in item.GetType().GetProperties())
                {
                    foreach (PropertyInfo targetInfo in newitem.GetType().GetProperties())
                    {
                        if (info.Name == targetInfo.Name && info.PropertyType == targetInfo.PropertyType)
                        {
                            if (targetInfo.CanWrite == true)
                            {
                                try
                                {
                                    targetInfo.SetValue(newitem, info.GetValue(item, null), null);
                                }
                                catch { }
                            }
                        }
                    }
                }
                newlist.Add(newitem);
            }
            return newlist;
        }
    }
}
