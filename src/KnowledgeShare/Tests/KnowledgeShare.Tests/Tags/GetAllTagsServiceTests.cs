using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Tags;

[TestFixture]
public class GetAllTagsServiceTests
{
    private GetAllTagsService _getAllTagsService;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;
    private Mock<ISearchPostQuery> _searchPostQuery;

    [SetUp]
    public void SetUp()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _searchPostQuery = new Mock<ISearchPostQuery>();
        _getAllTagsService = new GetAllTagsService(_tagRepositoryMock.Object, _currentAuthUserMock.Object, _searchPostQuery.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnDistinctTags()
    {
        // Arrange
        IEnumerable<Tag> tags = new List<Tag>
        {
            new Tag(Guid.NewGuid(), "Tag1"),
            new Tag(Guid.NewGuid(), "Tag2"),
            new Tag(Guid.NewGuid(), "Tag3"),
            new Tag(Guid.NewGuid(), "Tag3")
        };
        _tagRepositoryMock.Setup(r => r.GetAllTags()).ReturnsAsync(tags);

        // Act
        IEnumerable<Tag> result = await _getAllTagsService.GetAllAsync();

        // Assert
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.All(x => x.Value == "Tag1" || x.Value == "Tag2" || x.Value == "Tag3"));
    }

    [Test]
    public async Task GetLikedTagsAsync_WhenPersonIsNull_ShouldThrowException()
    {
        // Arrange
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync((Person)null);

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _getAllTagsService.GetLikedTagsAsync());
    }

    [Test]
    public async Task GetLikedTagsAsync_ShouldReturnLikedTags()
    {
        // Arrange
        Person person = Person.Create(string.Empty, string.Empty, string.Empty);
        IEnumerable<Tag> tags = new List<Tag>
        {
            new Tag(Guid.NewGuid(), "Tag1"),
            new Tag(Guid.NewGuid(), "Tag2")
        };
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);
        _tagRepositoryMock.Setup(r => r.GetTagsLikedByPersonId(person.Id)).ReturnsAsync(tags);

        // Act
        IEnumerable<Tag> result = await _getAllTagsService.GetLikedTagsAsync();

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(x => x.Value == "Tag1"));
        Assert.IsTrue(result.Any(x => x.Value == "Tag2"));
    }
}