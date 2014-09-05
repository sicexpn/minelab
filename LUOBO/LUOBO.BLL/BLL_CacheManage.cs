using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.BLL
{
    public class BLL_CacheManage
    {
        /// <summary>
        /// 获取所有缓存的Key
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCacheKey()
        {
            return Helper.CacheHelper.Instance().ShowAllCache().ToArray().Select(c => c.ToString()).ToList();
        }

        /// <summary>
        /// 获取指定Key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return Helper.CacheHelper.Instance().GetCache(key);
        }

        /// <summary>
        /// 移除指定Key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveOneCache(string key)
        {
            try
            {
                Helper.CacheHelper.Instance().RemoveOneCache(key);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 移除所有的缓存
        /// </summary>
        /// <returns></returns>
        public bool RemoveAllCache()
        {
            try
            {
                Helper.CacheHelper.Instance().RemoveAllCache();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
