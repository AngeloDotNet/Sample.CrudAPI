using Sample.API.Entity;
using Sample.API.Shared.Interfaces;

namespace Sample.API.Infrastructure.Interfaces;

public interface IDatabaseRepository : IDatabase<PersonEntity>
{
    // Add your custom code here
}