using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Social;
using Moq;

namespace KnowledgeShare.Tests.Social;

[TestFixture]
public class CreatePostCommentServiceTests
{
    private Mock<IPostCommentRepository> _postCommentRepositoryMock;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;
    private CreatePostCommentService _createPostCommentService;

    [SetUp]
    public void Setup()
    {
        _postCommentRepositoryMock = new Mock<IPostCommentRepository>();
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _createPostCommentService = new CreatePostCommentService(_postCommentRepositoryMock.Object, _currentAuthUserMock.Object);
    }

    [Test]
    public async Task CreatePostCommentAsync_WithValidPerson_CreatesPostComment()
    {
        // Arrange
        var comment = "This is a test comment.";
        var postId = Guid.NewGuid();
        var personId = Guid.NewGuid();
        var person = new Person(personId, string.Empty, string.Empty, string.Empty);

        // Mock the dependencies
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _createPostCommentService.CreatePostCommentAsync(comment, postId);

        // Assert
        _postCommentRepositoryMock.Verify(r => r.CreatePostCommentAsync(It.IsAny<PostComment>()), Times.Once);
    }

    [Test]
    public void CreatePostCommentAsync_WithNullPerson_ThrowsException()
    {
        // Arrange
        var comment = "This is a test comment.";
        var postId = Guid.NewGuid();

        // Mock the dependencies to return null
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync((Person?)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _createPostCommentService.CreatePostCommentAsync(comment, postId));
    }
}