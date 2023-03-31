using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class PostFactory : IPostFactory
{
    private readonly ICurrentAuthUser _currentAuthUser;
    
    public PostFactory(ICurrentAuthUser currentAuthUser)
    {
        _currentAuthUser = currentAuthUser;
    }
    
    public async Task<Post> Create(CreatePostDto createPostDto)
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            //TODO: Raise exception
            throw new ArgumentException();
        }
        switch (createPostDto.Discriminator)
        {
            case PostTypeDiscriminator.Article:
                return ArticlePost.Create(person, createPostDto.Title, createPostDto.Link, createPostDto.Summary);
            case PostTypeDiscriminator.Book:
                return BookPost.Create(person, createPostDto.Title, createPostDto.Author, createPostDto.Summary);
            case PostTypeDiscriminator.Free:
                return FreeFormPost.Create(person, createPostDto.Title, createPostDto.Body);
            default:
                throw new InvalidCastException();
        }
    }
}