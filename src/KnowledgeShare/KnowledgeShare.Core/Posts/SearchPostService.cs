using System.Collections;
using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private readonly IPostContext<ArticlePost> _articlePostContext;
    
    private readonly IPostContext<BookPost> _bookPostContext;

    public SearchPostService(IPostContext<ArticlePost> articlePostContext, IPostContext<BookPost> bookPostContext)
    {
        _articlePostContext = articlePostContext;
        _bookPostContext = bookPostContext;
    }

    public async Task<IEnumerable<Post>> SearchAsync(string search)
    {
        List<Post> posts = new List<Post>();
        posts.AddRange(await _articlePostContext.GetAllAsync());
        posts.AddRange(await _bookPostContext.GetAllAsync());
        return posts;
    }
}