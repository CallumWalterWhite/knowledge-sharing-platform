using System.Collections;

namespace KnowledgeShare.Core.Posts;

public interface ISearchPostService
{
    Task<IEnumerable<SearchPostResultDto>> SearchAsync(SearchPostDto searchPostDto);
    
    Task<IEnumerable<SearchPostResultDto>> RecommendAsync();
    
    Task<AdminSearchPostResultDto> GetAllAsync(AdminSearchPostDto adminSearchPostDto);
    
    Task<IEnumerable<SearchPostResultDto>> GetPostsByCurrentPersonAsync();
}