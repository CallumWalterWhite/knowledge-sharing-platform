namespace KnowledgeShare.Core.People;

public interface IPersonRepository
{
    Task<Person?> GetPersonByUserIdAsync(string userId);
    
    Task AddAsync(Person person);

    Task<Person?> GetAsync(Guid id);
}