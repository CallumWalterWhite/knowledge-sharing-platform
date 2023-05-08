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
            "MATCH (n) WHERE (n:Post)" +
            "MATCH (n)-[r:HAS_TAG]->(t)" +
            "WHERE toLower(n.title) CONTAINS toLower($searchTerm) OR toLower(t.value) CONTAINS toLower($searchTerm)" +
            "RETURN n.id, n.summary, n.title, n.type", statementParameters);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                results.Add(
                    new SearchPostResultDto()
                    {
                        Id = Guid.Parse(cursor.Current["n.id"].ToString()),
                        Title = cursor.Current["n.title"].ToString(),
                        Summary = cursor.Current["n.summary"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
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
            "MATCH (n) WHERE (n:Post) " +
            "MATCH (n)-[r:WROTE]-(person) " + 
            "RETURN n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name " +
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
                        Summary = cursor.Current["n.summary"].ToString(),
                        CreatedDate = cursor.Current["n.createdDateTime"].ToString(),
                        UserCreatedName = cursor.Current["person.name"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
                    }
                );
            }
        }

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsAsync(Guid personId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"person_id", personId.ToString() },
        };
        IResultCursor cursor = await _session.RunAsync(
            "MATCH (p:Person)-[:WROTE]->(post:Post) " +
            "WHERE p.id = $person_id "  +
            "RETURN post.id, post.summary, post.title, post.type", statementParameters);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                results.Add(
                    new SearchPostResultDto()
                    {
                        Id = Guid.Parse(cursor.Current["post.id"].ToString()),
                        Title = cursor.Current["post.title"].ToString(),
                        Summary = cursor.Current["post.summary"].ToString(),
                        Type = cursor.Current["post.type"].ToString()
                    }
                );
            }
        }

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsByTagAsync(Guid tagId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"tagId", tagId.ToString() },
        };
        IResultCursor cursor = await _session.RunAsync(
            "MATCH (n) WHERE (n:Post) " +
            "MATCH (n)-[r:HAS_TAG]->(t) " +
            "WHERE t.id = $tagId " +
            "RETURN n.id, n.summary, n.title, n.type", statementParameters);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                results.Add(
                    new SearchPostResultDto()
                    {
                        Id = Guid.Parse(cursor.Current["n.id"].ToString()),
                        Title = cursor.Current["n.title"].ToString(),
                        Summary = cursor.Current["n.summary"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
                    }
                );
            }
        }

        return results;
    }
}