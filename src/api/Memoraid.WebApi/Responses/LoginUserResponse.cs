public class LoginUserResponse
{
    public LoginUserResponse(string token)
    {
        Token = token;
    }

    public string Token { get; }
}
