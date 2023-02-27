using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Entities.Content;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Content;

public class ArticleSummaryContext : IArticleSummaryContext
{
    private readonly IAsyncSession _session;

    public ArticleSummaryContext(IAsyncSession session)
    {
        _session = session;
    }
    
    public async Task AddAsync(ArticleSummary articleSummary)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"title", articleSummary.Title },
            {"summary", articleSummary.Summary },
            {"link", articleSummary.Link },
            {"id", articleSummary.Id.ToString() }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (post:ArticleSummary {id: $id, title: $title, summary: $summary, link: $link}) ",
                statementParameters);
        });
    }

    public async Task<IEnumerable<ArticleSummary>> GetAllAsync()
    {
        List<ArticleSummary> articleSummaries = new List<ArticleSummary>();
        IResultCursor cursor = await _session.RunAsync("MATCH (post:ArticleSummary) RETURN post.id, post.title, post.summary, post.link");
        while (await cursor.FetchAsync())
        {
            object? id = cursor.Current["post.id"];
            object? title = cursor.Current["post.title"];
            object? summary = cursor.Current["post.summary"];
            object? link = cursor.Current["post.link"];
            articleSummaries.Add(new ArticleSummary(
                    Guid.Parse(id.ToString()),
                    title.ToString() ?? string.Empty,
                    summary.ToString() ?? string.Empty,
                    link.ToString() ?? string.Empty
                ));
        }

        return articleSummaries;
    }
}