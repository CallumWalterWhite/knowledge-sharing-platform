using System.Collections;
using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private readonly IPostContext<ArticlePost> _articlePostContext;
    
    private readonly IPostContext<BookPost> _bookPostContext;

    private readonly ISearchPostQuery _searchPostQuery;

    public SearchPostService(
        IPostContext<ArticlePost> articlePostContext, 
        IPostContext<BookPost> bookPostContext, 
        ISearchPostQuery searchPostQuery)
    {
        _articlePostContext = articlePostContext;
        _bookPostContext = bookPostContext;
        _searchPostQuery = searchPostQuery;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(string search)
        => await _searchPostQuery.SearchAsync(search);
}