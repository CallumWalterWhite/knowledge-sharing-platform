using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class PostBaseRepository
{
    private readonly IAsyncSession _session;

    public PostBaseRepository(IAsyncSession session)
    {
        _session = session;
    }
    
    public async Task AddAuthorAsync(Post post)
    {
        Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
        {
            {"postId", post.Id.ToString() },
            {"personId", post.GetAuthor().Id.ToString() }
        };

        await _session.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Person), (b:Post) " +
                           "WHERE a.id = $personId AND b.id = $postId " +
                           "CREATE (a)-[:WROTE]->(b)";
            await tx.RunAsync(query,
                statementParameters);
        });
    }
    
    public async Task AddTagsAsync(Post post)
    {
        foreach(Tag tag in post.Tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", post.Id.ToString() },
                {"tagId", tag.Id.ToString() }
            };

            await _session.ExecuteWriteAsync(async tx =>
            {
                string query = "MATCH (a:Post { id: $postId }), (t:Tag { id: $tagId }) " +
                               "MERGE (a)-[:HAS_TAG]->(t)";
                await tx.RunAsync(query,
                    statementParameters);
            });
        }
    }
    
    public async Task DeleteAsync(Guid postId)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", postId.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("MATCH (n:Post) WHERE n.id = $id DETACH DELETE n",
                statementParameters);
        });
    }
}