namespace Memoraid.WebApi.Persistence.Entities
{
    public class EntityBase<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
