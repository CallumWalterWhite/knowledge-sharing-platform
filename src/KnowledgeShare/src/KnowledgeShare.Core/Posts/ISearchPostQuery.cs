﻿namespace KnowledgeShare.Core.Posts;

public interface ISearchPostQuery
{
       Task<IEnumerable<SearchPostResultDto>> SearchAsync(SearchPostDto searchPostDto);
       
       Task<IEnumerable<SearchPostResultDto>> RecommendAsync(Guid personId);

       Task<IEnumerable<SearchPostResultDto>> GetPostsAsync(Guid personId);
       
       Task<IEnumerable<SearchPostResultDto>> GetPostsByTagAsync(Guid tagId);
}