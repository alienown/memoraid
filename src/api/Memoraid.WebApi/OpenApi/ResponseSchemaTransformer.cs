using Memoraid.WebApi.Responses;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.WebApi.OpenApi
{
    internal class ResponseSchemaTransformer : IOpenApiSchemaTransformer
    {
        public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            if (context.JsonTypeInfo.Type == typeof(Response) ||
                (context.JsonTypeInfo.Type.IsGenericType && context.JsonTypeInfo.Type.GetGenericTypeDefinition() == typeof(Response<>)))
            {
                var isSuccess = GetCamelCase(nameof(Response.IsSuccess));

                schema.Required.Add(isSuccess);
            }

            return Task.CompletedTask;
        }

        private static string GetCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return char.ToLowerInvariant(name[0]) + name[1..];
        }
    }
}
