using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts.Types;

[TestFixture]
public class GetArticlePostServiceTests
{
    private Mock<IPostRepository<ArticlePost>> _articlePostRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ILikeRepository> _likeRepositoryMock;
    private GetArticlePostService _getArticlePostService;

    [SetUp]
    public void Setup()
    {
        _articlePostRepositoryMock = new Mock<IPostRepository<ArticlePost>>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _getArticlePostService = new GetArticlePostService(_articlePostRepositoryMock.Object, _tagRepositoryMock.Object, _likeRepositoryMock.Object);
    }

    [Test]
    public async Task GetArticlePostAsync_WithExistingId_ReturnsArticlePost()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var person = new Person(Guid.NewGuid(), string.Empty, string.Empty, string.Empty);
        var articlePost = new ArticlePost(Guid.NewGuid(), person, DateTime.Now, "test", String.Empty, String.Empty);

        // Mock the dependencies
        _articlePostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(articlePost);
        _tagRepositoryMock.Setup(t => t.GetAllTagsByPostId(articlePost.Id)).ReturnsAsync(new List<Tag>());
        _likeRepositoryMock.Setup(l => l.GetPeopleIdsByPostIdAsync(articlePost.Id)).ReturnsAsync(new List<Guid>());

        // Act
        var result = await _getArticlePostService.GetArticlePostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(articlePost.Id, result.Id);
        Assert.AreEqual(articlePost.Tags, result.Tags);
        Assert.AreEqual(articlePost.PeopleLiked, result.PeopleLiked);
    }

    [Test]
    public void GetArticlePostAsync_WithNonExistingId_ThrowsException()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Mock the dependencies to return null
        _articlePostRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync((ArticlePost?)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _getArticlePostService.GetArticlePostAsync(postId));
    }

}