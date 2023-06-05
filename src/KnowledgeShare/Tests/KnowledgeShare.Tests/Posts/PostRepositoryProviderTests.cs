using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class PostRepositoryProviderTests
{
    private PostRepositoryProvider _repositoryProvider;
    private Mock<IPostRepository<ArticlePost>> _articlePostRepositoryMock;
    private Mock<IPostRepository<BookPost>> _bookPostRepositoryMock;
    private Mock<IPostRepository<FreeFormPost>> _freeFormPostRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _articlePostRepositoryMock = new Mock<IPostRepository<ArticlePost>>();
        _bookPostRepositoryMock = new Mock<IPostRepository<BookPost>>();
        _freeFormPostRepositoryMock = new Mock<IPostRepository<FreeFormPost>>();

        _repositoryProvider = new PostRepositoryProvider(
            _articlePostRepositoryMock.Object,
            _bookPostRepositoryMock.Object,
            _freeFormPostRepositoryMock.Object);
    }
    
    [Test]
    public void Get_WhenTypeIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        Type type = typeof(InvalidPost);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => _repositoryProvider.Get(type));
    }
}

// Helper class for testing
public class InvalidPost : Post
{
    protected InvalidPost(Guid id, Person author, DateTime dateTimeCreated) : base(id, author, dateTimeCreated)
    {
    }
}