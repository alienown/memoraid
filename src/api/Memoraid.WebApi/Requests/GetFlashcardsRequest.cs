namespace Memoraid.WebApi.Requests;

public class GetFlashcardsRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}