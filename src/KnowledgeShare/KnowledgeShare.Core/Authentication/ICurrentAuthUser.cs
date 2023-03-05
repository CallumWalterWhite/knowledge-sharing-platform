using Microsoft.Graph;

namespace KnowledgeShare.Core.Authentication;

public interface ICurrentAuthUser
{
    User User { get; }

    Task<Persons.Person?> GetPersonAsync();
}