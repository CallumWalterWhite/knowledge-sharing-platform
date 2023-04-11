namespace KnowledgeShare.Core.Posts;

public interface IPostRepository<T>
    where T : Post
{
    Task CreateAsync(T post);
    
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Guid id);
}