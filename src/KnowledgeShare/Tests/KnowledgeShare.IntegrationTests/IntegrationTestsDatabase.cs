using System.Diagnostics;
using Neo4j.Driver;

namespace KnowledgeShare.IntegrationTests;

[Category("Database Integration")]
public class IntegrationTestsDatabase
{
    private IDriver _driver;
    
    public IntegrationTestsDatabase()
    {
        _driver = GraphDatabase.Driver(GlobalSetupFixture.Neo4jConfigUri, AuthTokens.Basic(GlobalSetupFixture.Neo4jConfigUser, GlobalSetupFixture.Neo4jConfigPassword));
    }
    
    protected async Task WithSessionAsync(Func<IAsyncSession, Task> func)
    {
        IAsyncSession asyncSession = _driver.AsyncSession();
        //await asyncSession.BeginTransactionAsync();
        await func.Invoke(asyncSession);
        //await asyncSession.CloseAsync();
    }
    
    [TearDown]
    public virtual void Teardown()
    {
        IAsyncSession asyncSession = _driver.AsyncSession();
        asyncSession.ExecuteWriteAsync(async tx =>
        {
            string query = "MATCH (n) " +
                           "DETACH DELETE n";
            await tx.RunAsync(query);
        }).GetAwaiter().GetResult();
    }
}