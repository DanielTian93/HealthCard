using Core.Enum;
using Core.Extentions;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Linq;

namespace Core.Filter
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ////判断是否标记不验证
            var actionsFilter = context.ActionDescriptor.FilterDescriptors.Where(t => t.Filter.GetType() == typeof(NoToken));
            if (actionsFilter.Count() > 0)
                return;
            ErrDetail r = new ErrDetail() { Code = "0", Msg = "OK" };
            //获取接口路径
            var rule = context.HttpContext.Request.Path.ToString().ToLower();
            //获取token
            StringValues token;
            context.HttpContext.Request.Headers.TryGetValue("token", out token);
            if (!string.IsNullOrEmpty(token))
            {
                if (RedisHelper.Exists(token))
                    return;
                else
                {
                    r.Code = "401";
                    r.Msg = "未登录";
                }
            }
            if (r.Code != "0")
            {
                context.Result = new OkObjectResult(r.Msg) { StatusCode = int.Parse(r.Code) };
            }
        }
    }
}
