using Newtonsoft.Json.Serialization;
using System;

namespace Core.UnifiedResponseMessage
{
    public class CamCaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        public CamCaseContractResolver() : base()
        {
        }
        protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            if (objectType == typeof(long) || objectType == typeof(long?))
            {
                return new LongToStringContract(objectType);
            }
            return base.CreatePrimitiveContract(objectType);
        }
    }
}
