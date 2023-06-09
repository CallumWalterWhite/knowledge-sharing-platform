namespace KnowledgeShare.Core.People;

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
        Person person = Person.Create(createPersonDto.UserId, createPersonDto.Name, createPersonDto.DataImage);
        await _personRepository.AddAsync(person);
    }

    public async Task<IEnumerable<Person>> GetAllPeopleAsync()
    {
        return await _personRepository.GetAllAsync();
    }

    public async Task SetIsAdminAsync(Guid personId, bool isAdmin)
    {
        await _personRepository.SetAdminAsync(personId, isAdmin);
    }
}