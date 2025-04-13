public class Response<T> where T : class
{
    public Response(T data)
    {
        Data = data;
        Errors = [];
    }

    public Response(IReadOnlyList<Error> errors)
    {
        Errors = errors;
    }

    public T? Data { get; }
    public bool IsSuccess => Errors.Count == 0;
    public IReadOnlyList<Error> Errors { get; }

    public class Error
    {
        public Error(string message, string code)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; }
        public string Code { get; }
    }
}