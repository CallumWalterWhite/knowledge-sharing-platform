using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class PostBaseRepository
{
    private readonly IAsyncSession _session;

    protected PostBaseRepository(IAsyncSession session)
    {
        _session = session;
    }

    protected async Task AddAuthorAsync(Post post)
    {
        Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
        {
            {"postId", post.Id.ToString() },
            {"personId", post.GetAuthor().Id.ToString() }
        };

        await using var transaction = await _session.BeginTransactionAsync();
        string query = "MATCH (a:Person), (b:Post) " +
                       "WHERE a.id = $personId AND b.id = $postId " +
                       "CREATE (a)-[:WROTE]->(b)";
        await transaction.RunAsync(query,
            statementParameters);
        await transaction.CommitAsync();
    }

    protected async Task AddTagsAsync(Post post)
    {
        await using var transaction = await _session.BeginTransactionAsync();
        foreach(Tag tag in post.Tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", post.Id.ToString() },
                {"tagId", tag.Id.ToString() }
            };
            string query = "MATCH (a:Post { id: $postId }), (t:Tag { id: $tagId }) " +
                           "MERGE (a)-[:HAS_TAG]->(t)";
            await transaction.RunAsync(query,
                statementParameters);
        }
        await transaction.CommitAsync();
    }

    protected async Task RemoveTagsAsync(Post post)
    {
        await using var transaction = await _session.BeginTransactionAsync();
        foreach(Tag tag in post.Tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", post.Id.ToString() },
                {"tagId", tag.Id.ToString() }
            };

            string query = "MATCH (a:Post { id: $postId })-[t:HAS_TAG]->()" +
                           "DELETE t";
            await transaction.RunAsync(query,
                statementParameters);
        }
        await transaction.CommitAsync();
    }

    protected async Task DeleteAsync(Guid postId)
    {
        await using var transaction = await _session.BeginTransactionAsync();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", postId.ToString() }
        };
        await transaction.RunAsync("MATCH (n:Post) WHERE n.id = $id DETACH DELETE n",
            statementParameters);
        await transaction.CommitAsync();
    }
}