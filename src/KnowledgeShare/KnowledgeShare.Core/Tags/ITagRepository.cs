using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Tags
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);

        Task<IEnumerable<Tag>> GetAllTags();

        Task<bool> MatchAsync(string value);

        Task<Tag?> GetAsync(string value);
        
        Task<IEnumerable<Tag>> GetAllTagsByValue(string value);
        
        Task<IEnumerable<Tag>> GetAllTagsByPostId(Guid postId);
    }
}
