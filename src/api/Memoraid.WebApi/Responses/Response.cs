using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Memoraid.WebApi.Responses;

public class Response
{
    [SetsRequiredMembers]
    public Response()
    {
        Errors = [];
    }

    [SetsRequiredMembers]
    public Response(Error[] errors)
    {
        Errors = errors;
    }

    public bool IsSuccess => Errors.Length == 0;

    [Required]
    public required Error[] Errors { get; init; }

    public class Error
    {
        public Error(string code, string message, string? propertyName = null)
        {
            Code = code;
            Message = message;
            PropertyName = propertyName;
        }

        public string Code { get; }
        public string Message { get; }
        public string? PropertyName { get; }
    }
}

public class Response<T> : Response where T : class
{
    [SetsRequiredMembers]
    public Response(T data) : base()
    {
        Data = data;
    }

    [Required]
    public required T Data { get; init; }
}