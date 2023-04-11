namespace KnowledgeShare.Core.Social;

public interface ICreateLikeRelationshipService
{
    public Task CreateLike(Guid postId);
}