using System.ComponentModel.DataAnnotations;
using KnowledgeShare.Core.Validation;

namespace KnowledgeShare.Core.Posts;

public class ChangePostDto
{
    public  Guid Id { get; set; }
    
    [Required]
    public PostTypeDiscriminator Discriminator { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Article, 
        "A link is required for link type posts."
    )]
    public string Link { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Article, 
        "A summary is required for link type posts.")]
    public string Summary { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Free, 
        "A body is required for link type posts.")]
    public string Body { get; set; }
    
    [RequiredIf(nameof(Discriminator), PostTypeDiscriminator.Book, 
        "A author is required for link type posts.")]
    public string Author { get; set; }
    
    [Required, MinLength(1, ErrorMessage = "You must have one tag added to your post.")]
    public IEnumerable<string> Tags { get; set; }
}