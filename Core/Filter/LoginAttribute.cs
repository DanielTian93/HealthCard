using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Core.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class Login : ActionFilterAttribute
    {
        public Login()
        {
        }
    }
}
