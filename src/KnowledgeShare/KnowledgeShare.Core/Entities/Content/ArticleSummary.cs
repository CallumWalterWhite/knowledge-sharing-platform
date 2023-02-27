namespace KnowledgeShare.Core.Entities.Content;

public class ArticleSummary : Entity
{
    public ArticleSummary(
        Guid id,   
        string title,
        string summary,
        string link)
    {
        Title = title;
        Summary = summary;
        Link = link;
    }

    public string Title { get; }
    
    public string Summary { get; }

    public string Link { get; }

    public static ArticleSummary Create(
        string title,
        string summary,
        string link)
    {
        return new ArticleSummary(Guid.NewGuid(), title, summary, link);
    }
}