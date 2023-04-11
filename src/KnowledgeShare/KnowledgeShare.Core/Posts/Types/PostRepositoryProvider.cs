using Neo4j.Driver;

namespace KnowledgeShare.Core.Posts.Types;

public class PostRepositoryProvider : IPostRepositoryProvider
{
    private readonly Dictionary<Type, object> _contexts = new Dictionary<Type, object>();

    public PostRepositoryProvider(
        IPostRepository<ArticlePost> articlePostRepository,
        IPostRepository<BookPost> bookPostRepository,
        IPostRepository<FreeFormPost> freeFormPostRepository)
    {
        _contexts.Add(typeof(ArticlePost), articlePostRepository);
        _contexts.Add(typeof(BookPost), bookPostRepository);
        _contexts.Add(typeof(FreeFormPost), freeFormPostRepository);
    }

    public IPostRepository<Post> Get(Type type)
    {
        if (_contexts.TryGetValue(type, out var context))
        {
            return (IPostRepository<Post>)context;
        }
        else
        {
            throw new ArgumentException($"No repository found for type {type}");
        }
    }
}