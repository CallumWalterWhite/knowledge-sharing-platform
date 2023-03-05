using KnowledgeShare.Core.Persons;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class BookPostRepository<TPost> : IPostRepository<BookPost>
{
    private readonly IAsyncSession _session;
    
    public BookPostRepository(IAsyncSession session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(BookPost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"summary", post.GetSummary() },
            {"createdDateTime", post.GetDateTimeCreated() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:BookPost {id: $id, title: $title, summary: $summary, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post);
        await AddAuthor(post);
    }

    public Task DeleteAsync(BookPost post)
    {
        throw new NotImplementedException();
    }

    public Task AddTags(BookPost post)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookPost>> GetAllAsync()
    {
        List<BookPost> bookPosts = new List<BookPost>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:BookPost) RETURN post.id, post.title, post.summary, post.link");
        while (await cursor.FetchAsync())
        {
            bookPosts.Add(CreateBookPostFromResult(cursor.Current));
        }

        return bookPosts;
    }

    private async Task AddTagsAsync(BookPost post)
    {
        foreach(Tag tag in post.Tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", post.Id.ToString() },
                {"tagId", tag.ToString() }
            };

            await _session.ExecuteWriteAsync(async tx =>
            {
                string query = "MATCH (a:BookPost { id: $postId }), (t:Tag { id: $tagId }) " +
                               "MERGE (a)-[:HAS_TAG]->(t)";
                await tx.RunAsync(query,
                    statementParameters);
            });
        }
    }
    
    private async Task AddAuthor(BookPost post)
    {
        Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
        {
            {"postId", post.Id.ToString() },
            {"personId", post.GetAuthor().Id.ToString() }
        };

        await _session.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Person), (b:BookPost) " +
                           "WHERE a.id = $personId AND b.id = $postId " +
                           "CREATE (a)-[:WROTE]->(b)";
            await tx.RunAsync(query,
                statementParameters);
        });
    }
    
    private BookPost CreateBookPostFromResult(IRecord record)
    {
        object? id = record["post.id"];
        object? title = record["post.title"];
        object? summary = record["post.summary"];
        object? createdDateTime = record["post.createdDateTime"];
        object? personId = record["person.id"];
        object? userId = record["person.userId"];
        object? name = record["person.name"];
        return new BookPost(
            Guid.Parse(id.ToString()),
            new Person(Guid.Parse(personId.ToString()), userId.ToString(), name.ToString()),
            DateTime.Parse(createdDateTime.ToString()),
            title.ToString() ?? string.Empty,
            summary.ToString() ?? string.Empty
        );
    }
}