using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class PostFactory : IPostFactory
{
    private readonly ICurrentAuthUser _currentAuthUser;
    
    public PostFactory(ICurrentAuthUser currentAuthUser)
    {
        _currentAuthUser = currentAuthUser;
    }
    
    public Post Create(CreatePostDto createPostDto)
    {
        switch (createPostDto.Discriminator)
        {
            case PostTypeDiscriminator.Article:
                return ArticlePost.Create(_currentAuthUser.Person, createPostDto.Title, createPostDto.Link, createPostDto.Summary);
            case PostTypeDiscriminator.Book:
                return BookPost.Create(_currentAuthUser.Person, createPostDto.Title, createPostDto.Summary);
            default:
                throw new InvalidCastException();
        }
    }
}