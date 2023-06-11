using KnowledgeShare.Core.Posts;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class DeletePostServiceTests
{
    private Mock<IPostRepository<Post>> _postRepositoryMock;
    private DeletePostService _deletePostService;

    [SetUp]
    public void Setup()
    {
        _postRepositoryMock = new Mock<IPostRepository<Post>>();
        _deletePostService = new DeletePostService(_postRepositoryMock.Object);
    }

    [Test]
    public async Task DeletePostAsync_WithValidPostId_CallsDeleteAsyncOnPostRepository()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Act
        await _deletePostService.DeletePostAsync(postId);

        // Assert
        _postRepositoryMock.Verify(p => p.DeleteAsync(postId), Times.Once);
    }

}