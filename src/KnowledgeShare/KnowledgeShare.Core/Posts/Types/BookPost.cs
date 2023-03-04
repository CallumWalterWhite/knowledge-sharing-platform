namespace KnowledgeShare.Core.Posts.Types;

public class BookPost : Post
{
    public BookPost(string title, string summary)
        : base(Guid.NewGuid())
    {
        Title = title;
        Summary = summary;
    }
    
    public BookPost(Guid id, string title, string summary)
        : base(id)
    {
        Title = title;
        Summary = summary;
    }

    private string Title { get; set; }
    private string Summary { get; set; }
    

    public string GetTitle() => Title;
    
    public string GetSummary() => Summary;
}