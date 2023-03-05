using Microsoft.Graph;

namespace KnowledgeShare.Core.Authentication;

public interface ICurrentAuthUser
{
    User User { get; }
    
    Persons.Person Person { get; }
}