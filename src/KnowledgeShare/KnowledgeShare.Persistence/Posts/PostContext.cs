using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Posts;

namespace KnowledgeShare.Persistence.Posts;

public class PostContext<T> : IPostContext<Post>
{
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

    public Task<IEnumerable<Post>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}