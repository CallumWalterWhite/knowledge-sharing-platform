using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts;

public class Post
{
    protected Post(Guid id, Person author, DateTime dateTimeCreated)
    {
        Id = id;
        Author = author;
        DateTimeCreated = dateTimeCreated;
    }
    
    public string Title { get; set; }

    private DateTime DateTimeCreated { get; }

    private Person Author { get; }
    
    public Guid Id { get; set; }
    
    public IEnumerable<Tag> Tags { get; set; }

    public IEnumerable<Guid> PeopleLiked { get; set; }

    public DateTime GetDateTimeCreated() => DateTimeCreated;
    
    public Person GetAuthor() => Author;
}