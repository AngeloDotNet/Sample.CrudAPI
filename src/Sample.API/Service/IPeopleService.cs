namespace Sample.API.Service;

public interface IPeopleService
{
    Task<List<PersonEntity>> GetListItemAsync();
    Task<PersonEntity> GetItemAsync(Guid id);
    Task CreateItemAsync(PersonEntity item);
    Task UpdateItemAsync(PersonEntity item);
    Task DeleteItemAsync(PersonEntity item);
}