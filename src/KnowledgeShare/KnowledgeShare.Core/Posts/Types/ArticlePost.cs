namespace KnowledgeShare.Core.Posts.Types;

public class ArticlePost : Post
{
    public ArticlePost(string title, string link, string summary)
        : base(Guid.NewGuid())
    {
        Title = title;
        Link = link;
        Summary = summary;
    }
    
    public ArticlePost(Guid id, string title, string link, string summary)
        : base(id)
    {
        Title = title;
        Link = link;
        Summary = summary;
    }

    private string Title { get; set; }

    private string Link { get; set; }

    private string Summary { get; set; }

    public string GetTitle() => Title;
    
    public string GetLink() => Link;
    
    public string GetSummary() => Summary;
}