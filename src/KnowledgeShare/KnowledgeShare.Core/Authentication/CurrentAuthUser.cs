using KnowledgeShare.Core.Persons;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.Persons.Person;

namespace KnowledgeShare.Core.Authentication;

public class CurrentAuthUser : ICurrentAuthUser
{
    private bool _isPersonCreated;
    private Persons.Person? _person;

    private readonly IPersonRepository _personRepository;

    private readonly IPersonService _personService;

    private readonly GraphServiceClient _graphServiceClient;
    
    public CurrentAuthUser(GraphServiceClient graphServiceClient, IPersonRepository personRepository, IPersonService personService)
    {
        _graphServiceClient = graphServiceClient;
        _personRepository = personRepository;
        _personService = personService;
        User = graphServiceClient.Me.Request().GetAsync().ConfigureAwait(true).GetAwaiter().GetResult();
    }
    
    public User User { get; }
    public async Task<Persons.Person?> GetPersonAsync()
    {
        if (_isPersonCreated) return _person;
        Person? person = await _personService.GetPersonByUserIdAsync(User.Id);
        if (person == null)
        {
            await _personService.CreatePersonAsync(new CreatePersonDto(User.Id, User.DisplayName));
        } 
        _person = await _personService.GetPersonByUserIdAsync(User.Id);
        _isPersonCreated = true;
        return _person;

    }
}