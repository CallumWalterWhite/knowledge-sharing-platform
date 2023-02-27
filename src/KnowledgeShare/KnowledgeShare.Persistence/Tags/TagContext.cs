using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Entities.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Tags
{
    public class TagContext : ITagContext
    {
        private readonly IAsyncSession _session;

        public TagContext(IAsyncSession session)
        {
            _session = session;
        }

        public async Task AddAsync(Tag tag)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", tag.Value },
                {"id", tag.Id }
            };
            await _session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (tag:Tag {id: $id, value: $value}) ",
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
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag WHERE a.name = $value) RETURN tag");
            while (await cursor.FetchAsync())
            {
                object? tag = cursor.Current["tag"];
                matched = tag is not null;
            }

            return matched;
        }

        public async Task<IEnumerable<string>> GetAllTags()
        {
            List<string> results = new List<string>();
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag) RETURN tag.value");
            while (await cursor.FetchAsync())
            {
                object? result = cursor.Current["tag.value"];
                if (result is not null)
                {
                    results.Add(result.ToString());
                }
            }

            return results;
        }
    }
}
