namespace KnowledgeShare.Core.Posts;

public interface ICreatePostService
{
    Task Create(CreatePostDto createPostDto);
}