using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class ChangePostServiceTests
{
    private ChangePostService _changePostService;
    private Mock<IPostRepository<ArticlePost>> _articlePostRepositoryMock;
    private Mock<IPostRepository<BookPost>> _bookPostRepositoryMock;
    private Mock<IPostRepository<FreeFormPost>> _freeFormPostRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    
    private Person _person = Person.Create("1", "Callum", String.Empty);

    [SetUp]
    public void SetUp()
    {
        _articlePostRepositoryMock = new Mock<IPostRepository<ArticlePost>>();
        _bookPostRepositoryMock = new Mock<IPostRepository<BookPost>>();
        _freeFormPostRepositoryMock = new Mock<IPostRepository<FreeFormPost>>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _changePostService = new ChangePostService(
            _articlePostRepositoryMock.Object,
            _bookPostRepositoryMock.Object,
            _tagRepositoryMock.Object,
            _freeFormPostRepositoryMock.Object);
    }

    [Test]
    public async Task GetChangePostDto_WhenPostExists_ShouldReturnChangePostDto()
    {
        // Arrange
        Guid postId = Guid.NewGuid();
        PostTypeDiscriminator discriminator = PostTypeDiscriminator.Article;
        ArticlePost articlePost = ArticlePost.Create(_person, "Test Article", "https://example.com/article",
            "This is a test article.");
        IEnumerable<Tag> tags = new List<Tag>
        {
            Tag.Create("Tag1"),
            Tag.Create("Tag1")
        };
        _articlePostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(articlePost);
        _tagRepositoryMock.Setup(r => r.GetAllTagsByPostId(postId)).ReturnsAsync(tags);

        // Act
        ChangePostDto result = await _changePostService.GetChangePostDto(postId, discriminator);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(postId, result.Id);
        Assert.AreEqual(discriminator, result.Discriminator);
        Assert.AreEqual(articlePost.Title, result.Title);
        Assert.AreEqual(articlePost.GetLink(), result.Link);
        Assert.AreEqual(articlePost.GetSummary(), result.Summary);
        Assert.AreEqual(tags.Select(t => t.Value), result.Tags);
    }

    [Test]
    public async Task GetChangePostDto_WhenPostDoesNotExist_ShouldThrowException()
    {
        // Arrange
        Guid postId = Guid.NewGuid();
        PostTypeDiscriminator discriminator = PostTypeDiscriminator.Article;
        ArticlePost articlePost = null;
        _articlePostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(articlePost);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _changePostService.GetChangePostDto(postId, discriminator));
    }

    [Test]
    public async Task ChangeAsync_WhenPostTypeIsArticle_ShouldChangeArticlePost()
    {
        // Arrange
        ChangePostDto changePostDto = new ChangePostDto
        {
            Id = Guid.NewGuid(),
            Discriminator = PostTypeDiscriminator.Article,
            Title = "Test Article",
            Link = "https://example.com/article",
            Summary = "This is a test article.",
            Tags = new List<string> { "Tag1", "Tag2" }
        };
        ArticlePost articlePost = ArticlePost.Create(_person, "Test Article2", "https://example.com/article2",
            "This is a test article2.");
        _articlePostRepositoryMock.Setup(r => r.GetByIdAsync(changePostDto.Id)).ReturnsAsync(articlePost);
        _tagRepositoryMock.Setup(r => r.MatchAsync(It.IsAny<string>())).ReturnsAsync(true);
        _tagRepositoryMock.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(Tag.Create("Tag1"));
        _articlePostRepositoryMock.Setup(r => r.UpdateAsync(articlePost)).Returns(Task.CompletedTask);

        // Act
        await _changePostService.ChangeAsync(changePostDto);

        // Assert
        _articlePostRepositoryMock.Verify(r => r.UpdateAsync(articlePost), Times.Once);
        _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Never);
    }

    [Test]
    public async Task ChangeAsync_WhenPostTypeIsBook_ShouldChangeBookPost()
    {
        // Arrange
        ChangePostDto changePostDto = new ChangePostDto
        {
            Id = Guid.NewGuid(),
            Discriminator = PostTypeDiscriminator.Book,
            Title = "Test Book",
            Author = "John Doe",
            Summary = "This is a test book.",
            Tags = new List<string> { "Tag1", "Tag2" }
        };
        BookPost bookPost = BookPost.Create(_person, "Test Book", "John Doe",
            "This is a test book.");
        _bookPostRepositoryMock.Setup(r => r.GetByIdAsync(changePostDto.Id)).ReturnsAsync(bookPost);
        _tagRepositoryMock.Setup(r => r.MatchAsync(It.IsAny<string>())).ReturnsAsync(true);
        _tagRepositoryMock.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(Tag.Create("Tag1"));
        _bookPostRepositoryMock.Setup(r => r.UpdateAsync(bookPost)).Returns(Task.CompletedTask);

        // Act
        await _changePostService.ChangeAsync(changePostDto);

        // Assert
        _bookPostRepositoryMock.Verify(r => r.UpdateAsync(bookPost), Times.Once);
        _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Never);
    }

    [Test]
    public async Task ChangeAsync_WhenPostTypeIsFreeForm_ShouldChangeFreeFormPost()
    {
        // Arrange
        ChangePostDto changePostDto = new ChangePostDto
        {
            Id = Guid.NewGuid(),
            Discriminator = PostTypeDiscriminator.Free,
            Title = "Test Free Form",
            Body = "This is a test free form post.",
            Tags = new List<string> { "Tag1", "Tag2" }
        };
        FreeFormPost freeFormPost = FreeFormPost.Create(_person, "Test Free Form", "This is a test free form post");
        _freeFormPostRepositoryMock.Setup(r => r.GetByIdAsync(changePostDto.Id)).ReturnsAsync(freeFormPost);
        _tagRepositoryMock.Setup(r => r.MatchAsync(It.IsAny<string>())).ReturnsAsync(true);
        _tagRepositoryMock.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(Tag.Create("Tag1"));
        _freeFormPostRepositoryMock.Setup(r => r.UpdateAsync(freeFormPost)).Returns(Task.CompletedTask);

        // Act
        await _changePostService.ChangeAsync(changePostDto);

        // Assert
        _freeFormPostRepositoryMock.Verify(r => r.UpdateAsync(freeFormPost), Times.Once);
        _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Never);
    }
}