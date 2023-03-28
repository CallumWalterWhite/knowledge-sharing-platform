using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts.Types;

public class GetArticlePostService : IGetArticlePostService
{
    private readonly IPostRepository<ArticlePost> _articlePostRepository;

    private readonly ITagRepository _tagRepository;

    public GetArticlePostService(IPostRepository<ArticlePost> articlePostRepository, ITagRepository tagRepository)
    {
        _articlePostRepository = articlePostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ArticlePost> GetArticlePostAsync(Guid id)
    {
        ArticlePost? articlePost = await _articlePostRepository.GetByIdAsync(id);

        if (articlePost is null)
        {
            throw new Exception("Article post does not exist");
        }
        
        articlePost.Tags = await _tagRepository.GetAllTagsByPostId(articlePost.Id);
        return articlePost;
    }
}