namespace Memoraid.WebApi.Persistence.Entities
{
    public class User : EntityBase<long>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}