using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts.Types;

[TestFixture]
public class GetFreeFormPostServiceTests
{
    private Mock<IPostRepository<FreeFormPost>> _freeFormPostRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ILikeRepository> _likeRepositoryMock;
    private GetFreeFormPostService _getFreeFormPostService;

    [SetUp]
    public void Setup()
    {
        _freeFormPostRepositoryMock = new Mock<IPostRepository<FreeFormPost>>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _getFreeFormPostService = new GetFreeFormPostService(_freeFormPostRepositoryMock.Object, _tagRepositoryMock.Object, _likeRepositoryMock.Object);
    }

    [Test]
    public async Task GetFreeFormPostAsync_WithExistingId_ReturnsFreeFormPost()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var person = new Person(Guid.NewGuid(), string.Empty, string.Empty, string.Empty);
        var freeFormPost = new FreeFormPost(Guid.NewGuid(), person, DateTime.Now, "test", String.Empty, String.Empty);

        // Mock the dependencies
        _freeFormPostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(freeFormPost);
        _tagRepositoryMock.Setup(t => t.GetAllTagsByPostId(freeFormPost.Id)).ReturnsAsync(new List<Tag>());
        _likeRepositoryMock.Setup(l => l.GetPeopleIdsByPostIdAsync(freeFormPost.Id)).ReturnsAsync(new List<Guid>());

        // Act
        var result = await _getFreeFormPostService.GetFreeFormPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(freeFormPost.Id, result.Id);
        Assert.AreEqual(freeFormPost.Tags, result.Tags);
        Assert.AreEqual(freeFormPost.PeopleLiked, result.PeopleLiked);
    }

    [Test]
    public void GetFreeFormPostAsync_WithNonExistingId_ThrowsException()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Mock the dependencies to return null
        _freeFormPostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync((FreeFormPost?)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _getFreeFormPostService.GetFreeFormPostAsync(postId));
    }
}