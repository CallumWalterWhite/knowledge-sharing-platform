namespace KnowledgeShare.Core.Tags;

public interface ITagService
{
    Task CreateTagForPersonAsync(string tag);

    Task DeleteTagForPersonAsync(string tag);

    Task DeleteTag(Guid id);
}