using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Social;
using Moq;

namespace KnowledgeShare.Tests.Social;

[TestFixture]
public class CreateLikeRelationshipServiceTests
{
    private Mock<ILikeRepository> _likeRepositoryMock;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;
    private CreateLikeRelationshipService _createLikeRelationshipService;

    [SetUp]
    public void Setup()
    {
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _createLikeRelationshipService = new CreateLikeRelationshipService(_likeRepositoryMock.Object, _currentAuthUserMock.Object);
    }

    [Test]
    public async Task CreateLike_WithValidPerson_CreatesLikeRelationship()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var personId = Guid.NewGuid();
        var person = new Person(personId, string.Empty, string.Empty, string.Empty);

        // Mock the dependencies
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _createLikeRelationshipService.CreateLike(postId);

        // Assert
        _likeRepositoryMock.Verify(r => r.CreateLikeAsync(personId, postId), Times.Once);
    }

    [Test]
    public void CreateLike_WithNullPerson_ThrowsException()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Mock the dependencies to return null
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync((Person?)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _createLikeRelationshipService.CreateLike(postId));
    }
}