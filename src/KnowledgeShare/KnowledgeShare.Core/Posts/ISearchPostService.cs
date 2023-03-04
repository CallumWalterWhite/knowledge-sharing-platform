using System.Collections;

namespace KnowledgeShare.Core.Posts;

public interface ISearchPostService
{
    Task<IEnumerable<Post>> SearchAsync(string search);
}