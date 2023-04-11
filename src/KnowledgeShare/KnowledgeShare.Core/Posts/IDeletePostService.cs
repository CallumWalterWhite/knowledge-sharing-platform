namespace KnowledgeShare.Core.Posts;

public interface IDeletePostService
{
    Task DeletePostAsync(Guid postId);
}