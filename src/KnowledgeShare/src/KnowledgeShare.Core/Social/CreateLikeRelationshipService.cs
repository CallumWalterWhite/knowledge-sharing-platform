using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;

namespace KnowledgeShare.Core.Social;

public class CreateLikeRelationshipService : ICreateLikeRelationshipService
{
    private readonly ILikeRepository _likeRepository;
    
    private readonly ICurrentAuthUser _currentAuthUser;

    public CreateLikeRelationshipService(
        ILikeRepository likeRepository, 
        ICurrentAuthUser currentAuthUser)
    {
        _likeRepository = likeRepository;
        _currentAuthUser = currentAuthUser;
    }

    public async Task CreateLike(Guid postId)
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            throw new Exception("Person can not be null");
        }

        await _likeRepository.CreateLikeAsync(person.Id, postId);
    }
}