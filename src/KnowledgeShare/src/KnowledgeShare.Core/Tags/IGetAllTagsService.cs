namespace KnowledgeShare.Core.Tags;

public interface IGetAllTagsService
{
    Task<IEnumerable<Tag>> GetAllAsync();
    
    Task<IEnumerable<Tag>> GetAllAsyncByValue(string value);
    
    Task<IEnumerable<Tag>> GetLikedTagsAsync();
}