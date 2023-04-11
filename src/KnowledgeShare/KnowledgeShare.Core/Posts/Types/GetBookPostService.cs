using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts.Types;

public class GetBookPostService : IGetBookPostService
{
    private readonly IPostRepository<BookPost> _bookPostRepository;

    private readonly ITagRepository _tagRepository;

    private readonly ILikeRepository _likeRepository;

    public GetBookPostService(IPostRepository<BookPost> bookPostRepository, ITagRepository tagRepository, ILikeRepository likeRepository)
    {
        _bookPostRepository = bookPostRepository;
        _tagRepository = tagRepository;
        _likeRepository = likeRepository;
    }

    public async Task<BookPost> GetBookPostAsync(Guid id)
    {
        BookPost? bookPost = await _bookPostRepository.GetByIdAsync(id);

        if (bookPost is null)
        {
            throw new Exception("Book post does not exist");
        }
        
        bookPost.Tags = await _tagRepository.GetAllTagsByPostId(bookPost.Id);
        bookPost.PeopleLiked = await _likeRepository.GetPeopleIdsByPostIdAsync(bookPost.Id);
        return bookPost;
    }
}