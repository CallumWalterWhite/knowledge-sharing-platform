namespace KnowledgeShare.Core.Posts;

public class CreatePostDto
{
    public PostTypeDiscriminator Discriminator { get; set; }
    
    public string Title { get; set; }
    
    public string Link { get; set; }
    
    public string Summary { get; set; }
    
    public string Body { get; set; }
    
    public string Author { get; set; }
    
    public IEnumerable<string> Tags { get; set; }
}