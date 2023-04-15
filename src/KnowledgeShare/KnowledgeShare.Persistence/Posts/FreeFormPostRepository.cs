using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class FreeFormPostRepository : PostBaseRepository, IPostRepository<FreeFormPost>
{
    private readonly IAsyncSession _session;
    
    public FreeFormPostRepository(IAsyncSession session) : base(session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(FreeFormPost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"type", "FreeFormPost" },
            {"summary", post.GetSummary() },
            {"body", post.GetBody() },
            {"createdDateTime", post.GetDateTimeCreated() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:Post {id: $id, title: $title, summary: $summary, body: $body, type: $type, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post);
        await AddAuthorAsync(post);
    }

    public new async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync(id);
    }
    
    public async Task<IEnumerable<FreeFormPost>> GetAllAsync()
    {
        List<FreeFormPost> articleSummaries = new List<FreeFormPost>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post) MATCH (post)-[r:WROTE]-(person) RETURN post.id, post.createdDateTime, post.title, post.summary, post.body, person.id, person.userid, person.name, person.picture WHERE type='ArticlePost'");
        while (await cursor.FetchAsync())
        {
            articleSummaries.Add(CreateFreeFormPostFromResult(cursor.Current));
        }

        return articleSummaries;
    }

    public async Task<FreeFormPost?> GetByIdAsync(Guid id)
    {
        FreeFormPost? post = null;
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"value", id.ToString() }
        };
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post WHERE post.id = $value) MATCH (post)-[r:WROTE]-(person) RETURN post.id, post.createdDateTime, post.title, post.summary, post.body, person.id, person.userId, person.name, person.picture", statementParameters);
        while (await cursor.FetchAsync())
        {
            post = CreateFreeFormPostFromResult(cursor.Current);
        }

        return post;
    }

    private FreeFormPost CreateFreeFormPostFromResult(IRecord record)
    {
        object? id = record["post.id"];
        object? title = record["post.title"];
        object? summary = record["post.summary"];
        object? body = record["post.body"];
        object? createdDateTime = record["post.createdDateTime"];
        object? personId = record["person.id"];
        object? userId = record["person.userId"];
        object? name = record["person.name"];
        object? picture = record["person.picture"];
        return new FreeFormPost(
            Guid.Parse(id.ToString()),
            new Person(Guid.Parse(personId.ToString()), userId.ToString(), name.ToString(), picture.ToString()),
            DateTime.Parse(createdDateTime.ToString()),
            title?.ToString() ?? string.Empty,
            body?.ToString() ?? string.Empty,
            summary?.ToString() ?? string.Empty
        );
    }
}