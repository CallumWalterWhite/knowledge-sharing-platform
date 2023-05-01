using KnowledgeShare.Core.Posts;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Posts;

public class PostRepository : PostBaseRepository, IPostRepository<Post>
{

    public PostRepository(IAsyncSession session) : base(session)
    {
    }
    
    public Task CreateAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public new async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync(id);
    }

    public Task<IEnumerable<Post>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Post?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}