namespace KnowledgeShare.Core.Repository;

public interface IRepository<T>
{
    Task AddAsync(string message);
    
    Task<string> GetAsync();
}