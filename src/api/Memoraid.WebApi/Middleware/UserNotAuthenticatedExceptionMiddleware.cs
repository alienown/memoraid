using Memoraid.WebApi.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Middleware;

public class UserNotAuthenticatedExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public UserNotAuthenticatedExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserNotAuthenticatedException)
        {
            context.Response.StatusCode = 401;
        }
    }
}