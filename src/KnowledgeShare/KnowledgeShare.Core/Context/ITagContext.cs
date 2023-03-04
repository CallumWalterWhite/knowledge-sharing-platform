using KnowledgeShare.Core.Entities.Tags;

namespace KnowledgeShare.Core.Context
{
    public interface ITagContext
    {
        Task AddAsync(Tag tag);

        Task<IEnumerable<Tag>> GetAllTags();

        Task<bool> MatchAsync(string value);

        Task<Tag?> GetAsync(string value);
        
        Task<IEnumerable<Tag>> GetAllTagsByValue(string value);
    }
}
