using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private readonly IPostRepository<ArticlePost> _articlePostRepository;
    
    private readonly IPostRepository<BookPost> _bookPostRepository;

    private readonly ISearchPostQuery _searchPostQuery;

    public SearchPostService(
        IPostRepository<ArticlePost> articlePostRepository, 
        IPostRepository<BookPost> bookPostRepository, 
        ISearchPostQuery searchPostQuery)
    {
        _articlePostRepository = articlePostRepository;
        _bookPostRepository = bookPostRepository;
        _searchPostQuery = searchPostQuery;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(string search)
        => await _searchPostQuery.SearchAsync(search);
}