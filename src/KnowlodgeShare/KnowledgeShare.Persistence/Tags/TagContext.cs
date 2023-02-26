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
            await _session.ExecuteWriteAsync(async tx =>
            {
                IResultCursor result = await tx.RunAsync("CREATE (tag:Tag {value: '" + tag.Value + "'}) ",
                    new { tag.Value });
            });
        }

        public async Task<IEnumerable<string>> GetAllTags()
        {
            // Define your Cypher query
            var query = "MATCH (tag:Tag) RETURN tag.value";

            // Run the query and retrieve the results
            var results = new List<string>();

            var cursor = await _session.RunAsync(query);

            while (await cursor.FetchAsync())
            {
                var name = cursor.Current["tag.value"];
                if (name is not null)
                {
                    results.Add(name.ToString());
                }
            }

            return results;
        }
    }
}
