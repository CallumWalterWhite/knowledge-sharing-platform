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
            var personService = context.RequestServices.GetService<IPersonService>();
            var graphClient = context.RequestServices.GetService<GraphServiceClient>();
            User user = await graphClient!.Me.Request().GetAsync();
            Person? person = await personService!.GetPersonByUserIdAsync(user.Id);

            if (person == null)
            {
                await personService!.CreatePersonAsync(new CreatePersonDto(user.Id, user.DisplayName));
            }
        }
        await _next(context);
    }
}