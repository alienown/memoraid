using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Persistence.Interceptors;

internal class SaveEntityBaseInterceptor : SaveChangesInterceptor
{
    private const string System = "System";

    private readonly IUserContext _userContext;

    public SaveEntityBaseInterceptor(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            var entires = eventData.Context.ChangeTracker.Entries<EntityBase>();

            foreach (var entry in entires)
            {
                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    if (entry.Entity.CreatedOn == default)
                    {
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                    }

                    if (entry.Entity.CreatedBy == null)
                    {
                        entry.Entity.CreatedBy = _userContext.UserId?.ToString() ?? System;
                    }
                }
                else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    if (entry.Entity.LastModifiedOn == default)
                    {
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    }

                    if (entry.Entity.LastModifiedBy == null)
                    {
                        entry.Entity.LastModifiedBy = _userContext.UserId?.ToString() ?? System;
                    }
                }
            }
        }

        return ValueTask.FromResult(result);
    }
}
