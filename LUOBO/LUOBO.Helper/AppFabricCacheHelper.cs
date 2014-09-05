using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace LUOBO.Helper
{
    public class AppFabricCacheHelper
    {
        private static AppFabricCacheHelper instance = null;
        private const string dbName = "LUOBO";
        DataCacheFactory cacheFactory = null;
        
        public static AppFabricCacheHelper Instance()
        {
            if (instance == null)
            {
                instance = new AppFabricCacheHelper();
            }
            return instance;
        }

        AppFabricCacheHelper()
        {
            cacheFactory = new DataCacheFactory();
        }


        #region 查询数据
        /// <summary>
        /// 根据cacheKey和Region获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public object GetOneCache(string cacheKey, string region)
        {
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");

            DataCache myCache = cacheFactory.GetCache(dbName);
            return myCache.Get(cacheKey, region);
        }

        /// <summary>
        /// 根据cacheKey和Region获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<T> GetCacheByTag<T>(string region, string tag)
        {
            if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(region))
                throw new Exception("region或Tag不能为空");
            List<DataCacheTag> newTags = new List<DataCacheTag>();
            newTags.Add(new DataCacheTag(tag));

            DataCache myCache = cacheFactory.GetCache(dbName);
            IEnumerable<KeyValuePair<string, object>> obj = myCache.GetObjectsByAllTags(newTags, region);
            if (obj != null)
                return obj.Select(c=>(T)c.Value).ToList();
            else
                return null;
        }

        /// <summary>
        /// 根据cacheKey和Region获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<T> GetCacheByAllTag<T>(string region, IEnumerable<string> tags)
        {
            if (string.IsNullOrEmpty(region))
                throw new Exception("region不能为空");
            if (tags == null || tags.Count() == 0)
                throw new Exception("Tags不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            tags.ToList().ForEach(c => newTags.Add(new DataCacheTag(c)));

            DataCache myCache = cacheFactory.GetCache(dbName);
            IEnumerable<KeyValuePair<string, object>> obj = myCache.GetObjectsByAllTags(newTags, region);
            if (obj != null)
                return obj.Select(c => (T)c.Value).ToList();
            else
                return null;
        }

        /// <summary>
        /// 根据cacheKey和Region获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<T> GetCacheByAnyTag<T>(string region, IEnumerable<string> tags)
        {
            if (string.IsNullOrEmpty(region))
                throw new Exception("region不能为空");
            if (tags == null || tags.Count() == 0)
                throw new Exception("Tags不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            tags.ToList().ForEach(c => newTags.Add(new DataCacheTag(c)));

            DataCache myCache = cacheFactory.GetCache(dbName);
            IEnumerable<KeyValuePair<string, object>> obj = myCache.GetObjectsByAnyTag(newTags, region);
            if (obj != null)
                return obj.Select(c => (T)c.Value).ToList();
            else
                return null;
        }

        /// <summary>
        /// 取出Region中全部的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<T> GetRegionCache<T>(string region)
        {
            if (string.IsNullOrEmpty(region))
                throw new Exception("region不能为空");

            DataCache myCache = cacheFactory.GetCache(dbName);

            IEnumerable<KeyValuePair<string, object>> obj = myCache.GetObjectsInRegion(region);
            if (obj != null)
                return obj.Select(c => (T)c.Value).ToList();
            else
                return null;
        }
        #endregion

        #region 插入和修改数据

        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddCache(string cacheKey, string region, object value)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");

            DataCache myCache = cacheFactory.GetCache(dbName);
            if (!ExistRegion(region))
            {
                if (!CreateRegion(region))
                    throw new Exception("创建区域失败");
                myCache.Add(cacheKey, value, region);
                TableRegionNew(region);
            }
            else
            {
                myCache.Add(cacheKey, value, region);
                TableRegionAdd(region, 1);
            }
            flag = true;
            return flag;
        }

        /// <summary>
        /// 插入带有标签的新数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddCache(string cacheKey, string region, object value, IEnumerable<string> tags)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");
            if (tags == null || tags.Count() == 0)
                throw new Exception("Tags不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            tags.ToList().ForEach(c => newTags.Add(new DataCacheTag(c)));

            DataCache myCache = cacheFactory.GetCache(dbName);
            if (!ExistRegion(region))
            {
                if (!CreateRegion(region))
                    throw new Exception("创建区域失败");
                myCache.Add(cacheKey, value, newTags, region);
                TableRegionNew(region);
            }
            else
            {
                myCache.Add(cacheKey, value, region);
                TableRegionAdd(region, 1);
            }
            flag = true;
            return flag;
        }

        /// <summary>
        /// 插入带有标签的新数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddCache(string cacheKey, string region, object value, string tag)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");
            if (string.IsNullOrEmpty(tag))
                throw new Exception("Tag不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            newTags.Add(new DataCacheTag(tag));

            DataCache myCache = cacheFactory.GetCache(dbName);
            if (!ExistRegion(region))
            {
                if (!CreateRegion(region))
                    throw new Exception("创建区域失败");
                myCache.Add(cacheKey, value, newTags, region);
                TableRegionNew(region);
            }
            else
            {
                myCache.Add(cacheKey, value, newTags, region);
                TableRegionAdd(region, 1);
            }
            flag = true;
            return flag;
        }

        /// <summary>
        /// 修改指定cacheKey和Region的数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetCache(string cacheKey, string region, object value)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");

            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Put(cacheKey, value, region);
            flag = true;
            return flag;
        }

        /// <summary>
        /// 修改指定cacheKey和Region的数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetCache(string cacheKey, string region, object value, IEnumerable<string> tags)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");
            if (tags == null || tags.Count() == 0)
                throw new Exception("Tags不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            tags.ToList().ForEach(c => newTags.Add(new DataCacheTag(c)));

            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Put(cacheKey, value, newTags, region);
            flag = true;
            return flag;
        }

        /// <summary>
        /// 修改指定cacheKey和Region的数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetCache(string cacheKey, string region, object value, string tag)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");
            if (string.IsNullOrEmpty(tag))
                throw new Exception("Tag不能为空");

            List<DataCacheTag> newTags = new List<DataCacheTag>();
            newTags.Add(new DataCacheTag(tag));

            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Put(cacheKey, value, newTags, region);
            flag = true;
            return flag;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 清除单一键缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool RemoveOneCache(string cacheKey,string region)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrEmpty(region))
                throw new Exception("cacheKey或region不能为空");

            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Remove(cacheKey, region);
            TableRegionSub(region, 1);
            flag = true;
            return flag;
        }

        /// <summary>
        /// 清空指定Region缓存
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool ClearRegionCache(string region)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(region))
                throw new Exception("region不能为空");
            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.ClearRegion(region);
            myCache.Put(region, 0, "Table");
            flag = true;
            return flag;
        }

        /// <summary>
        /// 移除指定Region缓存
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool RemoveRegionCache(string region)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(region))
                throw new Exception("region不能为空");
            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.RemoveRegion(region);
            if (region != "Table")
                myCache.Remove(region, "Table");
            flag = true;
            return flag;
        }
        #endregion

        #region 公共方法
        private bool ExistRegion(string region)
        {
            List<KeyValuePair<string, object>> regions = GetAllRegion();
            return regions.Select(c => c.Key).Contains(region);
        }

        private bool CreateRegion(string region)
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            return myCache.CreateRegion(region);
        }
        #endregion

        #region Table区域表操作方法
        /// <summary>
        /// 在Table区域中指定表数据量增加num个
        /// </summary>
        /// <param name="region"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool TableRegionAdd(string region, int num)
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Put(region, (Int32)myCache.Get(region, "Table") + num, "Table");
            return true;
        }

        /// <summary>
        /// 在Table区域中指定表数据量减少num个
        /// </summary>
        /// <param name="region"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool TableRegionSub(string region, int num)
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Put(region, (Int32)myCache.Get(region, "Table") - num, "Table");
            return true;
        }

        /// <summary>
        /// 在Table区域中新增数据表
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private bool TableRegionNew(string region)
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            if (myCache.GetObjectsInRegion("Table").Count() == 0)
            {
                CreateRegion("Table");
            }
            myCache.Add(region, 1, "Table");
            return true;
        }

        /// <summary>
        /// 在Table区域中删除指定表数据
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private bool tableRegionRemoveOne(string region)
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            myCache.Remove(region, "Table");
            return true;
        }

        /// <summary>
        /// 返回已存在的所有缓存的表和数据量
        /// </summary>
        /// <returns></returns> 
        public List<KeyValuePair<string, object>> GetAllRegion()
        {
            DataCache myCache = cacheFactory.GetCache(dbName);
            return myCache.GetObjectsInRegion("Table").ToList();
        }
        #endregion
    }
}
