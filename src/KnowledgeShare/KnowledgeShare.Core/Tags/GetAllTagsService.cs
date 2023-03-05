namespace KnowledgeShare.Core.Tags;

public class GetAllTagsService : IGetAllTagsService
{
    private readonly ITagRepository _tagRepository;

    public GetAllTagsService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _tagRepository.GetAllTags();
    }

    public async Task<IEnumerable<Tag>> GetAllAsyncByValue(string value)
    {
        return await _tagRepository.GetAllTagsByValue(value);
    }
}