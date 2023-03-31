using System.Text.RegularExpressions;
using KnowledgeShare.Core.People;

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
        int maxCharLength = 300;
        Summary = Body.Length > maxCharLength ? Regex.Match(Body.Substring(0, Math.Min(Body.Length, maxCharLength)), @"^(.*?)\b").Value : Body;
    }
    
    public static FreeFormPost Create(Person person, string title, string body)
    {
        return new FreeFormPost(person, DateTime.Now, title, body);
    }
}