namespace KnowledgeShare.Core.Posts.Types;

public interface IPostRepositoryProvider
{
    IPostRepository<Post> Get(Type type);
}