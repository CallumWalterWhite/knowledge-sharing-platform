﻿using KnowledgeShare.Core.Posts;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class SearchPostQuery : ISearchPostQuery
{
    private readonly IAsyncSession _session;

    public SearchPostQuery(IAsyncSession session)
    {
        _session = session;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(SearchPostDto searchPostDto)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"searchTerm", searchPostDto.Title },
            {"tags", searchPostDto.Tags.ToArray() },
            {"skip", searchPostDto.Skip },
        };
        string query = "MATCH (n) WHERE (n:Post) " +
                       "MATCH (n)-[w:WROTE]-(person) " + 
                       "MATCH (n)-[r:HAS_TAG]->(t) " +
                       "RETURN distinct n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
                       "ORDER BY n.createdDateTime DESC " +
                       "SKIP $skip " +
                       "LIMIT 10";
        if (searchPostDto.Tags.Any() && string.IsNullOrWhiteSpace(searchPostDto.Title))
        {
            query = "MATCH (n) WHERE (n:Post)" +
                    "MATCH (n)-[w:WROTE]-(person) " + 
                    "MATCH (n)-[r:HAS_TAG]->(t)" +
                    "WHERE toLower(t.value) IN $tags " +
                    "RETURN distinct n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
                    "ORDER BY n.createdDateTime DESC " +
                    "SKIP $skip " +
                    "LIMIT 10";
        }
        else if (searchPostDto.Tags.Any() is false && string.IsNullOrWhiteSpace(searchPostDto.Title) is false)
        {
            query = "MATCH (n) WHERE (n:Post)" +
                    "MATCH (n)-[w:WROTE]-(person) " + 
                    "MATCH (n)-[r:HAS_TAG]->(t)" +
                    "WHERE toLower(n.title) CONTAINS toLower($searchTerm) OR toLower(n.summary) CONTAINS toLower($searchTerm) " +
                    "RETURN distinct n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
                    "ORDER BY n.createdDateTime DESC " +
                    "SKIP $skip " +
                    "LIMIT 10";
        }
        else if (searchPostDto.Tags.Any() || string.IsNullOrWhiteSpace(searchPostDto.Title) is false)
        {
            query = "MATCH (n) WHERE (n:Post)" +
                    "MATCH (n)-[w:WROTE]-(person) " + 
                    "MATCH (n)-[r:HAS_TAG]->(t)" +
                    "WHERE (toLower(n.title) CONTAINS toLower($searchTerm) OR toLower(n.summary) CONTAINS toLower($searchTerm)) AND toLower(t.value) IN $tags " +
                    "RETURN distinct n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
                    "ORDER BY n.createdDateTime DESC " +
                    "SKIP $skip " +
                    "LIMIT 10";
        }

        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(query, statementParameters);
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
                        UserPhoto = cursor.Current["person.picture"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
                    }
                );
            }
        } 
        await transaction.CommitAsync();

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> AdminSearchAsync(AdminSearchPostDto adminSearchPostDto)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"searchTerm", adminSearchPostDto.Title },
            {"skip", adminSearchPostDto.Skip },
        };
        string query = "MATCH (n) WHERE (n:Post) " +
                       "MATCH (n)-[w:WROTE]-(person) " + 
                       "MATCH (n)-[r:HAS_TAG]->(t) " +
                       "RETURN distinct n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
                       "ORDER BY n.createdDateTime DESC " +
                       "SKIP $skip " +
                       "LIMIT 100";
        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(query, statementParameters);
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
                        UserPhoto = cursor.Current["person.picture"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
                    }
                );
            }
        }  
        await transaction.CommitAsync();

        return results;
    }

    public async Task<int> GetTotalCountAsync()
    {
        int count = 0;
        string query = "MATCH (n:Post) " +
                       "RETURN count(n) as count ";
        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(query);
        while (await cursor.FetchAsync())
        {
            if (cursor.Current is not null)
            {
                count = int.Parse(cursor.Current["count"].ToString()!);
            }
        } 
        await transaction.CommitAsync();

        return count;
    }

    public async Task<IEnumerable<SearchPostResultDto>> RecommendAsync(Guid personId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"searchTerm", personId.ToString() },
        };
        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(
            "MATCH (n) WHERE (n:Post) " +
            "MATCH (n)-[r:WROTE]-(person) " + 
            "RETURN n.id, n.summary, n.title, n.type, n.createdDateTime, person.id, person.userid, person.name, person.picture " +
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
                        UserPhoto = cursor.Current["person.picture"].ToString(),
                        Type = cursor.Current["n.type"].ToString()
                    }
                );
            }
        }
        await transaction.CommitAsync();

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsAsync(Guid personId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"person_id", personId.ToString() },
        };
        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(
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
        await transaction.CommitAsync();

        return results;
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsByTagAsync(Guid tagId)
    {
        List<SearchPostResultDto> results = new List<SearchPostResultDto>();
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"tagId", tagId.ToString() },
        };
        await using var transaction = await _session.BeginTransactionAsync();
        IResultCursor cursor = await transaction.RunAsync(
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
        await transaction.CommitAsync();
        return results;
    }
}