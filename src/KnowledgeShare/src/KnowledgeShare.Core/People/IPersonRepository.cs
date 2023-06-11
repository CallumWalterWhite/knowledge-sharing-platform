namespace KnowledgeShare.Core.People;

public interface IPersonRepository
{
    Task<Person?> GetPersonByUserIdAsync(string userId);
    
    Task AddAsync(Person person);

    Task SetAdminAsync(Guid id, bool admin);
    
    Task<IEnumerable<Person>> GetAllAsync();
}