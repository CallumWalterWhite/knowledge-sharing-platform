using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Validator;
using MissingFieldException = System.MissingFieldException;

namespace KnowledgeShare.Tests.Posts.Validator;

[TestFixture]
public class CreatePostDtoValidatorTests
{
    [Test]
    public void Validate_WithArticlePostAndNonEmptyLink_DoesNotThrowException()
    {
        // Arrange
        var createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Article,
            Link = "https://example.com"
        };

        // Act and Assert
        Assert.DoesNotThrow(() => CreatePostDtoValidator.Validate(createPostDto));
    }

    [Test]
    public void Validate_WithNonArticlePost_DoesNotThrowException()
    {
        // Arrange
        var createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Book,
            Link = null // or any value
        };

        // Act and Assert
        Assert.DoesNotThrow(() => CreatePostDtoValidator.Validate(createPostDto));
    }
}