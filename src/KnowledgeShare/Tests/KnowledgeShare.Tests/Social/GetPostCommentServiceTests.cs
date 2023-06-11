using KnowledgeShare.Core.Social;
using Moq;

namespace KnowledgeShare.Tests.Social;

[TestFixture]
public class GetPostCommentServiceTests
{
    private Mock<IPostCommentRepository> _postCommentRepositoryMock;
    private GetPostCommentService _getPostCommentService;

    [SetUp]
    public void Setup()
    {
        _postCommentRepositoryMock = new Mock<IPostCommentRepository>();
        _getPostCommentService = new GetPostCommentService(_postCommentRepositoryMock.Object);
    }

    [Test]
    public async Task GetPostCommentsAsync_WithValidPostId_ReturnsPostComments()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var postComments = new List<PostCommentDto>
        {
            new PostCommentDto("test", string.Empty, "Comment 1", DateTime.Now),
            new PostCommentDto("test", string.Empty, "Comment 2", DateTime.Now),
            new PostCommentDto("test", string.Empty, "Comment 3", DateTime.Now)
        };

        // Mock the repository method to return the post comments
        _postCommentRepositoryMock.Setup(r => r.GetPostCommentsAsync(postId)).ReturnsAsync(postComments);

        // Act
        var result = await _getPostCommentService.GetPostCommentsAsync(postId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(postComments.Count, result.Count());
        Assert.IsTrue(postComments.SequenceEqual(result));
    }

    [Test]
    public async Task GetPostCommentsAsync_WithInvalidPostId_ReturnsEmptyList()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Mock the repository method to return an empty list
        _postCommentRepositoryMock.Setup(r => r.GetPostCommentsAsync(postId)).ReturnsAsync(new List<PostCommentDto>());

        // Act
        var result = await _getPostCommentService.GetPostCommentsAsync(postId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }
}