using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class PostFactoryTests
{
    private PostFactory _postFactory;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;

    [SetUp]
    public void SetUp()
    {
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _postFactory = new PostFactory(_currentAuthUserMock.Object);
    }

    [Test]
    public async Task Create_WhenPersonIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync((Person)null);
        CreatePostDto createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Article,
            Title = "Test Article",
            Link = "https://example.com/article",
            Summary = "This is a test article."
        };

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _postFactory.Create(createPostDto));
    }

    [Test]
    public async Task Create_WhenPostTypeIsArticle_ShouldReturnArticlePost()
    {
        // Arrange
        Person person = Person.Create("1", "Callum", String.Empty);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);
        CreatePostDto createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Article,
            Title = "Test Article",
            Link = "https://example.com/article",
            Summary = "This is a test article."
        };

        // Act
        Post result = await _postFactory.Create(createPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<ArticlePost>(result);
        ArticlePost articlePost = (ArticlePost)result;
        Assert.AreEqual(person, articlePost.GetAuthor());
        Assert.AreEqual(createPostDto.Title, articlePost.Title);
        Assert.AreEqual(createPostDto.Link, articlePost.GetLink());
        Assert.AreEqual(createPostDto.Summary, articlePost.GetSummary());
    }

    [Test]
    public async Task Create_WhenPostTypeIsBook_ShouldReturnBookPost()
    {
        // Arrange
        Person person = Person.Create("1", "Callum", String.Empty);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);
        CreatePostDto createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Book,
            Title = "Test Book",
            Author = "John Doe",
            Summary = "This is a test book."
        };

        // Act
        Post result = await _postFactory.Create(createPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<BookPost>(result);
        BookPost bookPost = (BookPost)result;
        Assert.AreEqual(person, bookPost.GetAuthor());
        Assert.AreEqual(createPostDto.Title, bookPost.Title);
        Assert.AreEqual(createPostDto.Summary, bookPost.GetSummary());
    }

    [Test]
    public async Task Create_WhenPostTypeIsFreeForm_ShouldReturnFreeFormPost()
    {
        // Arrange
        Person person = Person.Create("1", "Callum", String.Empty);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);
        CreatePostDto createPostDto = new CreatePostDto
        {
            Discriminator = PostTypeDiscriminator.Free,
            Title = "Test Free Form",
            Body = "This is a test free form post."
        };

        // Act
        Post result = await _postFactory.Create(createPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<FreeFormPost>(result);
        FreeFormPost freeFormPost = (FreeFormPost)result;
        Assert.AreEqual(person, freeFormPost.GetAuthor());
        Assert.AreEqual(createPostDto.Title, freeFormPost.Title);
        Assert.AreEqual(createPostDto.Body, freeFormPost.GetBody());
    }

    [Test]
    public void Create_WhenPostTypeIsInvalid_ShouldThrowInvalidCastException()
    {
        // Arrange
        Person person = Person.Create("1", "Callum", String.Empty);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);
        CreatePostDto createPostDto = new CreatePostDto
        {
            Discriminator = (PostTypeDiscriminator)99, // Invalid discriminator
            Title = "Test Invalid Post",
            Summary = "This is an invalid post."
        };

        // Act and Assert
        Assert.ThrowsAsync<InvalidCastException>(async () => await _postFactory.Create(createPostDto));
    }
}