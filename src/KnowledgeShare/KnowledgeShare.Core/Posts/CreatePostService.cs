using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Entities.Tags;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class CreatePostService : ICreatePostService
{
    private readonly IPostFactory _postFactory;

    private readonly IPostContext<ArticlePost> _articlePostContext;
    
    private readonly IPostContext<BookPost> _bookPostContext;

    private readonly ITagContext _tagContext;
    
    public CreatePostService(
        IPostContext<ArticlePost> articlePostContext, 
        IPostContext<BookPost> bookPostContext, 
        IPostFactory postFactory, 
        ITagContext tagContext)
    {
        _articlePostContext = articlePostContext;
        _bookPostContext = bookPostContext;
        _postFactory = postFactory;
        _tagContext = tagContext;
    }
    
    public async Task Create(CreatePostDto createPostDto)
    {
        Post post = _postFactory.Create(createPostDto);
        post.Tags = await CreateTags(createPostDto.Tags);
        if (post is ArticlePost articlePost)
        {
            await _articlePostContext.CreateAsync(articlePost);
        }
        else if (post is BookPost bookPost)
        {
            await _bookPostContext.CreateAsync(bookPost);
        }
    }

    private async Task<IEnumerable<Tag>> CreateTags(IEnumerable<string> tags)
    {
        List<Tag> tagEntities = new List<Tag>();
        foreach (string value in tags)
        {
            if (await _tagContext.MatchAsync(value))
            {
                tagEntities.Add((await _tagContext.GetAsync(value))!);
            }
            else
            {
                Tag newTag = new Tag(value);
                await _tagContext.AddAsync(newTag);
                tagEntities.Add(newTag);
            }
        }

        return tagEntities;
    }
}