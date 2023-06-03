using System.Security.Claims;
using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.People.Person;

namespace KnowledgeShare.Web.Setup.Middleware;

public class CheckUserMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context,
        IPersonService personService, 
        GraphServiceClient graphServiceClient, 
        ICurrentAuthUser currentAuthUser)
    {
        var identity = context.User.Identity;
        if (identity != null && identity.IsAuthenticated)
        {
            string userName = context.User.FindFirst(ClaimTypes.NameIdentifier).Value!;
            Person? person = await personService.GetPersonByUserIdAsync(userName);
            if (person == null)
            {
                User user = graphServiceClient.Me.Request().GetAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                if (user is not null)
                {
                    Stream photoStream = await graphServiceClient.Me.Photo.Content.Request().GetAsync();
                    byte[] buffer = new byte[16*1024];
                    string dataImage = string.Empty;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = await photoStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byte[] data = ms.ToArray();
                        dataImage = $"data:image/jpeg;base64,{Convert.ToBase64String(data)}";
                    }
                    await personService.CreatePersonAsync(new CreatePersonDto(userName, user.DisplayName, dataImage));
                }
            }
            person = await personService.GetPersonByUserIdAsync(userName);
            currentAuthUser.SetCurrentAuthUser(person!);
        }
        await _next(context);
    }
}