namespace KnowledgeShare.Core.Posts;

public interface IPostRepository<T>
    where T : Post
{
    Task CreateAsync(T post);
    
    Task DeleteAsync(T post);
    
    Task AddTags(T post);
    
    Task<IEnumerable<T>> GetAllAsync();
}