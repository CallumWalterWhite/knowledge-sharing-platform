using Neo4j.Driver;

namespace KnowledgeShare.Web.Setup.Ioc;

public class GraphInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddScoped<IAsyncSession>(
            (serviceProvider) =>
            {
                IDriver driver = serviceProvider.GetService<IDriver>();
                return driver.AsyncSession();
            }
        );
        services.AddScoped<IDriver>(
            (serviceProvider) =>
            {
#if DEBUG
                IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
                IConfigurationSection neoConfig = configuration.GetSection("Neo4j");
                IConfigurationSection neoConfigUri = neoConfig.GetSection("uri");
                IConfigurationSection neoConfigUser = neoConfig.GetSection("user");
                IConfigurationSection neoConfigPassword = neoConfig.GetSection("password");
                return GraphDatabase.Driver(neoConfigUri.Value, AuthTokens.Basic(neoConfigUser.Value, neoConfigPassword.Value));
#endif

#if !DEBUG
            string boltUrl = Environment.GetEnvironmentVariable("NEO4J_URI");
            string username = Environment.GetEnvironmentVariable("NEO4J_USERNAME");
            string password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD");
            return GraphDatabase.Driver(boltUrl, AuthTokens.Basic(username, password));
#endif
            }
        );
    }
}