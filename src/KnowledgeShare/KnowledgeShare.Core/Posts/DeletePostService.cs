namespace KnowledgeShare.Core.Posts;

public class DeletePostService : IDeletePostService
{
    private readonly IPostRepository<Post> _postRepository;

    public DeletePostService(IPostRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task DeletePostAsync(Guid postId)
    {
        await _postRepository.DeleteAsync(postId);
    }
}