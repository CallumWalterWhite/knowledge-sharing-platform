using System.Collections;

namespace KnowledgeShare.Core.Posts;

public class AdminSearchPostResultDto
{
    public IEnumerable<SearchPostResultDto> SearchPostResultDtos { get; set; }
    
    public int Count { get; set; }
}