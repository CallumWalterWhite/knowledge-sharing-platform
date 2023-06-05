using Microsoft.Extensions.Configuration;

namespace KnowledgeShare.IntegrationTests;

[SetUpFixture]
public class GlobalSetupFixture
{
    private static IConfigurationRoot _configurationRoot = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettingsdev.json", optional: false, reloadOnChange: true)
        .Build();

    public static string? Neo4jConfigUri => _configurationRoot.GetSection("Neo4j:uri").Value;
    
    public static string? Neo4jConfigUser => _configurationRoot.GetSection("Neo4j:user").Value;
    
    public static string? Neo4jConfigPassword => _configurationRoot.GetSection("Neo4j:password").Value;
}