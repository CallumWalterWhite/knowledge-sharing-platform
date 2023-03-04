using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Entities.Tags;

namespace KnowledgeShare.Core.Tags;

public class GetAllTagsService : IGetAllTagsService
{
    private readonly ITagContext _tagContext;

    public GetAllTagsService(ITagContext tagContext)
    {
        _tagContext = tagContext;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _tagContext.GetAllTags();
    }

    public async Task<IEnumerable<Tag>> GetAllAsyncByValue(string value)
    {
        return await _tagContext.GetAllTagsByValue(value);
    }
}