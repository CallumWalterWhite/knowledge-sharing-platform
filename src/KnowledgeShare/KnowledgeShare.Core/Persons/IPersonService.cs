namespace KnowledgeShare.Core.Persons;

public interface IPersonService
{
    Task<Person?> GetPersonByUserIdAsync(string userId);
    
    Task CreatePersonAsync(CreatePersonDto createPersonDto);
}