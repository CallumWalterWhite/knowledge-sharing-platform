namespace KnowledgeShare.Core.Posts.Types;

public interface IGetArticlePostService
{
    Task<ArticlePost> GetArticlePostAsync(Guid id);
}