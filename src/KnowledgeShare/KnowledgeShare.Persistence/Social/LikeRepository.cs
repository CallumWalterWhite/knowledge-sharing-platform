using KnowledgeShare.Core.Social;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Social;

public class LikeRepository : ILikeRepository
{
    private readonly IAsyncSession _session;

    public LikeRepository(IAsyncSession session)
    {
        _session = session;
    }

    public async Task CreateLikeAsync(Guid personId, Guid postId)
    {
        Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
        {
            {"personId", personId.ToString() },
            {"postId", postId.ToString() }
        };
        
        await _session.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Post { id: $postId }), (p:Person { id: $personId }) " +
                           "MERGE (a)-[:LIKED]->(p)";
            await tx.RunAsync(query,
                statementParameters);
        });
    }
    
    public async Task<IEnumerable<Guid>> GetPeopleIdsByPostIdAsync(Guid postId)
    {
        List<Guid> results = new List<Guid>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"postId", postId.ToString() },
        };
        IResultCursor cursor = await _session.RunAsync(
            "MATCH (n) WHERE (n:Post) AND n.id = $postId " +
            "MATCH (n)-[r:LIKED]->(p) " + 
            "RETURN p.id", statementParameters);
        while (await cursor.FetchAsync())
        {
            object? personId = cursor.Current["p.id"];
            if (personId is not null)
            {
                results.Add(Guid.Parse(personId.ToString()));   
            }
        }

        return results;
    }
}