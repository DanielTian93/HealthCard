using Core.Enum;
using Core.UnifiedResponseMessage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Core.Extensions
{
    public static class ControllerExtentions
    {
        public static ObjectResult Error<T>(this ControllerBase controller, T enumCode)
            where T : struct
        {
            return Error(controller, enumCode, default(object));
        }

        public static ObjectResult Result(this ControllerBase controller, Tuple<ErrDetail, object> res)
        {
            return Error(controller, res.Item2, res.Item1.Code, res.Item1.Msg, HttpStatusCode.OK);
        }
        public static ObjectResult Result(this ControllerBase controller, ErrDetail res)
        {
            return Error(controller, default(object), res.Code, res.Msg, HttpStatusCode.OK);
        }

        public static ObjectResult Error<T, TObj>(this ControllerBase controller, T enumCode, TObj obj)
            where T : struct
            where TObj : class
        {
            var def = enumCode.GetErrDefinition();
            return Error(controller, obj, def.Code, def.Msg, HttpStatusCode.OK);
        }

        public static ObjectResult Error(this ControllerBase controller, ErrDetail errDetail, object obj)
        {
            return Error(controller, obj, errDetail.Code, errDetail.Msg, HttpStatusCode.OK);
        }

        public static ObjectResult Error(this ControllerBase controller, string code, string msg, object obj)
        {
            return Error(controller, obj, code, msg, HttpStatusCode.OK);
        }


        static ObjectResult Error<T>(this ControllerBase controller, T obj, string code, string message, HttpStatusCode httpCode)
        {
            var response = obj.IncludeResponse(message, code);
            return controller.StatusCode((int)httpCode, response);
            //return controller.Request.CreateResponse(httpCode, response);
        }

        public static ObjectResult Success<T>(this ControllerBase controller, T obj)
        {
            var response = obj.IncludeResponse("操作成功", "0");
            controller.Response.StatusCode = (int)HttpStatusCode.OK;
            return controller.StatusCode((int)HttpStatusCode.OK, response);
            //return controller.Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public static ObjectResult Success<T>(this ControllerBase controller, string code, string msg, T obj)
        {
            var response = obj.IncludeResponse(msg, code);
            return controller.StatusCode((int)HttpStatusCode.OK, response);

            //return controller.Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }

}
