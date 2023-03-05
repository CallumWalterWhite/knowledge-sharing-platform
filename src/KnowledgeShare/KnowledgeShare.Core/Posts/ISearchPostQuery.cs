namespace KnowledgeShare.Core.Posts;

public interface ISearchPostQuery
{
       Task<IEnumerable<SearchPostResultDto>> SearchAsync(string searchQuery);
       
       Task<IEnumerable<SearchPostResultDto>> RecommendAsync(Guid personId);
}