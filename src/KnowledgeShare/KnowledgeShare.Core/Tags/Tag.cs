namespace KnowledgeShare.Core.Tags
{
    public class Tag
    {
        private Tag(
            string value)
        {
            Value = value;
        }
        
        public Tag(
            Guid id,
            string value)
        {
            Value = value;
        }
        
        public Guid Id { get; }

        public string Value { get; }

        public static Tag Create(string value)
        {
            return new Tag(value);
        }
    }
}
