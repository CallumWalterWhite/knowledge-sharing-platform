using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace KnowledgeShare.Web.Setup.Auth;

public class AuthenticationInstaller
{
    public static void Install(WebApplicationBuilder builder)
    {
#if DEBUG
// Add services to the container.
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
#endif
#if !DEBUG
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
#endif
    }
}