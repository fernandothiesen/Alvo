
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public class UtcDateTimeInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ConvertDateTimesToUtc(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ConvertDateTimesToUtc(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ConvertDateTimesToUtc(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || 
                        e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue is DateTime dt)
                {
                    property.CurrentValue = dt.Kind switch
                    {
                        DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
                        DateTimeKind.Local => dt.ToUniversalTime(),
                        _ => dt
                    };
                }
            }
        }
    }
}