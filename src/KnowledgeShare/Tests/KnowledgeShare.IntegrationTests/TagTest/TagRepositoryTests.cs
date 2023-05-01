using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence.Tags;

namespace KnowledgeShare.IntegrationTests.TagTest;

public class TagRepositoryTests : IntegrationTestsDatabase
{
    [Test]
    public async Task GivenTag_WhenGetByValue_ReturnTag()
    {
        await WithSessionAsync(async session =>
        {
            TagRepository repository = new TagRepository(session);
            Tag tag = Tag.Create("test");
            await repository.AddAsync(tag);
            Tag? tag1 = await repository.GetAsync("test");
            Assert.IsNotNull(tag1);
            Assert.That(tag1!.Id, Is.EqualTo(tag.Id));
        });
    }
    
    [Test]
    public async Task Given2Tag_WhenGetByValue_ReturnCorrectTag()
    {
        await WithSessionAsync(async session =>
        {
            TagRepository repository = new TagRepository(session);
            Tag tag = Tag.Create("test");
            await repository.AddAsync(tag);
            Tag tag2 = Tag.Create("test123");
            await repository.AddAsync(tag2);
            Tag? tag1 = await repository.GetAsync("test");
            Assert.IsNotNull(tag1);
            Assert.That(tag1!.Id, Is.EqualTo(tag.Id));
        });
    }
    
    [TestCase("tag", true)]
    [TestCase("tag2", false)]
    public async Task GivenTag_WhenCheckTagExist_ReturnTrue(string searchTerm, bool exist)
    {
        await WithSessionAsync(async session =>
        {
            TagRepository repository = new TagRepository(session);
            Tag tag = Tag.Create("tag");
            await repository.AddAsync(tag);
            bool match = await repository.MatchAsync(searchTerm);
            Assert.That(match, Is.EqualTo(exist));
        });
    }
}