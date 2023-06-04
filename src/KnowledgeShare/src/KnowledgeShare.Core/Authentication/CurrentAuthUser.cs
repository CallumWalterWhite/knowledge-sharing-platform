using System.Security.Claims;
using KnowledgeShare.Core.People;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Person = KnowledgeShare.Core.People.Person;

namespace KnowledgeShare.Core.Authentication;

public class CurrentAuthUser : ICurrentAuthUser
{
    private bool _isPersonCreated;
    private Person? _person;
    private readonly string _userName;
    private readonly IPersonService _personService;
    private readonly GraphServiceClient _graphServiceClient;
    
    public CurrentAuthUser(GraphServiceClient graphServiceClient, IPersonService personService, IHttpContextAccessor httpContextAccessor)
    {
        _userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        _graphServiceClient = graphServiceClient;
        _personService = personService;
    }
    
    public void SetCurrentAuthUser(Person person)
    {
        _person = person;
        _isPersonCreated = true;
    }

    public async Task<Person?> GetPersonAsync()
    {
        if (_isPersonCreated) return _person;
        Person? person = await _personService.GetPersonByUserIdAsync(_userName);
        if (person == null)
        {
            User user = await _graphServiceClient.Me.Request().GetAsync();
            if (user is not null)
            {
                string dataImage = string.Empty;
                try
                {
                    Stream photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
                    byte[] buffer = new byte[16 * 1024];
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
                }
                catch (Exception exception)
                {
                    //IGNORE exception
                    dataImage = string.Empty;
                }
                await _personService.CreatePersonAsync(new CreatePersonDto(_userName, user.DisplayName, dataImage));
            }
        } 
        _person = await _personService.GetPersonByUserIdAsync(_userName);
        _isPersonCreated = true;
        return _person;

    }
}