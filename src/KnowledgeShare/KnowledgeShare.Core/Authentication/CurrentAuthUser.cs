using KnowledgeShare.Core.People;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.People.Person;

namespace KnowledgeShare.Core.Authentication;

public class CurrentAuthUser : ICurrentAuthUser
{
    private bool _isPersonCreated;
    private Person? _person;

    private readonly IPersonService _personService;
    
    public CurrentAuthUser(GraphServiceClient graphServiceClient, IPersonService personService)
    {
        _personService = personService;
        User = graphServiceClient.Me.Request().GetAsync().ConfigureAwait(true).GetAwaiter().GetResult();
    }
    
    public User User { get; }
    public async Task<Person?> GetPersonAsync()
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