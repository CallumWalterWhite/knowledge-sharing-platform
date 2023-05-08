using Microsoft.Graph;
using Person = KnowledgeShare.Core.People.Person;

namespace KnowledgeShare.Core.Authentication;

public interface ICurrentAuthUser
{
    void SetCurrentAuthUser(Person person);
    
    Task<Person?> GetPersonAsync();
}