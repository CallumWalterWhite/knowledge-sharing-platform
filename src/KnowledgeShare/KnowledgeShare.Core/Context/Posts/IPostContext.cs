using KnowledgeShare.Core.Posts;

namespace KnowledgeShare.Core.Context.Posts;

public interface IPostContext<T>
    where T : Post
{
    Task CreateAsync(T post);
    
    Task DeleteAsync(T post);
    
    Task AddTags(T post);
    
    Task<IEnumerable<T>> GetAllAsync();
}