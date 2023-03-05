using KnowledgeShare.Core.Persons;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Persons;

public class PersonRepository : IPersonRepository
{
    private readonly IAsyncSession _session;

    public PersonRepository(IAsyncSession session)
    {
        _session = session;
    }

    public async Task<Person?> GetPersonByUserIdAsync(string userId)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"userId", userId }
        };
        Person? person = null;
        IResultCursor cursor = await _session.RunAsync("MATCH (u:Person WHERE u.userId = $userId) RETURN u", statementParameters);
        while (await cursor.FetchAsync())
        {
            object? personRecord = cursor.Current["u"];
            if (personRecord is not null)
            {
                //TODO: init
                person = new Person(Guid.NewGuid(), "", "");
            }
        }

        return person;
    }

    public async Task AddAsync(Person person)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", person.Id.ToString() },
            {"userId", person.UserId },
            {"name", person.Name }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (p:Person {id: $id, userId: $userId, name: $name}) ",
                statementParameters);
        });
    }
}