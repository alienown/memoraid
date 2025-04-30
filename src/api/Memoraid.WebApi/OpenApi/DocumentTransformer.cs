using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.WebApi.OpenApi
{
    internal class DocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Servers =
            [
                new OpenApiServer
                {
                    Url = "http://localhost:7000"
                }
            ];

            return Task.CompletedTask;
        }
    }
}
