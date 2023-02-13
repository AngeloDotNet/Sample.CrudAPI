using Sample.API.Entity;
using Sample.API.Shared.Interfaces;

namespace Sample.API.Infrastructure.Interfaces;

public interface ICommandRepository : ICommand<PersonEntity>
{
    // Add your custom code here
}