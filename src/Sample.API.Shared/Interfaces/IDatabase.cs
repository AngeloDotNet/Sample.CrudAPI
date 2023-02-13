namespace Sample.API.Shared.Interfaces;

public interface IDatabase<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
}