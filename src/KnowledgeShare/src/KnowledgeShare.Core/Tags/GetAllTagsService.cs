﻿using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;

namespace KnowledgeShare.Core.Tags;

public class GetAllTagsService : IGetAllTagsService
{
    private readonly ITagRepository _tagRepository;

    private readonly ISearchPostQuery _searchPostQuery;

    private readonly ICurrentAuthUser _currentAuthUser;

    public GetAllTagsService(
        ITagRepository tagRepository, 
        ICurrentAuthUser currentAuthUser, 
        ISearchPostQuery searchPostQuery)
    {
        _tagRepository = tagRepository;
        _currentAuthUser = currentAuthUser;
        _searchPostQuery = searchPostQuery;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return (await _tagRepository.GetAllTags()).DistinctBy(x => x.Value.ToLower());
    }

    public async Task<IEnumerable<TagPostCountDto>> GetAllWithPostCountAsync()
    {
        return (await _tagRepository.GetAllTagsWithPostCount()).ToList();
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