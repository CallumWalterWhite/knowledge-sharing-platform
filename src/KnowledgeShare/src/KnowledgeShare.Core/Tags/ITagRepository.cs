using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Tags
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);
        
        Task DeleteAsync(Tag tag);

        Task<IEnumerable<Tag>> GetAllTags();
        
        Task<IEnumerable<TagPostCountDto>> GetAllTagsWithPostCount();

        Task<bool> MatchAsync(string value);

        Task<Tag?> GetAsync(string value);
        
        Task<Tag?> GetTagByIdAsync(Guid id);
        
        Task<IEnumerable<Tag>> GetAllTagsByValue(string value);
        
        Task<IEnumerable<Tag>> GetAllTagsByPostId(Guid postId);

        Task AddPersonLikeTagRelationship(Guid personId, Tag tag);
        
        Task DeletePersonLikeTagRelationship(Guid personId, Tag tag);
        
        Task<IEnumerable<Tag>> GetTagsLikedByPersonId(Guid personId);
    }
}
