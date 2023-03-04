using KnowledgeShare.Core.Entities;
using KnowledgeShare.Core.Entities.Tags;

namespace KnowledgeShare.Core.Posts;

public class Post
{
    protected Post(Guid id)
    {
        Id = id;
    }
    
    public string Title { get; set; }
    
    public Guid Id { get; set; }
    
    public IEnumerable<Tag> Tags { get; set; }
}