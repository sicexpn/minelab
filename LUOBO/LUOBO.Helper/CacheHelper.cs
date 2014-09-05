using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace LUOBO.Helper
{
    public class CacheHelper
    {
        private static CacheHelper instance;
        public static CacheHelper Instance()
        {
            if (instance == null)
            {
                instance = new CacheHelper();
            }
            return instance;
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public  object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public  void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.UtcNow.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public  void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
        /// <summary>
        /// 清除单一键缓存
        /// </summary>
        /// <param name="key"></param>
        public  void RemoveOneCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(CacheKey);
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public  void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            if (_cache.Count > 0)
            {
                ArrayList al = new ArrayList();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
                foreach (string key in al)
                {
                    _cache.Remove(key);
                }
            }
        }
        /// <summary>
        /// 以列表形式返回已存在的所有缓存的Key
        /// </summary>
        /// <returns></returns> 
        public ArrayList ShowAllCache()
        {
            ArrayList al = new ArrayList();
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            if (_cache.Count > 0)
            {
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
            }
            return al;
        } 
    }
}
