using KnowledgeShare.Core.People;
using Moq;

namespace KnowledgeShare.Tests.People;

[TestFixture]
public class PersonServiceTests
{
    private PersonService _personService;
    private Mock<IPersonRepository> _personRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personService = new PersonService(_personRepositoryMock.Object);
    }

    [Test]
    public async Task GetPersonByUserIdAsync_WhenPersonExists_ShouldReturnPerson()
    {
        // Arrange
        string userId = "123";

        Person expectedPerson = Person.Create(userId, "John Doe", "image.jpg");
        _personRepositoryMock.Setup(repo => repo.GetPersonByUserIdAsync(userId))
            .ReturnsAsync(expectedPerson);

        // Act
        Person? result = await _personService.GetPersonByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(expectedPerson.UserId, result.UserId);
        Assert.AreEqual(expectedPerson.Name, result.Name);
        Assert.AreEqual(expectedPerson.Picture, result.Picture);
    }

    [Test]
    public async Task GetPersonByUserIdAsync_WhenPersonDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        string userId = "123";

        _personRepositoryMock.Setup(repo => repo.GetPersonByUserIdAsync(userId))
            .ReturnsAsync((Person)null);

        // Act
        Person? result = await _personService.GetPersonByUserIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Test]
    public async Task CreatePersonAsync_ShouldCreatePerson()
    {
        // Arrange
        CreatePersonDto createPersonDto = new CreatePersonDto("123", "John Doe", "Image.jpg");

        // Act
        await _personService.CreatePersonAsync(createPersonDto);

        // Assert
        _personRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Person>(
            p => p.UserId == createPersonDto.UserId &&
                 p.Name == createPersonDto.Name &&
                 p.Picture == createPersonDto.DataImage)), Times.Once);
    }
}