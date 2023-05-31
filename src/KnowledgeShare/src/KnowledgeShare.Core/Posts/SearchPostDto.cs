namespace KnowledgeShare.Core.Posts;

public class SearchPostDto
{
    public IEnumerable<string> Tags { get; set; }
    
    public string Title { get; set; }
    
    public int Skip { get; set; }
}