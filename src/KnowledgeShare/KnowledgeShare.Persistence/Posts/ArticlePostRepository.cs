using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class ArticlePostRepository : PostBaseRepository, IPostRepository<ArticlePost>
{
    private readonly IAsyncSession _session;
    
    public ArticlePostRepository(IAsyncSession session) : base(session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(ArticlePost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"type", "ArticlePost" },
            {"summary", post.GetSummary() },
            {"link", post.GetLink() },
            {"createdDateTime", post.GetDateTimeCreated() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:Post {id: $id, title: $title, summary: $summary, link: $link, type: $type, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post);
        await AddAuthorAsync(post);
    }

    public Task DeleteAsync(ArticlePost post)
    {
        throw new NotImplementedException();
    }
    
    public async Task<IEnumerable<ArticlePost>> GetAllAsync()
    {
        List<ArticlePost> articleSummaries = new List<ArticlePost>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post) MATCH (post)-[r:WROTE]-(person) RETURN post.id, post.createdDateTime, post.title, post.summary, post.link, person.id, person.userid, person.name WHERE type='ArticlePost'");
        while (await cursor.FetchAsync())
        {
            articleSummaries.Add(CreateArticlePostFromResult(cursor.Current));
        }

        return articleSummaries;
    }

    public async Task<ArticlePost?> GetByIdAsync(Guid id)
    {
        ArticlePost? post = null;
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"value", id.ToString() }
        };
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post WHERE post.id = $value) MATCH (post)-[r:WROTE]-(person) RETURN post.id, post.createdDateTime, post.title, post.summary, post.link, person.id, person.userId, person.name", statementParameters);
        while (await cursor.FetchAsync())
        {
            post = CreateArticlePostFromResult(cursor.Current);
        }

        return post;
    }

    private ArticlePost CreateArticlePostFromResult(IRecord record)
    {
        object? id = record["post.id"];
        object? title = record["post.title"];
        object? summary = record["post.summary"];
        object? link = record["post.link"];
        object? createdDateTime = record["post.createdDateTime"];
        object? personId = record["person.id"];
        object? userId = record["person.userId"];
        object? name = record["person.name"];
        return new ArticlePost(
            Guid.Parse(id.ToString()),
            new Person(Guid.Parse(personId.ToString()), userId.ToString(), name.ToString()),
            DateTime.Parse(createdDateTime.ToString()),
            title?.ToString() ?? string.Empty,
            link?.ToString() ?? string.Empty,
            summary?.ToString() ?? string.Empty
        );
    }
}