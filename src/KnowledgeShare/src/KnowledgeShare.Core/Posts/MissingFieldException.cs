namespace KnowledgeShare.Core.Posts;

public class MissingFieldException : Exception
{
    private Func<object> Property { get; }

    public MissingFieldException(Func<object> property)
    {
        Property = property;
    }
}