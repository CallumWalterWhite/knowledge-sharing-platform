using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class BookPostRepository : PostBaseRepository, IPostRepository<BookPost>
{
    private readonly IAsyncSession _session;
    
    public BookPostRepository(IAsyncSession session) : base(session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(BookPost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"type", "BookPost" },
            {"summary", post.GetSummary() },
            {"author", post.GetAuthor() },
            {"createdDateTime", post.GetDateTimeCreated() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:Post {id: $id, title: $title, summary: $summary, author: $author, type: $type, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post);
        await AddAuthorAsync(post);
    }

    public Task DeleteAsync(BookPost post)
    {
        throw new NotImplementedException();
    }
    
    public async Task<IEnumerable<BookPost>> GetAllAsync()
    {
        List<BookPost> bookPosts = new List<BookPost>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post) RETURN post.id, post.title, post.author, post.summary WHERE type='BookPost'");
        while (await cursor.FetchAsync())
        {
            bookPosts.Add(CreateBookPostFromResult(cursor.Current));
        }

        return bookPosts;
    }

    public async Task<BookPost?> GetByIdAsync(Guid id)
    {
        BookPost? post = null;
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"value", id.ToString() }
        };
        IResultCursor cursor = await _session.RunAsync("MATCH (post:Post WHERE post.id = $value) MATCH (post)-[r:WROTE]-(person) RETURN post.id, post.createdDateTime, post.title, post.author, post.summary, person.id, person.userId, person.name", statementParameters);
        while (await cursor.FetchAsync())
        {
            post = CreateBookPostFromResult(cursor.Current);
        }

        return post;
    }

    private BookPost CreateBookPostFromResult(IRecord record)
    {
        object? id = record["post.id"];
        object? title = record["post.title"];
        object? summary = record["post.summary"];
        object? author = record["post.author"];
        object? createdDateTime = record["post.createdDateTime"];
        object? personId = record["person.id"];
        object? userId = record["person.userId"];
        object? name = record["person.name"];
        return new BookPost(
            Guid.Parse(id.ToString() ?? string.Empty),
            new Person(Guid.Parse(personId?.ToString() ?? string.Empty), userId.ToString() ?? string.Empty, name.ToString() ?? string.Empty),
            DateTime.Parse(createdDateTime.ToString() ?? string.Empty),
            title?.ToString() ?? string.Empty,
            author?.ToString() ?? string.Empty,
            summary?.ToString() ?? string.Empty
        );
    }
}