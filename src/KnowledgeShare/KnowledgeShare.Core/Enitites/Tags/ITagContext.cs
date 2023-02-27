namespace KnowledgeShare.Core.Enitites.Tags
{
    public interface ITagContext
    {
        Task AddAsync(Tag tag);

        Task<IEnumerable<string>> GetAllTags();
    }
}
