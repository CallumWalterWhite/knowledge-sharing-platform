namespace KnowledgeShare.Core.Posts;

public class SearchPostResultDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string CreatedDate { get; set; }
    
    public Guid UserCreated { get; set; }
    
    public int Distance { get; set; }
}