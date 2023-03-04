using KnowledgeShare.Core.Context.Posts;

namespace KnowledgeShare.Core.Posts.Types;

public interface IPostContextProvider
{
    IPostContext<Post> Get(Type type);
}