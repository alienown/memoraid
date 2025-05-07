using Microsoft.AspNetCore.Http;
using System;

namespace Memoraid.WebApi.Services;

public interface IUserContext
{
    long? UserId { get; }
    long GetUserIdOrThrow();
}

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var userIdClaim = httpContext.User.FindFirst("sub");
            if (userIdClaim == null)
                return null;

            if (long.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }
    }

    public long GetUserIdOrThrow()
    {
        if (UserId == null)
            throw new UserNotAuthenticatedException();

        return UserId.Value;
    }
}

public class UserNotAuthenticatedException : Exception
{
}