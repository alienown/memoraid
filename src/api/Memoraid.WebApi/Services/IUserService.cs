using Memoraid.WebApi.Responses;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IUserService
{
    Task<Response> DeleteUserAsync();

    public static class ErrorCodes
    {
        public const string UserNotFound = "UserNotFound";
    }
}
