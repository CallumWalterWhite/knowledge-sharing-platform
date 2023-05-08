namespace KnowledgeShare.Core.Posts;

public interface IChangePostService
{
    Task<ChangePostDto> GetChangePostDto(Guid id, PostTypeDiscriminator postTypeDiscriminator);
    
    Task ChangeAsync(ChangePostDto changePostDto);
}