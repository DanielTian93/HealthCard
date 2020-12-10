using StackExchange.Redis;
using System;

namespace Core.Extentions
{
    /// <summary>
    /// Redis用户信息操作
    /// </summary>
    public interface ISessionService
    {

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        T GetSession<T>(string token);

        /// <summary>
        /// 移除Token信息
        /// </summary>
        /// <param name="token">用户token</param>
        void RemoveToken(string token);

        /// <summary>
        /// 添加用户Session
        /// </summary>
        /// <param name="token">用户token</param>
        /// <param name="userSession">用户Session</param>
        void SetSession<T>(string token, T session, TimeSpan expireTime = default(TimeSpan), CommandFlags flags = CommandFlags.None, bool slide = true);
    }
}
