namespace KnowledgeShare.Core.People;

public interface IPersonService
{
    Task<Person?> GetPersonByUserIdAsync(string userId);
    
    Task CreatePersonAsync(CreatePersonDto createPersonDto);
}