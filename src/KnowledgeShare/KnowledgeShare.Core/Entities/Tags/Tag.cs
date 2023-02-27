namespace KnowledgeShare.Core.Entities.Tags
{
    public class Tag : Entity
    {
        private Tag(
            Guid id,   
            string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Tag Create(string value)
        {
            return new Tag(Guid.NewGuid(), value);
        }
    }
}
