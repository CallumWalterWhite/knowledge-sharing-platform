using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts.Types;

public class GetFreeFormPostService : IGetFreeFormPostService
{
    private readonly IPostRepository<FreeFormPost> _freeFormPostRepository;

    private readonly ITagRepository _tagRepository;

    public GetFreeFormPostService(IPostRepository<FreeFormPost> freeFormPostRepository, ITagRepository tagRepository)
    {
        _freeFormPostRepository = freeFormPostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<FreeFormPost> GetFreeFormPostAsync(Guid id)
    {
        FreeFormPost? freeFormPost = await _freeFormPostRepository.GetByIdAsync(id);

        if (freeFormPost is null)
        {
            throw new Exception("Free form post does not exist");
        }
        
        freeFormPost.Tags = await _tagRepository.GetAllTagsByPostId(freeFormPost.Id);
        return freeFormPost;
    }
}