namespace KnowledgeShare.Core.Tags;

public interface IGetAllTagsService
{
    Task<IEnumerable<Tag>> GetAllAsync();
    
    Task<IEnumerable<TagPostCountDto>> GetAllWithPostCountAsync();

    Task<IEnumerable<Tag>> GetLikedTagsAsync();
}