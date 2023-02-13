using Sample.API.Entity;
using Sample.API.Infrastructure.Interfaces;
using Sample.API.Shared;

namespace Sample.API.Infrastructure.Repository;

public class CommandRepository : Command<PersonEntity>, ICommandRepository
{
    public CommandRepository(DataDbContext dbContext) : base(dbContext)
    {
    }
}