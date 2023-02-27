using KnowledgeShare.Core.Enitites.Tags;
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
            string value = tag.Value;
            string id = Guid.NewGuid().ToString();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", value },
                {"id", id }
            };
            await _session.ExecuteWriteAsync(async tx =>
            {
                IResultCursor result = await tx.RunAsync("CREATE (tag:Tag {id: $id, value: $value}) ",
                    statementParameters);
            });
        }

        public async Task<IEnumerable<string>> GetAllTags()
        {
            List<string> results = new List<string>();
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag) RETURN tag.value");
            while (await cursor.FetchAsync())
            {
                object? name = cursor.Current["tag.value"];
                if (name is not null)
                {
                    results.Add(name.ToString());
                }
            }

            return results;
        }
    }
}
