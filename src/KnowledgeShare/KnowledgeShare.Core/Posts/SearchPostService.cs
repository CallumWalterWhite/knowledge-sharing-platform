using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.Persons;
using KnowledgeShare.Core.Posts.Types;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private readonly ISearchPostQuery _searchPostQuery;

    private readonly ICurrentAuthUser _currentAuthUser;
    
    public SearchPostService(
        ISearchPostQuery searchPostQuery, ICurrentAuthUser currentAuthUser)
    {
        _searchPostQuery = searchPostQuery;
        _currentAuthUser = currentAuthUser;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(string search)
        => await _searchPostQuery.SearchAsync(search);

    public async Task<IEnumerable<SearchPostResultDto>> RecommendAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }

        return await _searchPostQuery.RecommendAsync(person.Id);
    }
}