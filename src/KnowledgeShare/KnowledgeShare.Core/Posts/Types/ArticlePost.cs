using KnowledgeShare.Core.Persons;

namespace KnowledgeShare.Core.Posts.Types;

public class ArticlePost : Post
{
    private ArticlePost(Person person, DateTime createdDateTime, string title, string link, string summary)
        : base(Guid.NewGuid(), person, createdDateTime)
    {
        Title = title;
        Link = link;
        Summary = summary;
    }
    
    public ArticlePost(Guid id, Person person, DateTime createdDateTime, string title, string link, string summary)
        : base(id, person, createdDateTime)
    {
        Title = title;
        Link = link;
        Summary = summary;
    }

    private string Link { get; set; }

    private string Summary { get; set; }

    public string GetTitle() => Title;
    
    public string GetLink() => Link;
    
    public string GetSummary() => Summary;

    public static ArticlePost Create(Person person, string title, string link, string summary)
    {
        return new ArticlePost(person, DateTime.Now, title, link, summary);
    }
}