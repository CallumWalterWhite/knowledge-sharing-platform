using KnowledgeShare.Core.Entities.Content;

namespace KnowledgeShare.Core.Context;

public interface IArticleSummaryContext
{
    Task AddAsync(ArticleSummary articleSummary);

    Task<IEnumerable<ArticleSummary>> GetAllAsync();
}