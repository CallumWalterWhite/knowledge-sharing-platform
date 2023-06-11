using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Posts;

[TestFixture]
public class SearchPostServiceTests
{
    private Mock<ISearchPostQuery> _searchPostQueryMock;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ILikeRepository> _likeRepositoryMock;
    private Mock<IPostCommentRepository> _commentRepositoryMock;
    private SearchPostService _searchPostService;

    [SetUp]
    public void Setup()
    {
        _searchPostQueryMock = new Mock<ISearchPostQuery>();
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _commentRepositoryMock = new Mock<IPostCommentRepository>();

        _searchPostService = new SearchPostService(
            _searchPostQueryMock.Object,
            _currentAuthUserMock.Object,
            _tagRepositoryMock.Object,
            _likeRepositoryMock.Object,
            _commentRepositoryMock.Object);
    }

    [Test]
    public async Task SearchAsync_WithValidSearchPostDto_ReturnsNonEmptyList()
    {
        // Arrange
        var searchPostDto = new SearchPostDto
        {
            Tags = new List<string> { "tag1", "tag2" }
        };

        // Mock the dependencies
        _searchPostQueryMock.Setup(s => s.SearchAsync(searchPostDto)).ReturnsAsync(GetSampleSearchPostResultDtos());

        // Act
        var result = await _searchPostService.SearchAsync(searchPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsTrue(result.Any());
    }

    [Test]
    public async Task SearchAsync_WithEmptySearchPostDto_ReturnsEmptyList()
    {
        // Arrange
        var searchPostDto = new SearchPostDto
        {
            Tags = new List<string>()
        };

        // Act
        var result = await _searchPostService.SearchAsync(searchPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsFalse(result.Any());
    }

    [Test]
    public async Task RecommendAsync_WithValidPerson_ReturnsNonEmptyList()
    {
        // Arrange
        var person = new Person(Guid.NewGuid(), string.Empty, string.Empty, string.Empty);

        // Mock the dependencies
        _currentAuthUserMock.Setup(c => c.GetPersonAsync()).ReturnsAsync(person);
        _searchPostQueryMock.Setup(s => s.RecommendAsync(person.Id)).ReturnsAsync(GetSampleSearchPostResultDtos());

        // Act
        var result = await _searchPostService.RecommendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsTrue(result.Any());
    }

    [Test]
    public async Task RecommendAsync_WithNullPerson_ReturnsEmptyList()
    {
        // Arrange
        Person person = null;

        // Mock the dependencies
        _currentAuthUserMock.Setup(c => c.GetPersonAsync()).ReturnsAsync(person);

        // Act
        var result = await _searchPostService.RecommendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsFalse(result.Any());
    }

    // Other test cases for GetAllAsync, GetPostsByCurrentPersonAsync, and Hydrate methods can be added here.

    // Helper method to return a sample list of SearchPostResultDto
    private IList<SearchPostResultDto> GetSampleSearchPostResultDtos()
    {
        // Create a sample list of SearchPostResultDto
        var searchPostResultDtos = new List<SearchPostResultDto>
        {
            new SearchPostResultDto { Id = Guid.NewGuid(), Summary = "Summary 1" },
            new SearchPostResultDto { Id = Guid.NewGuid(), Summary = "Summary 2" },
            new SearchPostResultDto { Id = Guid.NewGuid(), Summary = "Summary 3" }
        };

        return searchPostResultDtos;
    }
}