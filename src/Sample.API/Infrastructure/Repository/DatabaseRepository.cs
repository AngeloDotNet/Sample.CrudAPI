using Sample.API.Entity;
using Sample.API.Infrastructure.Interfaces;
using Sample.API.Shared;

namespace Sample.API.Infrastructure.Repository;

public class DatabaseRepository : Database<PersonEntity>, IDatabaseRepository
{
    public DatabaseRepository(DataDbContext dbContext) : base(dbContext)
    {
    }
}