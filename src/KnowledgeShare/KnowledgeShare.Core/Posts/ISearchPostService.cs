using System.Collections;

namespace KnowledgeShare.Core.Posts;

public interface ISearchPostService
{
    Task<IEnumerable<SearchPostResultDto>> SearchAsync(string search);
    
    Task<IEnumerable<SearchPostResultDto>> RecommendAsync();
    
    Task<IEnumerable<SearchPostResultDto>> GetPostsByCurrentPersonAsync();
}