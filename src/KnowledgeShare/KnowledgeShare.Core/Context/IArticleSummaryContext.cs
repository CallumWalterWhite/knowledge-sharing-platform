using KnowledgeShare.Core.Entities.Content;
using KnowledgeShare.Core.Entities.Tags;

namespace KnowledgeShare.Core.Context;

public interface IArticleSummaryContext
{
    Task AddAsync(ArticleSummary articleSummary, List<Tag> tags);

    Task AddTagAsync(ArticleSummary articleSummary, List<Tag> tags);

    Task<IEnumerable<ArticleSummary>> GetAllAsync();
}