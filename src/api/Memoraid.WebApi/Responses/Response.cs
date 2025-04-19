namespace Memoraid.WebApi.Responses;

public class Response
{
    public Response()
    {
        Errors = [];
    }

    public Response(Error[] errors)
    {
        Errors = errors;
    }

    public bool IsSuccess => Errors.Length == 0;
    public Error[] Errors { get; }

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
    public Response(T data) : base()
    {
        Data = data;
    }

    public T? Data { get; }
}