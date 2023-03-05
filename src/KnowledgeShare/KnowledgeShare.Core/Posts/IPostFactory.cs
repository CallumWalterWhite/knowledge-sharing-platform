namespace KnowledgeShare.Core.Posts;

public interface IPostFactory
{
    Task<Post> Create(CreatePostDto createPostDto);
}