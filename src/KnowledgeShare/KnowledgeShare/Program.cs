using KnowledgeShare.Core.Context;
using KnowledgeShare.Persistence.Content;
using KnowledgeShare.Persistence.Tags;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
#endif
#if !DEBUG
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
#endif

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddScoped<IDriver>(
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

builder.Services.AddScoped<IAsyncSession>(
    (serviceProvider) =>
    {
        IDriver driver = serviceProvider.GetService<IDriver>();
        return driver.AsyncSession();
    }
);

builder.Services.AddScoped(typeof(ITagContext), typeof(TagContext));
builder.Services.AddScoped(typeof(IArticleSummaryContext), typeof(ArticleSummaryContext));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#if DEBUG
// Add services to the container.
app.UseAuthentication();
#endif
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
