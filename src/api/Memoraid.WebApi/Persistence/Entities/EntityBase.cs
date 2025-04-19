using System;

namespace Memoraid.WebApi.Persistence.Entities;

public abstract class EntityBase
{
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}

public abstract class EntityBase<TKey> : EntityBase where TKey : struct
{
    public TKey Id { get; set; }
}
