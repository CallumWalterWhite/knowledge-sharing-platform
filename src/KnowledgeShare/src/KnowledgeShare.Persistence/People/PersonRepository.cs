using KnowledgeShare.Core.People;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.People;

public class PersonRepository : IPersonRepository
{
    private readonly IAsyncSession _session;

    private readonly INeo4jDataAccess _neo4JDataAccess;

    public PersonRepository(IAsyncSession session, INeo4jDataAccess neo4JDataAccess)
    {
        _session = session;
        _neo4JDataAccess = neo4JDataAccess;
    }

    public async Task<Person?> GetPersonByUserIdAsync(string userId)
    {
        string query = @"MATCH (p:Person WHERE p.userId = $userId) RETURN p{ id: p.id, userId: p.userId, name: p.name, picture: p.picture, isadmin: p.isadmin }";

        IDictionary<string, object> parameters = new Dictionary<string, object> { { "userId", userId } };

        IList<Dictionary<string,object>> persons = await _neo4JDataAccess.ExecuteReadDictionaryAsync(query, "p", parameters);

        Person? person = null;
        
        if (persons.Any())
        {
            person = CreatePersonFromResult(persons.Single());
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
    
    

    /// <summary>
    /// Searches the name of the person.
    /// </summary>
    public async Task<Person?> GetAsync(Guid id)
    {
        string query = @"MATCH (p:Person WHERE p.id = $id) RETURN p{ id: p.id, userId: p.userId, name: p.name, picture: p.picture, isadmin: p.isadmin }";

        IDictionary<string, object> parameters = new Dictionary<string, object> { { "id", id.ToString() } };

        IList<Dictionary<string,object>> persons = await _neo4JDataAccess.ExecuteReadDictionaryAsync(query, "p", parameters);

        Person? person = null;
        
        if (persons.Any())
        {
            person = CreatePersonFromResult(persons.Single());
        }

        return person;
    }
    
    private Person CreatePersonFromResult(Dictionary<string,object> dict)
    {
        return new Person(
            Guid.Parse(dict["id"].ToString()),
            dict["userId"].ToString(),
            dict["name"].ToString(),
            dict["picture"].ToString(),
                 bool.Parse(dict["isadmin"]?.ToString() ?? "false")
        );
    }
}