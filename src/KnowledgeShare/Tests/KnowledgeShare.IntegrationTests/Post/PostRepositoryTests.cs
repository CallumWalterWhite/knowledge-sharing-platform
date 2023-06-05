using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence.People;
using KnowledgeShare.Persistence.Posts;
using KnowledgeShare.Persistence.Social;

namespace KnowledgeShare.IntegrationTests.Post;

public class PostRepositoryTests : IntegrationTestsDatabase
{
    [Test]
    public async Task GivenPost_WhenGetGetByIdAsync_ReturnPost()
    {
        await WithSessionAsync(async session =>
        {
            PersonRepository personRepository = new PersonRepository(session);
            Person person = Person.Create("1", "Callum", String.Empty);
            await personRepository.AddAsync(person);
            ArticlePostRepository articlePostRepository = new ArticlePostRepository(session);
            ArticlePost articlePost = ArticlePost.Create(person, "Test Article", "https://example.com/article",
                "This is a test article.");
            articlePost.Tags = new List<Tag>();
            await articlePostRepository.CreateAsync(articlePost);
            ArticlePost? articlePost2 = await articlePostRepository.GetByIdAsync(articlePost.Id);
            Assert.IsNotNull(articlePost2);
            Assert.AreEqual(articlePost2.Id, articlePost.Id);
        });
    }
}