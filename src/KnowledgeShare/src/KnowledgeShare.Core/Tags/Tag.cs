namespace KnowledgeShare.Core.Tags
{
    public class Tag
    {
        private Tag(
            string value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
        
        public Tag(
            Guid id,
            string value)
        {
            Id = id;
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
