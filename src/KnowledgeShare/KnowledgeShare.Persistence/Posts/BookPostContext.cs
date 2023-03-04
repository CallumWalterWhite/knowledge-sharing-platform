using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Entities.Tags;
using KnowledgeShare.Core.Posts.Types;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class BookPostContext<TPost> : IPostContext<BookPost>, IBookPostContext
{
    private readonly IAsyncSession _session;
    
    public BookPostContext(IAsyncSession session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(BookPost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"summary", post.GetSummary() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:BookPost {id: $id, title: $title, summary: $summary, link: $link}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post.Id, post.Tags);
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
            object? id = cursor.Current["post.id"];
            object? title = cursor.Current["post.title"];
            object? summary = cursor.Current["post.summary"];
            bookPosts.Add(new BookPost(
                Guid.Parse(id.ToString()),
                title.ToString() ?? string.Empty,
                summary.ToString() ?? string.Empty
            ));
        }

        return bookPosts;
    }

    private async Task AddTagsAsync(Guid id, IEnumerable<Tag> tags)
    {
        foreach(Tag tag in tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", id.ToString() },
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
}