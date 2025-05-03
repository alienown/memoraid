using System.Collections.Generic;

namespace Memoraid.WebApi.Responses;

public class GetFlashcardsResponse
{
    public GetFlashcardsResponse(IReadOnlyList<FlashcardsListItem> items, int total)
    {
        Items = items;
        Total = total;
    }

    public IReadOnlyList<FlashcardsListItem> Items { get; }
    public int Total { get; }

    public class FlashcardsListItem
    {
        public FlashcardsListItem(long id, string front, string back)
        {
            Id = id;
            Front = front;
            Back = back;
        }

        public long Id { get; }
        public string Front { get; }
        public string Back { get; }
    }
}
