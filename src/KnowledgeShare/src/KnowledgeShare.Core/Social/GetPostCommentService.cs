namespace KnowledgeShare.Core.Social;

public class GetPostCommentService : IGetPostCommentService
{
    private readonly IPostCommentRepository _postCommentRepository;

    public GetPostCommentService(IPostCommentRepository postCommentRepository)
    {
        _postCommentRepository = postCommentRepository;
    }

    public async Task<IEnumerable<PostCommentDto>> GetPostCommentsAsync(Guid postId)
    {
        IList<PostCommentDto> postCommentDtos = (await _postCommentRepository.GetPostCommentsAsync(postId)).ToList();
        return postCommentDtos;
    }
}