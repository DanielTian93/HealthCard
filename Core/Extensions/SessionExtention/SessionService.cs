using Core.Helpers;
using StackExchange.Redis;
using System;

namespace Core.Extentions
{
    public class SessionService : ISessionService
    {
        public void SetSession<T>(string token, T session, TimeSpan expireTime = default(TimeSpan), CommandFlags flags = CommandFlags.None, bool slide = true)
        {
            //Guard.ArgumentNotNull(token, nameof(token));
            //Guard.ArgumentNotNull(session, nameof(session));
            RedisHelper.Set(token, session, expireTime, flags: flags, slide: slide);
        }
        public T GetSession<T>(string token)
        {
            var userSession = RedisHelper.Get<T>(token);
            return userSession;
        }
        public void RemoveToken(string token)
        {
            RedisHelper.Remove(token);
        }
    }
}
