using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Core.UnifiedResponseMessage
{
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter = null)
        {
            if (formatter == null)
            {
                formatter = new JsonMediaTypeFormatter();
            }

            var settings = formatter.SerializerSettings;
            settings.ContractResolver = new CamCaseContractResolver();

            ////settings.
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();

            //忽略Data
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.ContractResolver = new PropertySortResolver();
            //这里使用自定义日期格式
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            settings.Converters.Add(timeConverter);
            _jsonFormatter = formatter;
        }
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }

        /// <summary>
            /// 输出到json字符串时，属性名称按照字典顺序排序输出
            /// </summary>
        public class PropertySortResolver : DefaultContractResolver
        {
            /// <summary>
                    /// 属性名称按照字典顺序排序输出
                    /// </summary>
                    /// <param name="type"></param>
                    /// <param name="memberSerialization"></param>
                    /// <returns></returns>
            protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type,
            MemberSerialization memberSerialization)
            {
                IList<Newtonsoft.Json.Serialization.JsonProperty> list = base.CreateProperties(type, memberSerialization);
                return list.OrderBy(a => a.PropertyName).ToList();
            }
        }
    }
}
