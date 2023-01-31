using KnowledgeShare.Core.Repository;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddSingleton<IDriver>(
        (serviceProvider) =>
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            IConfigurationSection neoConfig = configuration.GetSection("Neo4j");
            IConfigurationSection neoConfigUri = neoConfig.GetSection("uri");
            IConfigurationSection neoConfigUser = neoConfig.GetSection("user");
            IConfigurationSection neoConfigPassword = neoConfig.GetSection("password");
            return GraphDatabase.Driver(neoConfigUri.Value, AuthTokens.Basic(neoConfigUser.Value, neoConfigPassword.Value));
        }
    );

builder.Services.AddScoped<IAsyncSession>(
    (serviceProvider) =>
    {
        IDriver driver = serviceProvider.GetService<IDriver>();
        return driver.AsyncSession();
    }
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
