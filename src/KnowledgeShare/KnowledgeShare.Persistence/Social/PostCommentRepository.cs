using KnowledgeShare.Core.Social;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Social;

public class PostCommentRepository : IPostCommentRepository
{
    private readonly IAsyncSession _asyncSession;

    public PostCommentRepository(IAsyncSession asyncSession)
    {
        _asyncSession = asyncSession;
    }

    public async Task CreatePostCommentAsync(PostComment postComment)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", postComment.Id.ToString() },
            {"createdDateTime", postComment.DateTimeCreated },
            {"value", postComment.CommentText }
        };
        await _asyncSession.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (c:Comment {value: $value, id: $id, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        statementParameters = new Dictionary<string, object>
        {
            {"postId", postComment.PostId.ToString() },
            {"commentId", postComment.Id.ToString() }
        };
        await _asyncSession.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Post { id: $postId }), (c:Comment { id: $commentId }) " +
                           "MERGE (a)-[:HAS_COMMENT]->(c)";
            await tx.RunAsync(query,
                statementParameters);
        });
        
        statementParameters = new Dictionary<string, object>
        {
            {"personId", postComment.PersonId.ToString() },
            {"commentId", postComment.Id.ToString() }
        };
        await _asyncSession.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Person { id: $personId }), (c:Comment { id: $commentId }) " +
                           "MERGE (a)-[:WROTE_COMMENT]->(c)";
            await tx.RunAsync(query,
                statementParameters);
        });
    }

    public async Task<IEnumerable<PostCommentDto>> GetPostCommentsAsync(Guid postId)
    {
        List<PostCommentDto> results = new List<PostCommentDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"postId", postId.ToString() },
        };
        IResultCursor cursor = await _asyncSession.RunAsync(
            "MATCH (p:Post)-[:HAS_COMMENT]->(c:Comment)<-[:WROTE_COMMENT]-(a:Person) " +
            "WHERE p.id = $postId " + 
            "RETURN c.value, c.createdDateTime, a.name", statementParameters);
        while (await cursor.FetchAsync())
        {
            object? comment = cursor.Current["c.value"];
            object? personName = cursor.Current["a.name"];
            object? createdDateTime = cursor.Current["c.createdDateTime"];
            if (comment is not null)
            {
                results.Add(new PostCommentDto(personName.ToString()!, comment.ToString()!, DateTime.Parse(createdDateTime.ToString()!)));   
            }
        }

        return results;
    }
}