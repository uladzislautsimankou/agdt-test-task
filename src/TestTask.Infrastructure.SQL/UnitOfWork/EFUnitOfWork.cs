using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestTask.Core.UnitOfWork;
using TestTask.Infrastructure.SQL.Contexts;

namespace TestTask.Infrastructure.SQL.UnitOfWork;

internal sealed class EFUnitOfWork(
    ApplicationDbContext context,
    ILogger<EFUnitOfWork> logger
    ) : IUnitOfWork
{
    public async Task CommitChangesAsync()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                logger.LogError(
                    "Concurrency error in entity: {Entity}, Id: {Id}",
                    entry.Entity.GetType().Name,
                    entry.Property("Id")?.CurrentValue);
            }
            throw;
        }
        //всякие другие ошибки...
    }

    public void CommitChanges() => context.SaveChanges();
}
