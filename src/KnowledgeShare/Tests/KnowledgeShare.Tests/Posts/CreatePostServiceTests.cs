using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class CreatePostServiceTests
{
    private CreatePostService _createPostService;
    private Mock<IPostFactory> _postFactoryMock;
    private Mock<IPostRepository<ArticlePost>> _articlePostRepositoryMock;
    private Mock<IPostRepository<BookPost>> _bookPostRepositoryMock;
    private Mock<IPostRepository<FreeFormPost>> _freeFormPostRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _postFactoryMock = new Mock<IPostFactory>();
        _articlePostRepositoryMock = new Mock<IPostRepository<ArticlePost>>();
        _bookPostRepositoryMock = new Mock<IPostRepository<BookPost>>();
        _freeFormPostRepositoryMock = new Mock<IPostRepository<FreeFormPost>>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _createPostService = new CreatePostService(
            _articlePostRepositoryMock.Object,
            _bookPostRepositoryMock.Object,
            _postFactoryMock.Object,
            _tagRepositoryMock.Object,
            _freeFormPostRepositoryMock.Object);
    }

    [Test]
    public async Task Create_ShouldCreatePostAndReturnPostId()
    {
        // Arrange
        Guid postId = Guid.NewGuid();
        CreatePostDto createPostDto = new CreatePostDto
        {
            Title = "Test Post",
            Discriminator = PostTypeDiscriminator.Article,
            Tags = new List<string> { "tag1", "tag2" }
        };

        ArticlePost createdPost = new ArticlePost(postId, Person.Create(String.Empty, String.Empty, String.Empty),
            DateTime.Now, createPostDto.Title, String.Empty, String.Empty);

        _postFactoryMock.Setup(factory => factory.Create(createPostDto)).ReturnsAsync(createdPost);
        _tagRepositoryMock.Setup(repo => repo.MatchAsync(It.IsAny<string>())).ReturnsAsync(true);
        _articlePostRepositoryMock.Setup(repo => repo.CreateAsync(createdPost)).Returns(Task.CompletedTask);

        // Act
        Guid result = await _createPostService.Create(createPostDto);

        // Assert
        Assert.AreEqual(postId, result);
        _postFactoryMock.Verify(factory => factory.Create(createPostDto), Times.Once);
        _tagRepositoryMock.Verify(repo => repo.MatchAsync("tag1"), Times.Once);
        _tagRepositoryMock.Verify(repo => repo.GetAsync("tag1"), Times.Once);
        _articlePostRepositoryMock.Verify(repo => repo.CreateAsync(createdPost), Times.Once);
    }
}