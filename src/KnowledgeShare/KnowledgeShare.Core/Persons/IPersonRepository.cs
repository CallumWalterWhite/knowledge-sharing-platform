namespace KnowledgeShare.Core.Persons;

public interface IPersonRepository
{
    Task<Person?> GetPersonByUserIdAsync(string userId);
    
    Task AddAsync(Person person);
}