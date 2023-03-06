using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.Persons;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.Persons.Person;

namespace KnowledgeShare.Web.Setup.Middleware;

public class CheckUserMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var identity = context.User.Identity;
        if (identity != null && identity.IsAuthenticated)
        {
        }
        await _next(context);
    }
}