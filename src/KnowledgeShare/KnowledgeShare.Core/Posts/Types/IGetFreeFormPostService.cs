namespace KnowledgeShare.Core.Posts.Types;

public interface IGetFreeFormPostService
{
    Task<FreeFormPost> GetFreeFormPostAsync(Guid id);
}