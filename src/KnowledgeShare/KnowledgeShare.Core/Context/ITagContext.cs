using KnowledgeShare.Core.Entities.Tags;

namespace KnowledgeShare.Core.Context
{
    public interface ITagContext
    {
        Task AddAsync(Tag tag);

        Task<IEnumerable<string>> GetAllTags();

        Task<bool> MatchAsync(string value);
    }
}
