using Microsoft.EntityFrameworkCore;
using Sample.API.Shared.Interfaces;

namespace Sample.API.Shared;

public class Database<T> : IDatabase<T> where T : class
{
    public DbContext DbContext { get; }

    public Database(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        DbContext.Entry(entity).State = EntityState.Detached;

        return entity;
    }
}