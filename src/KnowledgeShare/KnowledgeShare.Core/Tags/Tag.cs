namespace KnowledgeShare.Core.Tags
{
    public class Tag
    {
        public Tag(
            string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Tag Create(string value)
        {
            return new Tag(value);
        }
    }
}
