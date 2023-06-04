namespace KnowledgeShare.Core.Posts;

public class SearchPostResultDto
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Summary { get; set; }

    public string? CreatedDate
    {
        get => _createdDate?.Substring(0, 10);
        set
        {
            _createdDate = value;
        }
    }
    
    private string? _createdDate { get; set; }
    

    public string? UserPhoto { get; set; }

    public string? UserCreatedName { get; set; }
    
    public IList<string>? Tags { get; set; }

    public int Distance { get; set; }
    
    public string? Type { get; set; }

    public int TagCount => Tags?.Count ?? 0;
    
    public int Likes { get; set; }
    
    public int Comments { get; set; }

    public string? GetSummary()
    {
        return Summary;
    }

    public string? GetUserPhoto()
    {
        return string.IsNullOrWhiteSpace(UserPhoto) ? "/Images/avatardefault.png" : UserPhoto;
    }
}