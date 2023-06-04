using Microsoft.Graph.SecurityNamespace;

namespace KnowledgeShare.Core.People;

public class Person
{
    private Person(string userId, string name, string picture)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Picture = picture;
        PersonTags = new List<Tag>();
    }

    public Person(Guid id, string userId, string name, string picture, bool isAdmin = false)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Picture = picture;
        IsAdmin = isAdmin;
    }
    
    public Guid Id { get; set; }
    
    public string UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Picture { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public IList<Tag> PersonTags { get; set; }

    public string GetPicture()
    {
        return string.IsNullOrWhiteSpace(Picture) ? "/Images/avatardefault.png" : Picture;
    }

    public static Person Create(string userId, string name, string picture)
    {
        return new Person(Guid.NewGuid(), userId, name, picture);
    }
}