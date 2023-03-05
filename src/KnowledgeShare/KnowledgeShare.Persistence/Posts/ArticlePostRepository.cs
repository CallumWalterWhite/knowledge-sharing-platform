using KnowledgeShare.Core.Persons;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class ArticlePostRepository<TPost> : IPostRepository<ArticlePost>
{
    private readonly IAsyncSession _session;
    
    public ArticlePostRepository(IAsyncSession session)
    {
        _session = session;
    }
    
    public async Task CreateAsync(ArticlePost post)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", post.GetTitle() },
            {"summary", post.GetSummary() },
            {"link", post.GetLink() },
            {"createdDateTime", post.GetDateTimeCreated() },
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:ArticlePost {id: $id, title: $title, summary: $summary, link: $link, createdDateTime: $createdDateTime}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post);
        await AddAuthor(post);
    }

    public Task DeleteAsync(ArticlePost post)
    {
        throw new NotImplementedException();
    }

    public Task AddTags(ArticlePost post)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ArticlePost>> GetAllAsync()
    {
        List<ArticlePost> articleSummaries = new List<ArticlePost>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:ArticlePost) RETURN post.id, post.title, post.summary, post.link");
        while (await cursor.FetchAsync())
        {
            articleSummaries.Add(CreateArticlePostFromResult(cursor.Current));
        }

        return articleSummaries;
    }

    private async Task AddTagsAsync(ArticlePost post)
    {
        foreach(Tag tag in post.Tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", post.Id.ToString() },
                {"value", tag.Value }
            };

            await _session.ExecuteWriteAsync(async tx =>
            {
                string query = "MATCH (a:ArticlePost { id: $postId }), (t:Tag { value: $value }) " +
                               "MERGE (a)-[:HAS_TAG]->(t)";
                await tx.RunAsync(query,
                    statementParameters);
            });
        }
    }

    private async Task AddAuthor(ArticlePost post)
    {
        Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
        {
            {"postId", post.Id.ToString() },
            {"personId", post.GetAuthor().Id.ToString() }
        };

        await _session.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (a:Person), (b:ArticlePost) " +
                           "WHERE a.id = $personId AND b.id = $postId " +
                           "CREATE (a)-[:WROTE]->(b)";
            await tx.RunAsync(query,
                statementParameters);
        });
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
            title.ToString() ?? string.Empty,
            summary.ToString() ?? string.Empty,
            link.ToString() ?? string.Empty
        );
    }
}