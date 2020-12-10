using Core.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class RedisHelper
    {
        private static readonly string Coonstr = ConfigurationManager.AppSettings["Redis"].ToString();
        private static readonly object _locker = new object();
        private static ConnectionMultiplexer _instance = null;
        /// <summary>w
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = ConnectionMultiplexer.Connect(Coonstr);
                        }
                    }
                }
                return _instance;
            }
        }
        public static IDatabase GetDatabase()
        {
            return Instance.GetDatabase(3);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        public static void Removes(string[] keys)
        {
            var del = new List<RedisKey>();
            var batch = GetDatabase().CreateBatch();
            foreach (var key in keys)
            {
                del.Add(key);
            }
            GetDatabase().KeyDelete(del.ToArray());
        }
        /// <summary>
        /// 这里的 MergeKey 用来拼接 Key 的前缀，具体不同的业务模块使用不同的前缀。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string MergeKey(string key)
        {
            return $"{ConfigurationManager.AppSettings["RedisName"].ToString()}:" + key;
        }
        public static T GetNotMergeKey<T>(string key)
        {
            var jsonModel = GetDatabase().StringGet(key);
            if (jsonModel == default(RedisValue))
            {
                return default(T);
            }
            var res = JsonConvert.DeserializeObject<RedisData<T>>(jsonModel);
            if (res.Slide == true)
            {
                GetDatabase().StringSet(key, jsonModel, res.TimeSpan, flags: res.Flags);
            }
            return res.Value;
        }
        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            var mergeKey = MergeKey(key);
            var jsonModel = GetDatabase().StringGet(mergeKey);
            if (jsonModel == default(RedisValue))
            {
                return default(T);
            }
            var res = JsonConvert.DeserializeObject<RedisData<T>>(jsonModel);
            if (res.Slide == true)
            {
                GetDatabase().StringSet(mergeKey, jsonModel, res.TimeSpan, flags: res.Flags);
            }
            return res.Value;
        }
        ///// <summary>
        ///// 根据key获取缓存对象
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static object Get(string key)
        //{
        //    key = MergeKey(key);
        //    return JsonConvert.DeserializeObject<RedisData<object>>(GetDatabase().StringGet(key)).Value;
        //}
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="flags"></param>
        /// <param name="slide"></param>
        public static void Set(string key, object value, TimeSpan time = default(TimeSpan), CommandFlags flags = CommandFlags.None, bool slide = false)
        {
            key = MergeKey(key);
            var data = new RedisData<object>
            {
                Value = value,
                Slide = slide,
                TimeSpan = time == default(TimeSpan) ? TimeSpan.FromMinutes(20) : time,
                Flags = flags
            };
            GetDatabase().StringSet(key, JsonConvert.SerializeObject(data), data.TimeSpan, flags: flags);
        }
        /// <summary>
        /// 判断在缓存中是否存在该key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyExists(key);  //可直接调用
        }
        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyDelete(key);
        }
        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveNotMergeKey(string key)
        {
            return GetDatabase().KeyDelete(key);
        }
        /// <summary>
        /// 异步设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task SetAsync(string key, object value, TimeSpan time = default(TimeSpan), CommandFlags flags = CommandFlags.None, bool slide = false)
        {
            key = MergeKey(key);
            var data = new RedisData<object>
            {
                Value = value,
                Slide = slide,
                TimeSpan = time == default(TimeSpan) ? TimeSpan.FromMinutes(20) : time,
                Flags = flags
            };
            await GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(data), data.TimeSpan, flags: flags);
        }
        /// <summary>
        /// 判断是否存在包含keyPattern的key
        /// </summary>
        /// <param name="keyPattern"></param>
        /// <returns></returns>
        public static bool ExistPatternKey(string keyPattern)
        {
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var cacheResult = GetDatabase().ScriptEvaluate(prepared, new { pattern = keyPattern });
            if (cacheResult.IsNull)
            {
                return false;
            }
            return ((string[])cacheResult).Length > 0;
        }
        public static string[] KeyContains(string keyPattern)
        {
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var cacheResult = GetDatabase().ScriptEvaluate(prepared, new { pattern = keyPattern });
            return (string[])cacheResult;
        }


        public class RedisData<T>
        {
            public T Value { get; set; }
            public bool Slide { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public CommandFlags Flags { get; set; }
        }
    }
}
