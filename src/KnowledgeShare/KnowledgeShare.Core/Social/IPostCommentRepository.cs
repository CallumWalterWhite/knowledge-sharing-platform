namespace KnowledgeShare.Core.Social;

public interface IPostCommentRepository
{
    Task CreatePostCommentAsync(PostComment postComment);
    
    Task<IEnumerable<PostCommentDto>> GetPostCommentsAsync(Guid postId);
}