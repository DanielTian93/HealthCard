using Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Core.Extentions
{
    public static class CacheExtentions
    {
        /// <summary>
        /// 异步查询数据使用缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键</param>
        /// <param name="getData">数据获取逻辑</param>
        /// <param name="checkData">数据验证逻辑</param>
        /// <param name="expires">过期时间</param>
        /// <param name="useSlidingExpiration">是否使用滑动机制</param>
        /// <returns></returns>
        public static async Task<T> WithCacheQuery<T>(
            string key,
            Func<Task<T>> getData,
            Predicate<T> checkData,
            TimeSpan expires,
            bool useSlidingExpiration = false)
        {
            var cache = RedisHelper.Get<T>(key);
            if (checkData(cache))
            {
                return cache;
            }

            var data = await getData();
            if (checkData(data))
            {
                RedisHelper.Set(key, data, expires);
            }

            return data;
        }

        /// <summary>
        /// 同步查询数据使用缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键</param>
        /// <param name="getData">数据获取逻辑</param>
        /// <param name="checkData">数据验证逻辑</param>
        /// <param name="expires">过期时间</param>
        /// <param name="useSlidingExpiration">是否使用滑动机制</param>
        /// <returns></returns>
        public static T WithCacheQuerySync<T>(
            string key,
            Func<T> getData,
            Predicate<T> checkData,
            TimeSpan expires,
            bool useSlidingExpiration = false)
        {
            var cache = RedisHelper.Get<T>(key);
            if (checkData(cache))
            {
                return cache;
            }

            var data = getData();
            if (checkData(data))
            {
                RedisHelper.Set(key, data, expires);
            }

            return data;
        }
    }
}
