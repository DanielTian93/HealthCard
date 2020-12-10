using System;

namespace Core.Extensions.Swagger
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SwaggerApp : Attribute
    {
    }
}
