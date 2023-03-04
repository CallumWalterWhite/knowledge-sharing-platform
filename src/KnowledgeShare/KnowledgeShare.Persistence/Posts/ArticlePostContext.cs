using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Entities.Tags;
using KnowledgeShare.Core.Posts.Types;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class ArticlePostContext<TPost> : IPostContext<ArticlePost>, IArticlePostContext
{
    private readonly IAsyncSession _session;
    
    public ArticlePostContext(IAsyncSession session)
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
            {"id", post.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:ArticlePost {id: $id, title: $title, summary: $summary, link: $link}) ",
                statementParameters);
        });
        
        await AddTagsAsync(post.Id, post.Tags);
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
            object? id = cursor.Current["post.id"];
            object? title = cursor.Current["post.title"];
            object? summary = cursor.Current["post.summary"];
            object? link = cursor.Current["post.link"];
            articleSummaries.Add(new ArticlePost(
                Guid.Parse(id.ToString()),
                title.ToString() ?? string.Empty,
                summary.ToString() ?? string.Empty,
                link.ToString() ?? string.Empty
            ));
        }

        return articleSummaries;
    }

    private async Task AddTagsAsync(Guid id, IEnumerable<Tag> tags)
    {
        foreach(Tag tag in tags)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"postId", id.ToString() },
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
}