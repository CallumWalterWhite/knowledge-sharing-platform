namespace KnowledgeShare.Core.Posts.Validator;

public static class CreatePostDtoValidator
{
    public static void Validate(CreatePostDto createPostDto)
    {
        if (createPostDto.Discriminator == PostTypeDiscriminator.Article)
        {
            if (string.IsNullOrWhiteSpace(createPostDto.Link))
            {
                throw new MissingFieldException(() => createPostDto.Link);
            }
        }
    }
}