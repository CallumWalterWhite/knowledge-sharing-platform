using KnowledgeShare.Core.Posts;

namespace KnowledgeShare.Core.Social;

public class PostComment
{
    private PostComment(string commentText, Guid postId, Guid personId)
    {
        Id = Guid.NewGuid();
        DateTimeCreated = DateTime.Now;
        CommentText = commentText;
        PostId = postId;
        PersonId = personId;
    }
    
    public PostComment(Guid id, DateTime dateTime, string commentText, Guid postId, Guid personId)
    {
        Id = id;
        DateTimeCreated = dateTime;
        CommentText = commentText;
        PostId = postId;
        PersonId = personId;
    }

    public Guid Id { get; }
    
    public DateTime DateTimeCreated { get; }
    
    public string CommentText { get; }
    
    public Guid PostId { get; }

    public Guid PersonId { get; }

    public static PostComment Create(string commentText, Guid postId, Guid personId)
    {
        return new PostComment(commentText, postId, personId);
    }
}