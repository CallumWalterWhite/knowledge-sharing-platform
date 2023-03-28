using KnowledgeShare.Core.Persons;

namespace KnowledgeShare.Core.Posts.Types;

public class FreeFormPost : Post
{
    private FreeFormPost(Person person, DateTime createdDateTime, string title, string body)
        : base(Guid.NewGuid(), person, createdDateTime)
    {
        Title = title;
        Body = body;
        CreateSummary();
    }
    
    public FreeFormPost(Guid id, Person person, DateTime createdDateTime, string title, string body, string summary)
        : base(id, person, createdDateTime)
    {
        Title = title;
        Body = body;
        Summary = summary;
    }
    private string Body { get; }
    
    private string Summary { get; set; }
    
    public string GetTitle() => Title;
    
    public string GetBody() => Body;
    
    public string GetSummary() => Body;

    private void CreateSummary()
    {
        int maxCharLenth = 300;
        Summary = Body.Length > maxCharLenth ? Body.Substring(0, maxCharLenth) : Body;
    }
    
    public static FreeFormPost Create(Person person, string title, string body)
    {
        return new FreeFormPost(person, DateTime.Now, title, body);
    }
}