using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;

namespace KnowledgeShare.Core.Social;

public class CreatePostCommentService : ICreatePostCommentService
{
    private readonly IPostCommentRepository _postCommentRepository;

    private readonly ICurrentAuthUser _currentAuthUser;

    public CreatePostCommentService(IPostCommentRepository postCommentRepository, ICurrentAuthUser currentAuthUser)
    {
        _postCommentRepository = postCommentRepository;
        _currentAuthUser = currentAuthUser;
    }

    public async Task CreatePostCommentAsync(string comment, Guid postId)
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            throw new Exception("Person can not be null");
        }

        PostComment postComment = PostComment.Create(comment, postId, person.Id);
        await _postCommentRepository.CreatePostCommentAsync(postComment);
    }
}