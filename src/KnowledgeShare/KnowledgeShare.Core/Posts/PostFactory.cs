using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class PostFactory : IPostFactory
{
    public Post Create(CreatePostDto createPostDto)
    {
        switch (createPostDto.Discriminator)
        {
            case PostTypeDiscriminator.Article:
                return new ArticlePost(createPostDto.Title, createPostDto.Link, createPostDto.Summary);
            case PostTypeDiscriminator.Book:
                return new BookPost(createPostDto.Title, createPostDto.Summary);
            default:
                throw new InvalidCastException();
        }
    }
}