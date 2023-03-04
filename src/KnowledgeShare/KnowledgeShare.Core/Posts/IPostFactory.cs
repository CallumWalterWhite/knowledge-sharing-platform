namespace KnowledgeShare.Core.Posts;

public interface IPostFactory
{
    public Post Create(CreatePostDto createPostDto);
}