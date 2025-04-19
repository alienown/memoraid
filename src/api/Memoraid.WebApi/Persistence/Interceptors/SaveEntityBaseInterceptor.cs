using Memoraid.WebApi.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Persistence.Interceptors;

internal class SaveEntityBaseInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            var entires = eventData.Context.ChangeTracker.Entries<EntityBase>();

            foreach (var entry in entires)
            {
                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "User"; // TODO: Get the current user
                }
                else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "User"; // TODO: Get the current user
                }
            }
        }

        return ValueTask.FromResult(result);
    }
}
