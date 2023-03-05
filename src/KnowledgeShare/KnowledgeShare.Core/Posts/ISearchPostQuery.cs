using System.Collections;

namespace KnowledgeShare.Core.Posts;

public interface ISearchPostQuery
{
       Task<IEnumerable<SearchPostResultDto>> SearchAsync(string searchQuery);
}