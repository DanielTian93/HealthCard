using Core.Extensions.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Core.Extensions
{
    public class SwaggerAddToken : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var attrs = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.GetType() == typeof(SwaggerToken))
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "token",
                        In = ParameterLocation.Header,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        },
                        Required = true,
                        Description = "login seesion"
                    });
                }
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.GetType() == typeof(SwaggerApp))
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "app",
                        In = ParameterLocation.Header,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        },
                        Required = false,
                        Description = "mpInfo"
                    });
                }
                if (attr.GetType() == typeof(SwaggerAliApp))
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "aliapp",
                        In = ParameterLocation.Header,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        },
                        Required = false,
                        Description = "aliMpInfo"
                    });
                }
            }
        }


    }
}
