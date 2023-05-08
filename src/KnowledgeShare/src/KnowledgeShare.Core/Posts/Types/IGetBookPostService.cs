namespace KnowledgeShare.Core.Posts.Types;

public interface IGetBookPostService
{
    Task<BookPost> GetBookPostAsync(Guid id);
}