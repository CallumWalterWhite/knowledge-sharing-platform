using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.Tags;

 [TestFixture]
public class TagServiceTests
{
    private TagService _tagService;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ICurrentAuthUser> _currentAuthUserMock;

    [SetUp]
    public void SetUp()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _currentAuthUserMock = new Mock<ICurrentAuthUser>();
        _tagService = new TagService(_tagRepositoryMock.Object, _currentAuthUserMock.Object);
    }

    [Test]
    public async Task CreateTagForPersonAsync_WhenTagDoesNotExist_ShouldCreateTagAndAddRelationship()
    {
        // Arrange
        string tagValue = "Tag1";
        Person person = Person.Create(string.Empty, string.Empty, string.Empty);
        Tag tag = null;
        _tagRepositoryMock.Setup(r => r.GetAsync(tagValue)).ReturnsAsync(tag);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _tagService.CreateTagForPersonAsync(tagValue);

        // Assert
        _tagRepositoryMock.Verify(r => r.AddAsync(It.Is<Tag>(t => t.Value == tagValue)), Times.Once);
        _tagRepositoryMock.Verify(r => r.AddPersonLikeTagRelationship(person.Id, It.Is<Tag>(t => t.Value == tagValue)), Times.Once);
    }

    [Test]
    public async Task CreateTagForPersonAsync_WhenTagExists_ShouldAddRelationship()
    {
        // Arrange
        string tagValue = "Tag1";
        Person person = Person.Create(string.Empty, string.Empty, string.Empty);
        Tag tag = new Tag(Guid.NewGuid(), tagValue);
        _tagRepositoryMock.Setup(r => r.GetAsync(tagValue)).ReturnsAsync(tag);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _tagService.CreateTagForPersonAsync(tagValue);

        // Assert
        _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Never);
        _tagRepositoryMock.Verify(r => r.AddPersonLikeTagRelationship(person.Id, It.Is<Tag>(t => t.Value == tagValue)), Times.Once);
    }

    [Test]
    public async Task DeleteTagForPersonAsync_WhenTagDoesNotExist_ShouldNotDeleteRelationship()
    {
        // Arrange
        string tagValue = "Tag1";
        Person person = Person.Create(string.Empty, string.Empty, string.Empty);
        Tag tag = null;
        _tagRepositoryMock.Setup(r => r.GetAsync(tagValue)).ReturnsAsync(tag);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _tagService.DeleteTagForPersonAsync(tagValue);

        // Assert
        _tagRepositoryMock.Verify(r => r.DeletePersonLikeTagRelationship(It.IsAny<Guid>(), It.IsAny<Tag>()), Times.Never);
    }

    [Test]
    public async Task DeleteTagForPersonAsync_WhenTagExists_ShouldDeleteRelationship()
    {
        // Arrange
        string tagValue = "Tag1";
        Person person = Person.Create(string.Empty, string.Empty, string.Empty);
        Tag tag = new Tag(Guid.NewGuid(), tagValue);
        _tagRepositoryMock.Setup(r => r.GetAsync(tagValue)).ReturnsAsync(tag);
        _currentAuthUserMock.Setup(u => u.GetPersonAsync()).ReturnsAsync(person);

        // Act
        await _tagService.DeleteTagForPersonAsync(tagValue);

        // Assert
        _tagRepositoryMock.Verify(r => r.DeletePersonLikeTagRelationship(person.Id, It.Is<Tag>(t => t.Value == tagValue)), Times.Once);
    }
}