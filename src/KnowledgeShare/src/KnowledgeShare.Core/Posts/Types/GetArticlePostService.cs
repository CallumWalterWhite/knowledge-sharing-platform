using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts.Types;

public class GetArticlePostService : IGetArticlePostService
{
    private readonly IPostRepository<ArticlePost> _articlePostRepository;

    private readonly ITagRepository _tagRepository;

    private readonly ILikeRepository _likeRepository;

    public GetArticlePostService(
        IPostRepository<ArticlePost> articlePostRepository, 
        ITagRepository tagRepository, 
        ILikeRepository likeRepository)
    {
        _articlePostRepository = articlePostRepository;
        _tagRepository = tagRepository;
        _likeRepository = likeRepository;
    }

    public async Task<ArticlePost> GetArticlePostAsync(Guid id)
    {
        ArticlePost? articlePost = await _articlePostRepository.GetByIdAsync(id);

        if (articlePost is null)
        {
            throw new Exception("Article post does not exist");
        }
        
        articlePost.Tags = await _tagRepository.GetAllTagsByPostId(articlePost.Id);
        articlePost.PeopleLiked = await _likeRepository.GetPeopleIdsByPostIdAsync(articlePost.Id);
        return articlePost;
    }
}