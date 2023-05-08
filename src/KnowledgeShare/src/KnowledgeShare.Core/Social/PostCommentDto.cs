namespace KnowledgeShare.Core.Social;

public record PostCommentDto(string personName, string personPicture, string comment, DateTime dateTimeCreated);