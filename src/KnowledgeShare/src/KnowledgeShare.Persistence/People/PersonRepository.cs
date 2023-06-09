using KnowledgeShare.Core.People;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.People;

public class PersonRepository : IPersonRepository
{
    private readonly IAsyncSession _session;

    public PersonRepository(IAsyncSession session)
    {
        _session = session;
    }

    public async Task<Person?> GetPersonByUserIdAsync(string userId)
    {
        Person? person = null;
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"userId", userId }
        };
        IResultCursor cursor = await _session.RunAsync("MATCH (p:Person WHERE p.userId = $userId) RETURN p{ id: p.id, userId: p.userId, name: p.name, picture: p.picture, isadmin: p.isadmin}", statementParameters);
        while (await cursor.FetchAsync())
        {
            person = CreatePersonFromResult(cursor.Current);
        }

        return person;
    }

    public async Task AddAsync(Person person)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", person.Id.ToString() },
            {"userId", person.UserId },
            {"picture", person.Picture },
            {"name", person.Name }
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (p:Person {id: $id, userId: $userId, name: $name, picture: $picture}) ",
                statementParameters);
        });
    }
    
    public async Task<Person?> GetAsync(Guid id)
    {
        Person? person = null;
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", id.ToString() }
        };
        IResultCursor cursor = await _session.RunAsync("MATCH (p:Person WHERE p.id = $id) RETURN p{ id: p.id, userId: p.userId, name: p.name, picture: p.picture, isadmin: p.isadmin}", statementParameters);
        while (await cursor.FetchAsync())
        {
            person = CreatePersonFromResult(cursor.Current);
        }

        return person;
    }

    public async Task SetAdminAsync(Guid id, bool admin)
    {
        Dictionary<string, object> statementParameters = new Dictionary<string, object>
        {
            {"id", id.ToString() },
            {"isadmin", admin }
        };
        await _session.RunAsync("MATCH (p:Person WHERE p.id = $id) SET p.isadmin = $isadmin", statementParameters);
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        List<Person> people = new List<Person>();
        IResultCursor cursor = await _session.RunAsync("MATCH (p:Person) RETURN p{ id: p.id, userId: p.userId, name: p.name, picture: p.picture, isadmin: p.isadmin}");
        while (await cursor.FetchAsync())
        {
            people.Add(CreatePersonFromResult(cursor.Current));
        }

        return people;
    }

    private Person CreatePersonFromResult(IRecord record)
    {
        return new Person(
            Guid.Parse(((Dictionary<string,object>)record["p"])["id"].ToString()),
            ((Dictionary<string,object>)record["p"])["userId"].ToString(),
            ((Dictionary<string,object>)record["p"])["name"].ToString(),
            ((Dictionary<string,object>)record["p"])["picture"].ToString(),
                 bool.Parse(((Dictionary<string,object>)record["p"])["isadmin"]?.ToString() ?? "false")
        );
    }
}