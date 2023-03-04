using KnowledgeShare.Core.Context.Posts;
using Neo4j.Driver;

namespace KnowledgeShare.Core.Posts.Types;

public class PostContextProvider : IPostContextProvider
{
    private readonly IAsyncSession _asyncSession;
    
    private readonly Dictionary<Type, object> _contexts = new Dictionary<Type, object>();

    public PostContextProvider(IAsyncSession asyncSession, 
        IPostContext<ArticlePost> articlePostContext,
        IPostContext<BookPost> bookPostContext)
    {
        _asyncSession = asyncSession;
        _contexts.Add(typeof(ArticlePost), articlePostContext);
        _contexts.Add(typeof(BookPost), bookPostContext);
    }

    public IPostContext<Post> Get(Type type)
    {
        if (_contexts.TryGetValue(type, out var context))
        {
            return (IPostContext<Post>)context;
        }
        else
        {
            throw new ArgumentException($"No repository found for type {type}");
        }
    }
}