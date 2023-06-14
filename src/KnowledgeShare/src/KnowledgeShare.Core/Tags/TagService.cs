using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;

namespace KnowledgeShare.Core.Tags;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    private readonly ICurrentAuthUser _currentAuthUser;

    public TagService(ITagRepository tagRepository, ICurrentAuthUser currentAuthUser)
    {
        _tagRepository = tagRepository;
        _currentAuthUser = currentAuthUser;
    }

    public async Task CreateTagForPersonAsync(string tagValue)
    {
        tagValue = tagValue.ToLower();
        Tag? tag = await _tagRepository.GetAsync(tagValue);
        if (tag is null)
        {
            tag = Tag.Create(tagValue);
            await _tagRepository.AddAsync(tag);
        }

        Person? person = await _currentAuthUser.GetPersonAsync();
        await _tagRepository.AddPersonLikeTagRelationship(person!.Id, tag);
    }

    public async Task DeleteTagForPersonAsync(string tagValue)
    {
        Tag? tag = await _tagRepository.GetAsync(tagValue);
        if (tag is null)
        {
            return;
        }

        Person? person = await _currentAuthUser.GetPersonAsync();
        await _tagRepository.DeletePersonLikeTagRelationship(person!.Id, tag);
    }

    public async Task DeleteTag(Guid id)
    {
        Tag? tag = await _tagRepository.GetTagByIdAsync(id);
        if (tag is null)
        {
            return;
        }
        
        await _tagRepository.DeleteAsync(tag);
    }
}