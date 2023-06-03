using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Posts;

public class SearchPostService : ISearchPostService
{
    private const int MaxCharacterLength = 300;
    
    private readonly ISearchPostQuery _searchPostQuery;

    private readonly ICurrentAuthUser _currentAuthUser;

    private readonly ITagRepository _tagRepository;

    private readonly ILikeRepository _likeRepository;
    
    public SearchPostService(
        ISearchPostQuery searchPostQuery, ICurrentAuthUser currentAuthUser, ITagRepository tagRepository, ILikeRepository likeRepository)
    {
        _searchPostQuery = searchPostQuery;
        _currentAuthUser = currentAuthUser;
        _tagRepository = tagRepository;
        _likeRepository = likeRepository;
    }

    public async Task<IEnumerable<SearchPostResultDto>> SearchAsync(SearchPostDto searchPostDto)
    {
        searchPostDto.Tags = searchPostDto.Tags.Select(x => x.ToLower()).ToList();
        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.SearchAsync(searchPostDto)).ToList();
        return await Hydrate(searchPostResultDtos);
    }

    public async Task<IEnumerable<SearchPostResultDto>> RecommendAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }

        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.RecommendAsync(person.Id)).ToList();
        return await Hydrate(searchPostResultDtos);
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetAllAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }

        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.RecommendAsync(person.Id)).ToList();
        return await Hydrate(searchPostResultDtos);
    }

    public async Task<IEnumerable<SearchPostResultDto>> GetPostsByCurrentPersonAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            return new List<SearchPostResultDto>();
        }
        
        IList<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.GetPostsAsync(person.Id)).ToList();
        return await Hydrate(searchPostResultDtos, true);
    }

    private async Task<IList<SearchPostResultDto>> Hydrate(IList<SearchPostResultDto> searchPostResultDtos, bool ignoreForm = false)
    {
        foreach (SearchPostResultDto searchPostResultDto in searchPostResultDtos)
        {
            if (searchPostResultDto.Summary!.Length > MaxCharacterLength)
            {
                searchPostResultDto.Summary = searchPostResultDto.Type == TypeConstant.FreeFormPost
                    ? ignoreForm is false ? searchPostResultDto.Summary : "Can not render HTML"
                    : searchPostResultDto.Summary.Substring(0, MaxCharacterLength) + "...";   
            }
            IEnumerable<Tag> tags = await _tagRepository.GetAllTagsByPostId(searchPostResultDto.Id);
            searchPostResultDto.Tags = tags.Select(x => x.Value).ToList();
            searchPostResultDto.Likes = (await _likeRepository.GetPeopleIdsByPostIdAsync(searchPostResultDto.Id)).Count();
        }

        return searchPostResultDtos;
    }
}