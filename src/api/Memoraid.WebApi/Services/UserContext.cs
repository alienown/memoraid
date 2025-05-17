using Microsoft.AspNetCore.Http;
using System;

namespace Memoraid.WebApi.Services;

public interface IUserContext
{
    string? UserId { get; }
    string GetUserIdOrThrow();
}

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var userIdClaim = httpContext.User.FindFirst("sub");
            if (userIdClaim == null)
                return null;

            return userIdClaim.Value;
        }
    }

    public string GetUserIdOrThrow()
    {
        if (UserId == null)
            throw new UserNotAuthenticatedException();

        return UserId;
    }
}

public class UserNotAuthenticatedException : Exception
{
}