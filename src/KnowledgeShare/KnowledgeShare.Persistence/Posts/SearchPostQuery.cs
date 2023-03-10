using KnowledgeShare.Core.Posts;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class SearchPostQuery : ISearchPostQuery
{
    private readonly IAsyncSession _session;

    public SearchPostQuery(IAsyncSession session)
    {
        _session = session;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(string searchQuery)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"searchTerm", searchQuery },
        };
        IResultCursor cursor = await _session.RunAsync(
            "MATCH (n) WHERE (n:ArticlePost OR n:BookPost)" +
            "MATCH (n)-[r:HAS_TAG]->(t)" +
            "WHERE toLower(n.title) CONTAINS toLower($searchTerm) OR toLower(t.value) CONTAINS toLower($searchTerm)" +
            "RETURN n.id, n.title", statementParameters);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                results.Add(
                    new SearchPostResultDto()
                    {
                        Id = Guid.Parse(cursor.Current["n.id"].ToString()),
                        Title = cursor.Current["n.title"].ToString()
                    }
                );
            }
        }

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> RecommendAsync(Guid personId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"searchTerm", personId.ToString() },
        };
        IResultCursor cursor = await _session.RunAsync(
            "MATCH (n) WHERE (n:ArticlePost OR n:BookPost) " +
            "RETURN n.id, n.summary, n.title " +
            "ORDER BY n.createdDateTime DESC", statementParameters);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                results.Add(
                    new SearchPostResultDto()
                    {
                        Id = Guid.Parse(cursor.Current["n.id"].ToString()),
                        Title = cursor.Current["n.title"].ToString(),
                        Summary = cursor.Current["n.summary"].ToString()
                    }
                );
            }
        }

        return results;
    }
}