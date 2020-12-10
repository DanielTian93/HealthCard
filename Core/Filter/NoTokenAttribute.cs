using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Core.Filter
{
    /// <summary>
    /// 不进行验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class NoToken : ActionFilterAttribute
    {
        public NoToken()
        {
        }
    }
}
