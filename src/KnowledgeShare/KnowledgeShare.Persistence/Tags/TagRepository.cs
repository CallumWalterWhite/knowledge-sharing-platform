using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly IAsyncSession _session;

        public TagRepository(IAsyncSession session)
        {
            _session = session;
        }

        public async Task AddAsync(Tag tag)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"id", tag.Id.ToString() },
                {"value", tag.Value }
            };
            await _session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (tag:Tag {value: $value, id: $id}) ",
                    statementParameters);
            });
        }
        
        public async Task<bool> MatchAsync(string value)
        {
            bool matched = false;
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", value }
            };
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag WHERE tag.value = $value) RETURN tag.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tag = cursor.Current["tag.value"];
                matched = tag is not null;
            }

            return matched;
        }

        public async Task<Tag?> GetAsync(string tagValue)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", tagValue }
            };
            Tag? tag = null;
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag WHERE tag.value = $value) RETURN tag.id, tag.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["tag.value"];
                object? id = cursor.Current["tag.id"];
                if (value is not null)
                {
                    tag = new Tag(Guid.Parse(id.ToString()), value.ToString());
                }
            }

            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsByValue(string value)
        {
            List<Tag> results = new List<Tag>();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"searchQuery", value },
            };
            IResultCursor cursor = await _session.RunAsync(
                "MATCH (t:Tag)\nWHERE toLower(t.value) CONTAINS toLower($searchQuery)\nRETURN t.id, t.value, apoc.text.distance(t.value, $searchQuery, 2) AS distance\nORDER BY distance", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tagValue = cursor.Current["tag.value"];
                object? tagId = cursor.Current["tag.id"];
                if (tagValue is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(tagId.ToString()),
                        tagValue.ToString() ?? string.Empty
                    ));   
                }
            }

            return results;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsByPostId(Guid postId)
        {
            List<Tag> results = new List<Tag>();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"searchTerm", postId.ToString() },
            };
            IResultCursor cursor = await _session.RunAsync(
                "MATCH (n) WHERE (n:ArticlePost OR n:BookPost) AND n.id = $searchTerm " +
                "MATCH (n)-[r:HAS_TAG]->(t) " + 
                "RETURN t.id, t.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tagValue = cursor.Current["t.value"];
                object? tagId = cursor.Current["t.id"];
                if (tagValue is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(tagId.ToString()),
                        tagValue.ToString() ?? string.Empty
                    ));   
                }
            }

            return results;
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            List<Tag> results = new List<Tag>();
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag) RETURN tag.value, tag.id");
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["tag.value"];
                object? id = cursor.Current["tag.id"];
                if (value is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(id.ToString()),
                        value.ToString() ?? string.Empty
                    ));   
                }
            }

            return results;
        }
    }
}
