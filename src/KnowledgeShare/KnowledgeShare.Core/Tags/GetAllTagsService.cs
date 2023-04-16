using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;

namespace KnowledgeShare.Core.Tags;

public class GetAllTagsService : IGetAllTagsService
{
    private readonly ITagRepository _tagRepository;

    private readonly ICurrentAuthUser _currentAuthUser;

    public GetAllTagsService(ITagRepository tagRepository, ICurrentAuthUser currentAuthUser)
    {
        _tagRepository = tagRepository;
        _currentAuthUser = currentAuthUser;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _tagRepository.GetAllTags();
    }

    public async Task<IEnumerable<Tag>> GetAllAsyncByValue(string value)
    {
        return await _tagRepository.GetAllTagsByValue(value);
    }

    public async Task<IEnumerable<Tag>> GetLikedTagsAsync()
    {
        Person? person = await _currentAuthUser.GetPersonAsync();
        if (person is null)
        {
            throw new Exception("Person can not be null");
        }

        return await _tagRepository.GetTagsLikedByPersonId(person.Id);
    }
}