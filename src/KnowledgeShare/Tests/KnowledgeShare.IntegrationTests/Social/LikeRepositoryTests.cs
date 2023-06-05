using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence.People;
using KnowledgeShare.Persistence.Posts;
using KnowledgeShare.Persistence.Social;

namespace KnowledgeShare.IntegrationTests.Social;

public class LikeRepositoryTests : IntegrationTestsDatabase
{
    [Test]
    public async Task GivenLike_WhenGetPeopleIdsByPostIdAsync_ReturnLike()
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
            LikeRepository repository = new LikeRepository(session);
            await repository.CreateLikeAsync(person.Id, articlePost.Id);
            IEnumerable<Guid> postLikes = await repository.GetPeopleIdsByPostIdAsync(articlePost.Id);
            Assert.AreEqual(1, postLikes.Count());
        });
    }
}