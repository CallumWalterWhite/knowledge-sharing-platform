using KnowledgeShare.Core.Persons;

namespace KnowledgeShare.Core.Posts.Types;

public class BookPost : Post
{
    private BookPost(Person person, DateTime createdDateTime, string title, string summary)
        : base(Guid.NewGuid(), person, createdDateTime)
    {
        Title = title;
        Summary = summary;
    }
    
    public BookPost(Guid id, Person person, DateTime createdDateTime, string title, string summary)
        : base(id, person, createdDateTime)
    {
        Title = title;
        Summary = summary;
    }
    private string Summary { get; set; }
    

    public string GetTitle() => Title;
    
    public string GetSummary() => Summary;
    
    public static BookPost Create(Person person, string title, string summary)
    {
        return new BookPost(person, DateTime.Now, title, summary);
    }
}