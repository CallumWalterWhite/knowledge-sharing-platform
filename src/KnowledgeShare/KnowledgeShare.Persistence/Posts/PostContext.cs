using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class PostContext<T> : IPostContext<Post>
{
    private readonly IAsyncSession _session;

    public PostContext(IAsyncSession session)
    {
        _session = session;
    }

    public Task CreateAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task AddTags(Post post)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}