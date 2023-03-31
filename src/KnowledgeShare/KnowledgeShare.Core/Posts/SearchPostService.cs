using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private const int MaxCharacterLength = 300;
    
    private readonly ISearchPostQuery _searchPostQuery;

    private readonly ICurrentAuthUser _currentAuthUser;

    private readonly ITagRepository _tagRepository;
    
    public SearchPostService(
        ISearchPostQuery searchPostQuery, ICurrentAuthUser currentAuthUser, ITagRepository tagRepository)
    {
        _searchPostQuery = searchPostQuery;
        _currentAuthUser = currentAuthUser;
        _tagRepository = tagRepository;
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

        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.RecommendAsync(person.Id)).ToList();
        return await HydrateTags(searchPostResultDtos);
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetAllAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }

        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.RecommendAsync(person.Id)).ToList();
        return await HydrateTags(searchPostResultDtos);
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsByCurrentPersonAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }
        
        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.GetPostsAsync(person.Id)).ToList();
        return await HydrateTags(searchPostResultDtos);
    }

    private async Task<IList<SearchPostResultDto>> HydrateTags(IList<SearchPostResultDto> searchPostResultDtos)
    {
        foreach (SearchPostResultDto searchPostResultDto in searchPostResultDtos)
        {
            if (searchPostResultDto.Summary.Length > MaxCharacterLength)
            {
                searchPostResultDto.Summary = searchPostResultDto.Summary.Substring(0, MaxCharacterLength) + "...";   
            }
            IEnumerable<Tag> tags = await _tagRepository.GetAllTagsByPostId(searchPostResultDto.Id);
            searchPostResultDto.Tags = tags.Select(x => x.Value).ToList();
        }

        return searchPostResultDtos;
    }
}