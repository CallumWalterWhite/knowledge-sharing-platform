using System.ComponentModel.DataAnnotations;
using KnowledgeShare.Core.Validation;

namespace KnowledgeShare.Core.Posts;

public class CreatePostDto
{
    [Required]
    public PostTypeDiscriminator Discriminator { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Article)]
    public string Link { get; set; }
    
    [Required]
    public string Summary { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Free)]
    public string Body { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Book)]
    public string Author { get; set; }
    
    [Required, MinLength(1)]
    public List<string> Tags { get; set; }
}