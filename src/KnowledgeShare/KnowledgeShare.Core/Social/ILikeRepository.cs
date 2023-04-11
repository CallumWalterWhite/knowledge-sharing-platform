namespace KnowledgeShare.Core.Social;

public interface ILikeRepository
{
    Task CreateLikeAsync(Guid personId, Guid postId);

    Task<IEnumerable<Guid>> GetPeopleIdsByPostIdAsync(Guid postId);
}