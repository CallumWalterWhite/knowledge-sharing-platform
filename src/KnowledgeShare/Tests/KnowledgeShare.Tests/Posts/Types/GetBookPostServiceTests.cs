using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts.Types;

[TestFixture]
public class GetBookPostServiceTests
{
    private Mock<IPostRepository<BookPost>> _bookPostRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ILikeRepository> _likeRepositoryMock;
    private GetBookPostService _getBookPostService;

    [SetUp]
    public void Setup()
    {
        _bookPostRepositoryMock = new Mock<IPostRepository<BookPost>>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _getBookPostService = new GetBookPostService(_bookPostRepositoryMock.Object, _tagRepositoryMock.Object, _likeRepositoryMock.Object);
    }

    [Test]
    public async Task GetBookPostAsync_WithExistingId_ReturnsBookPost()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var person = new Person(Guid.NewGuid(), string.Empty, string.Empty, string.Empty);
        var bookPost = new BookPost(Guid.NewGuid(), person, DateTime.Now, "test", String.Empty, String.Empty);

        // Mock the dependencies
        _bookPostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(bookPost);
        _tagRepositoryMock.Setup(t => t.GetAllTagsByPostId(bookPost.Id)).ReturnsAsync(new List<Tag>());
        _likeRepositoryMock.Setup(l => l.GetPeopleIdsByPostIdAsync(bookPost.Id)).ReturnsAsync(new List<Guid>());

        // Act
        var result = await _getBookPostService.GetBookPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(bookPost.Id, result.Id);
        Assert.AreEqual(bookPost.Tags, result.Tags);
        Assert.AreEqual(bookPost.PeopleLiked, result.PeopleLiked);
    }

    [Test]
    public void GetBookPostAsync_WithNonExistingId_ThrowsException()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Mock the dependencies to return null
        _bookPostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync((BookPost?)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _getBookPostService.GetBookPostAsync(postId));
    }
}