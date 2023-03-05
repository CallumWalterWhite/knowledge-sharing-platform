using Microsoft.Graph;

namespace KnowledgeShare.Core.Authentication;

public class CurrentAuthUser : ICurrentAuthUser
{
    public CurrentAuthUser(GraphServiceClient graphServiceClient)
    {
        User = graphServiceClient.Me.Request().GetAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
    
    public User User { get; }
    public Persons.Person Person { get; }
}