namespace Memoraid.WebApi.Requests;

public class ChangeUserPasswordRequest
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}
