namespace KnowledgeShare.Core.Posts;

public class SearchPostResultDto
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Summary { get; set; }
    
    public string? CreatedDate { get; set; }
    
    public string? UserCreatedName { get; set; }
    
    public IList<string>? Tags { get; set; }

    public int Distance { get; set; }
    
    public string? Type { get; set; }

    public string? GetSummary()
    {
        return Summary;
    }
}