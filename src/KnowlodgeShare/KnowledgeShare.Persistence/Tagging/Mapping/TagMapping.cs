using KnowledgeShare.Core.Persistence;
using KnowledgeShare.Domain;

namespace KnowledgeShare.Persistence.Tagging.Mapping;

public class TagMapping : EntityMapping<Tag>
{
    public TagMapping()
    {
        Map(x => x.Id, "id");
        Map(x => x.Value, "value");
    }
}