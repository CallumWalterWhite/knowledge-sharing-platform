using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts.Types;

public class GetBookPostService : IGetBookPostService
{
    private readonly IPostRepository<BookPost> _bookPostRepository;

    private readonly ITagRepository _tagRepository;

    public GetBookPostService(IPostRepository<BookPost> bookPostRepository, ITagRepository tagRepository)
    {
        _bookPostRepository = bookPostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<BookPost> GetBookPostAsync(Guid id)
    {
        BookPost? bookPost = await _bookPostRepository.GetByIdAsync(id);

        if (bookPost is null)
        {
            throw new Exception("Book post does not exist");
        }
        
        bookPost.Tags = await _tagRepository.GetAllTagsByPostId(bookPost.Id);
        return bookPost;
    }
}