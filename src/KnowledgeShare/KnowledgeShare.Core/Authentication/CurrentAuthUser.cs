using KnowledgeShare.Core.Persons;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.Persons.Person;

namespace KnowledgeShare.Core.Authentication;

public class CurrentAuthUser : ICurrentAuthUser
{
    private bool _isPersonCreated;
    private Persons.Person? _person;

    private readonly IPersonRepository personRepository;
    
    public CurrentAuthUser(GraphServiceClient graphServiceClient, IPersonRepository personRepository)
    {
        this.personRepository = personRepository;
        User = graphServiceClient.Me.Request().GetAsync().ConfigureAwait(true).GetAwaiter().GetResult();
    }
    
    public User User { get; }
    public async Task<Persons.Person?> GetPersonAsync()
    {
        if (_isPersonCreated) return _person;
        _person = await personRepository.GetPersonByUserIdAsync(User.Id);
        _isPersonCreated = true;
        return _person;

    }
}