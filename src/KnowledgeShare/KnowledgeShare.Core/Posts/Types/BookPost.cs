using KnowledgeShare.Core.People;

namespace KnowledgeShare.Core.Posts.Types;

public class BookPost : Post
{
    private BookPost(Person person, DateTime createdDateTime, string title, string author, string summary)
        : base(Guid.NewGuid(), person, createdDateTime)
    {
        Title = title;
        BookAuthor = author;
        Summary = summary;
    }
    
    public BookPost(Guid id, Person person, DateTime createdDateTime, string title, string author, string summary)
        : base(id, person, createdDateTime)
    {
        Title = title;
        BookAuthor = author;
        Summary = summary;
    }
    private string Summary { get; set; }
    
    private string BookAuthor { get; set; }

    public string GetTitle() => Title;
    
    public string GetSummary() => Summary;
    
    public string GetBookAuthor() => BookAuthor;
    
    public static BookPost Create(Person person, string title, string author, string summary)
    {
        return new BookPost(person, DateTime.Now, title, author, summary);
    }
}