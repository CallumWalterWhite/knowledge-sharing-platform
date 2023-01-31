using Neo4j.Driver;

namespace KnowledgeShare.Core.Repository;

public class Repository<T> : IRepository<T>
{
    private readonly IAsyncSession _asyncSession;
    
    public Repository(IAsyncSession asyncSession)
    {
        _asyncSession = asyncSession;
    }

    public async Task AddAsync(string message)
    {
        var greeting = await _asyncSession.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync("CREATE (a:Greeting) " +
                                "SET a.message = $message " +
                                "RETURN a.message + ', from node ' + id(a)",
                new {message});
            return (await result.SingleAsync())[0].As<string>();
        });
    }

    public async Task<string> GetAsync()
    {
        var greeting = await _asyncSession.ExecuteReadAsync(async tx =>
        {
            var result = await tx.RunAsync("RETURN a.message + ', from node ' + id(a)");
            return (await result.SingleAsync())[0].As<string>();
        });
        return greeting;
    }
}