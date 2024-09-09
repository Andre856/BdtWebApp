using Bdt.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bdt.Api.Infrastructure.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is { State: EntityState.Deleted, Entity: IEntitySoftDelete delete })
            {
                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
            }
        }

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
    {
        if (eventData.Context is null)
        {
            return new ValueTask<InterceptionResult<int>>(result);
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is { State: EntityState.Deleted, Entity: IEntitySoftDelete delete })
            {
                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
            }
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}
