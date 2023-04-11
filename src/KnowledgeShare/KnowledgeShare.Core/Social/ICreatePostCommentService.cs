namespace KnowledgeShare.Core.Social;

public interface ICreatePostCommentService
{
    public Task CreatePostCommentAsync(string comment, Guid postId);
}