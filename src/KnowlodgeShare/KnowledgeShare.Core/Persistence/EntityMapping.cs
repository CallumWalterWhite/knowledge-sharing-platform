using System.Linq.Expressions;
using KnowledgeShare.Core.Domain;

namespace KnowledgeShare.Core.Persistence;

public class EntityMapping<T>
    where T : Entity
{
    private IList<Member> _members;

    public EntityMapping()
    {
        _members = new List<Member>();
    }
    
    public void Map(Expression<Func<T, object>> memberExpression, string column)
    {
        Type member = memberExpression.ReduceAndCheck().Type;
        _members.Add(new Member(member.ToString(), member.Name, column));
    }

    public void Reference(Expression<Func<T, object>> memberExpression, string column)
    {
        
    }
}