using Core.Enum;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Core.UnifiedResponseMessage
{
    public static class ResponseObjectExtention
    {
        public class ResponseObjectDomainModel<T>
        {
            public string Msg { get; set; }
            public int Code { get; set; } = 0;
            public T Data { get; set; }
        }
        public class ResponseObject<T>
        {
            public string Msg { get; set; }
            public string Code { get; set; }
            public T Data { get; set; }
        }
        public static ResponseObject<T> IncludeResponse<T>(this T t, string msg = null, string code = null)
        {
            var response = new ResponseObject<T>();
            response.Msg = msg;
            response.Code = code;
            response.Data = t;
            return response;
        }

        public static ErrDetail GetErrDefinition<T>(this T t)
            where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidCastException("传入参数不是枚举类型");
            }
            var detail = new ErrDetail();
            detail.Msg = t.ToString();
            detail.Code = Convert.ToInt64(t).ToString();
            MemberInfo[] memInfo = type.GetMember(detail.Msg);
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    detail.Msg = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return detail;
        }

        public static string GetCode<T>(this T t)
            where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidCastException("传入参数不是枚举类型");
            }
            return Convert.ToInt64(t).ToString();
        }
    }
}
