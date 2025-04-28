using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.WebApi.OpenApi
{
    internal class NonNullableEnumSchemaTransformer : IOpenApiSchemaTransformer
    {
        public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            if (context.JsonTypeInfo.Type.IsGenericType &&
                context.JsonTypeInfo.Type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                context.JsonTypeInfo.Type.GetGenericArguments()[0].IsEnum)
            {
                var enumType = context.JsonTypeInfo.Type.GetGenericArguments()[0];

                schema.Annotations["x-schema-id"] = enumType.Name;
            }

            return Task.CompletedTask;
        }
    }
}
