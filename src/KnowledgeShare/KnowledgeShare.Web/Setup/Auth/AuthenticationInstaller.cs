using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace KnowledgeShare.Web.Setup.Auth;

public class AuthenticationInstaller
{
    public static void Install(WebApplicationBuilder builder)
    {
        var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');
#if DEBUG
        
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();
#endif
#if !DEBUG
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme);
#endif
    }
}