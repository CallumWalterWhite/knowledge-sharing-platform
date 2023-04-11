namespace KnowledgeShare.Core.Social;

public interface IGetPostCommentService
{
    Task<IEnumerable<PostCommentDto>> GetPostCommentsAsync(Guid postId);
}