using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowledgeShare.Web.Pages;

[AllowAnonymous]  
[IgnoreAntiforgeryToken]  
public class LogoutModel : PageModel  
{  
    private readonly ILogger<LogoutModel> _logger;  
  
    public LogoutModel(ILogger<LogoutModel> logger)  
    {  
        _logger = logger;  
    }  
  
    public async Task<IActionResult> OnGet()  
    {  
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  
        _logger.LogInformation("User logged out.");  
        return Redirect("/");  
    }  
}  