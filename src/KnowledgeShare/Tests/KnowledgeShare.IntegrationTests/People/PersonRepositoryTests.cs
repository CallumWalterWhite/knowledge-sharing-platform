using KnowledgeShare.Core.People;
using KnowledgeShare.Persistence.People;

namespace KnowledgeShare.IntegrationTests.People;

[TestFixture]
public class PersonRepositoryTests : IntegrationTestsDatabase
{
    [Test]
    public async Task GivenPerson_WhenGetPersonByUserIdAsync_ReturnPerson()
    {
        await WithSessionAsync(async session =>
        {
            PersonRepository personRepository = new PersonRepository(session);
            Person person = Person.Create("1", "Callum", String.Empty);
            await personRepository.AddAsync(person);
            Person? personSearch = await personRepository.GetPersonByUserIdAsync("1"); 
            Assert.IsNotNull(personSearch);
            Assert.AreEqual(personSearch.Id, person.Id);
        });
    }
    
    [Test]
    public async Task GivenPersonIsAdmin_WhenSetAdminAsync_ReturnPerson()
    {
        await WithSessionAsync(async session =>
        {
            PersonRepository personRepository = new PersonRepository(session);
            Person person = Person.Create("1", "Callum", String.Empty);
            await personRepository.AddAsync(person);
            await personRepository.SetAdminAsync(person.Id, true);
            Person? personSearch = await personRepository.GetPersonByUserIdAsync("1"); 
            Assert.IsNotNull(personSearch);
            Assert.False(person.IsAdmin);
        });
    }
    
    [Test]
    public async Task GivenPersonIsNotAdmin_WhenSetAdminAsync_ReturnPerson()
    {
        await WithSessionAsync(async session =>
        {
            PersonRepository personRepository = new PersonRepository(session);
            Person person = Person.Create("1", "Callum", String.Empty);
            await personRepository.AddAsync(person);
            await personRepository.SetAdminAsync(person.Id, false);
            Person? personSearch = await personRepository.GetPersonByUserIdAsync("1"); 
            Assert.IsNotNull(personSearch);
            Assert.False(person.IsAdmin);
        });
    }
}