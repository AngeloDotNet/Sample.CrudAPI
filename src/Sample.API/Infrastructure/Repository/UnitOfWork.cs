using Sample.API.Infrastructure.Interfaces;

namespace Sample.API.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataDbContext dbContext;
    public IDatabaseRepository DatabaseRepository { get; }
    public ICommandRepository CommandRepository { get; }

    public UnitOfWork(DataDbContext dbContext, IDatabaseRepository databaseRepository, ICommandRepository commandRepository)
    {
        this.dbContext = dbContext;

        DatabaseRepository = databaseRepository;
        CommandRepository = commandRepository;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            dbContext.Dispose();
        }
    }
}