using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts;

public class ChangePostService : IChangePostService
{
    private readonly IPostRepository<ArticlePost> _articlePostRepository;
    
    private readonly IPostRepository<BookPost> _bookPostRepository;
    
    private readonly IPostRepository<FreeFormPost> _freeFormPostRepository;

    private readonly ITagRepository _tagRepository;

    public ChangePostService(
        IPostRepository<ArticlePost> articlePostRepository, 
        IPostRepository<BookPost> bookPostRepository,
        ITagRepository tagRepository, 
        IPostRepository<FreeFormPost> freeFormPostRepository)
    {
        _articlePostRepository = articlePostRepository;
        _bookPostRepository = bookPostRepository;
        _tagRepository = tagRepository;
        _freeFormPostRepository = freeFormPostRepository;
    }
    
    public async Task<ChangePostDto> GetChangePostDto(Guid id, PostTypeDiscriminator postTypeDiscriminator)
    {
        ChangePostDto changePostDto = new ChangePostDto()
        {
            Id = id,
            Discriminator = postTypeDiscriminator
        };
        switch (postTypeDiscriminator)
        {
            case PostTypeDiscriminator.Article:
                ArticlePost? articlePost = await _articlePostRepository.GetByIdAsync(id);
                
                if (articlePost is null)
                {
                    throw new Exception("Post does not exist");
                }

                changePostDto.Title = articlePost.Title;
                changePostDto.Link = articlePost.GetLink();
                changePostDto.Summary = articlePost.GetSummary();
                break;
            case PostTypeDiscriminator.Book:
                BookPost? bookPost = await _bookPostRepository.GetByIdAsync(id);
                
                if (bookPost is null)
                {
                    throw new Exception("Post does not exist");
                }

                changePostDto.Title = bookPost.Title;
                changePostDto.Author = bookPost.GetBookAuthor();
                changePostDto.Summary = bookPost.GetSummary();
                break;
            case PostTypeDiscriminator.Free:
                FreeFormPost? freeFormPost = await _freeFormPostRepository.GetByIdAsync(id);
                
                if (freeFormPost is null)
                {
                    throw new Exception("Post does not exist");
                }

                changePostDto.Title = freeFormPost.Title;
                changePostDto.Body = freeFormPost.GetBody();
                break;
        }

        IEnumerable<Tag> tags = await _tagRepository.GetAllTagsByPostId(id);
        changePostDto.Tags = tags.Select(x => x.Value);

        return changePostDto;
    }

    public async Task ChangeAsync(ChangePostDto changePostDto)
    {
        switch (changePostDto.Discriminator)
        {
            case PostTypeDiscriminator.Article:
                await ChangeArticlePost(changePostDto, await _articlePostRepository.GetByIdAsync(changePostDto.Id));
                break;
            case PostTypeDiscriminator.Book:
                await ChangeBookPost(changePostDto, await _bookPostRepository.GetByIdAsync(changePostDto.Id));
                break;
            case PostTypeDiscriminator.Free:
                await ChangeFreFormPost(changePostDto, await _freeFormPostRepository.GetByIdAsync(changePostDto.Id));
                break;
        }
    }

    private async Task<IEnumerable<Tag>> CreateTags(IEnumerable<string> tags)
    {
        tags = tags.Select(x => x.ToLower()).ToList();
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

    private async Task ChangeArticlePost(ChangePostDto changePostDto, ArticlePost? articlePost)
    {
        if (articlePost is null)
        {
            throw new Exception("Post does not exist");
        }
        
        articlePost.Change(changePostDto.Title, changePostDto.Link, changePostDto.Summary);
        articlePost.Tags = await CreateTags(changePostDto.Tags);

        await _articlePostRepository.UpdateAsync(articlePost);
    }

    private async Task ChangeBookPost(ChangePostDto changePostDto, BookPost? bookPost)
    {
        if (bookPost is null)
        {
            throw new Exception("Post does not exist");
        }
        
        bookPost.Change(changePostDto.Title, changePostDto.Author, changePostDto.Summary);
        bookPost.Tags = await CreateTags(changePostDto.Tags);

        await _bookPostRepository.UpdateAsync(bookPost);
    }

    private async Task ChangeFreFormPost(ChangePostDto changePostDto, FreeFormPost? freeFormPost)
    {
        if (freeFormPost is null)
        {
            throw new Exception("Post does not exist");
        }
        
        freeFormPost.Change(changePostDto.Title, changePostDto.Body);
        freeFormPost.Tags = await CreateTags(changePostDto.Tags);

        await _freeFormPostRepository.UpdateAsync(freeFormPost);
    }
}