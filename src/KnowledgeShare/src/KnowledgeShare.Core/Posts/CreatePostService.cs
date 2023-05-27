using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts;

public class CreatePostService : ICreatePostService
{
    private readonly IPostFactory _postFactory;

    private readonly IPostRepository<ArticlePost> _articlePostRepository;
    
    private readonly IPostRepository<BookPost> _bookPostRepository;
    
    private readonly IPostRepository<FreeFormPost> _freeFormPostRepository;

    private readonly ITagRepository _tagRepository;

    public CreatePostService(
        IPostRepository<ArticlePost> articlePostRepository, 
        IPostRepository<BookPost> bookPostRepository, 
        IPostFactory postFactory, 
        ITagRepository tagRepository, 
        IPostRepository<FreeFormPost> freeFormPostRepository)
    {
        _articlePostRepository = articlePostRepository;
        _bookPostRepository = bookPostRepository;
        _postFactory = postFactory;
        _tagRepository = tagRepository;
        _freeFormPostRepository = freeFormPostRepository;
    }
    
    public async Task<Guid> Create(CreatePostDto createPostDto)
    {
        createPostDto.Tags = createPostDto.Tags.Select(x => x.ToLower()).ToList();
        Post post = await _postFactory.Create(createPostDto);
        post.Tags = await CreateTags(createPostDto.Tags);
        if (post is ArticlePost articlePost)
        {
            await _articlePostRepository.CreateAsync(articlePost);
        }
        else if (post is BookPost bookPost)
        {
            await _bookPostRepository.CreateAsync(bookPost);
        }
        else if (post is FreeFormPost freeFormPost)
        {
            await _freeFormPostRepository.CreateAsync(freeFormPost);
        }

        return post.Id;
    }

    private async Task<IEnumerable<Tag>> CreateTags(IEnumerable<string> tags)
    {
        List<Tag> tagEntities = new List<Tag>();
        foreach (string value in tags)
        {
            if (await _tagRepository.MatchAsync(value))
            {
                tagEntities.Add((await _tagRepository.GetAsync(value))!);
            }
            else
            {
                Tag newTag = Tag.Create(value);
                await _tagRepository.AddAsync(newTag);
                tagEntities.Add(newTag);
            }
        }

        return tagEntities;
    }

    private IEnumerable<string> TagValidator(IEnumerable<string> tags)
    {
        List<string> vtags = new List<string>();
        foreach (string tag in tags)
        {
            vtags.Add(tag.ToLower());
        }

        return vtags;
    }
}