using FluentValidation;
using Memoraid.WebApi.Responses;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Middleware;

public class FluentValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public FluentValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 422;

            var errors = ex.Errors
                .Select(err => new Response.Error(err.ErrorCode, err.ErrorMessage, err.PropertyName))
                .ToArray();

            var response = new Response(errors);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
