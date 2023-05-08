namespace KnowledgeShare.Core.Posts;

public interface ICreatePostService
{
    Task<Guid> Create(CreatePostDto createPostDto);
}