namespace KnowledgeShare.Core.Persons;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Person?> GetPersonByUserIdAsync(string userId)
    {
        Person? person = await _personRepository.GetPersonByUserIdAsync(userId);
        return person;
    }

    public async Task CreatePersonAsync(CreatePersonDto createPersonDto)
    {
        Person person = Person.Create(createPersonDto.UserId, createPersonDto.Name);
        await _personRepository.AddAsync(person);
    }
}