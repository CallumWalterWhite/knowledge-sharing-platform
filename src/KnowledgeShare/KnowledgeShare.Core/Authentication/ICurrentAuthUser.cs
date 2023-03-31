using Microsoft.Graph;
using Person = KnowledgeShare.Core.People.Person;

namespace KnowledgeShare.Core.Authentication;

public interface ICurrentAuthUser
{
    User User { get; }

    Task<Person?> GetPersonAsync();
}