using System.Collections;

namespace KnowledgeShare.Core.Posts;

public interface ISearchPostService
{
    Task<IEnumerable<SearchPostResultDto>> SearchAsync(SearchPostDto searchPostDto);
    
    Task<IEnumerable<SearchPostResultDto>> RecommendAsync();
    
    Task<IEnumerable<SearchPostResultDto>> GetAllAsync();
    
    Task<IEnumerable<SearchPostResultDto>> GetPostsByCurrentPersonAsync();
}